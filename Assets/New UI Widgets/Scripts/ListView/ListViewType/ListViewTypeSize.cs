namespace UIWidgets
{
	using System;
	using EasyLayoutNS;
	using UIWidgets.Internal;
	using UIWidgets.Styles;
	using UnityEngine;

	/// <content>
	/// Base class for the custom ListViews.
	/// </content>
	public partial class ListViewCustom<TItemView, TItem> : ListViewCustom<TItem>, IUpdatable, ILateUpdatable, IListViewCallbacks<TItemView>
		where TItemView : ListViewItem
	{
		/// <summary>
		/// ListView renderer with items of variable size.
		/// </summary>
		protected class ListViewTypeSize : ListViewTypeRectangle
		{
			/// <inheritdoc/>
			public override bool SupportVariableSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>
			/// Sizes of instances.
			/// </summary>
			protected readonly IInstanceSizes<TItem> InstanceSizes;

			/// <summary>
			/// Initializes a new instance of the <see cref="ListViewTypeSize"/> class.
			/// </summary>
			/// <param name="owner">Owner.</param>
			/// <param name="instanceSizes">Container for the instances sizes.</param>
			public ListViewTypeSize(ListViewCustom<TItemView, TItem> owner, IInstanceSizes<TItem> instanceSizes = null)
				: base(owner)
			{
				InstanceSizes = instanceSizes ?? new InstanceSizes<TItem>();

				if (Owner.ChangeLayoutType && (Owner.Layout != null))
				{
					switch (Owner.Layout.LayoutType)
					{
						case LayoutTypes.Compact:
						case LayoutTypes.Staggered:
						case LayoutTypes.Ellipse:
						case LayoutTypes.Flex:
							Owner.Layout.LayoutType = LayoutTypes.Grid;
							Owner.Layout.GridConstraint = Owner.IsHorizontal() ? GridConstraints.FixedRowCount : GridConstraints.FixedColumnCount;
							Owner.Layout.GridConstraintCount = 1;
							break;
						case LayoutTypes.Grid:
							Owner.Layout.GridConstraint = Owner.IsHorizontal() ? GridConstraints.FixedRowCount : GridConstraints.FixedColumnCount;
							Owner.Layout.GridConstraintCount = 1;
							break;
					}
				}
			}

			/// <summary>
			/// Has instance sizes.
			/// </summary>
			protected bool HasInstanceSizes
			{
				get
				{
					return InstanceSizes.Count > 0;
				}
			}

			/// <summary>
			/// Check if has size of the specified item.
			/// </summary>
			/// <param name="item">Item.</param>
			/// <returns>true if has size of the specified item; otherwise false.</returns>
			protected bool HasInstanceSize(TItem item)
			{
				return InstanceSizes.Contains(item);
			}

			/// <summary>
			/// Save instance size of the specified item.
			/// </summary>
			/// <param name="item">Item.</param>
			/// <param name="size">Size.</param>
			protected void SaveInstanceSize(TItem item, Vector2 size)
			{
				InstanceSizes[item] = size;
			}

			/// <inheritdoc/>
			public override Vector2 GetInstanceFullSize(int index)
			{
				return InstanceSizes.Get(Owner.DataSource[index], base.GetInstanceFullSize(index));
			}

			/// <inheritdoc/>
			public override void SetInstanceFullSize(int index, Vector2 size)
			{
				InstanceSizes.SetOverridden(Owner.DataSource[index], size);
			}

			/// <inheritdoc/>
			public override void ResetInstanceFullSize(int index)
			{
				var item = Owner.DataSource[index];
				InstanceSizes.RemoveOverridden(item);

				var instance = Owner.GetItemInstance(index);
				if (instance == null)
				{
					return;
				}

				UpdateInstanceSize(instance, GetInstanceFullSize(index));
			}

			/// <inheritdoc/>
			protected override void UpdateInstanceSize(TItemView instance)
			{
				var item = Owner.DataSource[instance.Index];
				if (!InstanceSizes.TryGetOverridden(item, out var size))
				{
					return;
				}

				UpdateInstanceSize(instance, size);
			}

			void UpdateInstanceSize(TItemView instance, Vector2 size)
			{
				var current_size = instance.RectTransform.rect.size;

				if (Owner.IsHorizontal())
				{
					size.x = Mathf.Round(size.x);
					if (!Mathf.Approximately(current_size.x, size.x))
					{
						instance.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
					}
				}
				else
				{
					size.y = Mathf.Round(size.y);
					if (!Mathf.Approximately(current_size.y, size.y))
					{
						instance.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
					}
				}
			}

			/// <summary>
			/// Get maximum visible items count.
			/// </summary>
			/// <param name="isHorizontal">If true do calculation using items width; otherwise do calculation using items height.</param>
			/// <param name="visibleAreaSize">Size of the visible area.</param>
			/// <param name="spacing">Spacing between items.</param>
			/// <returns>Maximum visible items count.</returns>
			protected int MaxVisible(bool isHorizontal, float visibleAreaSize, float spacing)
			{
				return InstanceSizes.Visible(isHorizontal, visibleAreaSize, spacing);
			}

			/// <inheritdoc/>
			public override void CalculateMaxVisibleItems()
			{
				CalculateInstancesSizes(Owner.DataSource, false);

				MaxVisibleItems = CalculateMaxVisibleItems(Owner.DataSource);
			}

			/// <summary>
			/// Calculates the maximum count of the visible items.
			/// </summary>
			/// <param name="items">Items.</param>
			/// <returns>Maximum count of the visible items.</returns>
			protected virtual int CalculateMaxVisibleItems(ObservableList<TItem> items)
			{
				if (!Owner.Virtualization)
				{
					return Owner.DataSource.Count;
				}

				if (items.Count == 0)
				{
					return 0;
				}

				var visible = MaxVisible(Owner.IsHorizontal(), Owner.Viewport.ScaledAxisSize, Owner.LayoutBridge.GetSpacing());

				return MinVisibleItems + visible;
			}

			/// <inheritdoc/>
			public override float TopFillerSize()
			{
				return GetItemPosition(Visible.FirstVisible) - Owner.LayoutBridge.GetMargin();
			}

			/// <inheritdoc/>
			public override float BottomFillerSize()
			{
				var last = Owner.DisplayedIndexLast + 1;
				var size = last < 0 ? 0f : GetItemsSize(last, Owner.DataSource.Count - last);
				if (size > 0f)
				{
					size += Owner.LayoutBridge.GetSpacing();
				}

				return size;
			}

			/// <inheritdoc/>
			public override int GetFirstVisibleIndex(bool strict = false)
			{
				var first_visible_index = Mathf.Max(0, GetIndexAtPosition(GetPosition()));

				if (Owner.LoopedListAvailable)
				{
					return first_visible_index;
				}

				if (strict)
				{
					return first_visible_index;
				}

				return Mathf.Min(first_visible_index, Mathf.Max(0, Owner.DataSource.Count - MinVisibleItems));
			}

			/// <inheritdoc/>
			public override int GetLastVisibleIndex(bool strict = false)
			{
				var last_visible_index = GetIndexAtPosition(GetPosition() + Owner.Viewport.ScaledAxisSize);

				return strict ? last_visible_index : last_visible_index + 2;
			}

			/// <inheritdoc/>
			protected override int GetVisibleItems(int start_index)
			{
				var spacing = Owner.IsHorizontal() ? Owner.GetItemSpacingX() : Owner.GetItemSpacingY();

				var size = Owner.IsHorizontal() ? ViewportWidth() : ViewportHeight();

				var index = start_index;
				var max = Owner.DataSource.Count - 1;
				while ((size > 0) && (index < max))
				{
					index += 1;
					size -= GetInstanceSize(index) + spacing;
				}

				return (index + 1 - start_index) * GetItemsPerBlock();
			}

			/// <summary>
			/// Gets the width of the items.
			/// </summary>
			/// <returns>The items width.</returns>
			/// <param name="start">Start index.</param>
			/// <param name="count">Items count.</param>
			protected float GetItemsSize(int start, int count)
			{
				if (count == 0)
				{
					return 0f;
				}

				var width = 0f;
				var n = Owner.LoopedListAvailable ? start + count : Mathf.Min(start + count, Owner.DataSource.Count);
				for (int i = start; i < n; i++)
				{
					width += GetInstanceSize(VisibleIndex2ItemIndex(i));
				}

				width += Owner.LayoutBridge.GetSpacing() * (count - 1);

				return Mathf.Max(0, width);
			}

			/// <inheritdoc/>
			public override float GetItemPosition(int index)
			{
				var n = Mathf.Min(index, Owner.DataSource.Count);
				var size = IsRequiredCenterTheItems() ? CenteredFillerSize() : 0f;
				for (int i = 0; i < n; i++)
				{
					size += GetInstanceSize(i);
				}

				return size + (Owner.LayoutBridge.GetSpacing() * index) + Owner.LayoutBridge.GetMargin();
			}

			/// <inheritdoc/>
			public override float GetItemPositionBorderEnd(int index)
			{
				return GetItemPosition(index) + GetInstanceSize(index);
			}

			/// <inheritdoc/>
			public override float GetItemPositionMiddle(int index)
			{
				return GetItemPosition(index) + (GetInstanceSize(index) / 2f) - (Owner.Viewport.ScaledAxisSize / 2f);
			}

			/// <inheritdoc/>
			public override float GetItemPositionBottom(int index)
			{
				return GetItemPosition(index) + GetInstanceSize(index) + Owner.LayoutBridge.GetMargin() - Owner.LayoutBridge.GetSpacing() - Owner.Viewport.ScaledAxisSize;
			}

			/// <summary>
			/// Remove old items from saved sizes.
			/// </summary>
			/// <param name="items">New items.</param>
			protected virtual void RemoveOldItems(ObservableList<TItem> items)
			{
				InstanceSizes.RemoveUnexisting(items);
			}

			/// <inheritdoc/>
			public override void CalculateInstancesSizes(ObservableList<TItem> items, bool forceUpdate = true)
			{
				RemoveOldItems(items);

				if (Owner.PrecalculateItemSizes)
				{
					if (forceUpdate)
					{
						for (int index = 0; index < items.Count; index++)
						{
							var item = items[index];

							var template = Owner.ComponentsPool.GetTemplate(index);
							template.EnableTemplate();

							SaveInstanceSize(item, CalculateInstanceSize(item, template));
						}
					}
					else
					{
						for (int index = 0; index < items.Count; index++)
						{
							var item = items[index];
							if (!HasInstanceSize(item))
							{
								var template = Owner.ComponentsPool.GetTemplate(index);
								template.EnableTemplate();

								SaveInstanceSize(item, CalculateInstanceSize(item, template));
							}
						}
					}

					Owner.ComponentsPool.DisableTemplates();
				}
				else
				{
					if (forceUpdate)
					{
						for (int index = 0; index < items.Count; index++)
						{
							var item = items[index];
							SaveInstanceSize(item, Owner.DefaultInstanceSize);
						}
					}
					else
					{
						for (int index = 0; index < items.Count; index++)
						{
							var item = items[index];
							if (!HasInstanceSize(item))
							{
								SaveInstanceSize(item, Owner.DefaultInstanceSize);
							}
						}
					}
				}
			}

			/// <summary>
			/// Gets the index of the item at he specified position.
			/// </summary>
			/// <returns>The index.</returns>
			/// <param name="position">Position.</param>
			int GetIndexAtPosition(float position)
			{
				var result = GetIndexAtPosition(position, NearestType.Before);
				if (result >= Owner.DataSource.Count)
				{
					result = Owner.DataSource.Count - 1;
				}

				return result;
			}

			/// <summary>
			/// Gets the index of the item at he specified position.
			/// </summary>
			/// <returns>The index.</returns>
			/// <param name="position">Position.</param>
			/// <param name="type">Type.</param>
			int GetIndexAtPosition(float position, NearestType type)
			{
				position -= IsRequiredCenterTheItems() ? CenteredFillerSize() : 0f;

				var spacing = Owner.LayoutBridge.GetSpacing();
				var index = 0;
				for (int i = 0; i < Owner.DataSource.Count; i++)
				{
					index = i;

					var item_size = GetInstanceSize(i);
					if (i > 0)
					{
						item_size += spacing;
					}

					if (position < item_size)
					{
						break;
					}

					position -= item_size;
				}

				switch (type)
				{
					case NearestType.Auto:
						var item_size = GetInstanceSize(index);
						if (position >= (item_size / 2f))
						{
							index += 1;
						}

						break;
					case NearestType.Before:
						break;
					case NearestType.After:
						index += 1;
						break;
					default:
						throw new NotSupportedException(string.Format("Unsupported NearestType: {0}", EnumHelper<NearestType>.ToString(type)));
				}

				return index;
			}

			/// <inheritdoc/>
			public override void AddCallback(TItemView item)
			{
				item.onResize.AddListener(OnItemSizeChanged);
			}

			/// <inheritdoc/>
			public override void RemoveCallback(TItemView item)
			{
				item.onResize.RemoveListener(OnItemSizeChanged);
			}

			/// <summary>
			/// Handle component size changed event.
			/// </summary>
			/// <param name="index">Item index.</param>
			/// <param name="size">New size.</param>
			protected void OnItemSizeChanged(int index, Vector2 size)
			{
				if (!Owner.IsValid(index))
				{
					return;
				}

				// do not save size if instance size is overridden
				if (!InstanceSizes.HasOverridden(Owner.DataSource[index]))
				{
					UpdateItemSize(index, size);
				}
			}

			/// <summary>
			/// Update saved size of item.
			/// </summary>
			/// <param name="index">Index.</param>
			/// <param name="newSize">New size.</param>
			/// <returns>true if size different; otherwise, false.</returns>
			protected virtual bool UpdateItemSize(int index, Vector2 newSize)
			{
				var item = Owner.DataSource[index];
				newSize = ValidateSize(newSize);

				var current_size = GetInstanceFullSize(index);

				var is_equals = Owner.IsHorizontal()
					? Mathf.Approximately(current_size.x, newSize.x)
					: Mathf.Approximately(current_size.y, newSize.y);

				if (is_equals)
				{
					return false;
				}

				SaveInstanceSize(item, newSize);

				Owner.NeedUpdateView = true;

				return true;
			}

			/// <inheritdoc/>
			public override int GetNearestIndex(Vector2 point, NearestType type)
			{
				var pos_block = Owner.IsHorizontal() ? point.x : Mathf.Abs(point.y);
				var index = GetIndexAtPosition(pos_block, type);

				return Mathf.Min(index, Owner.DataSource.Count);
			}

			/// <inheritdoc/>
			public override int GetNearestItemIndex()
			{
				return GetIndexAtPosition(GetPosition());
			}

			/// <inheritdoc/>
			public override float ListSize()
			{
				if (Owner.DataSource.Count == 0)
				{
					return 0;
				}

				var size = 0f;
				for (var i = 0; i < Owner.DataSource.Count; i++)
				{
					size += GetInstanceSize(i);
				}

				return size + (Owner.DataSource.Count * Owner.LayoutBridge.GetSpacing()) - Owner.LayoutBridge.GetSpacing();
			}
		}
	}
}