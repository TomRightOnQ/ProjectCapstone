﻿namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UnityEngine;

	/// <summary>
	/// Templates for UI.
	/// </summary>
	/// <typeparam name="T">Type of template.</typeparam>
	public class Templates<T>
		where T : MonoBehaviour, ITemplatable
	{
		readonly Dictionary<string, T> templates = new Dictionary<string, T>();

		readonly Dictionary<string, List<T>> cache = new Dictionary<string, List<T>>();

		readonly Action<T> onCreateCallback;

		bool findTemplatesCalled;

		[DomainReloadExclude]
		static readonly Predicate<T> RemoveNull = x => x == null;

		/// <summary>
		/// Get cached instances.
		/// </summary>
		/// <param name="name">Template name.</param>
		/// <returns>Cached instances.</returns>
		public List<T> CachedInstances(string name)
		{
			var result = new List<T>();
			if (cache.TryGetValue(name, out var cached))
			{
				result.AddRange(cached);
			}

			return result;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Templates{T}"/> class.
		/// </summary>
		/// <param name="onCreateCallback">On create new UI object callback.</param>
		public Templates(Action<T> onCreateCallback = null)
		{
			this.onCreateCallback = onCreateCallback;
		}

		/// <summary>
		/// Finds the templates.
		/// </summary>
		public void FindTemplates()
		{
			findTemplatesCalled = true;

			foreach (var template in Resources.FindObjectsOfTypeAll<T>())
			{
				AddTemplate(template);
			}
		}

		void AddTemplate(T template)
		{
			Add(template.name, template, replace: true);
			template.gameObject.SetActive(false);
		}

		/// <summary>
		/// Clears the cached instance of templates.
		/// </summary>
		public void ClearCache()
		{
			foreach (var k in cache.Keys)
			{
				ClearCache(k);
			}
		}

		/// <summary>
		/// Clears the cached instance of specified template.
		/// </summary>
		/// <param name="name">Template name.</param>
		public void ClearCache(string name)
		{
			if (!cache.ContainsKey(name))
			{
				return;
			}

			foreach (var instance in cache[name])
			{
				DestroyCached(instance);
			}

			cache[name].Clear();
			cache[name].TrimExcess();
		}

		void DestroyCached(T cached)
		{
			if ((cached != null) && (cached.gameObject != null))
			{
				UnityEngine.Object.Destroy(cached.gameObject);
			}
		}

		/// <summary>
		/// Check if exists template with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>true if templates with the specified name exists; false otherwise.</returns>
		public bool Exists(string name)
		{
			return templates.ContainsKey(name);
		}

		/// <summary>
		/// Gets the template by name.
		/// </summary>
		/// <returns>The template.</returns>
		/// <param name="name">Template name.</param>
		public T Get(string name)
		{
			if (!Exists(name))
			{
				throw new ArgumentException(string.Format("Not found template with name '{0}'", name), "name");
			}

			return templates[name];
		}

		/// <summary>
		/// Deletes the template by name.
		/// </summary>
		/// <param name="name">Template name.</param>
		public void Delete(string name)
		{
			if (!Exists(name))
			{
				return;
			}

			templates.Remove(name);
			ClearCache(name);
		}

		/// <summary>
		/// Adds the template.
		/// </summary>
		/// <param name="name">Template name.</param>
		/// <param name="template">Template object.</param>
		/// <param name="replace">If set to <c>true</c> replace.</param>
		public void Add(string name, T template, bool replace = true)
		{
			if (Exists(name))
			{
				if (!replace)
				{
					throw new ArgumentException(string.Format("Template with name '{0}' already exists.", name), "name");
				}

				ClearCache(name);
				templates[name] = template;
			}
			else
			{
				templates.Add(name, template);
			}

			template.IsTemplate = true;
			template.TemplateName = name;
		}

		/// <summary>
		/// Return instance by the specified template name.
		/// </summary>
		/// <param name="name">Template name.</param>
		/// <returns>New template instance.</returns>
		public T Instance(string name)
		{
			if (!findTemplatesCalled)
			{
				FindTemplates();
			}

			if (!Exists(name))
			{
				throw new ArgumentException(string.Format("Not found template with name '{0}'", name), "name");
			}

			if (templates[name] == null)
			{
				templates.Clear();
				FindTemplates();
			}

			T template;

			if (!cache.TryGetValue(name, out var cached))
			{
				cached = new List<T>();
				cache[name] = cached;
			}

			cached.RemoveAll(RemoveNull);

			if (cached.Count > 0)
			{
				var n = cached.Count - 1;
				template = cached[n];
				cached.RemoveAt(n);
			}
			else
			{
				template = Compatibility.Instantiate(templates[name]);

				template.TemplateName = name;
				template.IsTemplate = false;

				if (onCreateCallback != null)
				{
					onCreateCallback(template);
				}
			}

			template.transform.SetParent(templates[name].transform.parent, false);

			return template;
		}

		/// <summary>
		/// Returns instance to the cache.
		/// </summary>
		/// <param name="instance">Instance.</param>
		public void ToCache(T instance)
		{
			instance.gameObject.SetActive(false);

			if (instance.TemplateName == null)
			{
				return;
			}

			if (!cache.TryGetValue(instance.TemplateName, out var cached))
			{
				cached = new List<T>();
				cache[instance.TemplateName] = cached;
			}

			cached.Add(instance);
		}

		/// <summary>
		/// Get all cached instances.
		/// </summary>
		/// <param name="output">Output list.</param>
		public void GetAll(List<T> output)
		{
			foreach (var s in cache.Values)
			{
				output.AddRange(s);
			}
		}

		/// <summary>
		/// Get all cached instances.
		/// </summary>
		/// <returns>All cached instances.</returns>
		public List<T> GetAll()
		{
			var result = new List<T>();
			GetAll(result);
			return result;
		}
	}
}