#if UIWIDGETS_TMPRO_SUPPORT
namespace UIWidgets.TMProSupport
{
	using TMPro;
	using UIWidgets;
	using UnityEngine;

	/// <summary>
	/// Tab component.
	/// </summary>
	[System.Obsolete("Use TabButtonComponent with TextAdapter.")]
	public class TabButtonComponentTMPro : TabButtonComponentBase
	{
		/// <summary>
		/// The name.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		[System.Obsolete("Replaced with NameAdapter.")]
		public TextMeshProUGUI Name;

		/// <summary>
		/// Upgrade this instance.
		/// </summary>
		public override void Upgrade()
		{
#pragma warning disable 0612, 0618
			Utilities.RequireComponent(Name, ref NameAdapter);
#pragma warning restore 0612, 0618
		}
	}
}
#endif