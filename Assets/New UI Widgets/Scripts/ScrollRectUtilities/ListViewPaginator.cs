namespace UIWidgets
{
	using System;
	using UnityEngine;

	/// <summary>
	/// ListView's paginator. Also works with TileView's.
	/// </summary>
	public class ListViewPaginator : ScrollRectPaginator
	{
		/// <summary>
		/// ListView.
		/// </summary>
		[SerializeField]
		protected ListViewBase ListView;

		/// <summary>
		/// Count of items on one page.
		/// </summary>
		[SerializeField]
		protected int perPage = 1;

		/// <summary>
		/// Gets or sets the count of items on one page.
		/// </summary>
		/// <value>The per page.</value>
		public int PerPage
		{
			get
			{
				return Mathf.Max(1, perPage);
			}

			set
			{
				perPage = Mathf.Max(1, value);
				RecalculatePages();
			}
		}

		bool isListViewPaginatorInited;

		bool animationRunning;

		Action animationEndedDelegate;

		Action animationMovementDelegate;

		/// <inheritdoc/>
		public override void Init()
		{
			if (isListViewPaginatorInited)
			{
				return;
			}

			isListViewPaginatorInited = true;

			if (ListView == null)
			{
				Debug.LogWarning("ListView is not specified.", this);
				return;
			}

			animationEndedDelegate = AnimationEnded;
			animationMovementDelegate = MovementInvoke;

			ListView.Init();
			ScrollRect = ListView.GetScrollRect();

			base.Init();
		}

		/// <inheritdoc/>
		protected override void MovementInvoke()
		{
			var scroll_position = GetCalculatedPosition();
			var v_pos = IsHorizontal()
				? new Vector2(scroll_position, 0f)
				: new Vector2(0f, scroll_position);
			var prev_page = Index2Page(ListView.GetNearestIndex(v_pos, NearestType.Before));
			var prev_page_position = Page2Position(prev_page);
			var page_size = Page2Position(prev_page + 1) - prev_page_position;

			var relative_position = scroll_position - prev_page_position; // because position not always started at 0
			var ratio = relative_position / page_size;

			OnMovement.Invoke(prev_page, ratio);
		}

		/// <inheritdoc/>
		protected override float GetCalculatedPosition()
		{
			return ListView.GetScrollPosition();
		}

		/// <inheritdoc/>
		protected override float GetLastPageMargin()
		{
			var items_per_block = ListView.GetItemsPerBlock();
			var items_per_page = items_per_block * PerPage;
			var items_at_last_page = ListView.GetItemsCount() % items_per_page;
			var unexisted_items = items_at_last_page == 0 ? 0 : items_per_page - items_at_last_page;
			var unexisted_blocks = Mathf.FloorToInt(unexisted_items / items_per_block);

			var size = ListView.GetDefaultItemSize();

			float margin;
			if (IsHorizontal())
			{
				margin = unexisted_blocks * size.x;
				if (unexisted_blocks > 0)
				{
					margin += (unexisted_blocks - 1) * Layout.Spacing.x;
				}
			}
			else
			{
				margin = unexisted_blocks * size.y;
				if (unexisted_blocks > 0)
				{
					margin += (unexisted_blocks - 1) * Layout.Spacing.y;
				}
			}

			return margin;
		}

		/// <inheritdoc/>
		protected override int GetPage()
		{
			var position = GetCalculatedPosition();

			var v_pos = IsHorizontal()
				? new Vector2(position, 0f)
				: new Vector2(0f, position);

			var index = ListView.GetNearestIndex(v_pos);

			return Index2Page(index);
		}

		/// <summary>
		/// Convert index to page.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <returns>Page.</returns>
		protected int Index2Page(int index)
		{
			var page = Mathf.RoundToInt(((float)index) / (ListView.GetItemsPerBlock() * PerPage));
			return Mathf.Min(page, Pages - 1);
		}

		/// <inheritdoc/>
		public override bool IsHorizontal()
		{
			return ListView.IsHorizontal();
		}

		/// <inheritdoc/>
		protected override void RecalculatePages()
		{
			SetScrollRectMaxDrag();

			var per_block = ListView.GetItemsPerBlock() * PerPage;

			Pages = (per_block == 0) ? 0 : Mathf.CeilToInt(((float)ListView.GetItemsCount()) / per_block);

			UpdateLastPageMargin();

			if (currentPage >= Pages)
			{
				GoToPage(Pages - 1);
			}
		}

		/// <inheritdoc/>
		public override float Page2Position(int page)
		{
			if (page == Pages)
			{
				page = ListView.LoopedListAvailable ? 0 : Pages - 1;
			}

			var index = page * ListView.GetItemsPerBlock() * PerPage;
			switch (ForcedPosition)
			{
				case PaginatorPagePosition.None:
				case PaginatorPagePosition.OnStart:
					return ListView.GetItemPosition(index);
				case PaginatorPagePosition.OnCenter:
					return ListView.GetItemPositionMiddle(index);
				case PaginatorPagePosition.OnEnd:
					return ListView.GetItemPositionBottom(index);
				default:
					throw new NotSupportedException(string.Format("Unknown forced position: {0}", EnumHelper<PaginatorPagePosition>.ToString(ForcedPosition)));
			}
		}

		/// <inheritdoc/>
		public override float GetPosition()
		{
			return ListView.GetScrollPosition();
		}

		/// <inheritdoc/>
		protected override void SetPosition(float position, bool isHorizontal)
		{
			ListView.ScrollToPosition(position);

			MovementInvoke();
		}

		/// <inheritdoc/>
		public override void SetPosition(float position)
		{
			SetPosition(position, IsHorizontal());
		}

		/// <inheritdoc/>
		protected override void StartAnimation(float target)
		{
			if (animationRunning)
			{
				return;
			}

			animationRunning = true;
			ListView.ScrollToPositionAnimated(target, animationEndedDelegate, animationMovementDelegate);
		}

		/// <inheritdoc/>
		protected override bool CanAnimate()
		{
			return base.CanAnimate() && ListView.gameObject.activeInHierarchy;
		}

		/// <summary>
		/// Animation ended.
		/// </summary>
		protected virtual void AnimationEnded()
		{
			animationRunning = false;
		}

		/// <inheritdoc/>
		protected override bool IsPrevAvailable(int page)
		{
			return (Pages == 0)
				? false
				: (page != 0) || ListView.LoopedListAvailable;
		}

		/// <inheritdoc/>
		protected override bool IsNextAvailable(int page)
		{
			return (Pages == 0)
				? false
				: (page != (Pages - 1)) || ListView.LoopedListAvailable;
		}

		/// <inheritdoc/>
		public override void Next()
		{
			if (CurrentPage == (Pages - 1))
			{
				CurrentPage = 0;
			}
			else
			{
				CurrentPage += 1;
			}
		}

		/// <inheritdoc/>
		public override void Prev()
		{
			if (CurrentPage == 0)
			{
				CurrentPage = Pages - 1;
			}
			else
			{
				CurrentPage -= 1;
			}
		}
	}
}