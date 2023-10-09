namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Runtime.CompilerServices;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for Pickers.
	/// </summary>
	public abstract class Picker : MonoBehaviourConditional, ITemplatable, IStylable, INotifyCompletion
	{
		bool isTemplate = true;

		/// <summary>
		/// Gets a value indicating whether this instance is template.
		/// </summary>
		/// <value><c>true</c> if this instance is template; otherwise, <c>false</c>.</value>
		public bool IsTemplate
		{
			get
			{
				return isTemplate;
			}

			set
			{
				isTemplate = value;
			}
		}

		/// <summary>
		/// Gets the name of the template.
		/// </summary>
		/// <value>The name of the template.</value>
		public string TemplateName
		{
			get;
			set;
		}

		/// <summary>
		/// Close button.
		/// </summary>
		[SerializeField]
		Button closeButton;

		/// <summary>
		/// Close button.
		/// </summary>
		public Button CloseButton
		{
			get
			{
				return closeButton;
			}

			set
			{
				if (isInited && (closeButton != null))
				{
					closeButton.onClick.RemoveListener(Cancel);
				}

				closeButton = value;

				if (isInited && (closeButton != null))
				{
					closeButton.onClick.AddListener(Cancel);
				}
			}
		}

		/// <summary>
		/// Hide on modal click.
		/// </summary>
		[SerializeField]
		public bool HideOnModalClick = false;

		/// <summary>
		/// Is instance destroyed?
		/// </summary>
		public bool IsDestroyed
		{
			get;
			protected set;
		}

		/// <summary>
		/// Opened base pickers.
		/// </summary>
		protected static HashSet<Picker> openedBasePickers = new HashSet<Picker>();

		/// <summary>
		/// List of the opened base pickers.
		/// </summary>
		protected static List<Picker> OpenedBasePickersList = new List<Picker>();

		/// <summary>
		/// Opened base pickers.
		/// </summary>
		public static ReadOnlyCollection<Picker> OpenedBasePickers
		{
			get
			{
				OpenedBasePickersList.Clear();
				OpenedBasePickersList.AddRange(openedBasePickers);

				return OpenedBasePickersList.AsReadOnly();
			}
		}

		/// <summary>
		/// Event on any instance opened.
		/// The parameter is opened instances count.
		/// </summary>
		public static event Action<int> OnBaseInstanceOpen;

		/// <summary>
		/// Event on any instance closed.
		/// The parameter is opened instances count.
		/// </summary>
		public static event Action<int> OnBaseInstanceClose;

		/// <summary>
		/// The modal ID.
		/// </summary>
		protected InstanceID? ModalKey;

		/// <summary>
		/// Hierarchy position.
		/// </summary>
		protected HierarchyPosition Position;

		/// <summary>
		/// Callback when picker closed without any value selected.
		/// </summary>
		protected Action OnCancel;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		protected virtual void Awake()
		{
			if (IsTemplate)
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			Init();
		}

		/// <summary>
		/// Instance inited.
		/// </summary>
		protected bool isInited;

		/// <summary>
		/// Init this instance.
		/// </summary>
		public virtual void Init()
		{
			if (isInited)
			{
				return;
			}

			isInited = true;

			if (closeButton != null)
			{
				closeButton.onClick.AddListener(Cancel);
			}

			AddListeners();
		}

#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		[DomainReload(nameof(openedBasePickers), nameof(OpenedBasePickersList), nameof(OnBaseInstanceOpen), nameof(OnBaseInstanceClose))]
		static void StaticInit()
		{
			openedBasePickers.Clear();
			OpenedBasePickersList.Clear();
			OnBaseInstanceOpen = null;
			OnBaseInstanceClose = null;
		}
#endif

		/// <summary>
		/// Add listeners.
		/// </summary>
		protected virtual void AddListeners()
		{
		}

		/// <summary>
		/// Remove listeners.
		/// </summary>
		protected virtual void RemoveListeners()
		{
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			Position.ParentDestroyed();
			RemoveListeners();
		}

		/// <summary>
		/// Close picker without specified value.
		/// </summary>
		public virtual void Cancel()
		{
			if (OnCancel != null)
			{
				OnCancel();
			}

			Complete();
			Close();
		}

		/// <summary>
		/// Set modal mode.
		/// Warning: modal block is created at the current root canvas.
		/// </summary>
		/// <param name="modalSprite">Modal sprite.</param>
		/// <param name="modalColor">Modal color.</param>
		/// <param name="parentCanvas">Parent canvas.</param>
		public virtual void SetModal(Sprite modalSprite = null, Color? modalColor = null, RectTransform parentCanvas = null)
		{
			ModalHelper.Close(ref ModalKey);
			ModalKey = ModalHelper.Open(this, modalSprite, modalColor, ProcessModalClick, parentCanvas);

			if (Position.Changed)
			{
				transform.SetAsLastSibling();
			}
		}

		/// <summary>
		/// Process modal click.
		/// </summary>
		protected void ProcessModalClick()
		{
			if (HideOnModalClick)
			{
				Cancel();
			}
		}

		/// <summary>
		/// Set canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <returns>Canvas RectTransform.</returns>
		public virtual RectTransform SetCanvas(Canvas canvas)
		{
			return SetCanvas(canvas != null ? canvas.transform as RectTransform : null);
		}

		/// <summary>
		/// Set canvas.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <returns>Canvas RectTransform.</returns>
		public virtual RectTransform SetCanvas(RectTransform parent)
		{
			if (parent == null)
			{
				parent = UtilitiesUI.FindTopmostCanvas(gameObject.transform);
			}

			Position.Restore();
			Position = HierarchyPosition.SetParent(transform, parent);

			return parent;
		}

		/// <summary>
		/// Prepare picker to close.
		/// </summary>
		public virtual void BeforeClose()
		{
		}

		/// <summary>
		/// Close picker.
		/// </summary>
		protected virtual void Close()
		{
			BeforeClose();

			ModalHelper.Close(ref ModalKey);
			Position.Restore();

			Return();
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		protected abstract void Return();

		/// <inheritdoc/>
		public abstract bool GetStyle(Style style);

		/// <inheritdoc/>
		public abstract bool SetStyle(Style style);

		/// <summary>
		/// Instance opened.
		/// </summary>
		protected virtual void InstanceOpened()
		{
			openedBasePickers.Add(this);
			OnBaseInstanceOpen?.Invoke(openedBasePickers.Count);
		}

		/// <summary>
		/// Instance closed.
		/// </summary>
		protected virtual void InstanceClosed()
		{
			openedBasePickers.Remove(this);
			OnBaseInstanceClose?.Invoke(openedBasePickers.Count);
		}

		#region async

		/// <summary>
		/// The action to perform when the wait operation completes.
		/// </summary>
		protected Action Continuation;

		/// <summary>
		/// Gets a value that indicates whether the asynchronous task has completed.
		/// </summary>
		public virtual bool IsCompleted
		{
			get;
			protected set;
		}

		/// <summary>
		/// Sets the action to perform when the this object stops waiting for the asynchronous task to complete.
		/// </summary>
		/// <param name="continuation">The action to perform when the wait operation completes.</param>
		public virtual void OnCompleted(Action continuation)
		{
			Continuation = continuation;
		}

		/// <summary>
		/// Complete asynchronous task.
		/// </summary>
		protected void Complete()
		{
			if (Continuation != null)
			{
				IsCompleted = true;

				var c = Continuation;
				Continuation = null;
				c?.Invoke();
			}
		}

		#endregion
	}
}