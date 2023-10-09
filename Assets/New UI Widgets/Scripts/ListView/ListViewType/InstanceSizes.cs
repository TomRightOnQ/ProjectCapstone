namespace UIWidgets.Internal
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Default container to save instance sizes of items for ListView.
	/// </summary>
	/// <typeparam name="TItem">Item type.</typeparam>
	public class InstanceSizes<TItem> : IInstanceSizes<TItem>
	{
		Dictionary<TItem, Vector2> sizes = new Dictionary<TItem, Vector2>();

		Dictionary<TItem, Vector2> overriddenSizes = new Dictionary<TItem, Vector2>();

		List<float> sortedSizes = new List<float>();

		Dictionary<TItem, bool> keep = new Dictionary<TItem, bool>();

		List<TItem> remove = new List<TItem>();

		/// <summary>
		/// Count.
		/// </summary>
		public int Count
		{
			get
			{
				return sizes.Count;
			}
		}

		/// <summary>
		/// Get and set instance size.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Size.</returns>
		public Vector2 this[TItem item]
		{
			get
			{
				return sizes[item];
			}

			set
			{
				sizes[item] = value;
			}
		}

		/// <summary>
		/// Check if container has size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>true if container has size of the specified item; otherwise false.</returns>
		public bool Contains(TItem item)
		{
			return sizes.ContainsKey(item);
		}

		/// <summary>
		/// Try to get size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Instance size.</param>
		/// <returns>true if container has size of the specified item; otherwise false.</returns>
		public bool TryGet(TItem item, out Vector2 size)
		{
			return sizes.TryGetValue(item, out size);
		}

		/// <summary>
		/// Remove size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>true if container has size; otherwise false.</returns>
		public bool Remove(TItem item)
		{
			return sizes.Remove(item);
		}

		/// <summary>
		/// Remove sizes of items that are not in the specified list.
		/// </summary>
		/// <param name="items">Items.</param>
		public void RemoveUnexisting(ObservableList<TItem> items)
		{
			RemoveUnexisting(sizes, items);
			RemoveUnexisting(overriddenSizes, items);
		}

		void RemoveUnexisting(Dictionary<TItem, Vector2> currentSizes, ObservableList<TItem> items)
		{
			foreach (var item in items)
			{
				if (currentSizes.ContainsKey(item))
				{
					keep[item] = true;
				}
			}

			foreach (var item in currentSizes)
			{
				if (!keep.ContainsKey(item.Key))
				{
					remove.Add(item.Key);
				}
			}

			foreach (var item in remove)
			{
				currentSizes.Remove(item);
			}

			keep.Clear();
			remove.Clear();
		}

		/// <summary>
		/// Get items count that fits into specified area.
		/// </summary>
		/// <param name="horizontal">If true do calculation using items width; otherwise do calculation using items height.</param>
		/// <param name="visibleArea">Size of the visible area.</param>
		/// <param name="spacing">Spacing between items.</param>
		/// <returns>Maximum items count.</returns>
		public int Visible(bool horizontal, float visibleArea, float spacing)
		{
			foreach (var info in sizes)
			{
				if (!overriddenSizes.TryGetValue(info.Key, out Vector2 size))
				{
					size = info.Value;
				}

				sortedSizes.Add(horizontal ? size.x : size.y);
			}

			sortedSizes.Sort();
			var max = 0;
			foreach (var s in sortedSizes)
			{
				visibleArea -= s;

				if (visibleArea <= 0f)
				{
					break;
				}

				max += 1;
				visibleArea -= spacing;
			}

			sortedSizes.Clear();

			return Mathf.Max(1, max);
		}

		/// <summary>
		/// Check if has overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>true if has size of the specified item; otherwise false.</returns>
		public bool HasOverridden(TItem item)
		{
			return overriddenSizes.ContainsKey(item);
		}

		/// <summary>
		/// Set overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Size.</param>
		public void SetOverridden(TItem item, Vector2 size)
		{
			overriddenSizes[item] = size;
		}

		/// <summary>
		/// Remove overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>true if container has overridden size; otherwise false.</returns>
		public bool RemoveOverridden(TItem item)
		{
			return overriddenSizes.Remove(item);
		}

		/// <summary>
		/// Try to get overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Instance size.</param>
		/// <returns>true if container has overridden size of the specified item; otherwise false.</returns>
		public bool TryGetOverridden(TItem item, out Vector2 size)
		{
			return overriddenSizes.TryGetValue(item, out size);
		}

		/// <summary>
		/// Get actual item size.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="defaultSize">Default size if any other size not specified.</param>
		/// <returns>Size.</returns>
		public Vector2 Get(TItem item, Vector2 defaultSize)
		{
			if (overriddenSizes.TryGetValue(item, out Vector2 result))
			{
				return result;
			}

			if (sizes.TryGetValue(item, out result))
			{
				return result;
			}

			return defaultSize;
		}

		/// <summary>
		/// Maximum size for each dimenstion.
		/// </summary>
		/// <param name="defaultSize">Default size.</param>
		/// <returns>Size.</returns>
		public Vector2 MaxSize(Vector2 defaultSize)
		{
			var result = defaultSize;
			foreach (var info in sizes)
			{
				if (!overriddenSizes.TryGetValue(info.Key, out Vector2 size))
				{
					size = info.Value;
				}

				result.x = Mathf.Max(result.x, size.x);
				result.y = Mathf.Max(result.y, size.y);
			}

			return result;
		}
	}
}