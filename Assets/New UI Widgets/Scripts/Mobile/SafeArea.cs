namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// Safe area.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class SafeArea : UIBehaviour, IUpdatable
	{
		struct ScreenState
		{
			public readonly Rect SafeArea;

			public readonly Resolution Resolution;

			public readonly ScreenOrientation Orientation;

			private ScreenState(Rect safeArea, Resolution resolution, ScreenOrientation orientation)
			{
				SafeArea = safeArea;

				Resolution = resolution;
				Orientation = orientation;
			}

			public static ScreenState Current => new ScreenState(Screen.safeArea, Screen.currentResolution, Screen.orientation);

			/// <summary>
			/// Determines whether the specified object is equal to the current object.
			/// </summary>
			/// <param name="obj">The object to compare with the current object.</param>
			/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
			public override bool Equals(object obj)
			{
				if (obj is ScreenState)
				{
					return Equals((ScreenState)obj);
				}

				return false;
			}

			bool SameResolution(Resolution resolution)
			{
				return Resolution.width == resolution.width && Resolution.height == resolution.height;
			}

			/// <summary>
			/// Determines whether the specified object is equal to the current object.
			/// </summary>
			/// <param name="other">The object to compare with the current object.</param>
			/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
			public bool Equals(ScreenState other)
			{
				return SafeArea == other.SafeArea && SameResolution(other.Resolution) && Orientation == other.Orientation;
			}

			/// <summary>
			/// Hash function.
			/// </summary>
			/// <returns>A hash code for the current object.</returns>
			public override int GetHashCode()
			{
				return SafeArea.GetHashCode() ^ Resolution.width ^ Resolution.height ^ (int)Orientation;
			}

			/// <summary>
			/// Compare specified instances.
			/// </summary>
			/// <param name="left">Left instance.</param>
			/// <param name="right">Right instances.</param>
			/// <returns>true if the instances are equal; otherwise, false.</returns>
			public static bool operator ==(ScreenState left, ScreenState right)
			{
				return left.Equals(right);
			}

			/// <summary>
			/// Compare specified instances.
			/// </summary>
			/// <param name="left">Left instance.</param>
			/// <param name="right">Right instances.</param>
			/// <returns>true if the instances are now equal; otherwise, false.</returns>
			public static bool operator !=(ScreenState left, ScreenState right)
			{
				return !left.Equals(right);
			}
		}

		RectTransform rectTransform;

		Canvas canvas;

		ScreenState prevState;

		/// <summary>
		/// Process the start event.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			rectTransform = GetComponent<RectTransform>();
			canvas = GetComponentInParent<Canvas>();

			prevState = ScreenState.Current;
			ChangeScreenState();
		}

		/// <summary>
		/// Process the enable event.
		/// </summary>
		protected override void OnEnable()
		{
			Updater.Add(this);
			base.OnEnable();
		}

		/// <summary>
		/// Process the disable event.
		/// </summary>
		protected override void OnDisable()
		{
			Updater.Remove(this);
			base.OnDisable();
		}

		/// <summary>
		/// Run update.
		/// </summary>
		public void RunUpdate()
		{
			CheckScreenState();
		}

		/// <summary>
		/// Process the resize event.
		/// </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();

			CheckScreenState();
		}

		void CheckScreenState()
		{
			var current = ScreenState.Current;
			if (current != prevState && canvas != null)
			{
				prevState = current;
				Updater.RunOnceNextFrame(ChangeScreenState);
			}
		}

		void ChangeScreenState()
		{
			var scale = canvas.scaleFactor;
			rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Mathf.Round(prevState.SafeArea.xMin / scale), Mathf.Round(prevState.SafeArea.width / scale));
			rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Mathf.Round(prevState.SafeArea.yMin / scale), Mathf.Round(prevState.SafeArea.height / scale));
		}
	}
}