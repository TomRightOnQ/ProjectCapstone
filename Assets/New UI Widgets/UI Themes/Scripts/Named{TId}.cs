namespace UIThemes
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Data with id and name.
	/// </summary>
	/// <typeparam name="TId">Type of ID.</typeparam>
	[Serializable]
	public class Named<TId> : IEquatable<Named<TId>>
		where TId : IEquatable<TId>
	{
		[SerializeField]
		TId id;

		/// <summary>
		/// ID.
		/// </summary>
		public TId Id
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

		[SerializeField]
		string name;

		/// <summary>
		/// Name.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Named{TId}"/> class.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		public Named(TId id, string name)
		{
			this.id = id;
			this.name = name;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Named<TId>)
			{
				return Equals((Named<TId>)obj);
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="other">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public bool Equals(Named<TId> other)
		{
			if (other == null)
			{
				return false;
			}

			return id.Equals(other.id) && (name == other.name);
		}

		/// <summary>
		/// Hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return id.GetHashCode() ^ name.GetHashCode();
		}

		/// <summary>
		/// Compare specified objects.
		/// </summary>
		/// <param name="a">First object.</param>
		/// <param name="b">Second object.</param>
		/// <returns>true if the objects are equal; otherwise, false.</returns>
		public static bool operator ==(Named<TId> a, Named<TId> b)
		{
			var a_null = ReferenceEquals(a, null);
			var b_null = ReferenceEquals(b, null);
			if (a_null && b_null)
			{
				return true;
			}

			if (a_null != b_null)
			{
				return false;
			}

			return a.Equals(b);
		}

		/// <summary>
		/// Compare specified objects.
		/// </summary>
		/// <param name="a">First object.</param>
		/// <param name="b">Second object.</param>
		/// <returns>true if the objects not equal; otherwise, false.</returns>
		public static bool operator !=(Named<TId> a, Named<TId> b)
		{
			var a_null = ReferenceEquals(a, null);
			var b_null = ReferenceEquals(b, null);
			if (a_null && b_null)
			{
				return false;
			}

			if (a_null != b_null)
			{
				return true;
			}

			return !a.Equals(b);
		}
	}
}