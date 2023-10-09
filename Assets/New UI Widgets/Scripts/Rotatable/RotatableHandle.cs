namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.Serialization;

	/// <summary>
	/// Handle for the Rotatable component.
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rotatable))]
	[AddComponentMenu("UI/New UI Widgets/Interactions/Rotatable Handle")]
	public class RotatableHandle : UIBehaviourConditional
	{
		#region Interactable
		[SerializeField]
		bool interactable = true;

		/// <summary>
		/// Is widget interactable.
		/// </summary>
		/// <value><c>true</c> if interactable; otherwise, <c>false</c>.</value>
		public bool Interactable
		{
			get
			{
				return interactable;
			}

			set
			{
				if (interactable != value)
				{
					interactable = value;
					InteractableChanged();
				}
			}
		}

		/// <summary>
		/// If the canvas groups allow interaction.
		/// </summary>
		protected bool GroupsAllowInteraction = true;

		/// <summary>
		/// The CanvasGroup cache.
		/// </summary>
		protected List<CanvasGroup> CanvasGroupCache = new List<CanvasGroup>();

		/// <summary>
		/// Process the CanvasGroupChanged event.
		/// </summary>
		protected override void OnCanvasGroupChanged()
		{
			var groupAllowInteraction = true;
			var t = transform;
			while (t != null)
			{
				t.GetComponents(CanvasGroupCache);
				var shouldBreak = false;
				foreach (var canvas_group in CanvasGroupCache)
				{
					if (!canvas_group.interactable)
					{
						groupAllowInteraction = false;
						shouldBreak = true;
					}

					shouldBreak |= canvas_group.ignoreParentGroups;
				}

				if (shouldBreak)
				{
					break;
				}

				t = t.parent;
			}

			if (groupAllowInteraction != GroupsAllowInteraction)
			{
				GroupsAllowInteraction = groupAllowInteraction;
				InteractableChanged();
			}
		}

		/// <summary>
		/// Returns true if the GameObject and the Component are active.
		/// </summary>
		/// <returns>true if the GameObject and the Component are active; otherwise false.</returns>
		public override bool IsActive()
		{
			return base.IsActive() && GroupsAllowInteraction && Interactable;
		}

		/// <summary>
		/// Is instance interactable?
		/// </summary>
		/// <returns>true if instance interactable; otherwise false</returns>
		public bool IsInteractable()
		{
			return GroupsAllowInteraction && Interactable;
		}

		/// <summary>
		/// Process interactable change.
		/// </summary>
		protected virtual void InteractableChanged()
		{
			if (!base.IsActive())
			{
				return;
			}

			OnInteractableChange(GroupsAllowInteraction && Interactable);
		}

		/// <summary>
		/// Process interactable change.
		/// </summary>
		/// <param name="interactableState">Current interactable state.</param>
		protected virtual void OnInteractableChange(bool interactableState)
		{
		}
		#endregion

		/// <summary>
		/// Use own handle or handle from the specified source.
		/// </summary>
		[SerializeField]
		public bool OwnHandle = true;

		/// <summary>
		/// Handle source.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("OwnHandle", false)]
		public RotatableHandle HandleSource;

		/// <summary>
		/// Handle.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("handler")]
		[EditorConditionBool("OwnHandle")]
		protected DragListener handle;

		/// <summary>
		/// Handle.
		/// </summary>
		public DragListener Handle
		{
			get
			{
				return handle;
			}

			set
			{
				if (handle != null)
				{
					handle.OnDragStartEvent.RemoveListener(StartDrag);
					handle.OnDragEvent.RemoveListener(Drag);
					handle.OnDragEndEvent.RemoveListener(EndDrag);

					handle.OnSelectEvent.RemoveListener(ShowHandle);
					handle.OnDeselectEvent.RemoveListener(HideHandle);
				}

				handle = value;

				if (handle != null)
				{
					handle.OnDragStartEvent.AddListener(StartDrag);
					handle.OnDragEvent.AddListener(Drag);
					handle.OnDragEndEvent.AddListener(EndDrag);

					handle.OnSelectEvent.AddListener(ShowHandle);
					handle.OnDeselectEvent.AddListener(HideHandle);
				}
			}
		}

		/// <summary>
		/// Is currently dragged?
		/// </summary>
		protected bool IsDrag;

		/// <summary>
		/// Target.
		/// </summary>
		protected Rotatable Target;

		/// <summary>
		/// Target listeners for focus and focus out events (select/deselect).
		/// </summary>
		protected SelectListener TargetFocusListener;

		bool isInited;

		/// <summary>
		/// Start rotation event.
		/// </summary>
		[SerializeField]
		public RotatableEvent OnStartRotate = new RotatableEvent();

		/// <summary>
		/// During rotation event.
		/// </summary>
		[SerializeField]
		public RotatableEvent OnRotate = new RotatableEvent();

		/// <summary>
		/// End rotation event.
		/// </summary>
		[SerializeField]
		public RotatableEvent OnEndRotate = new RotatableEvent();

		/// <summary>
		/// Return handle state (enabled/disabled) on select/deselect event (got or lost focus).
		/// Use case: show Rotatable and Resizable handles only if target (or one of handles) is selected, otherwise deselect.
		/// </summary>
		public Func<RotatableHandle, BaseEventData, bool, bool> HandleState = null;

		/// <summary>
		/// Show Rotatable handle only if target (or one of handles) is selected.
		/// </summary>
		[DomainReloadExclude]
		public static readonly Func<RotatableHandle, BaseEventData, bool, bool> ShowHandleOnSelect = (rotatableHandle, eventData, select) =>
		{
			if (select)
			{
				return true;
			}

			var ev = eventData as PointerEventData;
			if (eventData == null)
			{
				return false;
			}

			return rotatableHandle.IsControlled(ev.pointerEnter);
		};

		/// <summary>
		/// Starts this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			Init();
		}

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

			Target = GetComponent<Rotatable>();
			Target.Interactable = false;
			Target.OnTargetChanged.AddListener(TargetChanged);

			Handle = handle;
			TargetChanged(Target);
		}

		/// <summary>
		/// Check if specified gameobject is Target or one of Handles.
		/// </summary>
		/// <param name="go">Gameobject.</param>
		/// <returns>true if specified gameobject is Target or one of Handles; otherwise false.</returns>
		public virtual bool IsControlled(GameObject go)
		{
			if (go == null)
			{
				return false;
			}

			var id = new InstanceID(go);
			return Utilities.IsSame(id, Target.gameObject)
				|| Utilities.IsSame(id, Handle?.gameObject);
		}

		/// <summary>
		/// Process target changed.
		/// </summary>
		/// <param name="rotatable">Rotatable.</param>
		protected void TargetChanged(Rotatable rotatable)
		{
			if (TargetFocusListener != null)
			{
				TargetFocusListener.onSelect.RemoveListener(ShowHandle);
				TargetFocusListener.onDeselect.RemoveListener(HideHandle);
			}

			TargetFocusListener = Utilities.RequireComponent<SelectListener>(rotatable.Target);
			TargetFocusListener.onSelect.AddListener(ShowHandle);
			TargetFocusListener.onDeselect.AddListener(HideHandle);
			ToggleHandle();
		}

		/// <summary>
		/// Toggle handle.
		/// </summary>
		protected virtual void ToggleHandle()
		{
			if (HandleState != null)
			{
				ToggleHandle((TargetFocusListener != null) && IsControlled(EventSystem.current.currentSelectedGameObject));
			}
			else
			{
				ToggleHandle(true);
			}
		}

		/// <summary>
		/// Show handle.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void ShowHandle(BaseEventData eventData)
		{
			if (HandleState == null)
			{
				return;
			}

			ToggleHandle(HandleState(this, eventData, true));
		}

		/// <summary>
		/// Hide handle.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void HideHandle(BaseEventData eventData)
		{
			if (HandleState == null)
			{
				return;
			}

			ToggleHandle(HandleState(this, eventData, false));
		}

		/// <summary>
		/// Toggle handle.
		/// </summary>
		/// <param name="active">Activate or deactivate handle.</param>
		public virtual void ToggleHandle(bool active)
		{
			if (Handle == null)
			{
				return;
			}

			Handle.gameObject.SetActive(active);
		}

		/// <summary>
		/// Interactable state of the handle source.
		/// </summary>
		protected bool HandleSourceInteractable;

		/// <summary>
		/// Get source handle.
		/// </summary>
		public void GetSourceHandle()
		{
			HandleSourceInteractable = HandleSource.interactable;
			HandleSource.interactable = false;
			SetHandleParent(HandleSource, transform);

			Handle = HandleSource.Handle;
		}

		/// <summary>
		/// Release source handle.
		/// </summary>
		public void ReleaseSourceHandle()
		{
			HandleSource.interactable = HandleSourceInteractable;
			HandleSource.Init();
			SetHandleParent(HandleSource, HandleSource.transform);
		}

		/// <summary>
		/// Set new parent to the handle.
		/// </summary>
		/// <param name="rotatableHandle">Rotatable handle.</param>
		/// <param name="parent">Parent.</param>
		protected static void SetHandleParent(RotatableHandle rotatableHandle, Transform parent)
		{
			if (rotatableHandle.Handle != null)
			{
				rotatableHandle.Handle.transform.SetParent(parent, false);
			}
		}

		/// <summary>
		/// Enable handle.
		/// </summary>
		public void EnableHandle()
		{
			Init();

			if (Handle != null)
			{
				Handle.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Disable handle.
		/// </summary>
		public void DisableHandle()
		{
			if (Handle != null)
			{
				Handle.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Get angle between handle and target.
		/// </summary>
		/// <returns>Angle.</returns>
		protected virtual float GetAngle()
		{
			var handle_rt = handle.transform as RectTransform;
			var rt = transform as RectTransform;

			var rotation = rt.localRotation;
			rt.localRotation = Quaternion.Euler(Vector3.zero);

			var relative = GetCenter(rt) - GetCenter(handle_rt);
			relative.x *= -1f;

			var angle = Rotatable.Point2Angle(relative);

			handle_rt.localRotation = rotation;

			return angle;
		}

		/// <summary>
		/// Process begin drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void StartDrag(PointerEventData eventData)
		{
			if (!IsActive())
			{
				return;
			}

			IsDrag = true;

			var angle = GetAngle();
			Target.InitRotate(angle);
			OnStartRotate.Invoke(Target);
		}

		/// <summary>
		/// Get center of the specified RectTransform.
		/// </summary>
		/// <param name="rt">RectTransform.</param>
		/// <returns>Center.</returns>
		protected static Vector2 GetCenter(RectTransform rt)
		{
			var pos = rt.position;
			var size = rt.rect.size;
			var pivot = rt.pivot;
			var center = new Vector2(pos.x - (size.x * (pivot.x - 0.5f)), pos.y - (size.y * (pivot.y - 0.5f)));

			return center;
		}

		/// <summary>
		/// Process end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void EndDrag(PointerEventData eventData)
		{
			IsDrag = false;

			OnEndRotate.Invoke(Target);
		}

		/// <summary>
		/// Process drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void Drag(PointerEventData eventData)
		{
			if (!IsDrag)
			{
				return;
			}

			Target.Rotate(eventData);
			OnRotate.Invoke(Target);
		}

		/// <summary>
		/// Process destroy event.
		/// </summary>
		protected override void OnDestroy()
		{
			if (Target != null)
			{
				Target.OnTargetChanged.RemoveListener(TargetChanged);
			}

			if (TargetFocusListener != null)
			{
				TargetFocusListener.onSelect.RemoveListener(ShowHandle);
				TargetFocusListener.onDeselect.RemoveListener(HideHandle);
			}

			Handle = null;

			base.OnDestroy();
		}
	}
}