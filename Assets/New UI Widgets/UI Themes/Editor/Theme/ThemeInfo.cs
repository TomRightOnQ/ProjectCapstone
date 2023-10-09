#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Theme info.
	/// </summary>
	public class ThemeInfo
	{
		/// <summary>
		/// Property.
		/// </summary>
		public class Property
		{
			/// <summary>
			/// Name.
			/// </summary>
			public readonly string Name;

			/// <summary>
			/// Value type.
			/// </summary>
			public Type ValueType
			{
				get
				{
					return attr.ValueType;
				}
			}

			readonly Func<Theme, object> getValue;

			readonly UIThemes.PropertyAttribute attr;

			private Property(string name, Func<Theme, object> getValue, UIThemes.PropertyAttribute attr)
			{
				Name = name;
				this.getValue = getValue;
				this.attr = attr;
			}

			/// <summary>
			/// Create field view.
			/// </summary>
			/// <param name="theme">Theme.</param>
			/// <returns>Field view.</returns>
			public FieldViewBase CreateFieldView(Theme theme)
			{
				return attr.Constructor.Invoke(new[] { attr.UndoName, getValue(theme) }) as FieldViewBase;
			}

			/// <summary>
			/// Extract value type from the type of value wrapper.
			/// </summary>
			/// <param name="valuesWrapperType">Type of values wrapper.</param>
			/// <returns>Value type.</returns>
			protected static Type PropertyValueType(Type valuesWrapperType)
			{
				while (valuesWrapperType != null)
				{
					if (valuesWrapperType.IsGenericType)
					{
						var args = valuesWrapperType.GenericTypeArguments;
						if (args.Length == 1 && typeof(Theme.ValuesWrapper<>).MakeGenericType(args) == valuesWrapperType)
						{
							return args[0];
						}
					}

					valuesWrapperType = valuesWrapperType.BaseType;
				}

				return null;
			}

			/// <summary>
			/// Create property.
			/// </summary>
			/// <param name="property">Property info.</param>
			/// <returns>Property.</returns>
			public static Property Create(PropertyInfo property)
			{
				var attrs = property.GetCustomAttributes(typeof(UIThemes.PropertyAttribute), true);
				foreach (var attr in attrs)
				{
					var vp = attr as UIThemes.PropertyAttribute;
					if (vp.ValueType == null)
					{
						Debug.LogErrorFormat(
							"{0}: fieldView value of [ThemeProperty] of {1} should be inherited from FieldView<TValue>.",
							property.DeclaringType,
							property.Name);
						return null;
					}

					if (vp.Constructor == null)
					{
						Debug.LogErrorFormat(
							"{0}: fieldView value of [ThemeProperty] of {1} should have public constuctor with arguments (string, Theme.ValuesWrapper<TValue>).",
							property.DeclaringType,
							property.Name);
						return null;
					}

					var property_value_type = PropertyValueType(property.PropertyType);
					if (vp.ValueType != property_value_type)
					{
						Debug.LogErrorFormat(
							"{0}: Property {1} and fieldView values type does not match: {2} != {3}.",
							property.DeclaringType,
							property.Name,
							property_value_type,
							vp.ValueType);
						return null;
					}

					return new Property(property.Name, t => property.GetValue(t), vp);
				}

				return null;
			}
		}

		List<Property> properties = new List<Property>();

		/// <summary>
		/// Properties.
		/// </summary>
		public IReadOnlyList<Property> Properties
		{
			get
			{
				return properties;
			}
		}

		private ThemeInfo(Type type)
		{
			LoadProperties(type);
		}

		/// <summary>
		/// Get property by name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Property.</returns>
		public virtual Property GetProperty(string name)
		{
			foreach (var p in Properties)
			{
				if (p.Name == name)
				{
					return p;
				}
			}

			return null;
		}

		void LoadProperties(Type type)
		{
			if (type.BaseType != null)
			{
				LoadProperties(type.BaseType);
			}

			foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
			{
				var vp = Property.Create(p);
				if (vp != null)
				{
					properties.Add(vp);
				}
			}
		}

		[DomainReloadExclude]
		static Dictionary<Type, ThemeInfo> cache = new Dictionary<Type, ThemeInfo>();

		/// <summary>
		/// Get theme information for the specified theme.
		/// </summary>
		/// <param name="theme">Theme.</param>
		/// <returns>Theme information.</returns>
		public static ThemeInfo Get(Theme theme)
		{
			var type = theme.GetType();

			if (cache.TryGetValue(type, out var properties))
			{
				return properties;
			}

			properties = new ThemeInfo(type);
			cache[type] = properties;

			return properties;
		}
	}
}
#endif