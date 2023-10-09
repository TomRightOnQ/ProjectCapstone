#if UNITY_EDITOR
namespace UIWidgets
{
	using UnityEditor;

	/// <summary>
	/// Menu options to toggle third party packages support.
	/// </summary>
	[InitializeOnLoad]
	public static class ThirdPartySupportMenuOptions
	{
		/// <summary>
		/// Upgrade widgets at scene.
		/// </summary>
		// [MenuItem("Window/New UI Widgets/Upgrade Widgets at Scene")]
		public static void Upgrade()
		{
			#if UNITY_2017_1_OR_NEWER
			var gameobjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (var go in gameobjects)
			{
				var components = go.GetComponentsInChildren<IUpgradeable>();
				foreach (var component in components)
				{
					component.Upgrade();
				}
			}
			#endif
		}
	}
}
#endif