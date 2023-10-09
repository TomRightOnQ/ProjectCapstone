#if UNITY_EDITOR
namespace UIWidgets.WidgetGeneration
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Type info for widget generation.
	/// </summary>
	[Serializable]
	public class ClassInfo
	{
		/// <summary>
		/// Type.
		/// </summary>
		public Type InfoType;

		/// <summary>
		/// Is type can be used to create widget?
		/// </summary>
		public bool IsValid
		{
			get
			{
				return Fields.Count > 0;
			}
		}

		/// <summary>
		/// Is type has parameterless constructor?
		/// </summary>
		public bool ParameterlessConstructor
		{
			get
			{
				if (InfoType == null)
				{
					return false;
				}

				var is_so = typeof(ScriptableObject).IsAssignableFrom(InfoType);
				var constructor = InfoType.GetConstructor(Type.EmptyTypes);
				return (constructor != null) && !is_so;
			}
		}

		/// <summary>
		/// List of usable fields and properties.
		/// </summary>
		public List<ClassField> AllFields = new List<ClassField>();

		/// <summary>
		/// List of usable fields and properties.
		/// </summary>
		public List<ClassField> Fields = new List<ClassField>();

		/// <summary>
		/// Namespace.
		/// </summary>
		public string Namespace;

		/// <summary>
		/// Type name.
		/// </summary>
		public string FullTypeName;

		/// <summary>
		/// Short type name.
		/// </summary>
		public string ShortTypeName;

		/// <summary>
		/// Is TMPro package installed?
		/// </summary>
		public bool IsTMProText;

		/// <summary>
		/// Is TMPro package with InputField installed?
		/// </summary>
		public bool IsTMProInputField;

		/// <summary>
		/// Type of the text field (TMPro or Unity text).
		/// </summary>
		public Type TextFieldType;

		/// <summary>
		/// Type of TMP_InputField;
		/// </summary>
		public Type TMProInputField;

		/// <summary>
		/// Type of the InputField;
		/// </summary>
		public Type InputField;

		/// <summary>
		/// Type of the text of the InputField;
		/// </summary>
		public Type InputText;

		/// <summary>
		/// Field to use with Autocomplete.
		/// </summary>
		public string AutocompleteField;

		List<ClassField> textFields;

		/// <summary>
		/// The text fields.
		/// </summary>
		public List<ClassField> TextFields
		{
			get
			{
				if (textFields == null)
				{
					textFields = new List<ClassField>();
					foreach (var field in Fields)
					{
						if (field.IsText && (field.IsComparableNonGeneric || field.IsComparableGeneric))
						{
							textFields.Add(field);
						}
					}

					textFields.Sort(FieldsComparison);
				}

				return textFields;
			}
		}

		/// <summary>
		/// The text fields with IComparable{T} implementation.
		/// </summary>
		public List<ClassField> TextFieldsComparableGeneric
		{
			get
			{
				var result = new List<ClassField>();
				foreach (var field in TextFields)
				{
					if (field.IsComparableGeneric)
					{
						result.Add(field);
					}
				}

				result.Sort(FieldsComparison);

				return result;
			}
		}

		/// <summary>
		/// The text fields with IComparable implementation.
		/// </summary>
		public List<ClassField> TextFieldsComparableNonGeneric
		{
			get
			{
				var result = new List<ClassField>();
				foreach (var field in TextFields)
				{
					if (field.IsComparableNonGeneric && !field.IsComparableGeneric)
					{
						result.Add(field);
					}
				}

				result.Sort(FieldsComparison);

				return result;
			}
		}

		[DomainReloadExclude]
		static readonly Comparison<ClassField> FieldsComparison = (ClassField x, ClassField y) =>
		{
			return -(x.FieldType == typeof(string)).CompareTo(y.FieldType == typeof(string));
		};

		/// <summary>
		/// The first text field.
		/// </summary>
		public List<ClassField> TextFieldFirst
		{
			get
			{
				var fields = new List<ClassField>();
				if (TextFields.Count > 0)
				{
					fields.Add(TextFields[0]);
				}

				return fields;
			}
		}

		/// <summary>
		/// The first text field in the table or .
		/// </summary>
		public List<ClassField> TableFieldFirst
		{
			get
			{
				var first = TextFieldFirst;
				if (first.Count > 0)
				{
					return first;
				}

				return new List<ClassField>()
				{
					Fields[0],
				};
			}
		}

		/// <summary>
		/// Widgets namespace.
		/// </summary>
		public string WidgetsNamespace
		{
			get
			{
				return string.IsNullOrEmpty(Namespace) ? "UIWidgets.Custom." + ShortTypeName + "NS" : Namespace + ".Widgets";
			}
		}

		/// <summary>
		/// Scripts to create.
		/// </summary>
		public Dictionary<string, bool> Scripts = new Dictionary<string, bool>();

		/// <summary>
		/// Prefabs to create.
		/// </summary>
		public Dictionary<string, bool> Prefabs = new Dictionary<string, bool>();

		/// <summary>
		/// Scenes to create.
		/// </summary>
		public Dictionary<string, bool> Scenes = new Dictionary<string, bool>();

		/// <summary>
		/// Is type derived from UnityEngine.Object and not derived from ScriptableObject.
		/// </summary>
		public bool IsUnityObject
		{
			get;
			protected set;
		}

		/// <summary>
		/// GUID for the PrefabsMenu.
		/// </summary>
		public string PrefabsMenuGUID;

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			var fields = string.Empty;

			foreach (var field in Fields)
			{
				fields += field.FieldName + ", ";
			}

			return FullTypeName + " -> " + fields;
		}

		/// <summary>
		/// Errors.
		/// </summary>
		protected List<string> errors = new List<string>();

		/// <summary>
		/// Errors.
		/// </summary>
		public List<string> Errors
		{
			get
			{
				return new List<string>(errors);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassInfo"/> class.
		/// </summary>
		/// <param name="script">Script.</param>
		public ClassInfo(MonoScript script)
		{
			if (script == null)
			{
				errors.Add("Script is not specified.");
				return;
			}

			var type = script.GetClass();
			if (type == null)
			{
				var error = string.Format(
					"Type not found in the script {0}.\nPlease make sure that the type name match with a file name.\n<b>Known Problems:</b> MonoScript.GetClass() return null for the 'struct' and 'interface' types\n inside a namespace with some Unity versions.\n<b>Workaround:</b> temporarily change 'interface' or 'struct' to 'class' in the type definition\n or use Data Type field to specify the type.",
					script.name);
				errors.Add(error);
				return;
			}

			Process(type);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassInfo"/> class.
		/// </summary>
		/// <param name="typename">Type name.</param>
		public ClassInfo(string typename)
		{
			if (string.IsNullOrEmpty(typename))
			{
				errors.Add("Type name is not specified.");
				return;
			}

			var type = UtilitiesEditor.GetType(typename);
			if (type == null)
			{
				errors.Add("Type with name " + typename + " not found.");
				return;
			}

			Process(type);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassInfo"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		public ClassInfo(Type type)
		{
			Process(type);
		}

		void Process(Type type)
		{
			if (type == null)
			{
				errors.Add("Type is not specified.");
				return;
			}

			var is_mono = typeof(MonoBehaviour).IsAssignableFrom(type);
			if (is_mono)
			{
				errors.Add("Data type should not be inherited from MonoBehaviour.");
				return;
			}

			// ignore Unity objects except scriptable objects
			var is_unity_object = typeof(UnityEngine.Object).IsAssignableFrom(type);
			var is_unity_scriptable_object = typeof(ScriptableObject).IsAssignableFrom(type);
			IsUnityObject = is_unity_object && !is_unity_scriptable_object;

			// ignore static types
			if (type.IsAbstract && type.IsSealed)
			{
				errors.Add("Type cannot be abstract and sealed.");
				return;
			}

			// ignore generic type
			if (type.IsGenericTypeDefinition)
			{
				errors.Add("Generic types not supported. Please use concrete types:\n" +
					"class ConcreteType : GenericType<SomeType> { }");
				return;
			}

			InfoType = type;

			Namespace = type.Namespace;
			FullTypeName = type.FullName;
			ShortTypeName = type.Name;

			InputField = typeof(UnityEngine.UI.InputField);
			InputText = typeof(UnityEngine.UI.Text);

#if UIWIDGETS_TMPRO_SUPPORT
			IsTMProText = true;
			TextFieldType = typeof(TMPro.TextMeshProUGUI);
			TMProInputField = UtilitiesEditor.GetType("TMPro.TMP_InputField");
			IsTMProInputField = TMProInputField != null;
			if (IsTMProInputField)
			{
				InputField = TMProInputField;
				InputText = TextFieldType;
			}
#else
			IsTMProText = false;
			TextFieldType = typeof(UnityEngine.UI.Text);
			TMProInputField = null;
			IsTMProInputField = false;
#endif

			var prohibited_names = GetProhibitedNames(type);
			AllFields = FillFields(type);

			foreach (var f in AllFields)
			{
				if (!f.HasAttribute(typeof(GeneratorIgnoreAttribute)) || prohibited_names.Contains(f.FieldName))
				{
					Fields.Add(f);
				}
			}

			AutocompleteField = FindAutocompleteField(AllFields);

			if (!IsValid)
			{
				errors.Add("Not found any fields and properties of supported types.");
			}
		}

		string FindAutocompleteField(List<ClassField> fields)
		{
			foreach (var field in fields)
			{
				if (field.HasAttribute(typeof(GeneratorAutocompleteAttribute)))
				{
					return field.FieldName;
				}
			}

			foreach (var field in fields)
			{
				if (field.AllowAutocomplete)
				{
					return field.FieldName;
				}
			}

			return null;
		}

		HashSet<string> GetProhibitedNames(Type type)
		{
			var prohibited_names = new HashSet<string>();

			if (typeof(ScriptableObject).IsAssignableFrom(type))
			{
				prohibited_names.Add("name");
				prohibited_names.Add("hideFlags");
			}

			return prohibited_names;
		}

		List<ClassField> FillFields(Type type)
		{
			var fields = new List<ClassField>();

			foreach (var field in type.GetFields())
			{
				var option = ClassField.Create(field);
				if (option != null)
				{
					fields.Add(option);
				}
			}

			foreach (var property in type.GetProperties())
			{
				var option = ClassField.Create(property);
				if (option != null)
				{
					fields.Add(option);
				}
			}

			return fields;
		}
	}
}
#endif