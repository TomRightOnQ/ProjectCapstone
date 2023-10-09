#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Project settings.
	/// </summary>
	[InitializeOnLoad]
	public static class ProjectSettings
	{
		static ProjectSettings()
		{
			if (!ScriptingDefineSymbols.GetState("UITHEMES_INSTALLED").All)
			{
				ScriptingDefineSymbols.Add("UITHEMES_INSTALLED");

				if (TMPro.Installed && !TMPro.State.All)
				{
					TMPro.Enabled = true;
				}

				AssetDatabase.Refresh();
			}
		}

		class Labels
		{
			private Labels()
			{
			}

			[DomainReloadExclude]
			public static readonly GUIContent AssemblyDefinitions = new GUIContent("Assembly Definitions");

			/// <summary>
			/// TextMeshPro label.
			/// </summary>
			[DomainReloadExclude]
			public static readonly GUIContent TMPro = new GUIContent("TextMeshPro Support");
		}

		class Block
		{
			ScriptingDefineSymbol symbol;

			GUIContent label;

			Action<ScriptingDefineSymbol> info;

			string buttonEnable;

			string buttonDisable;

			public Block(ScriptingDefineSymbol symbol, GUIContent label, Action<ScriptingDefineSymbol> info = null, string buttonEnable = "Enable", string buttonDisable = "Disable")
			{
				this.symbol = symbol;
				this.label = label;
				this.info = info;

				this.buttonEnable = buttonEnable;
				this.buttonDisable = buttonDisable;
			}

			GUIContent CreateLabel()
			{
				var label = new GUIContent(symbol.Status);
				if (symbol.State.HasMissing)
				{
					label.tooltip = "Support is not enabled for all BuildTargets.";

					var color = EditorStyles.label.normal.textColor;
					EditorStyles.label.normal.textColor = Color.red;
					EditorStyles.label.normal.textColor = color;
				}

				return label;
			}

			void EnableForAllBlock()
			{
				if (symbol.Installed && symbol.Enabled && symbol.State.HasMissing)
				{
					EditorGUILayout.BeginVertical();
					EditorGUILayout.HelpBox("Feature is not enabled for all BuildTargets.", MessageType.Info);
					if (GUILayout.Button("Enable for All", GUILayout.ExpandWidth(true)))
					{
						symbol.EnableForAll();
					}

					EditorGUILayout.EndVertical();
				}
			}

			public void Show()
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(label, NameOptions);
				EditorGUILayout.LabelField(CreateLabel(), StatusOptions);

				if (symbol.Installed)
				{
					if (symbol.Enabled)
					{
						if (GUILayout.Button(buttonDisable))
						{
							symbol.Enabled = false;
						}
					}
					else
					{
						if (GUILayout.Button(buttonEnable))
						{
							symbol.Enabled = true;
						}
					}
				}

				EditorGUILayout.EndHorizontal();

				if (symbol.Installed)
				{
					EnableForAllBlock();

					if (info != null)
					{
						info(symbol);
					}
				}
			}
		}

		/// <summary>
		/// Enable/disable assembly definitions.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol AssemblyDefinitions = new ScriptingDefineSymbol(
			"UIWIDGETS_ASMDEF",
			true,
			AssemblyDefinitionsChanged);

		/// <summary>
		/// Enable/disable TextMeshPro support.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol TMPro = new ScriptingDefineSymbol(
			"UIWIDGETS_TMPRO_SUPPORT",
			UtilitiesEditor.GetType("TMPro.TextMeshProUGUI") != null);

		[DomainReloadExclude]
		static readonly GUILayoutOption[] NameOptions = new GUILayoutOption[] { GUILayout.Width(170) };

		[DomainReloadExclude]
		static readonly GUILayoutOption[] StatusOptions = new GUILayoutOption[] { GUILayout.Width(170) };

		static readonly List<Block> Blocks = new List<Block>()
		{
			new Block(AssemblyDefinitions, Labels.AssemblyDefinitions),
			new Block(TMPro, Labels.TMPro),
		};

		static void AssemblyDefinitionsChanged(bool enabled)
		{
			if (enabled)
			{
				AssemblyDefinitionGenerator.Create();
			}
			else
			{
				AssemblyDefinitionGenerator.Delete();
			}
		}

		/// <summary>
		/// Get required assemblies.
		/// </summary>
		/// <returns>Assemblies name list.</returns>
		public static List<string> GetAssemblies()
		{
			var result = new List<string>();

			if (TMPro.Installed)
			{
				result.Add("Unity.TextMeshPro");
			}

			return result;
		}

		/// <summary>
		/// Create settings provider.
		/// </summary>
		/// <returns>Settings provider.</returns>
		#if !UIWIDGETS_INSTALLED
		[SettingsProvider]
		#endif
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new SettingsProvider("Project/UI Themes", SettingsScope.Project)
			{
				guiHandler = (searchContext) =>
				{
					foreach (var block in Blocks)
					{
						block.Show();
						EditorGUILayout.Space(6);
					}
				},

				keywords = SettingsProvider.GetSearchKeywordsFromGUIContentProperties<Labels>(),
			};

			return provider;
		}
	}
}
#endif