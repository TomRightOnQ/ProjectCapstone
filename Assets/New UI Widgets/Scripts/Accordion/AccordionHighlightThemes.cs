namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Accordion highlight to use with themes.
	/// </summary>
	[RequireComponent(typeof(Accordion))]
	public class AccordionHighlightThemes : MonoBehaviour
	{
		[SerializeField]
		Color defaultBackgroundColor;

		/// <summary>
		/// Default background color.
		/// </summary>
		public Color DefaultBackgroundColor
		{
			get
			{
				return defaultBackgroundColor;
			}

			set
			{
				defaultBackgroundColor = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		Sprite defaultBackgroundSprite;

		/// <summary>
		/// Default background sprite.
		/// </summary>
		public Sprite DefaultBackgroundSprite
		{
			get
			{
				return defaultBackgroundSprite;
			}

			set
			{
				defaultBackgroundSprite = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		Color defaultLabelColor;

		/// <summary>
		/// Default label color.
		/// </summary>
		public Color DefaultLabelColor
		{
			get
			{
				return defaultLabelColor;
			}

			set
			{
				defaultLabelColor = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		Color activeBackgroundColor;

		/// <summary>
		/// Active background color.
		/// </summary>
		public Color ActiveBackgroundColor
		{
			get
			{
				return activeBackgroundColor;
			}

			set
			{
				activeBackgroundColor = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		Sprite activeBackgroundSprite;

		/// <summary>
		/// Active background color.
		/// </summary>
		public Sprite ActiveBackgroundSprite
		{
			get
			{
				return activeBackgroundSprite;
			}

			set
			{
				activeBackgroundSprite = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		Color activeLabelColor;

		/// <summary>
		/// Active text color.
		/// </summary>
		public Color ActiveLabelColor
		{
			get
			{
				return activeLabelColor;
			}

			set
			{
				activeLabelColor = value;
				UpdateHighlights();
			}
		}

		Accordion accordion;

		/// <summary>
		/// Accordion.
		/// </summary>
		protected Accordion Accordion
		{
			get
			{
				if (accordion == null)
				{
					accordion = GetComponent<Accordion>();
				}

				return accordion;
			}
		}

		bool isInited;

		/// <summary>
		/// Process the start event.
		/// </summary>
		protected virtual void Start()
		{
			Init();
		}

		/// <summary>
		/// Init this instances.
		/// </summary>
		public virtual void Init()
		{
			if (isInited)
			{
				return;
			}

			isInited = true;

			SetTargetOwner();

			Accordion.OnStartToggleAnimation.AddListener(OnToggle);
			Accordion.OnToggleItem.AddListener(OnToggle);
			Accordion.OnDataSourceChanged.AddListener(UpdateHighlights);

			UpdateHighlights();
		}

		/// <summary>
		/// Set theme target owner.
		/// </summary>
		protected virtual void SetTargetOwner()
		{
			foreach (var item in Accordion)
			{
				UIThemes.Utilities.SetTargetOwner(typeof(Color), item.ToggleObject.GetComponent<Graphic>(), nameof(Graphic.color), this);
				if (item.ToggleLabel == null)
				{
					item.ToggleLabel = item.ToggleObject.GetComponentInChildren<TextAdapter>();
				}

				if (item.ToggleLabel != null)
				{
					UIThemes.Utilities.SetTargetOwner(typeof(Color), item.ToggleLabel.Graphic, nameof(Graphic.color), this);
				}
			}
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (accordion != null)
			{
				accordion.OnStartToggleAnimation.RemoveListener(OnToggle);
				accordion.OnToggleItem.RemoveListener(OnToggle);
				accordion.OnDataSourceChanged.RemoveListener(UpdateHighlights);
			}
		}

		/// <summary>
		/// Process the toggle event.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void OnToggle(AccordionItem item)
		{
			UpdateHighlight(item);
		}

		/// <summary>
		/// Update item highlight.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void UpdateHighlight(AccordionItem item)
		{
			var graphic = item.ToggleObject.GetComponent<Graphic>();
			graphic.color = item.Open ? ActiveBackgroundColor : DefaultBackgroundColor;
			if (graphic is Image img)
			{
				img.sprite = item.Open ? ActiveBackgroundSprite : DefaultBackgroundSprite;
			}

			if (item.ToggleLabel == null)
			{
				item.ToggleLabel = item.ToggleObject.GetComponentInChildren<TextAdapter>();
			}

			if (item.ToggleLabel != null)
			{
				item.ToggleLabel.Graphic.color = item.Open ? ActiveLabelColor : DefaultLabelColor;
			}
		}

		/// <summary>
		/// Update highlights of all opened items.
		/// </summary>
		public virtual void UpdateHighlights()
		{
			foreach (var item in Accordion)
			{
				UpdateHighlight(item);
			}
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Process the validate event.
		/// </summary>
		protected virtual void OnValidate()
		{
			SetTargetOwner();
		}
		#endif
	}
}