#if UNITY_EDITOR
namespace UIWidgets.WidgetGeneration
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using UIThemes;
	using UIWidgets;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UIWidgets.UIThemesSupport;
	using UnityEditor;
	using UnityEditor.Events;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for widget prefab generator.
	/// </summary>
	public abstract class PrefabGenerator
	{
		/// <summary>
		/// Instantiant.
		/// </summary>
		public static readonly bool InstantiatePrefabs =
#if UIWIDGETS_INSTANTIATE_PREFABS
			true;
#else
			false;
#endif

		/// <summary>
		/// Label for the first style/theme button.
		/// </summary>
		protected readonly string[] StyleButtonLabels = new string[]
		{
#if UIWIDGETS_LEGACY_STYLE
			"Style Default",
			"Style Blue",
			"None",
			"None",
#else
			"Theme Blue",
			"Theme Red",
			"Theme Dark",
			"Theme Legacy",
#endif
		};

		/// <summary>
		/// Class info.
		/// </summary>
		protected ClassInfo Info = null;

		/// <summary>
		/// Path to save created files.
		/// </summary>
		protected string SavePath = null;

		/// <summary>
		/// Path to save created prefabs.
		/// </summary>
		protected string PrefabSavePath;

		/// <summary>
		/// Prefabs generation order.
		/// </summary>
		protected List<string> PrefabsOrder = new List<string>()
		{
			"ListView",
			"DragInfo",
			"Combobox",
			"ComboboxMultiselect",
			"Table",
			"TileView",
			"TreeView",
			"TreeGraph",
			"PickerListView",
			"PickerTreeView",
			"Autocomplete",
			"AutoCombobox",
			"Tooltip",
		};

		/// <summary>
		/// Prefabs.
		/// </summary>
		protected PrefabsMenuGenerated PrefabsMenu;

		/// <summary>
		/// Functions to create prefabs.
		/// </summary>
		protected Dictionary<string, Func<GameObject>> PrefabGenerators;

		/// <summary>
		/// Maximum value of the progress bar.
		/// </summary>
		protected int ProgressMax = 0;

		/// <summary>
		/// Theme.
		/// </summary>
		protected Theme Theme;

		/// <summary>
		/// Theme Variation ID.
		/// </summary>
		protected VariationId ThemeVariation;

		/// <summary>
		/// Initializes a new instance of the <see cref="PrefabGenerator"/> class.
		/// </summary>
		/// <param name="path">Path to save created files.</param>
		protected PrefabGenerator(string path)
		{
			Theme = UIThemes.Editor.ReferencesGUIDs.DefaultTheme;
			ThemeVariation = Theme.GetVariation("Blue").Id;

			SavePath = path;
			PrefabSavePath = SavePath + Path.DirectorySeparatorChar + "Prefabs";

			if (!Directory.Exists(PrefabSavePath))
			{
				Directory.CreateDirectory(PrefabSavePath);
			}

			PrefabGenerators = new Dictionary<string, Func<GameObject>>()
			{
				{ "ListView", GenerateListView },
				{ "DragInfo", GenerateDragInfo },
				{ "Combobox", GenerateCombobox },
				{ "ComboboxMultiselect", GenerateComboboxMultiselect },
				{ "Table", GenerateTable },
				{ "TileView", GenerateTileView },
				{ "TreeView", GenerateTreeView },
				{ "TreeGraph", GenerateTreeGraph },
				{ "PickerListView", GeneratePickerListView },
				{ "PickerTreeView", GeneratePickerTreeView },
				{ "Autocomplete", GenerateAutocomplete },
				{ "AutoCombobox", GenerateAutoCombobox },
				{ "Tooltip", GenerateTooltip },
			};

			ProgressMax = PrefabGenerators.Count + 1;
		}

		/// <summary>
		/// Instantiate gameobject.
		/// </summary>
		/// <param name="go">Gameobject.</param>
		/// <returns>Instance.</returns>
		public GameObject Instantiate(GameObject go)
		{
			return Instantiate(go, InstantiatePrefabs);
		}

		/// <summary>
		/// Instantiate gameobject.
		/// </summary>
		/// <param name="go">Gameobject.</param>
		/// <param name="instantiatePrefabs">Instantiate gameobject as prefab reference or copy of prefab.</param>
		/// <returns>Instance.</returns>
		public GameObject Instantiate(GameObject go, bool instantiatePrefabs)
		{
			return instantiatePrefabs
				? PrefabUtility.InstantiatePrefab(go) as GameObject
				: UnityEngine.Object.Instantiate(go);
		}

		/// <summary>
		/// Generate prefabs and test scene.
		/// </summary>
		protected void Generate()
		{
			var temp_go = new List<GameObject>();

			PrefabsMenu = ScriptableObject.CreateInstance<PrefabsMenuGenerated>();
			var real_menu = UIWidgets.UtilitiesEditor.LoadAssetWithGUID<PrefabsMenuGenerated>(Info.PrefabsMenuGUID);

			try
			{
				var i = 0;

				ProgressbarUpdate(i);

				foreach (var widget in PrefabsOrder)
				{
					var prefab_name = widget + Info.ShortTypeName;

					if (Info.Prefabs.ContainsKey(widget) && Info.Prefabs[widget])
					{
						var go = PrefabGenerators[widget]();
						if (go != null)
						{
							go.name = prefab_name;
							var prefab = Save(go);
							Prefab2Menu(PrefabsMenu, prefab, widget);
							Prefab2Menu(real_menu, prefab, widget);
							temp_go.Add(go);
						}
					}

					i += 1;
					ProgressbarUpdate(i);
				}

				if (Info.Scenes["TestScene"])
				{
					GenerateScene();
				}

				GenerateDataBindSupport();

				ProgressbarUpdate(ProgressMax);
			}
			catch (Exception)
			{
				EditorUtility.ClearProgressBar();
				throw;
			}
			finally
			{
				foreach (var go in temp_go)
				{
					UnityEngine.Object.DestroyImmediate(go);
				}

				temp_go.Clear();

				EditorUtility.SetDirty(real_menu);
			}
		}

		/// <summary>
		/// Add prefab to the menu.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="prefab">Prefab.</param>
		/// <param name="fieldName">Field name.</param>
		protected void Prefab2Menu(PrefabsMenuGenerated menu, GameObject prefab, string fieldName)
		{
			var type = menu.GetType();
			var field = type.GetField(fieldName);
			field.SetValue(menu, prefab);
		}

		/// <summary>
		/// Generate support scripts for the Data Bind.
		/// </summary>
		protected virtual void GenerateDataBindSupport()
		{
#if UIWIDGETS_DATABIND_SUPPORT
			var databind_path = SavePath + Path.DirectorySeparatorChar + "DataBindSupport";

			if (!Directory.Exists(databind_path))
			{
				Directory.CreateDirectory(databind_path);
			}

			var typenames = new List<string>()
			{
				"Autocomplete",
				"Combobox",
				"ListView",
				"TreeGraph",
				"TreeView",
			};

			foreach (var name in typenames)
			{
				var type = UtilitiesEditor.GetType(Info.WidgetsNamespace + "." + name + Info.ShortTypeName);
				if (type != null)
				{
					UIWidgets.DataBindSupport.DataBindGenerator.Run(type, databind_path);
				}
			}
#endif
		}

		/// <summary>
		/// Clear.
		/// </summary>
		protected void Clear()
		{
		}

		/// <summary>
		/// Delete file with meta data.
		/// </summary>
		/// <param name="file">File.</param>
		protected static void Delete(string file)
		{
			AssetDatabase.DeleteAsset(file);
		}

		/// <summary>
		/// Set label color.
		/// </summary>
		/// <param name="label">Label.</param>
		protected void ThemeLabel(Graphic label)
		{
#if !UIWIDGETS_LEGACY_STYLE
			var colors = Theme.Colors;
			label.color = colors.Get(ThemeVariation, "Text");
#endif
		}

		/// <summary>
		/// Set ListView colors.
		/// </summary>
		/// <param name="listView">ListView.</param>
		protected void ThemeListView(ListViewBase listView)
		{
#if !UIWIDGETS_LEGACY_STYLE
			var colors = Theme.Colors;
			listView.DefaultBackgroundColor = colors.Get(ThemeVariation, "Transparent");
			listView.DefaultColor = colors.Get(ThemeVariation, "Text");
			listView.HighlightedBackgroundColor = colors.Get(ThemeVariation, "Background");
			listView.HighlightedColor = colors.Get(ThemeVariation, "Text Highlight");
			listView.SelectedBackgroundColor = colors.Get(ThemeVariation, "Secondary");
			listView.SelectedColor = colors.Get(ThemeVariation, "Text Highlight");
			listView.DisabledColor = colors.Get(ThemeVariation, "Selectable Disabled");
			listView.DefaultEvenBackgroundColor = colors.Get(ThemeVariation, "Table/Even Row");
			listView.DefaultOddBackgroundColor = colors.Get(ThemeVariation, "Table/Odd Row");

			var drop = listView.GetComponentInChildren<ListViewDropIndicator>(true);
			drop.GetComponent<Image>().color = colors.Get(ThemeVariation, "Dialog, Popup Background");

			var sr = listView.GetScrollRect();
			sr.viewport.GetComponent<Image>().color = colors.Get(ThemeVariation, "Text");
			ThemeScrollbar(sr.horizontalScrollbar);
			ThemeScrollbar(sr.verticalScrollbar);
#endif
		}

		/// <summary>
		/// Set Scrollbar colors.
		/// </summary>
		/// <param name="scrollbar">Scrollbar.</param>
		protected void ThemeScrollbar(Scrollbar scrollbar)
		{
#if !UIWIDGETS_LEGACY_STYLE
			if (scrollbar == null)
			{
				return;
			}

			var colors = Theme.Colors;
			scrollbar.GetComponent<Image>().color = colors.Get(ThemeVariation, "Transparent");

			var handle = scrollbar.handleRect.GetComponent<Image>();
			handle.color = colors.Get(ThemeVariation, "Secondary");
#endif
		}

		/// <summary>
		/// Set TileView colors.
		/// </summary>
		/// <param name="tileView">TileView.</param>
		protected void ThemeTileView(ListViewBase tileView)
		{
#if !UIWIDGETS_LEGACY_STYLE
			ThemeListView(tileView);
#endif
		}

		/// <summary>
		/// Set Table colors.
		/// </summary>
		/// <param name="table">Table.</param>
		protected void ThemeTable(ListViewBase table)
		{
#if !UIWIDGETS_LEGACY_STYLE
			ThemeListView(table);
			table.ColoringStriped = true;
#endif
		}

		/// <summary>
		/// Set TableHeader colors.
		/// </summary>
		/// <param name="tableHeader">Table header.</param>
		protected void ThemeTableHeader(TableHeader tableHeader)
		{
#if !UIWIDGETS_LEGACY_STYLE
			var colors = Theme.Colors;
			tableHeader.GetComponent<Image>().color = colors.Get(ThemeVariation, "Table/Header");
#endif
		}

		/// <summary>
		/// Set TreeView colors.
		/// </summary>
		/// <param name="treeView">TreeView.</param>
		protected void ThemeTreeView(ListViewBase treeView)
		{
#if !UIWIDGETS_LEGACY_STYLE
			ThemeListView(treeView);
#endif
		}

		/// <summary>
		/// Set Tooltip colors.
		/// </summary>
		/// <param name="go">Tooltip.</param>
		protected void ThemeTooltip(TooltipBase go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set DragInfo colors.
		/// </summary>
		/// <param name="go">DragInfo.</param>
		protected void ThemeDragInfo(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set ComboboxMultiselect colors.
		/// </summary>
		/// <param name="go">ComboboxMultiselect.</param>
		protected void ThemeComboboxMultiSelect(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set Combobox colors.
		/// </summary>
		/// <param name="go">Combobox.</param>
		protected void ThemeCombobox(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set TreeGraph colors.
		/// </summary>
		/// <param name="go">TreeGraph.</param>
		protected void ThemeTreeGraph(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set PickerListView colors.
		/// </summary>
		/// <param name="picker">PickerListView.</param>
		protected void ThemePickerListView(Picker picker)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set PickerTreeView colors.
		/// </summary>
		/// <param name="picker">PickerTreeView.</param>
		protected void ThemePickerTreeView(Picker picker)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set Autocomplete colors.
		/// </summary>
		/// <param name="go">Autocomplete.</param>
		protected void ThemeAutocomplete(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set AutoCombobox colors.
		/// </summary>
		/// <param name="go">AutoCombobox.</param>
		protected void ThemeAutoCombobox(GameObject go)
		{
#if !UIWIDGETS_LEGACY_STYLE
#endif
		}

		/// <summary>
		/// Set Button colors.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="label">Button label.</param>
		protected void ThemeButton(Button button, Graphic label)
		{
#if !UIWIDGETS_LEGACY_STYLE
			var colors = Theme.Colors;
			label.color = colors.Get(ThemeVariation, "Text");
#endif
		}

		/// <summary>
		/// Save gameobject as prefab.
		/// </summary>
		/// <param name="go">Original gameobject.</param>
		/// <returns>Prefab.</returns>
		protected GameObject Save(GameObject go)
		{
			#if UIWIDGETS_LEGACY_STYLE
			var style = UIWidgets.PrefabsMenu.Instance.DefaultStyle;
			if (style != null)
			{
				style.ApplyTo(go);
			}
			#endif

			var filename = PrefabSavePath + Path.DirectorySeparatorChar + go.name + ".prefab";

			return Compatibility.CreatePrefab(filename, go);
		}

		/// <summary>
		/// Update progress bar.
		/// </summary>
		/// <param name="progress">Progress value.</param>
		protected void ProgressbarUpdate(int progress)
		{
			if (progress < ProgressMax)
			{
				EditorUtility.DisplayProgressBar("Widget Generation", "Step 2. Creating prefabs.", progress / (float)ProgressMax);
			}
			else
			{
				EditorUtility.ClearProgressBar();
			}
		}

		/// <summary>
		/// Generate test scene.
		/// </summary>
		protected void GenerateScene()
		{
			GenerateSceneContent();

			Compatibility.SceneSave(SavePath + Path.DirectorySeparatorChar + Info.ShortTypeName + ".unity");
		}

		/// <summary>
		/// Default size of the created gameobjects.
		/// </summary>
		[UIWidgets.Attributes.DomainReloadExclude]
		protected static readonly Vector2 DefaultSize = new Vector2(100, 20);

		/// <summary>
		/// Create gameobject with component of the specified type.
		/// </summary>
		/// <typeparam name="T">Type of the component.</typeparam>
		/// <param name="parent">Parent of the created gameobject.</param>
		/// <param name="name">Gameobject name</param>
		/// <returns>Created gameobject.</returns>
		protected static T CreateObject<T>(Transform parent, string name = null)
			where T : MonoBehaviour
		{
			var go = new GameObject(name ?? parent.gameObject.name);
			var rt = go.AddComponent<RectTransform>();
			rt.SetParent(parent, false);

			if (typeof(T) == typeof(TextAdapter))
			{
#if UIWIDGETS_TMPRO_SUPPORT
				var text = go.AddComponent<TMPro.TextMeshProUGUI>();
				InitTextComponent(text);
#else
				go.AddComponent<Text>();
#endif
			}

			rt.sizeDelta = DefaultSize;

			return go.AddComponent<T>();
		}

		/// <summary>
		/// Add layout element to gameobject.
		/// </summary>
		/// <param name="go">Gameobject.</param>
		/// <returns>Layout element.</returns>
		protected static LayoutElement AddLayoutElement(GameObject go)
		{
			var le = go.AddComponent<LayoutElement>();
			le.minWidth = 30;
			le.minHeight = 20;
			le.flexibleHeight = 0;
			le.flexibleWidth = 0;

			return le;
		}

		/// <summary>
		/// Create drop indicator.
		/// </summary>
		/// <param name="parent">Parent gameobject.</param>
		/// <returns>Drop indicator.</returns>
		protected static ListViewDropIndicator CreateDropIndicator(Transform parent)
		{
			var go = new GameObject("DropIndicator");
			var rt = go.AddComponent<RectTransform>();
			rt.sizeDelta = new Vector2(200, 2);
			rt.SetParent(parent, false);

			go.AddComponent<Image>();

			var drop = go.AddComponent<ListViewDropIndicator>();

			var le = go.AddComponent<LayoutElement>();
			le.ignoreLayout = true;

			go.SetActive(false);

			return drop;
		}

		/// <summary>
		/// Create cell.
		/// </summary>
		/// <param name="parent">Parent gameobject.</param>
		/// <param name="name">Cell name.</param>
		/// <param name="alignment">Cell layout alignment.</param>
		/// <returns>Cell transform.</returns>
		protected static Transform CreateCell(Transform parent, string name, TextAnchor alignment = TextAnchor.MiddleLeft)
		{
			var le = CreateObject<LayoutElement>(parent, name);
			le.minWidth = 100;

			var image = UIWidgets.Utilities.RequireComponent<Image>(le.gameObject);
#if UIWIDGETS_LEGACY_STYLE
			image.color = Color.black;
#else
			image.color = new Color(1f, 1f, 1f, 0f);
#endif

			var lg = UIWidgets.Utilities.RequireComponent<HorizontalLayoutGroup>(le.gameObject);
#if UNITY_5_5_OR_NEWER
			lg.childControlWidth = true;
			lg.childControlHeight = true;
#endif
			lg.childForceExpandWidth = false;
			lg.childForceExpandHeight = false;
			lg.childAlignment = alignment;
			lg.padding = new RectOffset(5, 5, 5, 5);
			lg.spacing = 5;

			return le.transform;
		}

		/// <summary>
		/// Add layout group to specified gameobject.
		/// </summary>
		/// <typeparam name="T">Type of the layout group,</typeparam>
		/// <param name="target">Gameobject.</param>
		/// <returns>Layout group.</returns>
		protected static T AddLayoutGroup<T>(GameObject target)
			where T : HorizontalOrVerticalLayoutGroup
		{
			var lg = UIWidgets.Utilities.RequireComponent<T>(target);
#if UNITY_5_5_OR_NEWER
			lg.childControlWidth = true;
			lg.childControlHeight = true;
#endif
			lg.childForceExpandWidth = false;
			lg.childForceExpandHeight = false;
			lg.padding = new RectOffset(5, 5, 5, 8);
			lg.spacing = 5;

			Compatibility.SetLayoutChildControlsSize(lg, true, true);

			return lg;
		}

		/// <summary>
		/// Add layout group for ListView component.
		/// </summary>
		/// <param name="target">Gameobject.</param>
		protected static void AddListViewLayoutGroup(GameObject target)
		{
			var lg = AddLayoutGroup<HorizontalLayoutGroup>(target);
			lg.childAlignment = TextAnchor.MiddleLeft;
		}

		/// <summary>
		/// Add layout group for ComboboxMultiselect component.
		/// </summary>
		/// <param name="target">Gameobject.</param>
		protected static void AddComboboxMultiselectLayoutGroup(GameObject target)
		{
			var lg = AddLayoutGroup<HorizontalLayoutGroup>(target);
			lg.padding = new RectOffset(5, 35, 0, 0);
			lg.childAlignment = TextAnchor.MiddleLeft;
		}

		/// <summary>
		/// Add layout group for TileView component.
		/// </summary>
		/// <param name="target">Gameobject.</param>
		protected static void AddTileViewLayoutGroup(GameObject target)
		{
			var lg = AddLayoutGroup<VerticalLayoutGroup>(target);
			lg.childAlignment = TextAnchor.MiddleCenter;
		}

		/// <summary>
		/// Add listener to call the specified action.
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="action">Action.</param>
		protected static void AddListener(UnityEvent listener, UnityAction action)
		{
			UnityEventTools.AddPersistentListener(listener, action);
		}

		/// <summary>
		/// Set text style.
		/// </summary>
		/// <param name="text">Text component.</param>
		/// <param name="style">Font style.</param>
		protected static void SetTextStyle(Text text, FontStyle style)
		{
			text.fontStyle = style;
		}

		/// <summary>
		/// Set text alignment.
		/// </summary>
		/// <param name="text">Text component.</param>
		/// <param name="alignment">Alignment.</param>
		protected static void SetTextAlignment(Text text, TextAnchor alignment)
		{
			text.alignment = alignment;
		}

		/// <summary>
		/// Init text component.
		/// </summary>
		/// <param name="unused">Text component.</param>
		protected static void InitTextComponent(Text unused)
		{
			// do nothing
		}

		/// <summary>
		/// Generate ListView.
		/// </summary>
		/// <returns>ListView.</returns>
		protected abstract GameObject GenerateListView();

		/// <summary>
		/// Generate DragInfo.
		/// </summary>
		/// <returns>DragInfo.</returns>
		protected abstract GameObject GenerateDragInfo();

		/// <summary>
		/// Generate Combobox.
		/// </summary>
		/// <returns>combobox.</returns>
		protected abstract GameObject GenerateCombobox();

		/// <summary>
		/// Generate ComboboxMultiselect.
		/// </summary>
		/// <returns>ComboboxMultiselect</returns>
		protected abstract GameObject GenerateComboboxMultiselect();

		/// <summary>
		/// Generate Table.
		/// </summary>
		/// <returns>Table.</returns>
		protected abstract GameObject GenerateTable();

		/// <summary>
		/// Generate TileView.
		/// </summary>
		/// <returns>TileView.</returns>
		protected abstract GameObject GenerateTileView();

		/// <summary>
		/// Generate TreeView.
		/// </summary>
		/// <returns>TreeView.</returns>
		protected abstract GameObject GenerateTreeView();

		/// <summary>
		/// Generate TreeGraph.
		/// </summary>
		/// <returns>TreeGraph.</returns>
		protected abstract GameObject GenerateTreeGraph();

		/// <summary>
		/// Generate PickerListView.
		/// </summary>
		/// <returns>PickerListView.</returns>
		protected abstract GameObject GeneratePickerListView();

		/// <summary>
		/// Generate PickerTreeView.
		/// </summary>
		/// <returns>PickerTreeView.</returns>
		protected abstract GameObject GeneratePickerTreeView();

		/// <summary>
		/// Generate Autocomplete.
		/// </summary>
		/// <returns>Autocomplete.</returns>
		protected abstract GameObject GenerateAutocomplete();

		/// <summary>
		/// Generate AutoCombobox.
		/// </summary>
		/// <returns>AutoCombobox.</returns>
		protected abstract GameObject GenerateAutoCombobox();

		/// <summary>
		/// Generate Tooltip.
		/// </summary>
		/// <returns>Tooltip.</returns>
		protected abstract GameObject GenerateTooltip();

		/// <summary>
		/// Generate test scene content.
		/// </summary>
		protected abstract void GenerateSceneContent();

#if UIWIDGETS_TMPRO_SUPPORT
		/// <summary>
		/// Init text component.
		/// </summary>
		/// <param name="text">Text component.</param>
		protected static void InitTextComponent(TMPro.TextMeshProUGUI text)
		{
			#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
			text.overflowMode = TMPro.TextOverflowModes.Truncate;
			#else
			text.OverflowMode = TMPro.TextOverflowModes.Truncate;
			#endif

			#if UIWIDGETS_TMPRO_3_2_OR_NEWER
			text.textWrappingMode = TMPro.TextWrappingModes.NoWrap;
			#else
			text.enableWordWrapping = false;
			#endif
			(text.transform as RectTransform).sizeDelta = DefaultSize;
		}

		/// <summary>
		/// Set text style.
		/// </summary>
		/// <param name="text">Text component.</param>
		/// <param name="style">Font style.</param>
		protected static void SetTextStyle(TMPro.TextMeshProUGUI text, FontStyle style)
		{
			text.fontStyle = ConvertStyle(style);
		}

		/// <summary>
		/// Set text alignment.
		/// </summary>
		/// <param name="text">Text component.</param>
		/// <param name="alignment">Alignment.</param>
		protected static void SetTextAlignment(TMPro.TextMeshProUGUI text, TextAnchor alignment)
		{
			text.alignment = ConvertAlignment(alignment);
		}

		/// <summary>
		/// Convert style.
		/// </summary>
		/// <param name="style">Unity font style.</param>
		/// <returns>TMPro font style.</returns>
		protected static TMPro.FontStyles ConvertStyle(FontStyle style)
		{
			switch (style)
			{
				case FontStyle.Normal:
					return TMPro.FontStyles.Normal;
				case FontStyle.Bold:
					return TMPro.FontStyles.Bold;
				case FontStyle.Italic:
					return TMPro.FontStyles.Italic;
				case FontStyle.BoldAndItalic:
					return TMPro.FontStyles.Bold | TMPro.FontStyles.Italic;
				default:
					return TMPro.FontStyles.Normal;
			}
		}

		/// <summary>
		/// Convert text alignment.
		/// </summary>
		/// <param name="alignment">Unity text alignment.</param>
		/// <returns>TMPro text alignment.</returns>
		protected static TMPro.TextAlignmentOptions ConvertAlignment(TextAnchor alignment)
		{
			switch (alignment)
			{
				// upper
				case TextAnchor.UpperLeft:
					return TMPro.TextAlignmentOptions.TopLeft;
				case TextAnchor.UpperCenter:
					return TMPro.TextAlignmentOptions.Top;
				case TextAnchor.UpperRight:
					return TMPro.TextAlignmentOptions.TopRight;

				// middle
				case TextAnchor.MiddleLeft:
					return TMPro.TextAlignmentOptions.Left;
				case TextAnchor.MiddleCenter:
					return TMPro.TextAlignmentOptions.Center;
				case TextAnchor.MiddleRight:
					return TMPro.TextAlignmentOptions.Right;

				// lower
				case TextAnchor.LowerLeft:
					return TMPro.TextAlignmentOptions.BottomLeft;
				case TextAnchor.LowerCenter:
					return TMPro.TextAlignmentOptions.Bottom;
				case TextAnchor.LowerRight:
					return TMPro.TextAlignmentOptions.BottomRight;

				default:
					return TMPro.TextAlignmentOptions.TopLeft;
			}
		}
#endif
	}
}
#endif