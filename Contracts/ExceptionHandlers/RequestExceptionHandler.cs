using System.Net;
using CodeChops.Contracts.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodeChops.Contracts.ExceptionHandlers;

/// <summary>
/// A global handler for uncaught exceptions during request handling.
/// </summary>
public class RequestExceptionHandler
{
	private IHostApplicationLifetime HostApplicationLifetime { get; }
	private IHttpContextAccessor HttpContextAccessor { get; }
	private ILogger<RequestExceptionHandler> Logger { get; }
	private JsonSerializerOptions JsonSerializerOptions { get; }
	
	public RequestExceptionHandler(IHostApplicationLifetime hostApplicationLifetime, IHttpContextAccessor httpContextAccessor, 
		ILogger<RequestExceptionHandler> logger, JsonSerializerOptions jsonSerializerOptions)
	{
		this.HostApplicationLifetime = hostApplicationLifetime;
		this.HttpContextAccessor = httpContextAccessor;
		this.Logger = logger;
		this.JsonSerializerOptions = jsonSerializerOptions;
	}

	public async Task HandleExceptionAsync()
	{
		var exceptionHandlerFeature = this.HttpContextAccessor.HttpContext?.Features.Get<IExceptionHandlerFeature>();
		var exception = exceptionHandlerFeature?.Error;

		// Note:
		// Cancellation checks are imperfect
		// Checking OperationCanceledException.CancellationToken: If multiple tokens are combined into a new token, we would not match, and wrongfully infer a "hard" failure
		// Checking CancellationToken.IsCancellationRequested: If a slow query or HTTP request times out, and the comparison token (RequestAborted, ApplicationStopping) was cancelled in the meantime, we would match, and wrongfully infer a "soft" failure
		// We choose the former as the lesser evil

		// Shutdown is an acceptable reason for cancellation
		if ((exception as OperationCanceledException)?.CancellationToken == this.HostApplicationLifetime.ApplicationStopping)
			this.Logger.LogInformation(exception, "Shutdown cancelled the request.");
		// An aborted request is an acceptable reason for cancellation
		else if (exception is OperationCanceledException opCanceledException && opCanceledException.CancellationToken == this.HttpContextAccessor.HttpContext?.RequestAborted)
			this.Logger.LogInformation(exception, "The caller cancelled the request.");
		else if (exception is IValidationException validationException)
			await this.HandleValidationExceptionAsync(validationException);
		else if (exception is not null)
			this.Logger.LogError(exception, "The request handler has thrown an exception.");
	}

	private static ValidationExceptionAdapter ValidationExceptionAdapter { get; } = new();
	
	private async Task HandleValidationExceptionAsync(IValidationException exception)
	{
		this.Logger.LogInformation(exception as Exception, "The request was invalid: {Message}", exception.Message);

		// Respond with the rejection if possible
		var httpContext = this.HttpContextAccessor.HttpContext;
		if (httpContext?.Response.HasStarted == false)
		{
			httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			var exceptionContract = ValidationExceptionAdapter.ConvertDomainObjectToContract(exception);
			
			var json = JsonSerializer.SerializeToUtf8Bytes(exceptionContract, this.JsonSerializerOptions);

			await httpContext.Response.Body.WriteAsync(json);
		}
	}
}
