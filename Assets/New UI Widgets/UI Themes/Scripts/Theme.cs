﻿namespace UIThemes
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
#if UIWIDGETS_TMPRO_SUPPORT && UIWIDGETS_TMPRO_4_0_OR_NEWER
	using FontAsset = UnityEngine.TextCore.Text.FontAsset;
#elif UIWIDGETS_TMPRO_SUPPORT
	using FontAsset = TMPro.TMP_FontAsset;
#else
	using FontAsset = UnityEngine.ScriptableObject;
#endif

	/// <summary>
	/// Theme.
	/// </summary>
	[Serializable]
	public partial class Theme : ScriptableObject
	{
		/// <summary>
		/// Variations.
		/// </summary>
		[SerializeField]
		protected List<Variation> variations = new List<Variation>();

		/// <summary>
		/// Variations.
		/// </summary>
		public IReadOnlyList<Variation> Variations
		{
			get
			{
				return variations;
			}
		}

		/// <summary>
		/// Active variation ID.
		/// </summary>
		[SerializeField]
		protected VariationId activeVariationId = VariationId.None;

		[NonSerialized]
		VariationId previousActiveVariationId = VariationId.None;

		/// <summary>
		/// Active variation ID.
		/// </summary>
		public VariationId ActiveVariationId
		{
			get
			{
				return activeVariationId;
			}

			set
			{
				if (!HasVariation(value))
				{
					throw new ArgumentException("Theme does not have variation: " + value, nameof(value));
				}

				if (activeVariationId != value)
				{
					#if UNITY_EDITOR
					if (Application.isPlaying && (previousActiveVariationId == VariationId.None))
					{
						previousActiveVariationId = activeVariationId;
						UnityEditor.EditorApplication.playModeStateChanged += EditorPlayModeStateChanged;
					}
					#endif

					activeVariationId = value;
					CurrentVariationChanged();
				}
			}
		}

		#if UNITY_EDITOR
		void EditorPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
		{
			if (state == UnityEditor.PlayModeStateChange.EnteredEditMode)
			{
				UnityEditor.EditorApplication.playModeStateChanged -= EditorPlayModeStateChanged;

				if (previousActiveVariationId != VariationId.None)
				{
					activeVariationId = previousActiveVariationId;
					previousActiveVariationId = VariationId.None;
					CurrentVariationChanged();
				}
			}
		}
		#endif

		/// <summary>
		/// Initial variation ID.
		/// </summary>
		[SerializeField]
		protected VariationId initialVariationId = VariationId.None;

		/// <summary>
		/// Initial variation ID.
		/// </summary>
		public VariationId InitialVariationId
		{
			get
			{
				return initialVariationId;
			}

			set
			{
				if (!HasVariation(value))
				{
					throw new ArgumentException("Theme does not have variation: " + value, nameof(value));
				}

				if (initialVariationId != value)
				{
					initialVariationId = value;
				}
			}
		}

		/// <summary>
		/// Colors comparer.
		/// </summary>
		protected IEqualityComparer<Color> colorsComparer = Color32Comparer.Instance;

		/// <summary>
		/// Colors comparer.
		/// </summary>
		public virtual IEqualityComparer<Color> ColorsComparer
		{
			get
			{
				return colorsComparer;
			}

			set
			{
				colorsComparer = value;
				ColorsTable.Comparer = value;
			}
		}

		/// <summary>
		/// Colors.
		/// </summary>
		[SerializeField]
		protected ValuesTable<Color> ColorsTable = new ValuesTable<Color>();

		/// <summary>
		/// Colors.
		/// </summary>
		[Property(typeof(ColorFieldView), "UI Themes: Change Color")]
		public ValuesWrapper<Color> Colors
		{
			get
			{
				if (!ColorsTable.HasComparer)
				{
					ColorsTable.Comparer = ColorsComparer;
				}

				return new ValuesWrapper<Color>(this, ColorsTable, Color.white);
			}
		}

		/// <summary>
		/// Sprites.
		/// </summary>
		[SerializeField]
		protected ValuesTable<Sprite> SpritesTable = new ValuesTable<Sprite>();

		/// <summary>
		/// Sprites.
		/// </summary>
		[Property(typeof(ObjectFieldView<Sprite>), "UI Themes: Change Sprite")]
		public ValuesWrapper<Sprite> Sprites
		{
			get
			{
				if (!SpritesTable.HasComparer)
				{
					SpritesTable.Comparer = UnityObjectComparer<Sprite>.Instance;
				}

				return new ValuesWrapper<Sprite>(this, SpritesTable);
			}
		}

		/// <summary>
		/// Textures.
		/// </summary>
		[SerializeField]
		protected ValuesTable<Texture> TexturesTable = new ValuesTable<Texture>();

		/// <summary>
		/// Textures.
		/// </summary>
		[Property(typeof(ObjectFieldView<Texture>), "UI Themes: Change Texture")]
		public ValuesWrapper<Texture> Textures
		{
			get
			{
				if (!TexturesTable.HasComparer)
				{
					TexturesTable.Comparer = UnityObjectComparer<Texture>.Instance;
				}

				return new ValuesWrapper<Texture>(this, TexturesTable);
			}
		}

		/// <summary>
		/// Fonts.
		/// </summary>
		[SerializeField]
		protected ValuesTable<Font> FontsTable = new ValuesTable<Font>();

		/// <summary>
		/// Fonts.
		/// </summary>
		[Property(typeof(ObjectFieldView<Font>), "UI Themes: Change Font")]
		public ValuesWrapper<Font> Fonts
		{
			get
			{
				if (!FontsTable.HasComparer)
				{
					FontsTable.Comparer = UnityObjectComparer<Font>.Instance;
				}

				return new ValuesWrapper<Font>(this, FontsTable);
			}
		}

		/// <summary>
		/// TMPro fonts.
		/// </summary>
		[SerializeField]
		protected ValuesTable<FontAsset> FontsTMProTable = new ValuesTable<FontAsset>();

		/// <summary>
		/// TMPro fonts.
		/// </summary>
		[Property(typeof(ObjectFieldView<FontAsset>), "UI Themes: Change Font TMPro")]
		public ValuesWrapper<FontAsset> FontsTMPro
		{
			get
			{
				if (!FontsTMProTable.HasComparer)
				{
					FontsTMProTable.Comparer = UnityObjectComparer<FontAsset>.Instance;
				}

				return new ValuesWrapper<FontAsset>(this, FontsTMProTable);
			}
		}

		/// <summary>
		/// On change event.
		/// </summary>
		public event Action<VariationId> OnChange;

		bool isChanged;

		bool delayUpdateEvent;

		/// <summary>
		/// Get next variation ID.
		/// </summary>
		/// <returns>Variation ID.</returns>
		protected VariationId NextVariationId()
		{
			var id = 0;
			foreach (var v in variations)
			{
				id = Mathf.Max(id, v.Id.Id);
			}

			return new VariationId(id + 1);
		}

		/// <summary>
		/// Add variation.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Variation.</returns>
		public virtual Variation AddVariation(string name)
		{
			var variation = new Variation(NextVariationId(), name);
			variations.Add(variation);

			return variation;
		}

		/// <summary>
		/// Has variation with specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>true if has variation; otherwise false.</returns>
		public virtual bool HasVariation(string name)
		{
			return GetVariation(name) != null;
		}

		/// <summary>
		/// Has variation with specified ID.
		/// </summary>
		/// <param name="id">Variation ID.</param>
		/// <returns>true if has variation; otherwise false.</returns>
		public virtual bool HasVariation(VariationId id)
		{
			return GetVariation(id) != null;
		}

		/// <summary>
		/// Get variation by name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Variation.</returns>
		public virtual Variation GetVariation(string name)
		{
			foreach (var v in variations)
			{
				if (v.Name == name)
				{
					return v;
				}
			}

			return null;
		}

		/// <summary>
		/// Get variation by ID.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>Variation.</returns>
		public virtual Variation GetVariation(VariationId id)
		{
			foreach (var s in variations)
			{
				if (s.Id == id)
				{
					return s;
				}
			}

			return null;
		}

		/// <summary>
		/// Delete variation by ID.
		/// </summary>
		/// <param name="variationId">Variation ID.</param>
		/// <returns>true if variation was deleted; otherwise false.</returns>
		public virtual bool DeleteVariation(VariationId variationId)
		{
			return DeleteVariation(GetVariation(variationId));
		}

		/// <summary>
		/// Delete variation.
		/// </summary>
		/// <param name="variation">Variation.</param>
		/// <returns>true if variation was deleted; otherwise false.</returns>
		public virtual bool DeleteVariation(Variation variation)
		{
			if (variation == null)
			{
				return false;
			}

			if (variations.Count == 1)
			{
				return false;
			}

			if (variation.Id == activeVariationId)
			{
				ActiveVariationId = variations[0].Id;
			}

			var result = variations.Remove(variation);

			if (result)
			{
				Colors.DeleteVariation(variation.Id);
				Sprites.DeleteVariation(variation.Id);
			}

			return result;
		}

		/// <summary>
		/// Set active variation.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>true if was set active variation; otherwise false.</returns>
		public virtual bool SetActiveVariation(string name)
		{
			var variation = GetVariation(name);
			if (variation == null)
			{
				return false;
			}

			ActiveVariationId = variation.Id;
			return true;
		}

		/// <summary>
		/// Process the current variation changed event.
		/// </summary>
		protected virtual void CurrentVariationChanged()
		{
			if (delayUpdateEvent)
			{
				isChanged = true;
				return;
			}

			OnChange?.Invoke(activeVariationId);

			#if UNITY_EDITOR
			UtilitiesEditor.RefreshTargets();
			#endif
		}

		/// <summary>
		/// Process the variation value changed event.
		/// </summary>
		/// <param name="variationId">Variation ID.</param>
		protected virtual void VariationValuesChanged(VariationId variationId)
		{
			if (variationId == activeVariationId)
			{
				CurrentVariationChanged();
			}
		}

		/// <summary>
		/// Clear cache.
		/// </summary>
		public virtual void ClearCache()
		{
			ColorsTable.ClearCache();
			SpritesTable.ClearCache();
		}

		/// <summary>
		/// Get theme target type.
		/// </summary>
		/// <returns>Theme target type.</returns>
		public virtual Type GetTargetType()
		{
			return typeof(ThemeTarget);
		}

		/// <summary>
		/// Get unique variation name.
		/// </summary>
		/// <param name="baseName">Base name.</param>
		/// <returns>Unique variation name.</returns>
		protected virtual string UniqueVariationName(string baseName)
		{
			var name = baseName;
			var i = 1;
			while (HasVariation(name))
			{
				name = string.Format("{0} ({1})", baseName, i);
				i += 1;
			}

			return name;
		}

		/// <summary>
		/// Clone variation.
		/// </summary>
		/// <param name="source">Source variation.</param>
		/// <returns>New variation.</returns>
		public virtual Variation Clone(Variation source)
		{
			var name = UniqueVariationName(source.Name + " (Clone)");
			var destination = AddVariation(name);
			Copy(source, destination);

			return destination;
		}

		/// <summary>
		/// Copy variation data.
		/// </summary>
		/// <param name="source">Source variation.</param>
		/// <param name="destination">Destination variation.</param>
		public virtual void Copy(Variation source, Variation destination)
		{
			ColorsTable.Copy(source.Id, destination.Id);
			SpritesTable.Copy(source.Id, destination.Id);
			TexturesTable.Copy(source.Id, destination.Id);
			FontsTable.Copy(source.Id, destination.Id);

			#if UIWIDGETS_TMPRO_SUPPORT
			FontsTMProTable.Copy(source.Id, destination.Id);
			#endif
		}

		/// <summary>
		/// Begin update.
		/// OnChange event will be invoked with EndUpdate() if any change was made.
		/// </summary>
		public void BeginUpdate()
		{
			delayUpdateEvent = true;
		}

		/// <summary>
		/// End update.
		/// OnChange event will be invoked with EndUpdate() if any change was made.
		/// </summary>
		public void EndUpdate()
		{
			if (!delayUpdateEvent)
			{
				return;
			}

			delayUpdateEvent = false;

			if (isChanged)
			{
				CurrentVariationChanged();
			}
		}

		/// <summary>
		/// Validate variations.
		/// </summary>
		/// <param name="theme">Theme.</param>
		/// <returns>true if active variation was changed; otherwise false.</returns>
		public static bool ValidateVariations(Theme theme)
		{
			if (theme.Variations.Count == 0)
			{
				return false;
			}

			var changed = false;
			if (!theme.HasVariation(theme.ActiveVariationId))
			{
				theme.ActiveVariationId = theme.Variations[0].Id;
				changed = true;
			}

			if (!theme.HasVariation(theme.InitialVariationId))
			{
				theme.InitialVariationId = theme.Variations[0].Id;
				changed = true;
			}

			return changed;
		}
	}
}