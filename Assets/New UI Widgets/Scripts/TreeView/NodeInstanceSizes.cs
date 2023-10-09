namespace UIWidgets.Internal
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Default container to save instance sizes of items for TreeView.
	/// </summary>
	/// <typeparam name="TItem">Item type.</typeparam>
	public class NodeInstanceSizes<TItem> : IInstanceSizes<ListNode<TItem>>
	{
		readonly Dictionary<TreeNode<TItem>, Vector2> sizes = new Dictionary<TreeNode<TItem>, Vector2>();

		readonly Dictionary<TreeNode<TItem>, Vector2> overriddenSizes = new Dictionary<TreeNode<TItem>, Vector2>();

		readonly List<float> sortedSizes = new List<float>();

		readonly Dictionary<TreeNode<TItem>, bool> keep = new Dictionary<TreeNode<TItem>, bool>();

		readonly List<TreeNode<TItem>> remove = new List<TreeNode<TItem>>();

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
		/// <param name="key">Item.</param>
		/// <returns>Size.</returns>
		public Vector2 this[ListNode<TItem> key]
		{
			get
			{
				return sizes[key.Node];
			}

			set
			{
				sizes[key.Node] = value;
			}
		}

		/// <summary>
		/// Check if container has size of the specified item.
		/// </summary>
		/// <param name="key">Item.</param>
		/// <returns>true if container has size of the specified item; otherwise false.</returns>
		public bool Contains(ListNode<TItem> key)
		{
			return sizes.ContainsKey(key.Node);
		}

		/// <summary>
		/// Try to get size of the specified item.
		/// </summary>
		/// <param name="key">Item.</param>
		/// <param name="size">Instance size.</param>
		/// <returns>true if container has size of the specified item; otherwise false.</returns>
		public bool TryGet(ListNode<TItem> key, out Vector2 size)
		{
			return sizes.TryGetValue(key.Node, out size);
		}

		/// <summary>
		/// Remove size of the specified item.
		/// </summary>
		/// <param name="key">Item.</param>
		/// <returns>true if container has size; otherwise false.</returns>
		public bool Remove(ListNode<TItem> key)
		{
			return sizes.Remove(key.Node);
		}

		/// <summary>
		/// Remove sizes of items that are not in the specified list.
		/// </summary>
		/// <param name="items">Items.</param>
		public void RemoveUnexisting(ObservableList<ListNode<TItem>> items)
		{
			// should not delete because collapsed nodes are not present in list
			// RemoveUnexisting(sizes, items);
			// RemoveUnexisting(overriddenSizes, items);
		}

		/// <summary>
		/// Remove sizes of items that are not in the specified list.
		/// </summary>
		/// <param name="isExisting">Function to check is item exists.</param>
		public void RemoveUnexisting(Func<TreeNode<TItem>, bool> isExisting)
		{
			RemoveUnexisting(sizes, isExisting);
			RemoveUnexisting(overriddenSizes, isExisting);
		}

		void RemoveUnexisting(Dictionary<TreeNode<TItem>, Vector2> currentSizes, Func<TreeNode<TItem>, bool> isExisting)
		{
			foreach (var node in currentSizes)
			{
				if (!isExisting(node.Key))
				{
					remove.Add(node.Key);
				}
			}

			foreach (var item in remove)
			{
				currentSizes.Remove(item);
			}

			keep.Clear();
			remove.Clear();
		}

		void RemoveUnexisting(Dictionary<TreeNode<TItem>, Vector2> currentSizes, ObservableList<ListNode<TItem>> items)
		{
			foreach (var item in items)
			{
				if (currentSizes.ContainsKey(item.Node))
				{
					keep[item.Node] = true;
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
		public bool HasOverridden(ListNode<TItem> item)
		{
			return overriddenSizes.ContainsKey(item.Node);
		}

		/// <summary>
		/// Set overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Size.</param>
		public void SetOverridden(ListNode<TItem> item, Vector2 size)
		{
			SetOverridden(item.Node, size);
		}

		/// <summary>
		/// Set overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Size.</param>
		public void SetOverridden(TreeNode<TItem> item, Vector2 size)
		{
			overriddenSizes[item] = size;
		}

		/// <summary>
		/// Remove size of the specified item.
		/// </summary>
		/// <param name="key">Item.</param>
		/// <returns>true if container has size; otherwise false.</returns>
		public bool RemoveOverridden(TreeNode<TItem> key)
		{
			return overriddenSizes.Remove(key);
		}

		/// <summary>
		/// Remove overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>true if container has overridden size; otherwise false.</returns>
		public bool RemoveOverridden(ListNode<TItem> item)
		{
			return overriddenSizes.Remove(item.Node);
		}

		/// <summary>
		/// Try to get overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Instance size.</param>
		/// <returns>true if container has overridden size of the specified item; otherwise false.</returns>
		public bool TryGetOverridden(ListNode<TItem> item, out Vector2 size)
		{
			return TryGetOverridden(item.Node, out size);
		}

		/// <summary>
		/// Try to get overridden size of the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="size">Instance size.</param>
		/// <returns>true if container has overridden size of the specified item; otherwise false.</returns>
		public bool TryGetOverridden(TreeNode<TItem> item, out Vector2 size)
		{
			return overriddenSizes.TryGetValue(item, out size);
		}

		/// <summary>
		/// Get actual item size.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="defaultSize">Default size if any other size not specified.</param>
		/// <returns>Size.</returns>
		public Vector2 Get(ListNode<TItem> item, Vector2 defaultSize)
		{
			if (overriddenSizes.TryGetValue(item.Node, out Vector2 result))
			{
				return result;
			}

			if (sizes.TryGetValue(item.Node, out result))
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
				if (info.Key.Index == -1)
				{
					continue;
				}

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