#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.UIElements;

	/// <summary>
	/// Reflection wrappers window.
	/// </summary>
	public partial class ReflectionWrappersWindow : EditorWindow
	{
		VisualElement rootBlock;

		/// <summary>
		/// Show reflection wrappers.
		/// </summary>
		[MenuItem("Window/UI Themes/Reflection Wrappers")]
		public static void Open()
		{
			var window = GetWindow<ReflectionWrappersWindow>();
			window.minSize = new Vector2(500, 600);
			window.titleContent = new GUIContent("Reflection Wrappers");
		}

		/// <summary>
		/// Create GUI.
		/// </summary>
		public virtual void CreateGUI()
		{
			if (rootBlock != null)
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

			rootBlock = new VisualElement();
			rootVisualElement.Add(rootBlock);

			ShowWrappers();
		}

		/// <summary>
		/// Show wrappers.
		/// </summary>
		protected virtual void ShowWrappers()
		{
			var scroll = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
			scroll.AddToClassList("wrappers-scroll");

			var all = ReflectionWrappersRegistry.All();
			if (all.Count == 0)
			{
				var none = new Label("There are no wrappers created with reflection.");
				none.AddToClassList("wrappers-none");
				scroll.Add(none);
			}
			else
			{
				var info = new Label("Types and fields / properties with wrappers created with reflection:");
				info.AddToClassList("wrappers-info");
				scroll.Add(info);
			}

			foreach (var values in all)
			{
				var block = ShowTypeProperties(values.Key, values.Value);
				scroll.Add(block);
			}

			rootBlock.Add(scroll);
		}

		/// <summary>
		/// Show type properties.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="properties">Properties.</param>
		/// <returns>Visual element.</returns>
		protected virtual VisualElement ShowTypeProperties(Type type, IReadOnlyCollection<string> properties)
		{
			var block = new VisualElement();
			block.AddToClassList("wrappers-block");

			var type_name = new Label(UtilitiesEditor.GetFriendlyTypeName(type));
			type_name.AddToClassList("wrappers-block-type");
			block.Add(type_name);
			foreach (var property in properties)
			{
				var label = new Label("- " + property);
				label.AddToClassList("wrappers-block-type-property");
				block.Add(label);
			}

			return block;
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