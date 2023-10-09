namespace UIWidgets
{
	using System.Collections.Generic;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Highlight accordion.
	/// </summary>
	[RequireComponent(typeof(Accordion))]
	public class AccordionHighlight : MonoBehaviour, IStylable
	{
		[SerializeField]
		StyleImage defaultToggleBackground;

		/// <summary>
		/// Default background.
		/// </summary>
		public StyleImage DefaultToggleBackground
		{
			get
			{
				return defaultToggleBackground;
			}

			set
			{
				defaultToggleBackground = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		[FormerlySerializedAs("defaultText")]
		StyleText defaultToggleText;

		/// <summary>
		/// Default text.
		/// </summary>
		public StyleText DefaultToggleText
		{
			get
			{
				return defaultToggleText;
			}

			set
			{
				defaultToggleText = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		StyleImage activeToggleBackground;

		/// <summary>
		/// Active background.
		/// </summary>
		public StyleImage ActiveToggleBackground
		{
			get
			{
				return activeToggleBackground;
			}

			set
			{
				activeToggleBackground = value;
				UpdateHighlights();
			}
		}

		[SerializeField]
		[FormerlySerializedAs("activeText")]
		StyleText activeToggleText;

		/// <summary>
		/// Active text.
		/// </summary>
		public StyleText ActiveToggleText
		{
			get
			{
				return activeToggleText;
			}

			set
			{
				activeToggleText = value;
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
				if (item.ToggleLabel != null)
				{
					UIThemes.Utilities.SetTargetOwner(typeof(Color), item.ToggleLabel.GetComponent<Graphic>(), nameof(Graphic.color), this);
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
				Accordion.OnToggleItem.RemoveListener(OnToggle);
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
			if (item.Open)
			{
				ActiveToggleBackground.ApplyTo(item.ToggleObject);
				if (item.ToggleLabel != null)
				{
					ActiveToggleText.ApplyTo(item.ToggleLabel.gameObject);
				}
			}
			else
			{
				DefaultToggleBackground.ApplyTo(item.ToggleObject);
				if (item.ToggleLabel != null)
				{
					DefaultToggleText.ApplyTo(item.ToggleLabel.gameObject);
				}
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

		/// <inheritdoc/>
		public virtual bool SetStyle(Style style)
		{
			defaultToggleBackground = style.Accordion.ToggleDefaultBackground;
			activeToggleBackground = style.Accordion.ToggleActiveBackground;
			defaultToggleText = style.Accordion.ToggleDefaultText;
			activeToggleText = style.Accordion.ToggleActiveText;

			UpdateHighlights();

			return true;
		}

		/// <inheritdoc/>
		public virtual bool GetStyle(Style style)
		{
			style.Accordion.ToggleDefaultBackground = defaultToggleBackground;
			style.Accordion.ToggleActiveBackground = activeToggleBackground;
			style.Accordion.ToggleDefaultText = defaultToggleText;
			style.Accordion.ToggleActiveText = activeToggleText;

			return true;
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