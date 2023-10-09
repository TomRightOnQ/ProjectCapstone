namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using EasyLayoutNS;
	using UIWidgets.Extensions;
	using UIWidgets.Internal;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <content>
	/// Base class for the custom ListViews.
	/// </content>
	public partial class ListViewCustom<TItemView, TItem> : ListViewCustom<TItem>, IUpdatable, ILateUpdatable, IListViewCallbacks<TItemView>
		where TItemView : ListViewItem
	{
		/// <summary>
		/// ListView renderer with items of variable size.
		/// </summary>
		protected class TileViewTypeSize : ListViewTypeSize
		{
			/// <summary>
			/// Blocks sizes.
			/// </summary>
			protected readonly List<float> BlockSizes = new List<float>();

			/// <summary>
			/// Items per row.
			/// </summary>
			protected int Columns;

			/// <summary>
			/// Rows.
			/// </summary>
			protected int Rows;

			/// <summary>
			/// Initializes a new instance of the <see cref="TileViewTypeSize"/> class.
			/// </summary>
			/// <param name="owner">Owner.</param>
			/// <param name="instanceSizes">Container for the instances sizes.</param>
			public TileViewTypeSize(ListViewCustom<TItemView, TItem> owner, IInstanceSizes<TItem> instanceSizes = null)
				: base(owner, instanceSizes)
			{
				if (Owner.ChangeLayoutType && (Owner.Layout != null))
				{
					Owner.Layout.LayoutType = LayoutTypes.Grid;

					if (Owner.StyleTable)
					{
						Owner.Layout.GridConstraint = Owner.IsHorizontal() ? GridConstraints.FixedRowCount : GridConstraints.FixedColumnCount;
						Owner.Layout.GridConstraintCount = 1;
					}
				}
			}

			/// <inheritdoc/>
			public override bool IsTileView
			{
				get
				{
					return true;
				}
			}

			/// <inheritdoc/>
			public override bool IsVirtualizationSupported()
			{
				var scrollRectSpecified = Owner.ScrollRect != null;
				var containerSpecified = Owner.Container != null;
				var currentLayout = containerSpecified ? ((Owner.Layout != null) ? Owner.Layout : Owner.Container.GetComponent<LayoutGroup>()) : null;
				var validLayout = currentLayout is EasyLayout;

				return scrollRectSpecified && validLayout;
			}

			/// <inheritdoc/>
			public override bool OnItemMove(AxisEventData eventData, ListViewItem item)
			{
				var step = 0;
				switch (eventData.moveDir)
				{
					case MoveDirection.Left:
						step = Owner.IsHorizontal() ? -1 : -GetItemsPerBlock();
						break;
					case MoveDirection.Right:
						step = Owner.IsHorizontal() ? 1 : GetItemsPerBlock();
						break;
					case MoveDirection.Up:
						step = Owner.IsHorizontal() ? -GetItemsPerBlock() : -1;
						break;
					case MoveDirection.Down:
						step = Owner.IsHorizontal() ? GetItemsPerBlock() : 1;
						break;
				}

				if (step == 0)
				{
					return false;
				}

				var target = GetSelectableComponentIndex(item.Index, step);

				return Owner.Navigate(eventData, target);
			}

			/// <inheritdoc/>
			public override void CalculateMaxVisibleItems()
			{
				CalculateInstancesSizes(Owner.DataSource, false);

				MaxVisibleItems = CalculateMaxVisibleItems(Owner.DataSource);
			}

			/// <inheritdoc/>
			protected override int CalculateMaxVisibleItems(ObservableList<TItem> items)
			{
				if (!HasInstanceSizes)
				{
					Rows = 1;
					Columns = 1;
					return 0;
				}

				var height = ViewportHeight();
				var width = ViewportWidth();

				if (Owner.IsHorizontal())
				{
					Rows = MaxVisible(false, height, Owner.GetItemSpacingY());
					Rows = Mathf.Max(1, Rows);
					Rows = Owner.LayoutBridge.ColumnsConstraint(Rows);

					CalculateBlockSizes(Rows);

					Columns = RequiredBlocksCount(width);
					Columns = Mathf.Max(MinVisibleItems, Columns);
				}
				else
				{
					Columns = MaxVisible(true, width, Owner.GetItemSpacingX());
					Columns = Mathf.Max(1, Columns);
					Columns = Owner.LayoutBridge.RowsConstraint(Columns);

					CalculateBlockSizes(Columns);

					Rows = RequiredBlocksCount(height);
					Rows = Mathf.Max(MinVisibleItems, Rows);
				}

				return Owner.Virtualization ? (Columns * Rows) : Owner.DataSource.Count;
			}

			/// <summary>
			/// Get required blocks count.
			/// </summary>
			/// <param name="size">Total size.</param>
			/// <returns>Required blocks count.</returns>
			protected int RequiredBlocksCount(float size)
			{
				var spacing = Owner.LayoutBridge.GetSpacing();
				var min = MinBlockSize();

				var blocks = 3;

				size -= min;
				if (size > 0)
				{
					blocks += Mathf.FloorToInt(size / (min + spacing));
				}

				return blocks;
			}

			/// <summary>
			/// Get minimal size of the blocks.
			/// </summary>
			/// <returns>Minimal size.</returns>
			protected float MinBlockSize()
			{
				if (BlockSizes.Count == 0)
				{
					return 1f;
				}

				var result = BlockSizes[0];

				for (int i = 1; i < BlockSizes.Count; i++)
				{
					result = Mathf.Min(result, BlockSizes[i]);
				}

				return Mathf.Max(1f, result);
			}

			/// <summary>
			/// Calculate block sizes.
			/// </summary>
			/// <param name="perBlock">Per block.</param>
			protected void CalculateBlockSizes(int perBlock)
			{
				BlockSizes.Clear();

				var blocks = Mathf.CeilToInt(Owner.DataSource.Count / (float)perBlock);
				for (int i = 0; i < blocks; i++)
				{
					var size = 0f;
					for (int j = i * perBlock; j < (i + 1) * perBlock; j++)
					{
						if (j < Owner.DataSource.Count)
						{
							size = Mathf.Max(size, GetInstanceSize(j));
						}
					}

					BlockSizes.Add(size);
				}
			}

			/// <inheritdoc/>
			public override float TopFillerSize()
			{
				return GetItemPosition(Visible.FirstVisible);
			}

			/// <inheritdoc/>
			public override float BottomFillerSize()
			{
				var last = Owner.DisplayedIndexLast + 1;
				var size = last < 0 ? 0f : GetBlocksSize(last, Owner.DataSource.Count - last);
				if (size > 0f)
				{
					size += Owner.LayoutBridge.GetSpacing();
				}

				return size;
			}

			/// <summary>
			/// Gets the blocks count.
			/// </summary>
			/// <returns>The blocks count.</returns>
			/// <param name="items">Items.</param>
			protected int GetBlocksCount(int items)
			{
				return items < 0
					? Mathf.FloorToInt(items / (float)GetItemsPerBlock())
					: Mathf.CeilToInt(items / (float)GetItemsPerBlock());
			}

			/// <summary>
			/// Convert visible block index to item block index.
			/// </summary>
			/// <returns>Block index.</returns>
			/// <param name="index">Visible block index.</param>
			protected virtual int VisibleBlockIndex2BlockIndex(int index)
			{
				return index % BlockSizes.Count;
			}

			/// <summary>
			/// Gets the size of the blocks.
			/// </summary>
			/// <returns>The blocks size.</returns>
			/// <param name="start">Start.</param>
			/// <param name="count">Count.</param>
			protected float GetBlocksSize(int start, int count)
			{
				int start_block;
				int end_block;

				if (count < 0)
				{
					start_block = GetBlocksCount(count);
					end_block = 0;
				}
				else
				{
					start_block = GetBlocksCount(start);
					end_block = GetBlocksCount(start + count);
				}

				var block_count = end_block - start_block;

				var size = 0f;
				for (int i = start_block; i < end_block; i++)
				{
					size += BlockSizes[VisibleBlockIndex2BlockIndex(i)];
				}

				size += Owner.LayoutBridge.GetSpacing() * (block_count - 1);
				if (count < 0)
				{
					size = -size;
				}

				return Owner.LoopedListAvailable ? size : Mathf.Max(0, size);
			}

			/// <inheritdoc/>
			protected override int GetBlockIndex(int index)
			{
				return Mathf.FloorToInt(index / (float)GetItemsPerBlock());
			}

			/// <inheritdoc/>
			public override float GetItemPosition(int index)
			{
				var block = GetBlockIndex(index);

				var size = IsRequiredCenterTheItems() ? CenteredFillerSize() : 0f;
				for (int i = 0; i < block; i++)
				{
					size += BlockSizes[i];
				}

				return size + (Owner.LayoutBridge.GetSpacing() * block);
			}

			/// <inheritdoc/>
			public override float GetItemPositionMiddle(int index)
			{
				var start = GetItemPosition(index);
				var end = GetItemPositionBottom(index);
				return start + ((end - start) / 2);
			}

			/// <inheritdoc/>
			public override float GetItemPositionBottom(int index)
			{
				var block = Mathf.Min(GetBlockIndex(index) + 1, BlockSizes.Count);

				var size = 0f;
				for (int i = 0; i < block; i++)
				{
					size += BlockSizes[i];
				}

				return size + (Owner.LayoutBridge.GetSpacing() * (block - 1)) + Owner.LayoutBridge.GetMargin() - Owner.Viewport.ScaledAxisSize;
			}

			int GetIndexAtPosition(float position)
			{
				var spacing = Owner.LayoutBridge.GetSpacing();
				int count = 0;

				if (position >= 0f)
				{
					for (int index = 0; index < BlockSizes.Count; index++)
					{
						position -= BlockSizes[index];
						if (index > 0)
						{
							position -= spacing;
						}

						if (position < 0)
						{
							break;
						}

						count += 1;
					}
				}
				else
				{
					position = -position;
					for (int index = BlockSizes.Count - 1; index >= 0; index--)
					{
						position -= BlockSizes[index];
						if (index > 0)
						{
							position -= spacing;
						}

						count--;
						if (position < 0)
						{
							break;
						}
					}
				}

				if (count >= BlockSizes.Count)
				{
					count = BlockSizes.Count - 1;
				}

				return Mathf.Min(count * GetItemsPerBlock(), Owner.DataSource.Count - 1);
			}

			/// <inheritdoc/>
			public override int GetFirstVisibleIndex(bool strict = false)
			{
				var first_visible_index = Mathf.Max(0, GetIndexAtPosition(GetPosition()));

				return first_visible_index;
			}

			/// <inheritdoc/>
			public override int GetLastVisibleIndex(bool strict = false)
			{
				var last_visible_index = GetIndexAtPosition(GetPosition() + Owner.Viewport.ScaledAxisSize);

				return strict ? last_visible_index : last_visible_index + GetItemsPerBlock();
			}

			/// <inheritdoc/>
			protected override int GetVisibleItems(int start_index)
			{
				var spacing = Owner.IsHorizontal() ? Owner.GetItemSpacingX() : Owner.GetItemSpacingY();

				var start_block = GetBlockIndex(start_index);
				var size = Owner.IsHorizontal() ? ViewportWidth() : ViewportHeight();

				var blocks = start_block;
				var max = BlockSizes.Count - 1;
				while ((size > 0) && (blocks < max))
				{
					blocks += 1;
					size -= BlockSizes[blocks] + spacing;
				}

				return (blocks + 1 - start_block) * GetItemsPerBlock();
			}

			/// <inheritdoc/>
			public override int GetNearestIndex(Vector2 point, NearestType type)
			{
				var pos_block = Owner.IsHorizontal() ? point.x : Mathf.Abs(point.y);
				pos_block -= IsRequiredCenterTheItems() ? CenteredFillerSize() : 0f;

				var start = GetIndexAtPosition(pos_block);

				var position = Owner.IsHorizontal() ? Mathf.Abs(point.y) : point.x;
				var spacing = Owner.LayoutBridge.GetSpacing();
				var end = Mathf.Min(Owner.DataSource.Count, start + GetItemsPerBlock());

				var index = 0;
				for (int i = start; i < end; i++)
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
						if (position >= (GetInstanceSize(index) / 2f))
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

				return Mathf.Min(Mathf.Min(index, Owner.DataSource.Count), start);
			}

			/// <inheritdoc/>
			public override int GetItemsPerBlock()
			{
				return Owner.IsHorizontal() ? Rows : Columns;
			}

			/// <inheritdoc/>
			public override float ListSize()
			{
				if (Owner.DataSource.Count == 0)
				{
					return 0;
				}

				return UtilitiesCollections.Sum(BlockSizes) + (BlockSizes.Count * Owner.LayoutBridge.GetSpacing()) - Owner.LayoutBridge.GetSpacing();
			}

			/// <inheritdoc/>
			public override void GetDebugInfo(System.Text.StringBuilder builder)
			{
				base.GetDebugInfo(builder);

				builder.AppendValue("Rows: ", Rows);
				builder.AppendValue("Columns: ", Columns);
				builder.AppendValue("BlockSizes: ", UtilitiesCollections.List2String(BlockSizes));
			}
		}
	}
}