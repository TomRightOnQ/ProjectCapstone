#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	/// <summary>
	/// Window to show ThemeTarget list.
	/// </summary>
	public class ThemeTargetsWindow : EditorWindow
	{
		/// <summary>
		/// Targets.
		/// </summary>
		protected List<ThemeTargetBase> Targets;

		/// <summary>
		/// Option.
		/// </summary>
		protected Option Option;

		/// <summary>
		/// Root.
		/// </summary>
		protected VisualElement Root;

		/// <summary>
		/// Open.
		/// </summary>
		/// <param name="targets">Targets.</param>
		/// <param name="option">Option.</param>
		public static void Open(List<ThemeTargetBase> targets, Option option)
		{
			var window = GetWindow<ThemeTargetsWindow>();
			window.Targets = targets;
			window.Option = option;
			window.minSize = new Vector2(350, 250);
			window.titleContent = new GUIContent("Theme Targets");
			window.Refresh();
		}

		/// <summary>
		/// Create GUI.
		/// </summary>
		public virtual void CreateGUI()
		{
			if (Root != null)
			{
				return;
			}

			foreach (var style in ReferencesGUIDs.GetStyleSheets())
			{
				rootVisualElement.styleSheets.Add(style);
			}

			if (EditorGUIUtility.isProSkin)
			{
				rootVisualElement.AddToClassList("dark-skin");
			}

			Root = new VisualElement();
			Root.AddToClassList("theme-targets-window");
			rootVisualElement.Add(Root);

			Refresh();
		}

		/// <summary>
		/// Refresh.
		/// </summary>
		public virtual void Refresh()
		{
			if ((Targets == null) || (Option == null))
			{
				return;
			}

			Root.Clear();

			var header = string.Format("Found {0} {1} with option \"{2}\"", Targets.Count, Targets.Count == 1 ? "object" : "objects", Option.Name);
			var name = new Label(header);
			name.AddToClassList("theme-targets-option-name");
			Root.Add(name);

			var scroll = new ScrollView(ScrollViewMode.Vertical);
			scroll.AddToClassList("theme-targets-list");
			Root.Add(scroll);

			for (int i = 0; i < Targets.Count; i++)
			{
				var t = Targets[i];
				var button = new Button(() => EditorGUIUtility.PingObject(t))
				{
					text = string.Format("{0}. {1}", i, t.name),
				};
				button.AddToClassList("theme-targets-target");

				scroll.Add(button);
			}
		}

		/// <summary>
		/// Process the enable event.
		/// </summary>
		protected virtual void OnEnable()
		{
			#if !UNITY_2020_3_OR_NEWER
			CreateGUI();
			#endif
		}
	}
}
#endif