namespace UIThemes
{
	using System;

	/// <summary>
	/// Mark static fields that does not need to be reseted for domain reload support.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class DomainReloadExcludeAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DomainReloadExcludeAttribute"/> class.
		/// </summary>
		public DomainReloadExcludeAttribute()
		{
		}
	}
}