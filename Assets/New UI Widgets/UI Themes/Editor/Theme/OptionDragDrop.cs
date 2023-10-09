#if UNITY_EDITOR && UNITY_2020_3_OR_NEWER
namespace UIThemes.Editor
{
	using UnityEngine;
	using UnityEngine.UIElements;

	/// <summary>
	/// Drag&amp;Drop for EditBlockView options.
	/// </summary>
	public class OptionDragDrop : PointerManipulator
	{
		VisualElement indicator;

		VisualElement currentHandle;

		/// <summary>
		/// Current handle.
		/// </summary>
		protected VisualElement CurrentHandle
		{
			get
			{
				return currentHandle;
			}

			set
			{
				if (currentHandle != null)
				{
					currentHandle.RemoveFromClassList("theme-option-drag-handle-selected");
				}

				currentHandle = value;

				if (currentHandle != null)
				{
					currentHandle.AddToClassList("theme-option-drag-handle-selected");
				}
			}
		}

		int oldIndex;

		int newIndex;

		bool enabled;

		/// <summary>
		/// Drop event.
		/// </summary>
		/// <param name="oldIndex">Old index.</param>
		/// <param name="newIndex">New index.</param>
		public delegate void DropEvent(int oldIndex, int newIndex);

		/// <summary>
		/// On drop event.
		/// </summary>
		public event DropEvent OnDrop;

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionDragDrop"/> class.
		/// </summary>
		/// <param name="target">Target.</param>
		public OptionDragDrop(VisualElement target)
		{
			this.target = target;

			indicator = new VisualElement();
			indicator.AddToClassList("drop-indicator");
		}

		/// <summary>
		/// Register callbacks.
		/// </summary>
		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<PointerDownEvent>(PointerDownHandler, TrickleDown.NoTrickleDown);
			target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
			target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
			target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
		}

		/// <summary>
		/// Unregister callbacks.
		/// </summary>
		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
			target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
			target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
			target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
		}

		bool HandleDown(PointerDownEvent evt)
		{
			if (evt.modifiers != EventModifiers.None)
			{
				return false;
			}

			if (evt.button != 0)
			{
				return false;
			}

			var b = evt.target as VisualElement;
			if (b == null)
			{
				return false;
			}

			return b.ClassListContains("theme-option-drag-handle");
		}

		void PointerDownHandler(PointerDownEvent evt)
		{
			if (!HandleDown(evt))
			{
				return;
			}

			CurrentHandle = evt.target as VisualElement;
			oldIndex = FindOptionIndex(evt, false);
			target.CapturePointer(evt.pointerId);
			enabled = true;
		}

		int FindOptionIndex(IPointerEvent evt, bool strict)
		{
			for (var i = 0; i < target.childCount; i++)
			{
				var element = target.ElementAt(i);
				if (!element.ClassListContains("theme-option"))
				{
					continue;
				}

				var local_position = element.WorldToLocal(evt.position);
				if (element.ContainsPoint(local_position))
				{
					if (strict && (local_position.y > (element.layout.height / 2f)))
					{
						i += 1;
					}

					return i;
				}
			}

			return -1;
		}

		void ShowIndicator(int index)
		{
			if (indicator.parent == null)
			{
				target.Add(indicator);
			}

			var end = index == target.childCount;
			var option = end ? target.ElementAt(index - 1) : target.ElementAt(index);
			var y = option.worldBound.y - target.worldBound.y - 1;
			if (end)
			{
				y += option.layout.height;
			}

			indicator.transform.position = new Vector2(0, y);
		}

		void HideIndicator()
		{
			if (indicator.parent != null)
			{
				indicator.parent.Remove(indicator);
			}
		}

		void PointerMoveHandler(PointerMoveEvent evt)
		{
			if (!enabled || !target.HasPointerCapture(evt.pointerId))
			{
				return;
			}

			newIndex = FindOptionIndex(evt, true);
			if (newIndex != -1)
			{
				ShowIndicator(newIndex);
			}
			else
			{
				HideIndicator();
			}
		}

		void PointerUpHandler(PointerUpEvent evt)
		{
			if (!enabled || !target.HasPointerCapture(evt.pointerId))
			{
				return;
			}

			CurrentHandle = null;
			target.ReleasePointer(evt.pointerId);
			HideIndicator();
		}

		void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
		{
			if (!enabled)
			{
				return;
			}

			if ((oldIndex != -1) && (newIndex != -1))
			{
				OnDrop?.Invoke(oldIndex, newIndex);
			}

			enabled = false;
		}
	}
}
#endif