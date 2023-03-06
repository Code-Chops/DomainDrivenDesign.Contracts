using System.Net;
using System.Net.Http.Json;

namespace CodeChops.Contracts.ExceptionHandlers;

public static class HttpResponseMessageExtensions
{
	public static async Task<TContract> ReadContractFromJson<TContract>(this HttpResponseMessage message, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
		where TContract : ICanReturnSimpleUserException
	{
		if (message.ContainsUserException(out var code))
			throw new InvalidOperationException($"Unexpected user friendly HTTP error {code} during request. Did you forget to handle user exceptions? Request content: {await message.RequestMessage?.Content?.ReadAsStringAsync(cancellationToken)!}.");
		
		options ??= JsonSerializerOptions.Default;
		return await message.Content.ReadFromJsonAsync<TContract>(options, cancellationToken)
			?? throw new InvalidOperationException($"Unexpected error during deserialization to contract {typeof(TContract).Name}? Response content: {await message.Content.ReadAsStringAsync(cancellationToken)}.");
	}
	
	public static bool ContainsUserException(this HttpResponseMessage message, out HttpStatusCode code)
	{
		code = message.StatusCode;
		return (int)code >= 400 && (int)code < 500;
	}
}