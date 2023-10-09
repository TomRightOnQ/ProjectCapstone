namespace UIWidgets
{
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// Used only to attach custom editor to DragSupport.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public abstract class BaseDragSupport : UIBehaviour
	{
		/// <summary>
		/// The drag points.
		/// </summary>
		protected static Dictionary<InstanceID, RectTransform> DragPoints = new Dictionary<InstanceID, RectTransform>();

		RectTransform parentCanvas;

		/// <summary>
		/// Gets a canvas transform of current gameobject.
		/// </summary>
		protected RectTransform ParentCanvas
		{
			get
			{
				if (parentCanvas == null)
				{
					parentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
				}

				return parentCanvas;
			}
		}

		/// <summary>
		/// Gets the drag point.
		/// </summary>
		public RectTransform DragPoint
		{
			get
			{
				var key = new InstanceID(ParentCanvas);
				var contains_key = DragPoints.ContainsKey(key);
				if (!contains_key || (DragPoints[key] == null))
				{
					var go = new GameObject("DragPoint");
					var dragPoint = go.AddComponent<RectTransform>();
					dragPoint.SetParent(ParentCanvas, false);
					dragPoint.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
					dragPoint.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
					dragPoint.pivot = new Vector2(0f, 1f);

					DragPoints[key] = dragPoint;
				}

				return DragPoints[key];
			}
		}

#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		[DomainReload(nameof(DragPoints))]
		static void StaticInit()
		{
			DragPoints.Clear();
		}
#endif
	}
}