namespace UIWidgets
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading;
	using EasyLayoutNS;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for custom ListViews.
	/// </summary>
	public abstract partial class ListViewCustomBase : ListViewBase, IAutoScroll
	{
		/// <summary>
		/// Virtualization.
		/// </summary>
		[SerializeField]
		protected bool virtualization = true;

		/// <summary>
		/// Virtualization.
		/// </summary>
		public bool Virtualization
		{
			get
			{
				return virtualization;
			}

			set
			{
				if (virtualization != value)
				{
					virtualization = value;
					UpdateView();
				}
			}
		}

		/// <summary>
		/// Reversed order.
		/// </summary>
		[SerializeField]
		protected bool reversedOrder = false;

		/// <summary>
		/// Reversed order.
		/// </summary>
		public bool ReversedOrder
		{
			get
			{
				return reversedOrder;
			}

			set
			{
				if (reversedOrder != value)
				{
					reversedOrder = value;
					UpdateView();
				}
			}
		}

		/// <summary>
		/// ListView display type.
		/// </summary>
		[SerializeField]
		protected ListViewType listType = ListViewType.ListViewWithFixedSize;

		/// <summary>
		/// ListView display type.
		/// </summary>
		public abstract ListViewType ListType
		{
			get;
			set;
		}

		/// <summary>
		/// Change EasyLayout.LayoutType together with ListType.
		/// </summary>
		[SerializeField]
		[Tooltip("Change EasyLayout.LayoutType together with ListType.")]
		public bool ChangeLayoutType = false;

		/// <summary>
		/// If data source setted?
		/// </summary>
		protected bool DataSourceSetted;

		/// <summary>
		/// Is data source changed?
		/// </summary>
		protected bool IsDataSourceChanged;

		/// <summary>
		/// Destroy instances of the previous DefaultItem when replacing DefaultItem.
		/// </summary>
		[SerializeField]
		[Tooltip("Destroy instances of the previous DefaultItem when replacing DefaultItem.")]
		protected bool destroyDefaultItemsCache = true;

		/// <summary>
		/// Destroy instances of the previous DefaultItem when replacing DefaultItem.
		/// </summary>
		public abstract bool DestroyDefaultItemsCache
		{
			get;
			set;
		}

		[SerializeField]
		[FormerlySerializedAs("Sort")]
		bool sort = true;

		/// <summary>
		/// Sort items.
		/// Deprecated. Replaced with DataSource.Comparison.
		/// </summary>
		[Obsolete("Replaced with DataSource.Comparison.")]
		public virtual bool Sort
		{
			get
			{
				return sort;
			}

			set
			{
				sort = value;
				if (sort && isListViewCustomInited)
				{
					UpdateItems();
				}
			}
		}

		/// <summary>
		/// Disable ScrollRect if ListView is not interactable.
		/// </summary>
		[SerializeField]
		[Tooltip("Disable ScrollRect if not interactable.")]
		protected bool disableScrollRect = false;

		/// <summary>
		/// Disable ScrollRect if not interactable.
		/// </summary>
		public bool DisableScrollRect
		{
			get
			{
				return disableScrollRect;
			}

			set
			{
				if (disableScrollRect != value)
				{
					disableScrollRect = value;
					ToggleScrollRect();
				}
			}
		}

		/// <summary>
		/// The displayed indices.
		/// </summary>
		protected List<int> DisplayedIndices = new List<int>();

		/// <summary>
		/// The disabled recycling indices.
		/// </summary>
		protected List<int> DisableRecyclingIndices = new List<int>();

		/// <summary>
		/// Gets the first displayed index.
		/// </summary>
		/// <value>The first displayed index.</value>
		[Obsolete("Renamed to DisplayedIndexFirst.")]
		public int DisplayedIndicesFirst
		{
			get
			{
				return DisplayedIndices.Count > 0 ? DisplayedIndices[0] : -1;
			}
		}

		/// <summary>
		/// Gets the last displayed index.
		/// </summary>
		/// <value>The last displayed index.</value>
		[Obsolete("Renamed to DisplayedIndexLast.")]
		public int DisplayedIndicesLast
		{
			get
			{
				return DisplayedIndices.Count > 0 ? DisplayedIndices[DisplayedIndices.Count - 1] : -1;
			}
		}

		/// <inheritdoc/>
		public override int DisplayedIndexFirst
		{
			get
			{
				return DisplayedIndices.Count > 0 ? DisplayedIndices[0] : -1;
			}
		}

		/// <inheritdoc/>
		public override int DisplayedIndexLast
		{
			get
			{
				return DisplayedIndices.Count > 0 ? DisplayedIndices[DisplayedIndices.Count - 1] : -1;
			}
		}

		/// <summary>
		/// If enabled scroll limited to last item.
		/// </summary>
		[SerializeField]
		[Obsolete("Use ScrollRect.MovementType = Clamped instead.")]
		public bool LimitScrollValue = false;

		/// <summary>
		/// The ScrollRect.
		/// </summary>
		[SerializeField]
		protected ScrollRect scrollRect;

		/// <summary>
		/// Gets or sets the ScrollRect.
		/// </summary>
		/// <value>The ScrollRect.</value>
		public ScrollRect ScrollRect
		{
			get
			{
				return scrollRect;
			}

			set
			{
				SetScrollRect(value);
			}
		}

		ScrollRectData viewport;

		/// <summary>
		/// ScrollRect viewport data.
		/// </summary>
		protected ScrollRectData Viewport
		{
			get
			{
				if (viewport == null)
				{
					viewport = new ScrollRectData(this, ScrollRect);
				}

				return viewport;
			}
		}

		/// <summary>
		/// The size of the ScrollRect.
		/// </summary>
		[Obsolete("Replaced with ScrollRectData.Size.")]
		protected Vector2 ScrollRectSize
		{
			get
			{
				return Viewport.Size;
			}
		}

		/// <summary>
		/// The direction.
		/// </summary>
		[SerializeField]
		protected ListViewDirection direction = ListViewDirection.Vertical;

		/// <summary>
		/// Set content size fitter settings?
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_setContentSizeFitter")]
		protected bool setContentSizeFitter = true;

		/// <summary>
		/// The set ContentSizeFitter parameters according direction.
		/// </summary>
		public bool SetContentSizeFitter
		{
			get
			{
				return setContentSizeFitter;
			}

			set
			{
				setContentSizeFitter = value;

				UpdateLayoutBridgeContentSizeFitter();
			}
		}

		/// <summary>
		/// Update LayoutBridge ContentSizeFitter.
		/// </summary>
		protected abstract void UpdateLayoutBridgeContentSizeFitter();

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public ListViewDirection Direction
		{
			get
			{
				return direction;
			}

			set
			{
				SetDirection(value);
			}
		}

		/// <summary>
		/// The layout.
		/// </summary>
		protected EasyLayout layout;

		/// <summary>
		/// Gets the layout.
		/// </summary>
		/// <value>The layout.</value>
		public EasyLayout Layout
		{
			get
			{
				if (layout == null)
				{
					layout = Container.GetComponent<EasyLayout>();
				}

				return layout;
			}
		}

		/// <summary>
		/// LayoutBridge.
		/// </summary>
		protected ILayoutBridge layoutBridge;

		/// <summary>
		/// LayoutBridge.
		/// </summary>
		protected abstract ILayoutBridge LayoutBridge
		{
			get;
		}

		/// <summary>
		/// Require EasyLayout.
		/// </summary>
		protected virtual bool RequireEasyLayout
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Scroll use unscaled time.
		/// </summary>
		[SerializeField]
		public bool ScrollUnscaledTime = true;

		/// <summary>
		/// Scroll movement curve.
		/// </summary>
		[SerializeField]
		[Tooltip("Requirements: start value should be less than end value; Recommended start value = 0; end value = 1;")]
		public AnimationCurve ScrollMovement = AnimationCurve.EaseInOut(0, 0, 0.25f, 1);

		/// <summary>
		/// The scroll coroutine.
		/// </summary>
		protected IEnumerator ScrollCoroutine;

		/// <summary>
		/// The main thread.
		/// </summary>
		protected Thread MainThread;

		/// <summary>
		/// Gets a value indicating whether this instance is executed in main thread.
		/// </summary>
		/// <value><c>true</c> if this instance is executed in main thread; otherwise, <c>false</c>.</value>
		protected bool IsMainThread
		{
			get
			{
				return MainThread != null && MainThread.Equals(Thread.CurrentThread);
			}
		}

		/// <summary>
		/// Is DefaultItem implements IViewData{TItem}.
		/// </summary>
		protected bool CanSetData;

		/// <summary>
		/// Center the list items if all items visible.
		/// </summary>
		[SerializeField]
		[Tooltip("Center the list items if all items visible.")]
		protected bool centerTheItems;

		/// <summary>
		/// Center the list items if all items visible.
		/// </summary>
		public virtual bool CenterTheItems
		{
			get
			{
				return centerTheItems;
			}

			set
			{
				centerTheItems = value;
				UpdateView();
			}
		}

		/// <summary>
		/// List should be looped.
		/// </summary>
		[SerializeField]
		protected bool loopedList = false;

		/// <summary>
		/// List can be looped.
		/// </summary>
		/// <value><c>true</c> if list can be looped; otherwise, <c>false</c>.</value>
		public virtual bool LoopedList
		{
			get
			{
				return loopedList;
			}

			set
			{
				loopedList = value;
			}
		}

		/// <summary>
		/// Precalculate item sizes.
		/// Disabling this option increase performance with huge lists of items with variable sizes and decrease scroll precision.
		/// </summary>
		[SerializeField]
		public bool PrecalculateItemSizes = true;

		/// <summary>
		/// Header.
		/// </summary>
		[SerializeField]
		protected TableHeader header;

		/// <summary>
		/// Header.
		/// </summary>
		public TableHeader Header
		{
			get
			{
				return header;
			}

			set
			{
				if (header != value)
				{
					if (header != null)
					{
						header.List = null;
					}

					header = value;

					if (header != null)
					{
						header.List = this;

						if (isListViewCustomInited)
						{
							header.Refresh();
						}
					}
				}
			}
		}

		/// <summary>
		/// Maximal count of the visible items.
		/// </summary>
		public abstract int MaxVisibleItems
		{
			get;
		}

		/// <summary>
		/// The size of the DefaultItem.
		/// </summary>
		[Obsolete("Renamed to DefaultInstanceSize.")]
		protected Vector2 ItemSize
		{
			get
			{
				return DefaultInstanceSize;
			}
		}

		/// <summary>
		/// The size of the DefaultItem.
		/// </summary>
		protected Vector2 DefaultInstanceSize;

		/// <summary>
		/// Is ListViewCustom inited?
		/// </summary>
		[NonSerialized]
		protected bool isListViewCustomInited = false;

		/// <inheritdoc/>
		protected override void UpdateComponents<TItemBase>(List<TItemBase> newItems)
		{
			instances.Clear();
			foreach (var item in newItems)
			{
				instances.Add(item);
			}
		}

		/// <summary>
		/// Toggle ScrollRect state.
		/// </summary>
		protected void ToggleScrollRect()
		{
			if (ScrollRect != null)
			{
				ScrollRect.enabled = !DisableScrollRect || IsInteractable();
			}
		}

		/// <summary>
		/// What to do when widget became interactable.
		/// </summary>
		protected override void OnInteractableEnabled()
		{
			ToggleScrollRect();
		}

		/// <summary>
		/// What to do when widget became not interactable.
		/// </summary>
		protected override void OnInteractableDisabled()
		{
			ToggleScrollRect();
		}

		/// <summary>
		/// Sets the direction.
		/// </summary>
		/// <param name="newDirection">New direction.</param>
		/// <param name="updateView">Update view.</param>
		protected abstract void SetDirection(ListViewDirection newDirection, bool updateView = true);

		/// <summary>
		/// Updates the view.
		/// </summary>
		public abstract void UpdateView();

		/// <summary>
		/// Set ScrollRect.
		/// </summary>
		/// <param name="newScrollRect">New ScrollRect.</param>
		protected abstract void SetScrollRect(ScrollRect newScrollRect);

		/// <summary>
		/// Get secondary scroll position (for the cross direction).
		/// </summary>
		/// <param name="index">Index.</param>
		/// <returns>Secondary scroll position.</returns>
		protected virtual float GetScrollPositionSecondary(int index)
		{
			var current_position = ContainerAnchoredPosition;

			return IsHorizontal() ? current_position.y : current_position.x;
		}

		/// <summary>
		/// Check currently selected GameObject.
		/// </summary>
		/// <param name="position">Scroll position.</param>
		protected void SelectableCheck(Vector2 position)
		{
			SelectableCheck();
		}

		/// <summary>
		/// Select new selectable GameObject if needed.
		/// </summary>
		/// <param name="position">Scroll position.</param>
		protected void SelectableSet(Vector2 position)
		{
			SelectableSet();
		}

		/// <summary>
		/// Check currently selected GameObject.
		/// </summary>
		protected abstract void SelectableCheck();

		/// <summary>
		/// Select new selectable GameObject if needed.
		/// </summary>
		protected abstract void SelectableSet();

		/// <summary>
		/// Gets the layout margin.
		/// </summary>
		/// <returns>The layout margin.</returns>
		public override Vector4 GetLayoutMargin()
		{
			return LayoutBridge.GetMarginSize();
		}

		/// <summary>
		/// Gets the spacing between items.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public override float GetItemSpacing()
		{
			return LayoutBridge.GetSpacing();
		}

		/// <summary>
		/// Gets the horizontal spacing between items.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public override float GetItemSpacingX()
		{
			return LayoutBridge.GetSpacingX();
		}

		/// <summary>
		/// Gets the vertical spacing between items.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public override float GetItemSpacingY()
		{
			return LayoutBridge.GetSpacingY();
		}

		/// <summary>
		/// Determines whether this instance is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		public override bool IsHorizontal()
		{
			return direction == ListViewDirection.Horizontal;
		}

		/// <summary>
		/// Change DefaultItem size.
		/// </summary>
		/// <param name="size">Size.</param>
		public abstract void ChangeDefaultItemSize(Vector2 size);

		/// <summary>
		/// Resize this instance.
		/// </summary>
		public abstract void Resize();

		#region AutoScroll

		/// <summary>
		/// Auto scroll area.
		/// </summary>
		[SerializeField]
		[Tooltip("Distance from the ScrollRect borders where auto scroll enabled.")]
		public float AutoScrollArea = 40f;

		/// <summary>
		/// Auto scroll speed.
		/// </summary>
		[SerializeField]
		public float AutoScrollSpeed = 200f;

		/// <summary>
		/// Scroll coroutine.
		/// </summary>
		protected Coroutine AutoScrollCoroutine;

		/// <summary>
		/// Auto scroll direction.
		/// </summary>
		protected int AutoScrollDirection;

		/// <summary>
		/// Auto scroll EventData for callback.
		/// </summary>
		protected PointerEventData AutoScrollEventData;

		/// <summary>
		/// Auto scroll callback.
		/// </summary>
		protected Action<PointerEventData> AutoScrollCallback;

		/// <summary>
		/// Stop auto-scroll.
		/// </summary>
		public virtual void AutoScrollStop()
		{
			if (AutoScrollCoroutine == null)
			{
				return;
			}

			AutoScrollDirection = 0;
			StopCoroutine(AutoScrollCoroutine);

			AutoScrollCoroutine = null;
			AutoScrollCallback = null;
			AutoScrollEventData = null;
		}

		/// <summary>
		/// Start scroll when pointer is near ScrollRect border.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		/// <param name="callback">Callback.</param>
		/// <returns>true if auto scroll started; otherwise false.</returns>
		public virtual bool AutoScrollStart(PointerEventData eventData, Action<PointerEventData> callback)
		{
			var target = ScrollRect.transform as RectTransform;

			Vector2 point;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(target, eventData.position, eventData.pressEventCamera, out point))
			{
				AutoScrollStop();
				return false;
			}

			var rect_start = target.rect;
			var rect_end = target.rect;

			if (IsHorizontal())
			{
				rect_start.width = AutoScrollArea;

				rect_end.position = new Vector2(rect_end.position.x + rect_end.width - AutoScrollArea, rect_end.position.y);
				rect_end.width = AutoScrollArea;
			}
			else
			{
				rect_start.position = new Vector2(rect_start.position.x, rect_start.position.y + rect_start.height - AutoScrollArea);
				rect_start.height = AutoScrollArea;

				rect_end.height = AutoScrollArea;
			}

			var new_direction = 0;
			if (rect_start.Contains(point))
			{
				new_direction = -1;
			}
			else if (rect_end.Contains(point))
			{
				new_direction = +1;
			}

			if (new_direction != AutoScrollDirection)
			{
				AutoScrollStop();

				if (new_direction != 0)
				{
					AutoScrollDirection = new_direction;
					AutoScrollEventData = eventData;
					AutoScrollCallback = callback;
					AutoScrollCoroutine = StartCoroutine(AutoScroll());
				}
			}

			return AutoScrollDirection != 0;
		}

		/// <summary>
		/// Auto scroll.
		/// </summary>
		/// <returns>Coroutine.</returns>
		protected abstract IEnumerator AutoScroll();
		#endregion

		/// <summary>
		/// Start ListView in editor mode.
		/// </summary>
		public virtual void InEditorStart()
		{
#if UNITY_EDITOR
			Init();
#endif
		}

		/// <summary>
		/// Stop ListView in editor mode.
		/// </summary>
		public virtual void InEditorStop()
		{
#if UNITY_EDITOR
			OnDestroy();
			isListViewCustomInited = false;
#endif
		}

		/// <summary>
		/// Restart ListView in editor mode.
		/// </summary>
		public virtual void InEditorRestart()
		{
#if UNITY_EDITOR
			InEditorStop();
			InEditorStart();
#endif
		}
	}
}