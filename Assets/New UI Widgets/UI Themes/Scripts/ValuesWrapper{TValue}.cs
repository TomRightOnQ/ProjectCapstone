namespace UIThemes
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Theme.
	/// </summary>
	public partial class Theme : ScriptableObject
	{
		/// <summary>
		/// Theme values wrapper.
		/// </summary>
		/// <typeparam name="TValue">Type of value.</typeparam>
		public struct ValuesWrapper<TValue> : IValuesWrapper
		{
			Theme theme;

			/// <summary>
			/// Theme.
			/// </summary>
			public Theme Theme
			{
				get
				{
					return theme;
				}
			}

			ValuesTable<TValue> values;

			/// <summary>
			/// Options.
			/// </summary>
			public IReadOnlyList<Option> Options
			{
				get
				{
					return values.Options;
				}
			}

			/// <summary>
			/// Values comparer.
			/// </summary>
			public IEqualityComparer<TValue> Comparer
			{
				get
				{
					return values.Comparer;
				}

				set
				{
					values.Comparer = value;
				}
			}

			/// <summary>
			/// Has value comparer.
			/// </summary>
			public bool HasComparer
			{
				get
				{
					return values.HasComparer;
				}
			}

			/// <summary>
			/// Value type.
			/// </summary>
			public Type ValueType
			{
				get
				{
					return typeof(TValue);
				}
			}

			readonly TValue defaultValue;

			/// <summary>
			/// Initializes a new instance of the <see cref="ValuesWrapper{TValue}"/> struct.
			/// </summary>
			/// <param name="theme">Theme.</param>
			/// <param name="values">Theme values.</param>
			/// <param name="defaultValue">Default value.</param>
			public ValuesWrapper(Theme theme, ValuesTable<TValue> values, TValue defaultValue = default(TValue))
			{
				this.theme = theme;
				this.values = values;
				this.defaultValue = defaultValue;
			}

			/// <summary>
			/// Get value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="optionId">Option ID.</param>
			/// <returns>Value.</returns>
			public TValue Get(VariationId variationId, OptionId optionId)
			{
				return Get(variationId, optionId, defaultValue);
			}

			/// <summary>
			/// Get value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="optionName">Option name.</param>
			/// <returns>Value.</returns>
			public TValue Get(VariationId variationId, string optionName)
			{
				var option = GetOption(optionName);
				if (option == null)
				{
					throw new ArgumentException("Not found option with name: " + optionName, nameof(optionName));
				}

				return Get(variationId, option.Id, defaultValue);
			}

			/// <summary>
			/// Get value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="optionId">Option ID.</param>
			/// <param name="defaultValue">Default value.</param>
			/// <returns>Value.</returns>
			/// <exception cref="ArgumentException">Thrown if variation not found.</exception>
			public TValue Get(VariationId variationId, OptionId optionId, TValue defaultValue)
			{
				if (!theme.HasVariation(variationId))
				{
					throw new ArgumentException("Not found variation with id: " + variationId.Id, nameof(variationId));
				}

				return values.Get(variationId, optionId, defaultValue);
			}

			/// <summary>
			/// Set value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="optionId">Option ID.</param>
			/// <param name="value">Value.</param>
			/// <returns>true if value was changed; otherwise false.</returns>
			/// <exception cref="ArgumentException">Thrown if variation not found.</exception>
			public bool Set(VariationId variationId, OptionId optionId, TValue value)
			{
				if (!theme.HasVariation(variationId))
				{
					throw new ArgumentException("Not found variation with id: " + variationId.Id, nameof(variationId));
				}

				var changed = values.Set(variationId, optionId, value);
				if (changed)
				{
					theme.VariationValuesChanged(variationId);
				}

				return changed;
			}

			/// <summary>
			/// Has value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="optionId">Option ID.</param>
			/// <returns>true if has value; otherwise false.</returns>
			/// <exception cref="ArgumentException">Thrown if variation not found.</exception>
			public bool HasValue(VariationId variationId, OptionId optionId)
			{
				if (!theme.HasVariation(variationId))
				{
					throw new ArgumentException("Not found variation with id: " + variationId.Id, nameof(variationId));
				}

				return values.HasValue(variationId, optionId);
			}

			/// <summary>
			/// Move option.
			/// </summary>
			/// <param name="oldIndex">Old index.</param>
			/// <param name="newIndex">New index.</param>
			/// <returns>true if option was moved; otherwise false.</returns>
			public bool OptionMove(int oldIndex, int newIndex)
			{
				return values.OptionMove(oldIndex, newIndex);
			}

			/// <summary>
			/// Delete variation by ID.
			/// </summary>
			/// <param name="id">ID.</param>
			/// <returns>true if variation was deleted; otherwise false.</returns>
			public bool DeleteVariation(VariationId id)
			{
				var result = theme.DeleteVariation(id);
				if (result)
				{
					theme.VariationValuesChanged(theme.ActiveVariationId);
				}

				return result;
			}

			/// <summary>
			/// Delete option by ID.
			/// </summary>
			/// <param name="id">ID.</param>
			/// <returns>true if option was deleted; otherwise false.</returns>
			public bool DeleteOption(OptionId id)
			{
				var result = values.DeleteOption(id);
				if (result)
				{
					theme.VariationValuesChanged(theme.ActiveVariationId);
				}

				return result;
			}

			/// <summary>
			/// Add option.
			/// </summary>
			/// <param name="name">Option name.</param>
			/// <returns>Option.</returns>
			public Option AddOption(string name)
			{
				return AddOption(name, defaultValue);
			}

			/// <summary>
			/// Add option.
			/// </summary>
			/// <param name="name">Option name.</param>
			/// <param name="defaultValue">Default value.</param>
			/// <returns>Option.</returns>
			public Option AddOption(string name, TValue defaultValue)
			{
				var option = values.AddOption(name);

				foreach (var variation in theme.Variations)
				{
					values.Set(variation.Id, option.Id, defaultValue);
					theme.VariationValuesChanged(variation.Id);
				}

				return option;
			}

			/// <summary>
			/// Get option by name.
			/// </summary>
			/// <param name="name">Option name.</param>
			/// <returns>Option.</returns>
			public Option GetOption(string name)
			{
				return values.GetOption(name);
			}

			/// <summary>
			/// Get option by ID.
			/// </summary>
			/// <param name="optionId">Option ID.</param>
			/// <returns>Option.</returns>
			public Option GetOption(OptionId optionId)
			{
				return values.GetOption(optionId);
			}

			/// <summary>
			/// Has option with the specified name.
			/// </summary>
			/// <param name="name">Option name.</param>
			/// <returns>true if has option with the specified name; otherwise false.</returns>
			public bool HasOption(string name)
			{
				return values.HasOption(name);
			}

			/// <summary>
			/// Has option with the specified ID.
			/// </summary>
			/// <param name="optionId">Option ID.</param>
			/// <returns>true if has option with the specified ID; otherwise false.</returns>
			public bool HasOption(OptionId optionId)
			{
				return values.HasOption(optionId);
			}

			/// <summary>
			/// Find option with the specified value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="value">Value.</param>
			/// <returns>Option ID.</returns>
			public OptionId FindOption(VariationId variationId, TValue value)
			{
				return values.FindOption(variationId, value);
			}

			/// <summary>
			/// Find option or create option with the specified value.
			/// </summary>
			/// <param name="variationId">Variation ID.</param>
			/// <param name="value">Value.</param>
			/// <param name="optionName">Option name.</param>
			/// <returns>Option ID.</returns>
			public OptionId RequireOption(VariationId variationId, TValue value, string optionName)
			{
				return values.RequireOption(variationId, value, optionName);
			}
		}
	}
}