namespace UIThemes
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Registry of properties created with reflection.
	/// </summary>
	public static class ReflectionWrappersRegistry
	{
		static Dictionary<Type, HashSet<string>> types = new Dictionary<Type, HashSet<string>>();

		/// <summary>
		/// Add.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="property">Property or field name.</param>
		public static void Add(Type type, string property)
		{
			if (!types.TryGetValue(type, out var properties))
			{
				properties = new HashSet<string>();
				types[type] = properties;
			}

			properties.Add(property);
		}

		/// <summary>
		/// Get all registered properties.
		/// </summary>
		/// <returns>Registered properties.</returns>
		public static IReadOnlyDictionary<Type, IReadOnlyCollection<string>> All()
		{
			var result = new Dictionary<Type, IReadOnlyCollection<string>>();
			foreach (var item in types)
			{
				result[item.Key] = item.Value;
			}

			return result;
		}
	}
}