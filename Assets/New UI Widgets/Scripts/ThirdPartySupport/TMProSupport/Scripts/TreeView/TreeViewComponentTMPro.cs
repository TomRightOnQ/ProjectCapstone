#if UIWIDGETS_TMPRO_SUPPORT
namespace UIWidgets.TMProSupport
{
	using TMPro;
	using UIWidgets;
	using UnityEngine;

	/// <summary>
	/// TreeView component TMPro.
	/// </summary>
	[System.Obsolete("Use TreeViewComponent with TextAdapter.")]
	public class TreeViewComponentTMPro : TreeViewComponent
	{
		/// <summary>
		/// Text.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		[System.Obsolete("Replaced with TextAdapter.")]
		public TextMeshProUGUI TextTMPro;

		/// <summary>
		/// Upgrade this instance.
		/// </summary>
		public override void Upgrade()
		{
#pragma warning disable 0612, 0618
			Utilities.RequireComponent(TextTMPro, ref TextAdapter);
#pragma warning restore 0612, 0618
		}
	}
}
#endif