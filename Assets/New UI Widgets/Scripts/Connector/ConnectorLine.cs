namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using UnityEngine;

	/// <summary>
	/// Connector line.
	/// </summary>
	[Serializable]
	public class ConnectorLine : IObservable, INotifyPropertyChanged
	{
		[SerializeField]
		RectTransform target;

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public RectTransform Target
		{
			get
			{
				return target;
			}

			set
			{
				Change(ref target, value, "Target");
			}
		}

		[SerializeField]
		ConnectorPosition start = ConnectorPosition.Right;

		/// <summary>
		/// Gets or sets the start.
		/// </summary>
		/// <value>The start.</value>
		public ConnectorPosition Start
		{
			get
			{
				return start;
			}

			set
			{
				Change(ref start, value, "Start");
			}
		}

		[SerializeField]
		ConnectorPosition end = ConnectorPosition.Left;

		/// <summary>
		/// Gets or sets the end.
		/// </summary>
		/// <value>The end.</value>
		public ConnectorPosition End
		{
			get
			{
				return end;
			}

			set
			{
				Change(ref end, value, "End");
			}
		}

		[SerializeField]
		ConnectorArrow arrow = ConnectorArrow.None;

		/// <summary>
		/// Gets or sets the arrow.
		/// </summary>
		/// <value>The arrow.</value>
		public ConnectorArrow Arrow
		{
			get
			{
				return arrow;
			}

			set
			{
				Change(ref arrow, value, "Arrow");
			}
		}

		[SerializeField]
		Vector2 arrowSize = new Vector2(20f, 10f);

		/// <summary>
		/// Gets or sets the arrow.
		/// </summary>
		/// <value>The arrow.</value>
		public Vector2 ArrowSize
		{
			get
			{
				return arrowSize;
			}

			set
			{
				Change(ref arrowSize, value, "ArrowSize");
			}
		}

		[SerializeField]
		ConnectorType type;

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public ConnectorType Type
		{
			get
			{
				return type;
			}

			set
			{
				Change(ref type, value, "Type");
			}
		}

		[SerializeField]
		float thickness = 1f;

		/// <summary>
		/// Gets or sets the thickness.
		/// </summary>
		/// <value>The thickness.</value>
		public float Thickness
		{
			get
			{
				return thickness;
			}

			set
			{
				Change(ref thickness, value, "Thickness");
			}
		}

		[SerializeField]
		float margin = 30f;

		/// <summary>
		/// Gets or sets the margin.
		/// </summary>
		/// <value>The margin.</value>
		public float Margin
		{
			get
			{
				return margin;
			}

			set
			{
				Change(ref margin, value, "Margin");
			}
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event OnChange OnChange;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Change value.
		/// </summary>
		/// <typeparam name="T">Type of field.</typeparam>
		/// <param name="field">Field value.</param>
		/// <param name="value">New value.</param>
		/// <param name="propertyName">Property name.</param>
		protected void Change<T>(ref T field, T value, string propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				NotifyPropertyChanged(propertyName);
			}
		}

		/// <summary>
		/// Changed the specified propertyName.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected void NotifyPropertyChanged(string propertyName)
		{
			var c_handlers = OnChange;
			if (c_handlers != null)
			{
				c_handlers();
			}

			var handlers = PropertyChanged;
			if (handlers != null)
			{
				handlers(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}