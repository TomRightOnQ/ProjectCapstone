﻿namespace UIWidgets.Timeline
{
	using System;
	using UIWidgets.Styles;
	using UnityEngine;

	/// <summary>
	/// Timeline Scale.
	/// </summary>
	public class TimelineScale : MonoBehaviourConditional, IStylable
	{
		[SerializeField]
		SerializedTimeSpan size = new SerializedTimeSpan(0, 0, 5);

		TimeSpan convertedSize;

		/// <summary>
		/// Size.
		/// </summary>
		public TimeSpan Size
		{
			get
			{
				return convertedSize;
			}

			set
			{
				if (convertedSize != value)
				{
					convertedSize = value;
					UpdateScale();
				}
			}
		}

		[SerializeField]
		Scale scale;

		/// <summary>
		/// Scale.
		/// </summary>
		public Scale Scale
		{
			get
			{
				return scale;
			}

			set
			{
				if (scale != null)
				{
					scale.Clear();
				}

				scale = value;
				UpdateScale();
			}
		}

		[SerializeField]
		string format = string.Empty;

		/// <summary>
		/// Value format.
		/// </summary>
		public string Format
		{
			get
			{
				return format;
			}

			set
			{
				if (format != value)
				{
					format = value;
					UpdateScale();
				}
			}
		}

		Func<float, string> formatter;

		/// <summary>
		/// Custom value formatter.
		/// </summary>
		public Func<float, string> Formatter
		{
			get
			{
				return formatter;
			}

			set
			{
				if (formatter != value)
				{
					formatter = value;
					UpdateScale();
				}
			}
		}

		RectTransform rectTransform;

		/// <summary>
		/// RectTransform.
		/// </summary>
		public RectTransform RectTransform
		{
			get
			{
				if (rectTransform == null)
				{
					rectTransform = transform as RectTransform;
				}

				return rectTransform;
			}
		}

		Func<float, Scale.MarkData> value2MarkDataDelegate;

		/// <summary>
		/// Delegate for the Value2MarkData method.
		/// </summary>
		protected Func<float, Scale.MarkData> Value2MarkDataDelegate
		{
			get
			{
				if (value2MarkDataDelegate == null)
				{
					value2MarkDataDelegate = Value2MarkData;
				}

				return value2MarkDataDelegate;
			}
		}

		bool isInited;

		/// <summary>
		/// Process the start event.
		/// </summary>
		protected virtual void Start()
		{
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

			Size = size;

			AddListeners();

			UpdateScale();
		}

		/// <summary>
		/// Add listeners.
		/// </summary>
		protected virtual void AddListeners()
		{
			Scale.OnChange += UpdateScale;
		}

		/// <summary>
		/// Remove listeners.
		/// </summary>
		protected virtual void RemoveListeners()
		{
			Scale.OnChange -= UpdateScale;
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected virtual void OnDestroy()
		{
			RemoveListeners();
		}

		/// <summary>
		/// Update scale.
		/// </summary>
		public virtual void UpdateScale()
		{
			Scale.Set(Value2MarkDataDelegate, 0, (float)Size.TotalSeconds, 0);
		}

		/// <summary>
		/// Convert value to label.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Label.</returns>
		protected virtual string Value2Label(float value)
		{
			if (Formatter != null)
			{
				return Formatter(value);
			}

			return value.ToString(Format);
		}

		/// <summary>
		/// Convert value to mark data.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Mark data.</returns>
		protected virtual Scale.MarkData Value2MarkData(float value)
		{
			var a = Value2Anchor(value);
			return new Scale.MarkData(value, anchorMin: a, anchorMax: a, value2label: Value2Label);
		}

		/// <summary>
		/// Convert value to anchor.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Anchor.</returns>
		protected virtual Vector2 Value2Anchor(float value)
		{
			return new Vector2((float)(value / Size.TotalSeconds), 0.5f);
		}

		#region IStylable implementation

		/// <inheritdoc/>
		public virtual bool SetStyle(Style style)
		{
			Scale.SetStyle(style);

			return true;
		}

		/// <inheritdoc/>
		public virtual bool GetStyle(Style style)
		{
			Scale.GetStyle(style);

			return true;
		}
		#endregion
	}
}