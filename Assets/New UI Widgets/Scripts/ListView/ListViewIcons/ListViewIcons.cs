namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.l10n;
	using UnityEngine;

	/// <summary>
	/// ListViewIcons.
	/// </summary>
	public class ListViewIcons : ListViewCustom<ListViewIconsItemComponent, ListViewIconsItemDescription>
	{
		[SerializeField]
		[Tooltip("If enabled translates items names using Localization.GetTranslation().")]
		bool localizationSupport = true;

		/// <summary>
		/// Localization support.
		/// </summary>
		public bool LocalizationSupport
		{
			get
			{
				return localizationSupport;
			}

			set
			{
				localizationSupport = value;
			}
		}

		/// <summary>
		/// Sort items.
		/// Deprecated. Replaced with DataSource.Comparison.
		/// </summary>
		[Obsolete("Replaced with DataSource.Comparison.")]
		public override bool Sort
		{
			get
			{
				return DataSource.Comparison == ItemsComparison;
			}

			set
			{
				if (value)
				{
					DataSource.Comparison = ItemsComparison;
				}
				else
				{
					DataSource.Comparison = null;
				}
			}
		}

		static string GetLocalizedItemName(ListViewIconsItemDescription item)
		{
			if (item == null)
			{
				return string.Empty;
			}

			return item.LocalizedName ?? Localization.GetTranslation(item.Name);
		}

		static string GetItemName(ListViewIconsItemDescription item)
		{
			if (item == null)
			{
				return string.Empty;
			}

			return item.LocalizedName ?? item.Name;
		}

		[NonSerialized]
		bool isListViewIconsInited = false;

		/// <summary>
		/// Init this instance.
		/// </summary>
		public override void Init()
		{
			if (isListViewIconsInited)
			{
				return;
			}

			isListViewIconsInited = true;

			base.Init();

#pragma warning disable 0618
			if (base.Sort)
			{
				if (LocalizationSupport)
				{
					DataSource.Comparison = LocalizedItemsComparison;
				}
				else
				{
					DataSource.Comparison = ItemsComparison;
				}
			}
#pragma warning restore 0618
		}

		/// <summary>
		/// Process locale changes.
		/// </summary>
		public override void LocaleChanged()
		{
			base.LocaleChanged();

			if (DataSource.Comparison != null)
			{
				DataSource.CollectionChanged();
			}
		}

		/// <summary>
		/// Items comparison by localized names.
		/// </summary>
		/// <param name="x">First item.</param>
		/// <param name="y">Second item.</param>
		/// <returns>Result of the comparison.</returns>
		public static int LocalizedItemsComparison(ListViewIconsItemDescription x, ListViewIconsItemDescription y)
		{
			return UtilitiesCompare.Compare(GetLocalizedItemName(x), GetLocalizedItemName(y));
		}

		/// <summary>
		/// Items comparison.
		/// </summary>
		/// <param name="x">First item.</param>
		/// <param name="y">Second item.</param>
		/// <returns>Result of the comparison.</returns>
		public static int ItemsComparison(ListViewIconsItemDescription x, ListViewIconsItemDescription y)
		{
			return UtilitiesCompare.Compare(GetItemName(x), GetItemName(y));
		}
	}
}