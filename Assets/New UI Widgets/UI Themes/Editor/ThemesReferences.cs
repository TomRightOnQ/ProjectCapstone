#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;
	using System.IO;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.Serialization;

	/// <summary>
	/// Themes references.
	/// </summary>
	[Serializable]
	public class ThemesReferences : ScriptableObject
	{
		[SerializeField]
		[FormerlySerializedAs("Current")]
		Theme current;

		/// <summary>
		/// Current theme.
		/// </summary>
		[SerializeField]
		public Theme Current
		{
			get
			{
				return current;
			}

			set
			{
				current = value;
				EditorUtility.SetDirty(this);
			}
		}

		/// <summary>
		/// Default themes references.
		/// </summary>
		public static ThemesReferences Default
		{
			get
			{
				var refs = Resources.FindObjectsOfTypeAll<ThemesReferences>();
				if (refs.Length > 0)
				{
					return refs[0];
				}

				var guids = AssetDatabase.FindAssets("t:ThemesReferences");
				if (guids.Length > 0)
				{
					var path = AssetDatabase.GUIDToAssetPath(guids[0]);
					return AssetDatabase.LoadAssetAtPath<ThemesReferences>(path);
				}

				return Create();
			}
		}

		/// <summary>
		/// Create ThemesReferences.
		/// </summary>
		/// <returns>Created instance.</returns>
		static ThemesReferences Create()
		{
			var folder = ReferencesGUIDs.AssetsFolder;
			if (string.IsNullOrEmpty(folder))
			{
				return null;
			}

			var path = folder + Path.DirectorySeparatorChar + "UI Themes References.asset";
			return UtilitiesEditor.CreateScriptableObjectAsset<ThemesReferences>(path);
		}
	}
}
#endif