namespace UIWidgets
{
	using UIWidgets.l10n;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Tab button.
	/// </summary>
	public abstract class TabButtonBase : Button, ILocalizationSupport
	{
		[SerializeField]
		[Tooltip("If enabled translates button name using Localization.GetTranslation().")]
		bool localizationSupport = true;

		/// <summary>
		/// Localization support.
		/// </summary>
		public bool LocalizationSupport
		{
			get
			{
				return localizationSupport;
			}

			set
			{
				localizationSupport = value;
			}
		}

		/// <summary>
		/// Select event.
		/// </summary>
		[SerializeField]
		public UnityEvent OnSelectEvent = new UnityEvent();

		/// <summary>
		/// Select event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);

			OnSelectEvent.Invoke();
		}
	}
}