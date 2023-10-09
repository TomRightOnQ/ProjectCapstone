#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System.Collections.Generic;
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Menu options.
	/// </summary>
	public static class MenuOptions
	{
		/// <summary>
		/// Create theme.
		/// </summary>
		[MenuItem("Assets/Create/UI Themes/Theme", false)]
		public static void CreateTheme()
		{
			Theme theme;

			var default_theme = ReferencesGUIDs.DefaultTheme;
			var path = UtilitiesEditor.SelectedAssetPath() + Path.DirectorySeparatorChar + "UI Theme.asset";
			if (default_theme != null)
			{
				path = AssetDatabase.GenerateUniqueAssetPath(path);
				if (!AssetDatabase.CopyAsset(ReferencesGUIDs.DefaultThemePath, path))
				{
					Debug.LogError("Failed to copy theme: " + ReferencesGUIDs.DefaultThemePath);
				}

				theme = AssetDatabase.LoadAssetAtPath<Theme>(path);
			}
			else
			{
				theme = UtilitiesEditor.CreateScriptableObjectAsset<Theme>(path, false);
				theme.AddVariation("Initial Variation");
			}

			var refs = ThemesReferences.Default;
			if ((refs != null) && (refs.Current == null))
			{
				refs.Current = theme;
			}

			AssetDatabase.SaveAssets();

			Selection.activeObject = theme;
			ThemeEditor.Open(theme);
		}

		static bool CanAttachTheme(out string error)
		{
			var target = Selection.activeGameObject;
			if (target == null)
			{
				error = "Gameobject is not selected.";
				return false;
			}

			var themes = ThemesReferences.Default;
			if ((themes == null) || (themes.Current == null))
			{
				error = "Current theme is not specified.";
				return false;
			}

			var theme = themes.Current;
			if (!theme.HasVariation(theme.InitialVariationId))
			{
				error = "Initial variation is not specified for the current theme.";
				return false;
			}

			error = string.Empty;
			return true;
		}

		/// <summary>
		/// Can attach theme.
		/// </summary>
		/// <returns>true if can attach theme; otherwise false.</returns>
		[MenuItem("GameObject/UI/UI Themes/Attach Theme", true, 10)]
		public static bool CanAttachTheme()
		{
			return CanAttachTheme(out var _);
		}

		/// <summary>
		/// Attach theme.
		/// </summary>
		[MenuItem("GameObject/UI/UI Themes/Attach Theme", false, 10)]
		public static void AttachTheme()
		{
			if (!CanAttachTheme(out var error))
			{
				Debug.LogError(error);
				return;
			}

			var theme = ThemesReferences.Default.Current;
			var target = Selection.activeGameObject;

			Undo.RegisterFullObjectHierarchyUndo(target, "Undo Theme Attach");
			ThemeAttach.Attach(target, theme, false);
			RecordPrefabInstanceModifications(target);

			ThemeEditor.RefreshWindow();
		}

		/// <summary>
		/// Can attach theme and create options.
		/// </summary>
		/// <returns>true if can attach theme and create options.</returns>
		[MenuItem("GameObject/UI/UI Themes/Attach Theme and Create Options", true, 20)]
		public static bool CanAttachThemeAndCreateOptions()
		{
			return CanAttachTheme(out var _);
		}

		/// <summary>
		/// Attach theme and create options.
		/// </summary>
		[MenuItem("GameObject/UI/UI Themes/Attach Theme and Create Options", false, 20)]
		public static void AttachThemeAndCreateOptions()
		{
			if (!CanAttachTheme(out var error))
			{
				Debug.LogError(error);
				return;
			}

			var theme = ThemesReferences.Default.Current;
			var target = Selection.activeGameObject;

			Undo.RecordObject(theme, "Attach Theme and Create Options");
			Undo.RegisterFullObjectHierarchyUndo(target, "Attach Theme and Create Options");
			ThemeAttach.Attach(target, theme, true);
			RecordPrefabInstanceModifications(target);

			ThemeEditor.RefreshWindow();
		}

		/// <summary>
		/// Remove ThemeTarget components.
		/// </summary>
		[MenuItem("GameObject/UI/UI Themes/Remove All ThemeTarget", false, 30)]
		public static void RemoveThemeTargets()
		{
			var target = Selection.activeGameObject;
			if (target == null)
			{
				return;
			}

			Undo.RegisterFullObjectHierarchyUndo(target, "Undo Remove ThemeTarget");
			var temp = target.GetComponentsInChildren<ThemeTargetBase>(true);
			foreach (var t in temp)
			{
				UnityEngine.Object.DestroyImmediate(t);
			}
		}

		[DomainReloadExclude]
		static readonly List<Component> Components = new List<Component>();

		/// <summary>
		/// Record prefab instance modifications.
		/// </summary>
		/// <param name="target">Target.</param>
		public static void RecordPrefabInstanceModifications(GameObject target)
		{
			if (PrefabUtility.IsPartOfAnyPrefab(target))
			{
				PrefabUtility.RecordPrefabInstancePropertyModifications(target);

				target.GetComponents(Components);

				foreach (var c in Components)
				{
					PrefabUtility.RecordPrefabInstancePropertyModifications(c);
				}

				Components.Clear();
			}

			var t = target.transform;
			for (int i = 0; i < t.childCount; i++)
			{
				RecordPrefabInstanceModifications(t.GetChild(i).gameObject);
			}
		}
	}
}
#endif