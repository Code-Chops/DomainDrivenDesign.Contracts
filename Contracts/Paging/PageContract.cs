namespace CodeChops.Contracts.Paging;

/// <summary>
/// The contract of a page with a default (maximum page) size that can be overriden.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record PageContract : Contract
{
	[JsonIgnore] public virtual uint MaximumSize => 1000;
	[JsonIgnore] public virtual uint DefaultSize => 20;

	/// <summary>
	/// The page number (starting with page 0).
	/// </summary>
	public uint Page { get; } 
	
	/// <summary>
	/// The page size.
	/// </summary>
	public uint Size { get; }

	[JsonConstructor]
	// ReSharper disable VirtualMemberCallInConstructor
	public PageContract(uint page, uint? size)
	{
		this.Page = page;
		this.Size = size ?? this.DefaultSize;

		if (this.Size == 0) this.Size = 1;
		if (this.Size > this.MaximumSize) this.Size = this.MaximumSize;
	}
}