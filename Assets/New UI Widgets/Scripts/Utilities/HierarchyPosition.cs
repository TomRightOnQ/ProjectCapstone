namespace UIWidgets
{
	using UnityEngine;

	/// <summary>
	/// Hierarchy position.
	/// </summary>
	public struct HierarchyPosition
	{
		/// <summary>
		/// Target.
		/// </summary>
		public readonly Transform Target;

		/// <summary>
		/// Original parent of the target.
		/// </summary>
		public readonly Transform Parent;

		/// <summary>
		/// Original sibling index of the target.
		/// </summary>
		public readonly int SiblingIndex;

		/// <summary>
		/// World position stays.
		/// </summary>
		public readonly bool WorldPositionStays;

		/// <summary>
		/// Is parent changed?
		/// </summary>
		public bool Changed
		{
			get;
			private set;
		}

		bool parentDestroyed;

		private HierarchyPosition(Transform target, Transform parent, int siblingIndex, bool worldPositionStays)
		{
			Target = target;
			Parent = parent;
			SiblingIndex = siblingIndex;
			WorldPositionStays = worldPositionStays;
			Changed = true;
			parentDestroyed = false;
		}

		/// <summary>
		/// Restore parent with sibling index.
		/// </summary>
		public void Restore()
		{
			if (!Changed)
			{
				return;
			}

			// can be null if destroyed
			if (Parent == null)
			{
				return;
			}

			// can be null if destroyed
			if (Target == null)
			{
				return;
			}

			if (!parentDestroyed)
			{
				Target.SetParent(Parent);
				Target.SetSiblingIndex(SiblingIndex);
			}

			Changed = false;
		}

		/// <summary>
		/// Mark parent as destroyed.
		/// </summary>
		public void ParentDestroyed()
		{
			parentDestroyed = true;
		}

		/// <summary>
		/// Set parent and return object to restore original position in hierarchy.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="newParent">New parent/</param>
		/// <param name="worldPositionStays">World position stays.</param>
		/// <returns>Object to restore original position in hierarchy.</returns>
		public static HierarchyPosition SetParent(Transform target, Transform newParent, bool worldPositionStays = true)
		{
			var result = new HierarchyPosition(target, target.parent, target.GetSiblingIndex(), worldPositionStays);

			target.SetParent(newParent, worldPositionStays);
			target.SetAsLastSibling();

			return result;
		}
	}
}