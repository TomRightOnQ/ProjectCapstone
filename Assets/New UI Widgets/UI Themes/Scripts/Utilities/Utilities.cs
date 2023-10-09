﻿namespace UIThemes
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Utilities.
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Asset label for sprites for exclude from ThemeTarget list.
		/// </summary>
		public static readonly string LabelExclude = "ui-themes-exclude";

		[DomainReloadExclude]
		static List<MethodInfo> staticMethods = null;

		static List<MethodInfo> GetStaticMethods()
		{
			if (staticMethods == null)
			{
				staticMethods = new List<MethodInfo>();

				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					AssemblyStaticMethods(assembly, staticMethods);
				}
			}

			return staticMethods;
		}

		static void AssemblyStaticMethods(Assembly assembly, List<MethodInfo> methods)
		{
			Type[] types;
			try
			{
				types = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				types = e.Types;
			}

			foreach (var type in types)
			{
				TypeStaticMethods(type, methods);
			}
		}

		static void TypeStaticMethods(Type type, List<MethodInfo> methods)
		{
			var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			if (!type.IsClass)
			{
				return;
			}

			foreach (var method in type.GetMethods(flags))
			{
				methods.Add(method);
			}
		}

		[DomainReloadExclude]
		static Dictionary<Type, List<MethodInfo>> StaticMethodsOfAttribute = new Dictionary<Type, List<MethodInfo>>();

		/// <summary>
		/// Invoke static methods with the specified attributes.
		/// </summary>
		/// <typeparam name="TAttribute">Type of attribute.</typeparam>
		public static void InvokeStaticMethods<TAttribute>()
			where TAttribute : Attribute
		{
			var attr_type = typeof(TAttribute);
			if (!StaticMethodsOfAttribute.TryGetValue(attr_type, out var methods))
			{
				methods = FilterStaticMethods<TAttribute>(GetStaticMethods());

				StaticMethodsOfAttribute[attr_type] = methods;
			}

			foreach (var method in methods)
			{
				method.Invoke(null, null);
			}
		}

		static List<MethodInfo> FilterStaticMethods<TAttribute>(List<MethodInfo> methods)
			where TAttribute : Attribute
		{
			var result = new List<MethodInfo>();

			foreach (var method in methods)
			{
				if (method.GetCustomAttribute<TAttribute>() == null)
				{
					continue;
				}

				var p = method.GetParameters();
				if (p.Length != 0)
				{
					continue;
				}

				result.Add(method);
			}

			return result;
		}

		[DomainReloadExclude]
		static Dictionary<Tuple<Type, Type, Type>, List<MethodInfo>> GenericStaticMethodsOfAttribute = new Dictionary<Tuple<Type, Type, Type>, List<MethodInfo>>();

		/// <summary>
		/// Invoke static methods with the specified attributes.
		/// </summary>
		/// <typeparam name="TAttribute">Type of attribute.</typeparam>
		/// <typeparam name="TArgument0">Type of the first argument.</typeparam>
		/// <typeparam name="TArgument1">Type of the second argument.</typeparam>
		/// <param name="args0">First argument.</param>
		/// <param name="args1">Second argument.</param>
		public static void InvokeStaticMethods<TAttribute, TArgument0, TArgument1>(TArgument0 args0, TArgument1 args1)
			where TAttribute : Attribute
		{
			var key = new Tuple<Type, Type, Type>(typeof(TAttribute), typeof(TArgument0), typeof(TArgument1));
			if (!GenericStaticMethodsOfAttribute.TryGetValue(key, out var methods))
			{
				methods = FilterStaticMethods<TAttribute, TArgument0, TArgument1>(GetStaticMethods());

				GenericStaticMethodsOfAttribute[key] = methods;
			}

			var args = new object[] { args0, args1 };
			foreach (var method in methods)
			{
				method.Invoke(null, args);
			}
		}

		static List<MethodInfo> FilterStaticMethods<TAttribute, TArgument0, TArgument1>(List<MethodInfo> methods)
			where TAttribute : Attribute
		{
			var result = new List<MethodInfo>();

			foreach (var method in methods)
			{
				if (!IsValidMethod<TAttribute, TArgument0, TArgument1>(method))
				{
					continue;
				}

				result.Add(method);
			}

			return result;
		}

		static bool IsValidMethod<TAttribute, TArgument0, TArgument1>(MethodInfo method)
			where TAttribute : Attribute
		{
			var p = method.GetParameters();
			if (p.Length != 2)
			{
				return false;
			}

			if (p[0].ParameterType != typeof(TArgument0))
			{
				return false;
			}

			if (p[1].ParameterType != typeof(TArgument1))
			{
				return false;
			}

			if (method.GetCustomAttribute<TAttribute>() == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get root gameobjects at all opened scenes.
		/// </summary>
		/// <returns>Gameobjects.</returns>
		public static List<GameObject> GetRootGameObjects()
		{
			var result = new List<GameObject>();

			for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
			{
				var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
				result.AddRange(scene.GetRootGameObjects());
			}

			return result;
		}

		/// <summary>
		/// Set owner of the property.
		/// </summary>
		/// <typeparam name="TComponent">Type of component.</typeparam>
		/// <param name="propertyType">Property type.</param>
		/// <param name="components">Components.</param>
		/// <param name="property">Property.</param>
		/// <param name="owner">Owner.</param>
		public static void SetTargetOwner<TComponent>(Type propertyType, IReadOnlyList<TComponent> components, string property, Component owner)
			where TComponent : Component
		{
			foreach (var component in components)
			{
				if (component == null)
				{
					continue;
				}

				if (component.TryGetComponent<ThemeTargetBase>(out var target))
				{
					target.SetPropertyOwner(propertyType, component, property, owner);
					if ((owner == null) && Application.isPlaying)
					{
						target.Refresh();
					}
				}
			}
		}

		/// <summary>
		/// Set owner of the property.
		/// </summary>
		/// <typeparam name="TComponent">Type of component.</typeparam>
		/// <param name="propertyType">Property type.</param>
		/// <param name="component">Component.</param>
		/// <param name="property">Property.</param>
		/// <param name="owner">Owner.</param>
		public static void SetTargetOwner<TComponent>(Type propertyType, TComponent component, string property, Component owner)
			where TComponent : Component
		{
			if (component == null)
			{
				return;
			}

			if (component.TryGetComponent<ThemeTargetBase>(out var target))
			{
				target.SetPropertyOwner(propertyType, component, property, owner);
				if ((owner == null) && Application.isPlaying)
				{
					target.Refresh();
				}
			}
		}

#if UNITY_EDITOR
		[DomainReloadExclude]
		static Dictionary<int, Tuple<bool, DateTime>> whiteSprites = new Dictionary<int, Tuple<bool, DateTime>>();
#endif

		/// <summary>
		/// Should attach sprite?
		/// Editor only.
		/// </summary>
		/// <param name="sprite">Sprite.</param>
		/// <param name="color">Color.</param>
		/// <returns>true if should attach sprite; otherwise false.</returns>
		public static bool ShouldAttachSprite(Sprite sprite, Color color)
		{
			if (sprite == null)
			{
				return true;
			}

			if (color != Color.white)
			{
				return true;
			}

			if (Mathf.Approximately(color.a, 0f))
			{
				return true;
			}

			return
#if UNITY_EDITOR
				IsWhiteSprite(sprite);
#else
				true;
#endif
		}

		/// <summary>
		/// Is white sprite?
		/// Editor only.
		/// </summary>
		/// <param name="sprite">Sprite.</param>
		/// <returns>true if sprite is white; otherwise false.</returns>
		public static bool IsWhiteSprite(Sprite sprite)
		{
#if !UNITY_EDITOR
			return false;
#else
			if (sprite == null)
			{
				return false;
			}

			var id = sprite.GetInstanceID();
			var path = UnityEditor.AssetDatabase.GetAssetPath(sprite);

			// built-in sprites
			if (!System.IO.File.Exists(path))
			{
				return false;
			}

			var time = System.IO.File.GetLastWriteTimeUtc(path);
			if (whiteSprites.TryGetValue(id, out var cache))
			{
				var (cache_is_white, cache_changed_at) = cache;
				if (cache_changed_at == time)
				{
					return cache_is_white;
				}
			}

			var texture = new Texture2D(sprite.texture.width, sprite.texture.height);
			texture.LoadImage(System.IO.File.ReadAllBytes(path));

			var is_white = IsWhite(texture, sprite.rect);
			whiteSprites[id] = new Tuple<bool, DateTime>(is_white, time);

			return is_white;
#endif
		}

		private static bool IsWhite(Texture2D texture, Rect rect)
		{
			var colors = texture.GetPixels(Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));
			foreach (var color in colors)
			{
				if (Mathf.Approximately(color.a, 0f))
				{
					continue;
				}

				if (!IsWhite(color))
				{
					return false;
				}
			}

			return true;
		}

		private static bool IsWhite(Color a)
		{
			return a.r == 1f && a.g == 1f && a.b == 1f;
		}

		/// <summary>
		/// Is sprite should be excluded from theme control.
		/// </summary>
		/// <param name="sprite">Sprite.</param>
		/// <returns>true if sprite should be excluded from theme control; otherwise false.</returns>
		public static bool IsExcludedSprite(Sprite sprite)
		{
#if !UNITY_EDITOR
			return false;
#else
			if (sprite == null)
			{
				return true;
			}

			var culture = CultureInfo.InvariantCulture;

			var id = sprite.GetInstanceID();
			var path = UnityEditor.AssetDatabase.GetAssetPath(sprite);
			var guid = UnityEditor.AssetDatabase.GUIDFromAssetPath(path);
			var labels = UnityEditor.AssetDatabase.GetLabels(guid);

			foreach (var l in labels)
			{
				if (culture.CompareInfo.Compare(l, LabelExclude, CompareOptions.IgnoreCase) == 0)
				{
					return true;
				}
			}

			return false;
#endif
		}
	}
}