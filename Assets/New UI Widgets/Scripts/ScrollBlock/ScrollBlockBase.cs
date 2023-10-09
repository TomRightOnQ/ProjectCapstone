namespace UIWidgets
{
	using System;
	using System.Collections;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// Base class for the ScrollBlock.
	/// </summary>
	[RequireComponent(typeof(EasyLayoutNS.EasyLayout))]
	public abstract class ScrollBlockBase : UIBehaviourConditional, IStylable, ILateUpdatable,
		IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IScrollHandler
	{
		/// <summary>
		/// Position.
		/// </summary>
		public enum Position
		{
			/// <summary>
			/// Start.
			/// </summary>
			Start = 0,

			/// <summary>
			/// Center.
			/// </summary>
			Center = 1,
		}

		/// <summary>
		/// Do nothing.
		/// </summary>
		public static void DoNothing()
		{
		}

		/// <summary>
		/// Default function to get value.
		/// </summary>
		/// <param name="step">Step.</param>
		/// <returns>Value.</returns>
		public static string DefaultValue(int step)
		{
			return string.Format("Index: {0}", step.ToString());
		}

		/// <summary>
		/// Default function to check is interactable.
		/// </summary>
		/// <returns>true.</returns>
		protected static bool DefaultInteractable()
		{
			return true;
		}

		/// <summary>
		/// Default function to check is action allowed.
		/// </summary>
		/// <returns>true.</returns>
		protected static bool DefaultAllow()
		{
			return true;
		}

		/// <summary>
		/// Action to increase the value.
		/// </summary>
		public Action Increase = DoNothing;

		/// <summary>
		/// Action to decrease the value.
		/// </summary>
		public Action Decrease = DoNothing;

		/// <summary>
		/// Function to check is increase allowed.
		/// </summary>
		public Func<bool> AllowIncrease = DefaultAllow;

		/// <summary>
		/// Function to check is decrease allowed.
		/// </summary>
		public Func<bool> AllowDecrease = DefaultAllow;

		/// <summary>
		/// Convert index to the displayed string.
		/// </summary>
		public Func<int, string> Value = DefaultValue;

		/// <summary>
		/// Is interactable?
		/// </summary>
		public Func<bool> IsInteractable = DefaultInteractable;

		/// <summary>
		/// Size of the DefaultItem.
		/// </summary>
		public Vector2 DefaultItemSize
		{
			get;
			protected set;
		}

		/// <summary>
		/// Base position.
		/// </summary>
		[SerializeField]
		protected Position basePosition = Position.Center;

		/// <summary>
		/// Base position.
		/// </summary>
		public Position BasePosition
		{
			get
			{
				return basePosition;
			}
		}

		/// <summary>
		/// Layout.
		/// </summary>
		[NonSerialized]
		protected EasyLayoutBridge Layout;

		/// <summary>
		/// Count of the visible items.
		/// </summary>
		public abstract int Count
		{
			get;
		}

		/// <summary>
		/// Is horizontal scroll?
		/// </summary>
		[SerializeField]
		protected bool IsHorizontal;

		/// <summary>
		/// Drag sensitivity.
		/// </summary>
		[SerializeField]
		public float DragSensitivity = 0.5f;

		/// <summary>
		/// Scroll sensitivity.
		/// </summary>
		[SerializeField]
		public float ScrollSensitivity = 15f;

		/// <summary>
		/// Layout internal padding.
		/// </summary>
		public float Padding
		{
			get
			{
				return Layout.GetFiller().x;
			}

			set
			{
				var padding = ClampPadding(value);
				Layout.SetFiller(padding, 0f);
			}
		}

		/// <summary>
		/// Animate inertia scroll with unscaled time.
		/// </summary>
		[SerializeField]
		public bool UnscaledTime = true;

		/// <summary>
		/// Auto-scroll to center after scroll/drag.
		/// </summary>
		[SerializeField]
		public bool AlwaysCenter = true;

		/// <summary>
		/// Time to stop.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("AlwaysCenter")]
		public float TimeToStop = 0.5f;

		/// <summary>
		/// Velocity.
		/// </summary>
		[NonSerialized]
		protected float ScrollVelocity;

		/// <summary>
		/// Inertia velocity.
		/// </summary>
		[NonSerialized]
		protected float IntertiaVelocity;

		/// <summary>
		/// Current deceleration rate.
		/// </summary>
		[NonSerialized]
		protected float CurrentDecelerationRate;

		/// <summary>
		/// Inertia distance.
		/// </summary>
		[NonSerialized]
		protected float InertiaDistance;

		/// <summary>
		/// Is drag event occurring?
		/// </summary>
		[NonSerialized]
		protected bool IsDragging;

		/// <summary>
		/// Is scrolling occurring?
		/// </summary>
		[NonSerialized]
		protected bool IsScrolling;

		/// <summary>
		/// Previous scroll value.
		/// </summary>
		[NonSerialized]
		protected float PrevScrollValue;

		/// <summary>
		/// Current scroll value.
		/// </summary>
		[NonSerialized]
		protected float CurrentScrollValue;

		RectTransform rectTransform;

		/// <summary>
		/// Current RectTransformn.
		/// </summary>
		protected RectTransform RectTransform
		{
			get
			{
				if (rectTransform == null)
				{
					rectTransform = transform as RectTransform;
				}

				return rectTransform;
			}
		}

		/// <summary>
		/// Base index for the components.
		/// </summary>
		protected abstract int ComponentsBaseIndex
		{
			get;
		}

		/// <summary>
		/// Distance to center.
		/// </summary>
		[Obsolete("Renamed to DistanceToBase.")]
		public float DistanceToCenter
		{
			get
			{
				return DistanceToBase;
			}
		}

		/// <summary>
		/// Distance to base position.
		/// </summary>
		public float DistanceToBase
		{
			get
			{
				return GetBasePosition() - Padding;
			}
		}

		/// <summary>
		/// Is inited?
		/// </summary>
		protected bool IsInited
		{
			get;
			private set;
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			if (IsInited)
			{
				return;
			}

			IsInited = true;

			UpdateLayout();

			var resizer = Utilities.RequireComponent<ResizeListener>(this);
			resizer.OnResizeNextFrame.AddListener(Resize);

			Resize();

			AlignComponents();
		}

		/// <summary>
		/// Clamp padding value.
		/// </summary>
		/// <param name="padding">Padding.</param>
		/// <returns>Clamped value.</returns>
		protected float ClampPadding(float padding)
		{
			var current = Padding;
			var delta = current - padding;
			if (delta >= 0)
			{
				return ProcessIncrease(current, delta);
			}
			else
			{
				return ProcessDecrease(current, delta);
			}
		}

		/// <summary>
		/// Process padding decrease.
		/// </summary>
		/// <param name="current">Current padding.</param>
		/// <param name="delta">Padding change.</param>
		/// <returns>Decreased padding.</returns>
		protected float ProcessDecrease(float current, float delta)
		{
			var sign = Mathf.Sign(delta);
			var abs = Mathf.Abs(delta);
			var steps = Mathf.CeilToInt(abs);

			var size = ItemFullSize();
			var base_position = GetBasePosition();
			var update_view = false;

			var padding = current;
			var last = steps - 1;
			for (var i = 0; i < steps; i++)
			{
				var step = (i == last) ? (last * sign) - delta : -sign;
				padding += step;

				var distance = padding - base_position;
				if ((distance > 0) && !AllowDecrease())
				{
					padding = base_position;
					break;
				}

				var decrease = Position2Count(distance / size, false);
				for (var j = 0; j < decrease; j++)
				{
					if (!AllowDecrease())
					{
						break;
					}

					Decrease();
					padding -= size;
					update_view = true;
				}
			}

			if (update_view)
			{
				UpdateView();
			}

			return padding;
		}

		/// <summary>
		/// Process padding increased.
		/// </summary>
		/// <param name="current">Current padding.</param>
		/// <param name="delta">Padding change.</param>
		/// <returns>Increased padding.</returns>
		protected float ProcessIncrease(float current, float delta)
		{
			var sign = Mathf.Sign(delta);
			var abs = Mathf.Abs(delta);
			var steps = Mathf.CeilToInt(abs);

			var size = ItemFullSize();
			var base_position = GetBasePosition();
			var update_view = false;

			var padding = current;
			var last = steps - 1;
			for (var i = 0; i < steps; i++)
			{
				padding += (i == last) ? (last * sign) - delta : -sign;

				var distance = base_position - padding;
				if ((distance > 0) && !AllowIncrease())
				{
					padding = base_position;
					break;
				}

				var increase = Position2Count(distance / size, true);
				for (var j = 0; j < increase; j++)
				{
					if (!AllowIncrease())
					{
						break;
					}

					Increase();
					padding += size;
					update_view = true;
				}
			}

			if (update_view)
			{
				UpdateView();
			}

			return padding;
		}

		/// <summary>
		/// Scroll on specified amount of steps.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <param name="curve">Animation curve.</param>
		public void Scroll(int steps, AnimationCurve curve = null)
		{
			StartCoroutine(ScrollAnimation(steps, curve));
		}

		/// <summary>
		/// Scroll animation on specified amount of steps.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <param name="curve">Animation curve.</param>
		/// <returns>Animation coroutine.</returns>
		public IEnumerator ScrollAnimation(int steps, AnimationCurve curve = null)
		{
			if (curve == null)
			{
				curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			}

			var direction = steps > 0 ? -1 : 1;
			var total = steps * ItemFullSize() * direction;
			var prev = 0f;

			var time = 0f;
			var duration = curve[curve.length - 1].time;

			while (time < duration)
			{
				var current = Mathf.Lerp(0f, total, time / duration);
				Padding += current - prev;
				prev = current;
				yield return null;

				time += UtilitiesTime.GetDeltaTime(UnscaledTime);
			}

			Padding += total - prev;
		}

		/// <summary>
		/// Position to items count.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="increase">true if increase; otherwise decrease.</param>
		/// <returns>Items count.</returns>
		protected int Position2Count(float position, bool increase)
		{
			switch (BasePosition)
			{
				case Position.Start:
					return increase ? Mathf.FloorToInt(position) : Mathf.CeilToInt(position);
				case Position.Center:
					return Mathf.RoundToInt(position);
				default:
					return 0;
			}
		}

		/// <summary>
		/// Update the layout.
		/// </summary>
		protected abstract void UpdateLayout();

		/// <summary>
		/// Container size.
		/// </summary>
		/// <returns>Size.</returns>
		protected float ContainerSize()
		{
			return (IsHorizontal ? RectTransform.rect.width : RectTransform.rect.height) - Layout.GetFullMargin();
		}

		/// <summary>
		/// Content size.
		/// </summary>
		/// <returns>Size.</returns>
		protected float ContentSize()
		{
			return (ItemFullSize() * Count) - Layout.GetSpacing();
		}

		/// <summary>
		/// Item size.
		/// </summary>
		/// <returns>Size.</returns>
		protected float ItemSize()
		{
			return IsHorizontal ? DefaultItemSize.x : DefaultItemSize.y;
		}

		/// <summary>
		/// Item size with spacing.
		/// </summary>
		/// <returns>Size.</returns>
		protected float ItemFullSize()
		{
			return ItemSize() + Layout.GetSpacing();
		}

		/// <summary>
		/// Calculate the maximum count of the visible components.
		/// </summary>
		/// <returns>Maximum count of the visible components.</returns>
		protected int CalculateMax()
		{
			var result = Mathf.CeilToInt((ContainerSize() + Layout.GetSpacing()) / ItemFullSize()) + 1;
			if (result < 0)
			{
				result = 0;
			}

			if ((result % 2) == 0)
			{
				result += 1;
			}

			return result;
		}

		/// <summary>
		/// Is component is null?
		/// </summary>
		/// <param name="component">Component.</param>
		/// <returns>true if component is null; otherwise, false.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed.")]
		protected virtual bool IsNullComponent(ScrollBlockItem component)
		{
			return component == null;
		}

		/// <summary>
		/// Process RectTransform resize.
		/// </summary>
		protected abstract void Resize();

		/// <summary>
		/// Update view.
		/// </summary>
		public abstract void UpdateView();

		/// <summary>
		/// Returns true if the GameObject and the Component are active.
		/// </summary>
		/// <returns>true if the GameObject and the Component are active; otherwise false.</returns>
		public override bool IsActive()
		{
			return base.IsActive() && IsInited && IsInteractable();
		}

		/// <summary>
		/// Process the begin drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			if (!IsActive())
			{
				return;
			}

			IsDragging = true;

			PrevScrollValue = Padding;
			CurrentScrollValue = Padding;

			StopInertia();
		}

		/// <summary>
		/// Process the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!IsDragging)
			{
				return;
			}

			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			if (!IsActive())
			{
				return;
			}

			StopInertia();
			var drag_delta = IsHorizontal ? eventData.delta.x : -eventData.delta.y;
			Scroll(drag_delta * DragSensitivity);
		}

		/// <summary>
		/// Process scroll event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnScroll(PointerEventData eventData)
		{
			if (!IsActive())
			{
				return;
			}

			IsScrolling = true;
			var scroll_delta = IsHorizontal ? eventData.scrollDelta.x : -eventData.scrollDelta.y;
			Scroll(scroll_delta * ScrollSensitivity);
		}

		/// <summary>
		/// Scroll.
		/// </summary>
		/// <param name="delta">Delta.</param>
		protected virtual void Scroll(float delta)
		{
			Padding += delta;

			CurrentScrollValue += delta;
			var time_delta = UtilitiesTime.DefaultGetDeltaTime(UnscaledTime);
			var new_velocity = (PrevScrollValue - CurrentScrollValue) / time_delta;
			ScrollVelocity = Mathf.Lerp(ScrollVelocity, new_velocity, time_delta * 10);
			PrevScrollValue = CurrentScrollValue;
		}

		/// <summary>
		/// Process the end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!IsDragging)
			{
				return;
			}

			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}

			IsDragging = false;
			InitIntertia();
		}

		/// <summary>
		/// Init inertia.
		/// </summary>
		protected virtual void InitIntertia()
		{
			IntertiaVelocity = -ScrollVelocity;
			CurrentDecelerationRate = -IntertiaVelocity / TimeToStop;

			var direction = Mathf.Sign(IntertiaVelocity);
			var time_to_stop_sq = Mathf.Pow(TimeToStop, 2f);
			var distance = (-Mathf.Abs(CurrentDecelerationRate) * time_to_stop_sq / 2f) + (Mathf.Abs(IntertiaVelocity) * TimeToStop);
			InertiaDistance = ClampDistance(distance, direction);
			IntertiaVelocity = (InertiaDistance + (Mathf.Abs(CurrentDecelerationRate) * time_to_stop_sq / 2f)) / TimeToStop;
			IntertiaVelocity *= direction;
		}

		/// <summary>
		/// Process the enable event.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			Updater.AddLateUpdate(this);
		}

		/// <summary>
		/// Process the disable event.
		/// </summary>
		protected override void OnDisable()
		{
			base.OnDisable();

			Updater.RemoveLateUpdate(this);
		}

		/// <summary>
		/// Late update.
		/// </summary>
		public virtual void RunLateUpdate()
		{
			if (IsScrolling)
			{
				IsScrolling = false;
				InitIntertia();
			}
			else if (!IsDragging && (Mathf.Abs(InertiaDistance) >= 0.25f) && AlwaysCenter)
			{
				var delta = UtilitiesTime.DefaultGetDeltaTime(UnscaledTime);
				var distance = IntertiaVelocity > 0f
					? Mathf.Min(InertiaDistance, IntertiaVelocity * delta)
					: Mathf.Max(-InertiaDistance, IntertiaVelocity * delta);

				Padding += distance;
				InertiaDistance -= Mathf.Abs(distance) * Mathf.Sign(InertiaDistance);

				if (Mathf.Abs(InertiaDistance) >= 0.25f)
				{
					IntertiaVelocity += CurrentDecelerationRate * delta;
					ScrollVelocity = -IntertiaVelocity;
				}
				else
				{
					StopInertia();
					Padding = GetBasePosition();
				}
			}
		}

		/// <summary>
		/// Stop inertia.
		/// </summary>
		protected void StopInertia()
		{
			CurrentDecelerationRate = 0f;
			InertiaDistance = 0f;
		}

		/// <summary>
		/// Clamp distance to stop right at value.
		/// </summary>
		/// <param name="distance">Distance.</param>
		/// <param name="direction">Scroll direction.</param>
		/// <returns>Clamped distance.</returns>
		protected float ClampDistance(float distance, float direction)
		{
			var extra = (GetBasePosition() - Padding) * direction;
			var steps = Mathf.Round((Mathf.Abs(distance) - extra) / ItemFullSize());
			var new_distance = (steps * ItemFullSize()) + extra;
			return new_distance;
		}

		/// <summary>
		/// Get zero position.
		/// </summary>
		/// <returns>Position.</returns>
		protected float GetBasePosition()
		{
			switch (BasePosition)
			{
				case Position.Start:
					return 0f;
				case Position.Center:
					return -(ContentSize() - ContainerSize()) / 2f;
				default:
					return 0f;
			}
		}

		/// <summary>
		/// Align components.
		/// </summary>
		protected void AlignComponents()
		{
			Padding = GetBasePosition();
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected override void OnDestroy()
		{
			var resizer = GetComponent<ResizeListener>();
			if (resizer != null)
			{
				resizer.OnResizeNextFrame.RemoveListener(Resize);
			}
		}

		/// <summary>
		/// Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
		}

		#region IStylable implementation

		/// <inheritdoc/>
		public abstract bool SetStyle(Style style);

		/// <inheritdoc/>
		public abstract bool GetStyle(Style style);
		#endregion
	}
}