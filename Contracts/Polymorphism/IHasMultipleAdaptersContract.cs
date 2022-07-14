namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

/// <summary>
/// A contract can be used by multiple adapters. In order to know which adapter has to be used, use <see cref="AdapterId"/>.
/// </summary>
public interface IHasMultipleAdaptersContract
{
	public string AdapterId { get; }
}