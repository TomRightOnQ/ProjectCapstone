namespace UIThemes
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Option ID.
	/// </summary>
	[Serializable]
	public struct OptionId : IEquatable<OptionId>
	{
		/// <summary>
		/// None.
		/// </summary>
		[DomainReloadExclude]
		public static readonly OptionId None = new OptionId(-1);

		[SerializeField]
		int id;

		/// <summary>
		/// ID.
		/// </summary>
		public int Id
		{
			get
			{
				return id;
			}

			private set
			{
				id = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionId"/> struct.
		/// </summary>
		/// <param name="id">ID.</param>
		public OptionId(int id)
		{
			this.id = id;
		}

		/// <summary>
		/// Convert this instance to the string.
		/// </summary>
		/// <returns>String.</returns>
		public override string ToString()
		{
			return id.ToString();
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is OptionId)
			{
				return Equals((OptionId)obj);
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="other">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public bool Equals(OptionId other)
		{
			return id == other.id;
		}

		/// <summary>
		/// Hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return id;
		}

		/// <summary>
		/// Compare specified objects.
		/// </summary>
		/// <param name="a">First object.</param>
		/// <param name="b">Second object.</param>
		/// <returns>true if the objects are equal; otherwise, false.</returns>
		public static bool operator ==(OptionId a, OptionId b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compare specified objects.
		/// </summary>
		/// <param name="a">First object.</param>
		/// <param name="b">Second object.</param>
		/// <returns>true if the objects not equal; otherwise, false.</returns>
		public static bool operator !=(OptionId a, OptionId b)
		{
			return !a.Equals(b);
		}
	}
}