namespace CodeChops.Contracts;

/// <summary>
/// A paging contract with a default (maximum page) size that can be overriden.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record PagingContract : Contract
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
	public PagingContract(uint page, uint? size)
	{
		this.Page = page;
		this.Size = size ?? this.DefaultSize;

		if (this.Size == 0) this.Size = 1;
		if (this.Size > this.MaximumSize) this.Size = this.MaximumSize;
	}
}