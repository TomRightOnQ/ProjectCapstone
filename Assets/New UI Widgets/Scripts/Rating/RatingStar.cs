namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Rating star.
	/// </summary>
	public class RatingStar : MonoBehaviour
	{
		/// <summary>
		/// Owner.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		public Rating Owner;

		/// <summary>
		/// Rating.
		/// </summary>
		public int Rating
		{
			get;
			set;
		}

		/// <summary>
		/// Interactable.
		/// </summary>
		public virtual bool Interactable
		{
			get
			{
				return Button.interactable;
			}

			set
			{
				Button.interactable = value;
			}
		}

		/// <summary>
		/// RectTransform.
		/// </summary>
		public RectTransform RectTransform
		{
			get;
			protected set;
		}

		/// <summary>
		/// Graphic.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("Graphic")]
		protected Graphic graphic;

		/// <summary>
		/// Graphic.
		/// </summary>
		public Graphic Graphic
		{
			get
			{
				return graphic;
			}
		}

		/// <summary>
		/// Button.
		/// </summary>
		[SerializeField]
		protected Button Button;

		bool isInited;

		/// <summary>
		/// Process the start event.
		/// </summary>
		protected virtual void Start()
		{
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

			RectTransform = transform as RectTransform;
			Button.onClick.AddListener(ProcessClick);
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected virtual void OnDestroy()
		{
			Button.onClick.RemoveListener(ProcessClick);
		}

		/// <summary>
		/// Process click.
		/// </summary>
		protected virtual void ProcessClick()
		{
			Owner.Value = Rating;
		}

		/// <summary>
		/// Update star color.
		/// </summary>
		/// <param name="color">Color.</param>
		public virtual void Coloring(Color color)
		{
			if (Graphic != null)
			{
				Graphic.color = color;
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Validate this instance.
		/// </summary>
		protected virtual void OnValidate()
		{
			if (graphic == null)
			{
				graphic = GetComponentInChildren<Graphic>();
			}

			if (Button == null)
			{
				Button = GetComponentInChildren<Button>();
			}
		}
#endif
	}
}