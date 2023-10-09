namespace UIWidgets
{
	using System.Collections.Generic;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// ListViewBase.
	/// You can use it for creating custom ListViews.
	/// </summary>
	public abstract partial class ListViewBase : UIBehaviour,
			ISelectHandler, IDeselectHandler,
			ISubmitHandler, ICancelHandler,
			IStylable, IUpgradeable
	{
		/// <summary>
		/// Find ListVew items under pointer.
		/// </summary>
		protected class ItemsUnderPointerFinder
		{
			readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

			readonly List<ListViewItem> items = new List<ListViewItem>();

			PointerEventData eventData;

			EventSystem eventSystem;

			int frame = -1;

			/// <summary>
			/// Reset this instance.
			/// </summary>
			public void Reset()
			{
				raycastResults.Clear();
				items.Clear();
				eventData = null;
				eventSystem = null;
				frame = -1;
			}

			/// <summary>
			/// Find items under pointer.
			/// </summary>
			/// <returns>Items enumerator.</returns>
			public List<ListViewItem>.Enumerator GetEnumerator()
			{
				Find();
				return items.GetEnumerator();
			}

			IReadOnlyList<ListViewItem> Find()
			{
				if (frame == Time.frameCount)
				{
					return items;
				}

				frame = Time.frameCount;
				raycastResults.Clear();
				items.Clear();

				if (!CompatibilityInput.MousePresent)
				{
					return items;
				}

				if (EventSystem.current != null)
				{
					var es = EventSystem.current;
					if ((eventData == null) || (eventSystem != es))
					{
						eventSystem = es;
						eventData = new PointerEventData(es);
					}

					eventData.position = CompatibilityInput.MousePosition;

					EventSystem.current.RaycastAll(eventData, raycastResults);
				}

				foreach (var raycast in raycastResults)
				{
					if (!raycast.isValid)
					{
						continue;
					}

					var item = raycast.gameObject.GetComponent<ListViewItem>();
					if ((item != null) && (item.Owner != null))
					{
						items.Add(item);
					}
				}

				return items;
			}
		}
	}
}