﻿namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.l10n;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for custom combobox.
	/// </summary>
	/// <typeparam name="TListViewCustom">Type of ListView.</typeparam>
	/// <typeparam name="TItemView">Type of ListView component.</typeparam>
	/// <typeparam name="TItem">Type of ListView item.</typeparam>
	public class ComboboxCustom<TListViewCustom, TItemView, TItem> : MonoBehaviour, ISubmitHandler, IStylable
			where TListViewCustom : ListViewCustom<TItemView, TItem>
			where TItemView : ListViewItem
	{
		/// <summary>
		/// Custom Combobox event.
		/// </summary>
		[Serializable]
		public class ComboboxCustomEvent : UnityEvent<int, TItem>
		{
		}

		[SerializeField]
		TListViewCustom listView;

		/// <summary>
		/// Gets or sets the ListView.
		/// </summary>
		/// <value>ListView component.</value>
		public TListViewCustom ListView
		{
			get
			{
				return listView;
			}

			set
			{
				ListViewSubscribe();
				listView = value;
				ListViewUnsubscribe();
			}
		}

		[SerializeField]
		Button toggleButton;

		/// <summary>
		/// Gets or sets the toggle button.
		/// </summary>
		/// <value>The toggle button.</value>
		public Button ToggleButton
		{
			get
			{
				return toggleButton;
			}

			set
			{
				SetToggleButton(value);
			}
		}

		[SerializeField]
		TItemView current;

		/// <summary>
		/// Gets or sets the current component.
		/// </summary>
		/// <value>The current.</value>
		public TItemView Current
		{
			get
			{
				return current;
			}

			set
			{
				SetCurrent(value);
			}
		}

		/// <summary>
		/// Is Current implements IViewData{TItem}.
		/// </summary>
		protected bool CanSetData;

		/// <summary>
		/// Parent canvas.
		/// </summary>
		[SerializeField]
		public RectTransform ParentCanvas;

		/// <summary>
		/// Combobox original position.
		/// </summary>
		protected HierarchyPosition ComboboxPosition;

		/// <summary>
		/// ListView original position.
		/// </summary>
		protected HierarchyPosition ListViewPosition;

		/// <summary>
		/// The components list.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<TItemView> components = new List<TItemView>();

		/// <summary>
		/// The components cache list.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		protected List<TItemView> componentsCache = new List<TItemView>();

		/// <summary>
		/// Hide ListView on item select or deselect.
		/// </summary>
		[SerializeField]
		[Tooltip("Hide ListView on item select or deselect.")]
		public bool HideAfterItemToggle = true;

		/// <summary>
		/// Hide ListView on Combobox submit.
		/// </summary>
		[SerializeField]
		public bool ShowListViewOnSubmit = true;

		/// <summary>
		/// OnSelect event.
		/// </summary>
		[Obsolete("Use Combobox.ListView.OnSelect instead.")]
		public ComboboxCustomEvent OnSelect = new ComboboxCustomEvent();

		/// <summary>
		/// Raised when ListView opened.
		/// </summary>
		[SerializeField]
		public UnityEvent OnShowListView = new UnityEvent();

		/// <summary>
		/// Raised when ListView closed.
		/// </summary>
		[SerializeField]
		public UnityEvent OnHideListView = new UnityEvent();

		/// <summary>
		/// Raised when click on current.
		/// </summary>
		[SerializeField]
		public ComboboxCustomEvent OnCurrentClick = new ComboboxCustomEvent();

		/// <summary>
		/// The modal ID.
		/// </summary>
		protected InstanceID? ModalKey;

		[NonSerialized]
		bool isComboboxCustomInited;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			Init();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public virtual void Init()
		{
			if (isComboboxCustomInited)
			{
				return;
			}

			isComboboxCustomInited = true;

			CanSetData = current is IViewData<TItem>;

			SetToggleButton(toggleButton);

			ListViewSubscribe();

			Current.SetThemeImagesPropertiesOwner(this);

			if (listView != null)
			{
				Current.Owner = ListView;
				Current.ComboboxInstance = true;

				current.gameObject.SetActive(false);

				listView.OnSelectInternal.RemoveListener(UpdateView);
				listView.OnDeselectInternal.RemoveListener(UpdateView);
				listView.OnUpdateView.RemoveListener(UpdateViewBase);

				if (ParentCanvas == null)
				{
					ParentCanvas = UtilitiesUI.FindTopmostCanvas(listView.transform.parent as RectTransform);
				}

				listView.gameObject.SetActive(true);
				listView.Init();
				if ((listView.SelectedIndex == -1) && (listView.DataSource.Count > 0) && (!listView.MultipleSelect))
				{
					listView.SelectedIndex = 0;
				}

				if (listView.SelectedIndex != -1)
				{
					UpdateViewBase();
				}

				InitCustomWidgets();

				listView.gameObject.SetActive(false);

				listView.OnSelectInternal.AddListener(UpdateView);
				listView.OnDeselectInternal.AddListener(UpdateView);
				listView.OnUpdateView.AddListener(UpdateViewBase);
			}
		}

		/// <summary>
		/// Init custom widgets.
		/// </summary>
		protected virtual void InitCustomWidgets()
		{
		}

		bool localeSubscription;

		/// <summary>
		/// Process the enable event.
		/// </summary>
		protected virtual void OnEnable()
		{
			if (!localeSubscription)
			{
				Init();

				localeSubscription = true;
				Localization.OnLocaleChanged += LocaleChanged;
				LocaleChanged();
			}
		}

		/// <summary>
		/// Process locale changes.
		/// </summary>
		public virtual void LocaleChanged()
		{
			Current.LocaleChanged();

			for (int i = 0; i < components.Count; i++)
			{
				components[i].LocaleChanged();
			}
		}

		/// <summary>
		/// Set new component to display current item.
		/// </summary>
		/// <param name="newCurrent">New component.</param>
		protected virtual void SetCurrent(TItemView newCurrent)
		{
			foreach (var c in components)
			{
				DeactivateComponent(c);
				Destroy(c);
			}

			components.Clear();

			foreach (var c in componentsCache)
			{
				Destroy(c);
			}

			componentsCache.Clear();

			current = newCurrent;
			current.SetThemeImagesPropertiesOwner(this);
			current.gameObject.SetActive(false);

			CanSetData = current is IViewData<TItem>;
		}

		/// <summary>
		/// Sets the toggle button.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetToggleButton(Button value)
		{
			if (toggleButton != null)
			{
				toggleButton.onClick.RemoveListener(ToggleList);
			}

			toggleButton = value;

			if (toggleButton != null)
			{
				toggleButton.onClick.AddListener(ToggleList);
			}
		}

		/// <summary>
		/// Sets the list view.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="isDestroying">Is destroying.</param>
		protected virtual void SetListView(TListViewCustom value, bool isDestroying)
		{
			ListViewSubscribe();
			listView = value;
			ListViewUnsubscribe();
		}

		/// <summary>
		/// Subscribe on ListView events.
		/// </summary>
		protected virtual void ListViewSubscribe()
		{
			if (listView == null)
			{
				return;
			}

			listView.KeepHighlight = false;

			listView.OnSelectInternal.AddListener(UpdateView);
			listView.OnDeselectInternal.AddListener(UpdateView);
			listView.OnUpdateView.AddListener(UpdateViewBase);

			listView.OnFocusOut.AddListener(OnFocusHideList);

			listView.onCancel.AddListener(OnListViewCancel);
			listView.InstancesEventsInternal.Cancel.AddListener(OnListViewCancel);
			listView.InstancesEventsInternal.PointerUp.AddListener(OnListViewPointerUp);

			AddDeselectCallbacks();
		}

		/// <summary>
		/// Unsubscribe off ListView events.
		/// </summary>
		/// <param name="restorePosition">Restore position.</param>
		protected virtual void ListViewUnsubscribe(bool restorePosition = true)
		{
			if (listView == null)
			{
				return;
			}

			if (restorePosition)
			{
				ListViewPosition.Restore();
			}

			listView.OnSelectInternal.RemoveListener(UpdateView);
			listView.OnDeselectInternal.RemoveListener(UpdateView);
			listView.OnUpdateView.RemoveListener(UpdateViewBase);

			listView.OnFocusOut.RemoveListener(OnFocusHideList);

			listView.onCancel.RemoveListener(OnListViewCancel);
			listView.InstancesEventsInternal.Cancel.RemoveListener(OnListViewCancel);
			listView.InstancesEventsInternal.PointerUp.RemoveListener(OnListViewPointerUp);

			RemoveDeselectCallbacks();
		}

		/// <summary>
		/// Set the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="allowDuplicate">If set to <c>true</c> allow duplicate.</param>
		/// <returns>Index of item.</returns>
		public virtual int Set(TItem item, bool allowDuplicate = true)
		{
			return listView.Set(item, allowDuplicate);
		}

		/// <summary>
		/// Clear ListView and selected item.
		/// </summary>
		public virtual void Clear()
		{
			listView.DataSource.Clear();
			UpdateView();
		}

		/// <summary>
		/// Toggles the list visibility.
		/// </summary>
		public virtual void ToggleList()
		{
			if (listView == null)
			{
				return;
			}

			if (listView.gameObject.activeSelf)
			{
				HideList();
			}
			else
			{
				ShowList();
			}
		}

		/// <summary>
		/// Show current values.
		/// </summary>
		public virtual void ShowCurrent()
		{
			for (int i = 0; i < components.Count; i++)
			{
				components[i].gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Hide current values.
		/// </summary>
		public virtual void HideCurrent()
		{
			for (int i = 0; i < components.Count; i++)
			{
				components[i].gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Process click on current.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void CurrentClick(ListViewItem item)
		{
			OnCurrentClick.Invoke(item.Index, ListView.DataSource[item.Index]);
		}

		/// <summary>
		/// Shows the list.
		/// </summary>
		public virtual void ShowList()
		{
			if (listView == null)
			{
				return;
			}

			ModalKey = ModalHelper.Open(this, null, new Color(0, 0, 0, 0f), HideList, ParentCanvas);

			if (ParentCanvas != null)
			{
				ComboboxPosition = HierarchyPosition.SetParent(transform, ParentCanvas);
				ListViewPosition = HierarchyPosition.SetParent(listView.transform, ParentCanvas);
			}

			listView.gameObject.SetActive(true);

			// prevent instant close of the ListView on first open, happens because of select/deselect events
			if (!listView.gameObject.activeSelf)
			{
				listView.gameObject.SetActive(true);
			}

			if (listView.Layout != null)
			{
				listView.Layout.UpdateLayout();
			}

			listView.ScrollToPosition(listView.GetScrollPosition());
			if (listView.SelectComponent())
			{
				SetChildDeselectListener(EventSystem.current.currentSelectedGameObject);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(listView.gameObject);
			}

			OnShowListView.Invoke();
		}

		/// <summary>
		/// Hides the list.
		/// </summary>
		public virtual void HideList()
		{
			ModalHelper.Close(ref ModalKey);

			ComboboxPosition.Restore();
			ListViewPosition.Restore();

			if (listView != null)
			{
				listView.gameObject.SetActive(false);
			}

			OnHideListView.Invoke();
		}

		/// <summary>
		/// The children deselect.
		/// </summary>
		protected List<SelectListener> childrenDeselect = new List<SelectListener>();

		/// <summary>
		/// Hide list when focus lost.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected void OnFocusHideList(BaseEventData eventData)
		{
			if (eventData.selectedObject == gameObject)
			{
				return;
			}

			var ev_item = eventData as ListViewItemEventData;
			if (ev_item != null)
			{
				if (ev_item.NewSelectedObject != null)
				{
					SetChildDeselectListener(ev_item.NewSelectedObject);
				}

				return;
			}

			var ev_axis = eventData as AxisEventData;
			if ((ev_axis != null) && ListView.Navigation)
			{
				return;
			}

			var ev_pointer = eventData as PointerEventData;
			if (ev_pointer == null)
			{
				HideList();
				return;
			}

			var go = ev_pointer.pointerPressRaycast.gameObject;
			if (go == null)
			{
				HideList();
				return;
			}

			if (go.Equals(toggleButton.gameObject))
			{
				return;
			}

			if (go.transform.IsChildOf(listView.transform))
			{
				SetChildDeselectListener(go);
				return;
			}

			HideList();
		}

		/// <summary>
		/// Sets the child deselect listener.
		/// </summary>
		/// <param name="child">Child.</param>
		protected void SetChildDeselectListener(GameObject child)
		{
			var deselectListener = GetDeselectListener(child);
			if (!childrenDeselect.Contains(deselectListener))
			{
				deselectListener.onDeselect.AddListener(OnFocusHideList);
				childrenDeselect.Add(deselectListener);
			}
		}

		/// <summary>
		/// Gets the deselect listener.
		/// </summary>
		/// <returns>The deselect listener.</returns>
		/// <param name="go">Go.</param>
		protected SelectListener GetDeselectListener(GameObject go)
		{
			return Utilities.RequireComponent<SelectListener>(go);
		}

		/// <summary>
		/// Adds the deselect callbacks.
		/// </summary>
		protected void AddDeselectCallbacks()
		{
			if (listView.ScrollRect == null)
			{
				return;
			}

			if (listView.ScrollRect.verticalScrollbar == null)
			{
				return;
			}

			var scrollbar = listView.ScrollRect.verticalScrollbar.gameObject;
			var deselectListener = GetDeselectListener(scrollbar);

			deselectListener.onDeselect.AddListener(OnFocusHideList);
			childrenDeselect.Add(deselectListener);
		}

		/// <summary>
		/// Removes the deselect callbacks.
		/// </summary>
		protected void RemoveDeselectCallbacks()
		{
			foreach (var c in childrenDeselect)
			{
				RemoveDeselectCallback(c);
			}

			childrenDeselect.Clear();
		}

		/// <summary>
		/// Removes the deselect callback.
		/// </summary>
		/// <param name="listener">Listener.</param>
		protected void RemoveDeselectCallback(SelectListener listener)
		{
			if (listener != null)
			{
				listener.onDeselect.RemoveListener(OnFocusHideList);
			}
		}

		void UpdateView(int index)
		{
			UpdateView();
		}

		/// <summary>
		/// The current indices.
		/// </summary>
		protected List<int> currentIndices = new List<int>();

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateViewBase()
		{
			currentIndices.Clear();
			ListView.GetSelectedIndices(currentIndices);

			UpdateComponentsCount();

			for (int i = 0; i < components.Count; i++)
			{
				SetData(components[i], i);
			}
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			UpdateViewBase();

			if (HideAfterItemToggle)
			{
				HideList();

				if ((EventSystem.current != null) && (!EventSystem.current.alreadySelecting))
				{
					EventSystem.current.SetSelectedGameObject(gameObject);
				}
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="i">The index.</param>
		protected virtual void SetData(TItemView component, int i)
		{
			component.Index = currentIndices[i];
			SetData(component, ListView.DataSource[currentIndices[i]]);
		}

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected virtual void SetData(TItemView component, TItem item)
		{
			if (CanSetData)
			{
				(component as IViewData<TItem>).SetData(item);
			}
		}

		/// <summary>
		/// Hide list view.
		/// </summary>
		void OnListViewPointerUp(int index, ListViewItem item, BaseEventData eventData)
		{
			if (index == ListView.SelectedIndex && !ListView.MultipleSelect)
			{
				HideList();
			}
		}

		/// <summary>
		/// Hide list view.
		/// </summary>
		void OnListViewCancel(int index, ListViewItem item, BaseEventData eventData)
		{
			HideList();
		}

		void OnListViewCancel()
		{
			HideList();
		}

		/// <summary>
		/// Adds the component.
		/// </summary>
		protected virtual void AddComponent()
		{
			TItemView component;
			if (componentsCache.Count > 0)
			{
				component = componentsCache[componentsCache.Count - 1];
				componentsCache.RemoveAt(componentsCache.Count - 1);
			}
			else
			{
				component = Compatibility.Instantiate(current);
				component.SetThemeImagesPropertiesOwner(this);
				component.RectTransform.SetParent(current.RectTransform.parent, false);

				Utilities.FixInstantiated(current, component);
			}

			component.Index = -2;
			component.RectTransform.SetAsLastSibling();
			component.gameObject.SetActive(true);
			component.onClickItem.AddListener(CurrentClick);
			component.Owner = ListView;
			component.ComboboxInstance = true;
			components.Add(component);

			ToggleButton.transform.SetAsLastSibling();
		}

		/// <summary>
		/// Deactivates the component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void DeactivateComponent(TItemView component)
		{
			if (component != null)
			{
				component.onClickItem.RemoveListener(CurrentClick);
				component.MovedToCache();
				component.Index = -1;
				component.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Updates the components count.
		/// </summary>
		protected void UpdateComponentsCount()
		{
			components.RemoveAll(IsNullComponent);

			if (components.Count == currentIndices.Count)
			{
				return;
			}

			if (components.Count < currentIndices.Count)
			{
				componentsCache.RemoveAll(IsNullComponent);

				for (int i = components.Count; i < currentIndices.Count; i++)
				{
					AddComponent();
				}
			}
			else
			{
				for (int i = currentIndices.Count; i < components.Count; i++)
				{
					DeactivateComponent(components[i]);
					componentsCache.Add(components[i]);
				}

				components.RemoveRange(currentIndices.Count, components.Count - currentIndices.Count);
			}
		}

		/// <summary>
		/// Determines whether the specified component is null.
		/// </summary>
		/// <returns><c>true</c> if the specified component is null; otherwise, <c>false</c>.</returns>
		/// <param name="component">Component.</param>
		protected bool IsNullComponent(TItemView component)
		{
			return component == null;
		}

		/// <summary>
		/// Gets the index of the component.
		/// </summary>
		/// <returns>The component index.</returns>
		/// <param name="item">Item.</param>
		protected int GetComponentIndex(TItemView item)
		{
			return item.Index;
		}

		/// <summary>
		/// Process the submit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (ShowListViewOnSubmit)
			{
				ShowList();
			}
		}

		/// <summary>
		/// Updates the current component.
		/// </summary>
		[Obsolete("Use SetData() instead.")]
		protected virtual void UpdateCurrent()
		{
			HideList();
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			ListViewPosition.ParentDestroyed();
			ComboboxPosition.ParentDestroyed();

			Localization.OnLocaleChanged -= LocaleChanged;

			ListViewUnsubscribe(restorePosition: false);
			listView = null;

			ToggleButton = null;
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Validate this instance.
		/// </summary>
		protected virtual void OnValidate()
		{
			if (Current != null)
			{
				Current.SetThemeImagesPropertiesOwner(this);
			}

			if (ParentCanvas == null)
			{
				ParentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
			}
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		protected virtual void Reset()
		{
			if (ParentCanvas == null)
			{
				ParentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
			}
		}
		#endif

		#region IStylable implementation

		/// <summary>
		/// Set components style.
		/// </summary>
		/// <param name="style">Style.</param>
		/// <param name="component">Component.</param>
		protected virtual void SetComponentStyle(Style style, TItemView component)
		{
			if (component == null)
			{
				return;
			}

			if (listView.MultipleSelect)
			{
				component.SetStyle(style.Combobox.MultipleDefaultItemBackground, style.Combobox.MultipleDefaultItemText, style);
			}
			else
			{
				component.SetStyle(style.Combobox.SingleDefaultItemBackground, style.Combobox.SingleDefaultItemText, style);
			}

			var remove_button = component.transform.Find("Remove");
			if (remove_button != null)
			{
				style.Combobox.RemoveBackground.ApplyTo(remove_button);
				style.Combobox.RemoveText.ApplyTo(remove_button.Find("Text"));
			}
		}

		/// <summary>
		/// Get button image.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <returns>Image component.</returns>
		protected virtual Image GetButtonImage(Transform button)
		{
			for (int i = 0; i < button.childCount; i++)
			{
				var child = button.GetChild(i);
				var img = child.GetComponent<Image>();
				if (img != null)
				{
					return img;
				}
			}

			return button.GetComponent<Image>();
		}

		/// <inheritdoc/>
		public virtual bool SetStyle(Style style)
		{
			if (listView.MultipleSelect)
			{
				style.Combobox.MultipleInputBackground.ApplyTo(GetComponent<Image>());
			}
			else
			{
				style.Combobox.SingleInputBackground.ApplyTo(GetComponent<Image>());
			}

			if (toggleButton != null)
			{
				style.Combobox.ToggleButton.ApplyTo(GetButtonImage(toggleButton.transform));
			}

			if (current != null)
			{
				SetComponentStyle(style, current);
			}

			for (int i = 0; i < components.Count; i++)
			{
				SetComponentStyle(style, components[i]);
			}

			for (int i = 0; i < componentsCache.Count; i++)
			{
				SetComponentStyle(style, componentsCache[i]);
			}

			if (listView != null)
			{
				listView.SetStyle(style);
			}

			return true;
		}

		/// <summary>
		/// Set style options from the specified component.
		/// </summary>
		/// <param name="style">Style.</param>
		/// <param name="component">Component.</param>
		protected virtual void GetComponentStyle(Style style, TItemView component)
		{
			if (component == null)
			{
				return;
			}

			if (listView.MultipleSelect)
			{
				component.GetStyle(style.Combobox.MultipleDefaultItemBackground, style.Combobox.MultipleDefaultItemText, style);
			}
			else
			{
				component.GetStyle(style.Combobox.SingleDefaultItemBackground, style.Combobox.SingleDefaultItemText, style);
			}

			var remove_button = component.transform.Find("Remove");
			if (remove_button != null)
			{
				style.Combobox.RemoveBackground.GetFrom(remove_button);
				style.Combobox.RemoveText.GetFrom(remove_button.Find("Text"));
			}
		}

		/// <inheritdoc/>
		public virtual bool GetStyle(Style style)
		{
			if (listView.MultipleSelect)
			{
				style.Combobox.MultipleInputBackground.GetFrom(GetComponent<Image>());
			}
			else
			{
				style.Combobox.SingleInputBackground.GetFrom(GetComponent<Image>());
			}

			if (toggleButton != null)
			{
				style.Combobox.ToggleButton.GetFrom(GetButtonImage(toggleButton.transform));
			}

			if (current != null)
			{
				GetComponentStyle(style, current);
			}

			if (listView != null)
			{
				listView.GetStyle(style);
			}

			return true;
		}
		#endregion
	}
}