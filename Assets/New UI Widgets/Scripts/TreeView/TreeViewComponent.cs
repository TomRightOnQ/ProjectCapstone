namespace UIWidgets
{
	using System;
	using UIWidgets.l10n;
	using UnityEngine;

	/// <summary>
	/// TreeView component.
	/// </summary>
	public class TreeViewComponent : TreeViewComponentBase<TreeViewItem>, ILocalizationSupport
	{
		/// <summary>
		/// Observe item changes.
		/// </summary>
		[SerializeField]
		protected bool ObserveItem = false;

		[SerializeField]
		[Tooltip("If enabled translates item name using Localization.GetTranslation().")]
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

		TreeViewItem item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public TreeViewItem Item
		{
			get
			{
				return item;
			}

			set
			{
				if ((item != null) && ObserveItem)
				{
					item.OnChange -= UpdateView;
				}

				item = value;

				if ((item != null) && ObserveItem)
				{
					item.OnChange += UpdateView;
				}

				UpdateView();
			}
		}

		/// <inheritdoc/>
		public override void SetData(TreeNode<TreeViewItem> node, int depth)
		{
			#if UNITY_EDITOR
			name = node.Item.Name;
			#endif

			Node = node;
			base.SetData(Node, depth);

			Item = (Node == null) ? null : Node.Item;
		}

		/// <inheritdoc/>
		public override void LocaleChanged()
		{
			UpdateName();
		}

		/// <summary>
		/// Update display name.
		/// </summary>
		protected virtual void UpdateName()
		{
			if (Item == null)
			{
				return;
			}

			TextAdapter.text = Item.LocalizedName ?? (LocalizationSupport ? Localization.GetTranslation(Item.Name) : Item.Name);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			if ((Icon == null) || (TextAdapter == null))
			{
				return;
			}

			if (Item == null)
			{
				Icon.sprite = null;
				TextAdapter.text = string.Empty;
			}
			else
			{
				Icon.sprite = Item.Icon;
				UpdateName();
			}

			if (SetNativeSize)
			{
				Icon.SetNativeSize();
			}

			Icon.enabled = Icon.sprite != null;
		}

		/// <inheritdoc/>
		public override void MovedToCache()
		{
			if (Icon != null)
			{
				Icon.sprite = null;
			}
		}

		/// <inheritdoc/>
		protected override void OnDestroy()
		{
			if (item != null)
			{
				item.OnChange -= UpdateView;
			}

			base.OnDestroy();
		}
	}
}