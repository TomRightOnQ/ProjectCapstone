#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.Attributes;
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
			if (!ScriptingDefineSymbols.GetState("UIWIDGETS_INSTALLED").All)
			{
				ScriptingDefineSymbols.Add("UIWIDGETS_INSTALLED");

				if (TMPro.Installed && !TMPro.State.All)
				{
					TMPro.EnableForAll();
				}

				AssemblyDefinitions.EnableForAll();

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

			[DomainReloadExclude]
			public static readonly GUIContent InstantiateWidgets = new GUIContent("Instantiate Widgets");

			[DomainReloadExclude]
			public static readonly GUIContent StylesLabel = new GUIContent("Styles or Themes");

			[DomainReloadExclude]
			public static readonly GUIContent TMPro = new GUIContent("TextMeshPro Support");

			[DomainReloadExclude]
			public static readonly GUIContent DataBind = new GUIContent("Data Bind for Unity Support");

			[DomainReloadExclude]
			public static readonly GUIContent I2Localization = new GUIContent("I2 Localization Support");
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
		/// Toggle widgets instance type.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol InstantiateWidgets = new ScriptingDefineSymbol(
			"UIWIDGETS_INSTANTIATE_PREFABS",
			true,
			enabledText: "Prefabs",
			disabledText: "Copies");

		/// <summary>
		/// Enable/disable legacy styles.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol LegacyStyles = new ScriptingDefineSymbol(
			"UIWIDGETS_LEGACY_STYLE",
			true,
			enabledText: "Styles (obsolete)",
			disabledText: "UI Themes");

		/// <summary>
		/// Enable/disable TextMeshPro support.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol TMPro = new ScriptingDefineSymbol(
			"UIWIDGETS_TMPRO_SUPPORT",
			UtilitiesEditor.GetType("TMPro.TextMeshProUGUI") != null);

		/// <summary>
		/// Enable/disable DataBind support.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol DataBind = new ScriptingDefineSymbol(
			"UIWIDGETS_DATABIND_SUPPORT",
			UtilitiesEditor.GetType("Slash.Unity.DataBind.Core.Presentation.DataProvider") != null);

		[DomainReloadExclude]
		static readonly string DataBindWarning = "Data Bind for Unity does not have assembly definitions by default.\n" +
			"You must create them and add references to UIWidgets.asmdef," +
			" UIWidgets.Editor.asmdef, and UIWidgets.Samples.asmdef.\n" +
			"Or you can disable assembly definitions.";

		/// <summary>
		/// Enable/disable I2Localization support.
		/// </summary>
		[DomainReloadExclude]
		public static readonly ScriptingDefineSymbol I2Localization = new ScriptingDefineSymbol(
			"I2_LOCALIZATION_SUPPORT",
			UtilitiesEditor.GetType("I2.Loc.LocalizationManager") != null);

		[DomainReloadExclude]
		static readonly string I2LocalizationWarning = "I2 Localization does not have assembly definitions by default.\n" +
			"You must create them and add references to UIWidgets.asmdef," +
			" UIWidgets.Editor.asmdef, and UIWidgets.Samples.asmdef.\n" +
			"Or you can disable assembly definitions.";

		[DomainReloadExclude]
		static readonly GUILayoutOption[] NameOptions = new GUILayoutOption[] { GUILayout.Width(170) };

		[DomainReloadExclude]
		static readonly GUILayoutOption[] StatusOptions = new GUILayoutOption[] { GUILayout.Width(170) };

		static readonly Action<ScriptingDefineSymbol> TMProInfo = (ScriptingDefineSymbol symbol) =>
		{
			if (symbol.Installed && symbol.Enabled)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.HelpBox(
					"You can replace all Unity text with TMPro text by using" +
					" the context menu \"UI / New UI Widgets / Replace Unity Text with TextMeshPro\"" +
					" or by using the menu \"Window / New UI Widgets / Replace Unity Text with TextMeshPro\".",
					MessageType.Info);
				EditorGUILayout.EndVertical();
			}
		};

		static readonly Action<ScriptingDefineSymbol> DataBindInfo = (ScriptingDefineSymbol symbol) =>
		{
			if (symbol.Installed)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.HelpBox(DataBindWarning, MessageType.Warning);
				EditorGUILayout.EndVertical();
			}
		};

		static readonly Action<ScriptingDefineSymbol> I2LocalizationInfo = (ScriptingDefineSymbol symbol) =>
		{
			if (symbol.Installed)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.HelpBox(I2LocalizationWarning, MessageType.Warning);
				EditorGUILayout.EndVertical();
			}
		};

		static readonly List<Block> Blocks = new List<Block>()
		{
			new Block(AssemblyDefinitions, Labels.AssemblyDefinitions),
			new Block(InstantiateWidgets, Labels.InstantiateWidgets, buttonDisable: "Create Copies", buttonEnable: "Create Prefabs"),
			new Block(LegacyStyles, Labels.StylesLabel, buttonDisable: "Use UI Themes", buttonEnable: "Use Legacy Styles"),

			// ScriptsRecompile.SetStatus(ReferenceGUID.TMProStatus, ScriptsRecompile.StatusSymbolsAdded);
			new Block(TMPro, Labels.TMPro, TMProInfo),

			// ScriptsRecompile.SetStatus(ReferenceGUID.DataBindStatus, ScriptsRecompile.StatusSymbolsAdded);
			new Block(DataBind, Labels.DataBind, DataBindInfo),

			// ScriptsRecompile.SetStatus(ReferenceGUID.I2LocalizationStatus, ScriptsRecompile.StatusSymbolsAdded);
			new Block(I2Localization, Labels.I2Localization, I2LocalizationInfo),
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
			var result = new List<string>()
			{
				"UIThemes",
			};

			if (TMPro.Installed)
			{
				result.Add("Unity.TextMeshPro");
			}

			if (DataBind.Enabled)
			{
				Debug.LogWarning(DataBindWarning);
			}

			if (I2Localization.Enabled)
			{
				Debug.LogWarning(I2LocalizationWarning);
			}

			if (UtilitiesEditor.GetType("UnityEngine.InputSystem.InputSystem") != null)
			{
				result.Add("Unity.InputSystem");
			}

			return result;
		}

		/// <summary>
		/// Create settings provider.
		/// </summary>
		/// <returns>Settings provider.</returns>
		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new SettingsProvider("Project/New UI Widgets", SettingsScope.Project)
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