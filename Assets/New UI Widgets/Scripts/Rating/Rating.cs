namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Rating.
	/// </summary>
	public class Rating : UIBehaviour
	{
		/// <summary>
		/// Rating event.
		/// </summary>
		[Serializable]
		public class RatingEvent : UnityEvent<int>
		{
		}

		/// <summary>
		/// Color lerp mode.
		/// </summary>
		public enum ColorLerpMode
		{
			/// <summary>
			/// RGB lerp.
			/// </summary>
			RGB = 0,

			/// <summary>
			/// HSV lerp.
			/// Prevent dirty colors unlike RGB.
			/// </summary>
			HSV = 1,
		}

		#region Interactable
		[SerializeField]
		bool interactable = true;

		/// <summary>
		/// Is widget interactable.
		/// </summary>
		/// <value><c>true</c> if interactable; otherwise, <c>false</c>.</value>
		public bool Interactable
		{
			get
			{
				return interactable;
			}

			set
			{
				if (interactable != value)
				{
					interactable = value;
					InteractableChanged();
				}
			}
		}

		/// <summary>
		/// If the canvas groups allow interaction.
		/// </summary>
		protected bool GroupsAllowInteraction = true;

		/// <summary>
		/// The CanvasGroup cache.
		/// </summary>
		protected List<CanvasGroup> CanvasGroupCache = new List<CanvasGroup>();

		/// <summary>
		/// Process the CanvasGroupChanged event.
		/// </summary>
		protected override void OnCanvasGroupChanged()
		{
			var groupAllowInteraction = true;
			var t = transform;
			while (t != null)
			{
				t.GetComponents(CanvasGroupCache);
				var shouldBreak = false;
				foreach (var canvas_group in CanvasGroupCache)
				{
					if (!canvas_group.interactable)
					{
						groupAllowInteraction = false;
						shouldBreak = true;
					}

					shouldBreak |= canvas_group.ignoreParentGroups;
				}

				if (shouldBreak)
				{
					break;
				}

				t = t.parent;
			}

			if (groupAllowInteraction != GroupsAllowInteraction)
			{
				GroupsAllowInteraction = groupAllowInteraction;
				InteractableChanged();
			}
		}

		/// <summary>
		/// Returns true if the GameObject and the Component are active.
		/// </summary>
		/// <returns>true if the GameObject and the Component are active; otherwise false.</returns>
		public override bool IsActive()
		{
			return base.IsActive() && GroupsAllowInteraction && Interactable;
		}

		/// <summary>
		/// Process interactable change.
		/// </summary>
		protected virtual void InteractableChanged()
		{
			if (!base.IsActive())
			{
				return;
			}

			OnInteractableChange(GroupsAllowInteraction && Interactable);
		}

		/// <summary>
		/// Process interactable change.
		/// </summary>
		/// <param name="interactableState">Current interactable state.</param>
		protected virtual void OnInteractableChange(bool interactableState)
		{
			foreach (var s in StarsPoolEmpty)
			{
				s.Interactable = interactableState;
			}

			foreach (var s in StarsPoolFull)
			{
				s.Interactable = interactableState;
			}
		}
		#endregion

		[SerializeField]
		int value = 1;

		/// <summary>
		/// Value.
		/// </summary>
		[DataBindField]
		public int Value
		{
			get
			{
				return value;
			}

			set
			{
				var v = Mathf.Clamp(value, 0, valueMax);
				if (v != this.value)
				{
					this.value = v;
					ValueChanged();
				}
			}
		}

		[SerializeField]
		int valueMax = 5;

		/// <summary>
		/// Maximum value.
		/// </summary>
		public int ValueMax
		{
			get
			{
				return valueMax;
			}

			set
			{
				valueMax = value > 2 ? value : 2;
				Value = this.value;
			}
		}

		/// <summary>
		/// Empty star.
		/// </summary>
		[SerializeField]
		protected RatingStar StarEmpty;

		/// <summary>
		/// Full star.
		/// </summary>
		[SerializeField]
		protected RatingStar StarFull;

		[Header("Colors")]
		[SerializeField]
		Color colorMin = Color.red;

		/// <summary>
		/// Color when Value = 1.
		/// </summary>
		public Color ColorMin
		{
			get
			{
				return colorMin;
			}

			set
			{
				if (colorMin != value)
				{
					colorMin = value;
					Coloring();
				}
			}
		}

		[SerializeField]
		Color colorMax = Color.green;

		/// <summary>
		/// Color when Value = ValueMax.
		/// </summary>
		public Color ColorMax
		{
			get
			{
				return colorMax;
			}

			set
			{
				if (colorMax != value)
				{
					colorMax = value;
					Coloring();
				}
			}
		}

		[SerializeField]
		ColorLerpMode lerpMode = ColorLerpMode.HSV;

		/// <summary>
		/// Color lerp mode.
		/// </summary>
		public ColorLerpMode LerpMode
		{
			get
			{
				return lerpMode;
			}

			set
			{
				if (lerpMode != value)
				{
					lerpMode = value;
					Coloring();
				}
			}
		}

		Func<int, Color> value2Color;

		/// <summary>
		/// Convert value to color.
		/// </summary>
		public Func<int, Color> Value2Color
		{
			get
			{
				if (value2Color == null)
				{
					value2Color = DefaultValue2Color;
				}

				return value2Color;
			}

			set
			{
				if (value2Color == value)
				{
					return;
				}

				value2Color = value;

				if (value2Color != null)
				{
					Coloring();
				}
			}
		}

		/// <summary>
		/// Value changed event.
		/// </summary>
		[SerializeField]
		[DataBindEvent("Value")]
		public RatingEvent OnChange = new RatingEvent();

		/// <summary>
		/// Default converter for value to color.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Color.</returns>
		public Color DefaultValue2Color(int value)
		{
			var t = (float)Mathf.Max(value - 1, 0) / (ValueMax - 1);
			return LerpMode == ColorLerpMode.RGB
				? Color.Lerp(ColorMin, ColorMax, t)
				: ColorHSV.Lerp(new ColorHSV(ColorMin), new ColorHSV(ColorMax), t);
		}

		[NonSerialized]
		ListComponentPool<RatingStar> starsPoolEmpty;

		/// <summary>
		/// Pool for empty stars.
		/// </summary>
		protected ListComponentPool<RatingStar> StarsPoolEmpty
		{
			get
			{
				if ((starsPoolEmpty == null) || (starsPoolEmpty.Template == null))
				{
					starsPoolEmpty = new ListComponentPool<RatingStar>(StarEmpty, StarsPoolEmptyInstances, StarsPoolEmptyCache, transform as RectTransform);
				}

				return starsPoolEmpty;
			}
		}

		/// <summary>
		/// Cache for StarsPoolEmpty.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<RatingStar> StarsPoolEmptyCache = new List<RatingStar>();

		/// <summary>
		/// Instances for StarsPoolEmpty.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<RatingStar> StarsPoolEmptyInstances = new List<RatingStar>();

		[NonSerialized]
		ListComponentPool<RatingStar> starsPoolFull;

		/// <summary>
		/// Pool for full stars.
		/// </summary>
		protected ListComponentPool<RatingStar> StarsPoolFull
		{
			get
			{
				if ((starsPoolFull == null) || (starsPoolFull.Template == null))
				{
					starsPoolFull = new ListComponentPool<RatingStar>(StarFull, StarsPoolFullCache, StarsPoolFullActive, transform as RectTransform);
				}

				return starsPoolFull;
			}
		}

		/// <summary>
		/// Cache for StarsPoolFull.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<RatingStar> StarsPoolFullCache = new List<RatingStar>();

		/// <summary>
		/// Instances for StarsPoolFull.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<RatingStar> StarsPoolFullActive = new List<RatingStar>();

		bool isInited;

		/// <summary>
		/// Process the start event.
		/// </summary>
		protected override void Start()
		{
			base.Start();
			Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public virtual void Init()
		{
			if (isInited)
			{
				return;
			}

			isInited = true;

			foreach (var star in StarsPoolEmpty.GetEnumerator(PoolEnumeratorMode.All))
			{
				OnStarCreate(star);
			}

			StarsPoolEmpty.OnCreate = OnStarCreate;
			StarsPoolEmpty.OnDestroy = OnStarDestoy;

			foreach (var star in StarsPoolFull.GetEnumerator(PoolEnumeratorMode.All))
			{
				OnStarCreate(star);
			}

			StarsPoolFull.OnCreate = OnStarCreate;
			StarsPoolFull.OnDestroy = OnStarDestoy;

			UpdateStars();
			Coloring();
			InteractableChanged();
		}

		/// <summary>
		/// Process created star.
		/// </summary>
		/// <param name="star">Star.</param>
		protected virtual void OnStarCreate(RatingStar star)
		{
			star.Init();
			star.Owner = this;
			UIThemes.Utilities.SetTargetOwner(typeof(Color), star.Graphic, nameof(star.Graphic.color), this);
		}

		/// <summary>
		/// Process destroyed star.
		/// </summary>
		/// <param name="star">Star.</param>
		protected virtual void OnStarDestoy(RatingStar star)
		{
			star.Owner = null;
			UIThemes.Utilities.SetTargetOwner(typeof(Color), star.Graphic, nameof(star.Graphic.color), null);
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected override void OnDestroy()
		{
			StarsPoolEmpty.Clear();
			StarsPoolEmpty.OnCreate = null;
			StarsPoolEmpty.OnDestroy = null;
			starsPoolEmpty = null;

			StarsPoolFull.Clear();
			StarsPoolFull.OnCreate = null;
			StarsPoolFull.OnDestroy = null;
			starsPoolFull = null;

			value2Color = null;

			base.OnDestroy();
		}

		/// <summary>
		/// Process value changes.
		/// </summary>
		protected virtual void ValueChanged()
		{
			UpdateStars();
			Coloring();
			OnChange.Invoke(Value);
		}

		/// <summary>
		/// Update stars instances.
		/// </summary>
		protected virtual void UpdateStars()
		{
			StarsPoolFull.Require(Value);
			for (var i = 0; i < StarsPoolFull.Count; i++)
			{
				var s = StarsPoolFull[i];
				s.Rating = i + 1;
				s.RectTransform.SetAsLastSibling();
			}

			StarsPoolEmpty.Require(ValueMax - Value);
			for (var i = 0; i < StarsPoolEmpty.Count; i++)
			{
				var s = StarsPoolEmpty[i];
				s.Rating = Value + i + 1;
				s.RectTransform.SetAsLastSibling();
			}
		}

		/// <summary>
		/// Update stars color.
		/// </summary>
		protected virtual void Coloring()
		{
			var color = Value2Color(Value);
			for (int i = 0; i < StarsPoolFull.Count; i++)
			{
				StarsPoolFull[i].Coloring(color);
			}
		}

#if UNITY_EDITOR

		/// <summary>
		/// Validate this instance.
		/// </summary>
		protected override void OnValidate()
		{
			if (valueMax < 2)
			{
				valueMax = 2;
			}

			value = Mathf.Clamp(value, 0, valueMax);
		}
#endif
	}
}