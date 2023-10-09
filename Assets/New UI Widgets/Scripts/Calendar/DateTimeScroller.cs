﻿namespace UIWidgets
{
	using System;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Serialization;

	/// <summary>
	/// DateTime ScrollBlock widget.
	/// </summary>
	public class DateTimeScroller : DateScroller
	{
		[SerializeField]
		bool hours = true;

		/// <summary>
		/// Display ScrollBlock for the hours.
		/// </summary>
		public bool Hours
		{
			get
			{
				return hours;
			}

			set
			{
				hours = value;

				if (HoursScrollBlock != null)
				{
					HoursScrollBlock.gameObject.SetActive(hours);
				}
			}
		}

		/// <summary>
		/// ScrollBlock for the hours.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("hours")]
		[FormerlySerializedAs("HoursScroller")]
		[FormerlySerializedAs("HoursScrollBlock")]
		protected ScrollBlockBase hoursScrollBlock;

		/// <summary>
		/// ScrollBlock for the hours.
		/// </summary>
		public ScrollBlockBase HoursScrollBlock
		{
			get
			{
				return hoursScrollBlock;
			}
		}

		/// <summary>
		/// Step to change hour.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("hours")]
		public int HoursStep = 1;

		/// <summary>
		/// Format to display hours.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("hours")]
		public string HoursFormat = "HH";

		/// <summary>
		/// Format to display hours.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("hours")]
		public string HoursAMPMFormat = "hh";

		[SerializeField]
		bool minutes = true;

		/// <summary>
		/// Display ScrollBlock for the minutes.
		/// </summary>
		public bool Minutes
		{
			get
			{
				return minutes;
			}

			set
			{
				minutes = value;

				if (MinutesScrollBlock != null)
				{
					MinutesScrollBlock.gameObject.SetActive(minutes);
				}
			}
		}

		/// <summary>
		/// ScrollBlock for the minutes.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("minutes")]
		[FormerlySerializedAs("MinutesScroller")]
		[FormerlySerializedAs("MinutesScrollBlock")]
		protected ScrollBlockBase minutesScrollBlock;

		/// <summary>
		/// ScrollBlock for the minutes.
		/// </summary>
		public ScrollBlockBase MinutesScrollBlock
		{
			get
			{
				return minutesScrollBlock;
			}
		}

		/// <summary>
		/// Step to change minutes.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("minutes")]
		public int MinutesStep = 1;

		/// <summary>
		/// Format to display minutes.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("minutes")]
		public string MinutesFormat = "mm";

		[SerializeField]
		bool seconds = true;

		/// <summary>
		/// Display ScrollBlock for the seconds.
		/// </summary>
		public bool Seconds
		{
			get
			{
				return seconds;
			}

			set
			{
				seconds = value;

				if (SecondsScrollBlock != null)
				{
					SecondsScrollBlock.gameObject.SetActive(seconds);
				}
			}
		}

		/// <summary>
		/// ScrollBlock for the seconds.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("seconds")]
		[FormerlySerializedAs("SecondsScroller")]
		[FormerlySerializedAs("SecondsScrollBlock")]
		protected ScrollBlockBase secondsScrollBlock;

		/// <summary>
		/// ScrollBlock for the seconds.
		/// </summary>
		public ScrollBlockBase SecondsScrollBlock
		{
			get
			{
				return secondsScrollBlock;
			}
		}

		/// <summary>
		/// Step to change seconds.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("seconds")]
		public int SecondsStep = 1;

		/// <summary>
		/// Format to display seconds.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("seconds")]
		public string SecondsFormat = "ss";

		[SerializeField]
		bool ampm = true;

		/// <summary>
		/// Display ScrollBlock for the AM-PM.
		/// </summary>
		public bool AMPM
		{
			get
			{
				return ampm;
			}

			set
			{
				ampm = value;

				if (AMPMScrollBlock != null)
				{
					AMPMScrollBlock.gameObject.SetActive(ampm);
				}
			}
		}

		/// <summary>
		/// ScrollBlock for the AM-PM.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("ampm")]
		[FormerlySerializedAs("AMPMScroller")]
		protected ScrollBlockBase AMPMScrollBlock;

		/// <summary>
		/// Format to display AM-PM.
		/// </summary>
		[SerializeField]
		[EditorConditionBool("ampm")]
		public string AMPMFormat = "tt";

		/// <summary>
		/// Disable multiple values at AM/PM block.
		/// </summary>
		[SerializeField]
		[Tooltip("Disable multiple values at AM/PM block.")]
		protected bool singleAMPM = true;

		/// <summary>
		/// Disable multiple values at AM/PM block.
		/// </summary>
		public bool SingleAMPM
		{
			get
			{
				return singleAMPM;
			}

			set
			{
				if (singleAMPM != value)
				{
					SetSingleAMPM(value);
				}
			}
		}

		/// <summary>
		/// If enabled any time period changes will not change other time periods.
		/// </summary>
		[SerializeField]
		[Tooltip("If enabled hours changes will not change AM PM value.")]
		public bool IndependentAMPMScroll = true;

		/// <summary>
		/// Set SingleAMPM.
		/// </summary>
		/// <param name="value">Value.</param>
		protected void SetSingleAMPM(bool value)
		{
			singleAMPM = value;

			if (AMPMScrollBlock != null)
			{
				if (value)
				{
					AMPMScrollBlock.AllowDecrease = AllowAMPMDecrease;
					AMPMScrollBlock.AllowIncrease = AllowAMPMIncrease;
				}
				else
				{
					AMPMScrollBlock.AllowDecrease = AllowAlways;
					AMPMScrollBlock.AllowIncrease = AllowAlways;
				}
			}
		}

		/// <summary>
		/// Allow always.
		/// </summary>
		/// <returns>true.</returns>
		protected bool AllowAlways()
		{
			return true;
		}

		/// <summary>
		/// Allow AM/PM decrease.
		/// </summary>
		/// <returns>true if decrease allowed; otherwise false.</returns>
		protected bool AllowAMPMDecrease()
		{
			return Date.Hour >= 12;
		}

		/// <summary>
		/// Allow AM/PM increase.
		/// </summary>
		/// <returns>true if increase allowed; otherwise false.</returns>
		protected bool AllowAMPMIncrease()
		{
			return Date.Hour < 12;
		}

		/// <summary>
		/// Increment/decrement current date by hours.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <returns>Date.</returns>
		protected virtual DateTime LoopedHours(int steps)
		{
			return LoopedHours(steps, HoursStep);
		}

		/// <summary>
		/// Add hours to the specified date.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <param name="hours">Hours to add.</param>
		/// <returns>New date.</returns>
		protected virtual DateTime AddHours(DateTime date, int hours)
		{
			return date.AddHours(hours);
		}

		/// <summary>
		/// Is AM time?
		/// </summary>
		/// <param name="time">Time.</param>
		/// <returns>true if AMPM time.</returns>
		protected bool IsAM(TimeSpan time)
		{
			while (time.Ticks < 0)
			{
				time += new TimeSpan(24, 0, 0);
			}

			return time.Hours < 12;
		}

		/// <summary>
		/// Increment/decrement current date by hours.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <param name="hoursStep">Step.</param>
		/// <returns>Date.</returns>
		protected virtual DateTime LoopedHours(int steps, int hoursStep)
		{
			var value = Date;
			var increase = steps > 0;
			if (!increase)
			{
				steps = -steps;
				hoursStep = -hoursStep;
			}

			if (!IndependentScroll)
			{
				return LoopedDate(value, steps, hoursStep, AddHours);
			}

			var is_am = IsAM(value.TimeOfDay);
			var ampm_fix = AMPM && IndependentAMPMScroll && ((hoursStep == 1) || (hoursStep == -1));

			for (int i = 0; i < steps; i++)
			{
				value = value.AddHours(hoursStep);

				// AM/PM value should not be changed on hours scroll
				if (ampm_fix && (is_am != IsAM(value.TimeOfDay)))
				{
					value += new TimeSpan(increase ? 12 : -12, 0, 0);
				}

				value = CopyTimePeriods(value, Precision.Days);

				var new_hours = value.Hour;
				if (DateTimeEquals(Date, DateMin, Precision.Days) && (new_hours < DateMin.Hour))
				{
					new_hours = increase ? DateMax.Hour : Mathf.Max(DateMin.Hour, 24 - hoursStep);
				}

				if (DateTimeEquals(Date, DateMax, Precision.Days) && (new_hours > DateMax.Hour))
				{
					new_hours = increase ? DateMin.Hour : Mathf.Min(DateMax.Hour, 24 - hoursStep);
				}

				if (value.Hour != new_hours)
				{
					value = value.AddHours(new_hours - value.Hour);
				}
			}

			return value;
		}

		/// <summary>
		/// Add minutes to the specified date.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <param name="minutes">Minutes to add.</param>
		/// <returns>New date.</returns>
		protected virtual DateTime AddMinutes(DateTime date, int minutes)
		{
			return date.AddMinutes(minutes);
		}

		/// <summary>
		/// Increment/decrement current date by minutes.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <returns>Date.</returns>
		protected virtual DateTime LoopedMinutes(int steps)
		{
			var value = Date;
			var step = MinutesStep;
			var increase = steps > 0;
			if (!increase)
			{
				steps = -steps;
				step = -step;
			}

			if (!IndependentScroll)
			{
				return LoopedDate(value, steps, step, AddMinutes);
			}

			for (int i = 0; i < steps; i++)
			{
				value = value.AddMinutes(step);
				value = CopyTimePeriods(value, Precision.Hours);

				var new_minutes = value.Minute;
				if (DateTimeEquals(Date, DateMin, Precision.Hours) && (new_minutes < DateMin.Minute))
				{
					new_minutes = increase ? DateMax.Minute : Mathf.Max(DateMin.Minute, 60 - MinutesStep);
				}

				if (DateTimeEquals(Date, DateMax, Precision.Hours) && (new_minutes > DateMax.Minute))
				{
					new_minutes = increase ? DateMin.Minute : Mathf.Min(DateMax.Minute, 60 - MinutesStep);
				}

				if (value.Minute != new_minutes)
				{
					value = value.AddMinutes(new_minutes - value.Minute);
				}
			}

			return value;
		}

		/// <summary>
		/// Add seconds to the specified date.
		/// </summary>
		/// <param name="date">Date.</param>
		/// <param name="seconds">Seconds to add.</param>
		/// <returns>New date.</returns>
		protected virtual DateTime AddSeconds(DateTime date, int seconds)
		{
			return date.AddSeconds(seconds);
		}

		/// <summary>
		/// Increment/decrement current date by seconds.
		/// </summary>
		/// <param name="steps">Steps.</param>
		/// <returns>Date.</returns>
		protected virtual DateTime LoopedSeconds(int steps)
		{
			var value = Date;
			var step = SecondsStep;
			var increase = steps > 0;
			if (!increase)
			{
				steps = -steps;
				step = -step;
			}

			if (!IndependentScroll)
			{
				return LoopedDate(value, steps, step, AddSeconds);
			}

			for (int i = 0; i < steps; i++)
			{
				value = value.AddSeconds(step);
				value = CopyTimePeriods(value, Precision.Minutes);

				var new_seconds = value.Second;
				if (DateTimeEquals(Date, DateMin, Precision.Minutes) && (new_seconds < DateMin.Second))
				{
					new_seconds = increase ? DateMax.Second : Mathf.Max(DateMin.Second, 60 - SecondsStep);
				}

				if (DateTimeEquals(Date, DateMax, Precision.Minutes) && (new_seconds > DateMax.Second))
				{
					new_seconds = increase ? DateMin.Second : Mathf.Min(DateMax.Second, 60 - SecondsStep);
				}

				if (value.Second != new_seconds)
				{
					value = value.AddSeconds(new_seconds - value.Second);
				}
			}

			return value;
		}

		/// <summary>
		/// Get value for the hours at specified step.
		/// </summary>
		/// <param name="steps">Step.</param>
		/// <returns>Formatted hours.</returns>
		protected string HoursValue(int steps)
		{
			return Date.AddHours(steps * HoursStep).ToString(AMPM ? HoursAMPMFormat : HoursFormat, Culture);
		}

		/// <summary>
		/// Get value for the minutes at specified step.
		/// </summary>
		/// <param name="steps">Step.</param>
		/// <returns>Formatted minutes.</returns>
		protected string MinutesValue(int steps)
		{
			return Date.AddMinutes(steps * MinutesStep).ToString(MinutesFormat, Culture);
		}

		/// <summary>
		/// Get value for the seconds at specified step.
		/// </summary>
		/// <param name="steps">Step.</param>
		/// <returns>Formatted seconds.</returns>
		protected string SecondsValue(int steps)
		{
			return Date.AddSeconds(steps * SecondsStep).ToString(SecondsFormat, Culture);
		}

		/// <summary>
		/// Get value for the AM-PM at specified step.
		/// </summary>
		/// <param name="steps">Step.</param>
		/// <returns>Formatted AM-PM.</returns>
		protected virtual string AMPMValue(int steps)
		{
			var date = Date.AddHours(steps * 12);

			if (SingleAMPM)
			{
				if ((steps < -1) || (steps > 1))
				{
					return string.Empty;
				}

				if (date.Hour < 12)
				{
					return (steps <= 0) ? date.ToString(AMPMFormat, Culture) : string.Empty;
				}
				else
				{
					return (steps >= 0) ? date.ToString(AMPMFormat, Culture) : string.Empty;
				}
			}
			else
			{
				return date.ToString(AMPMFormat, Culture);
			}
		}

		/// <summary>
		/// Updates the calendar.
		/// Should be called only after changing culture settings.
		/// </summary>
		public override void UpdateCalendar()
		{
			base.UpdateCalendar();

			if (HoursScrollBlock != null)
			{
				HoursScrollBlock.UpdateView();
			}

			if (MinutesScrollBlock != null)
			{
				MinutesScrollBlock.UpdateView();
			}

			if (SecondsScrollBlock != null)
			{
				SecondsScrollBlock.UpdateView();
			}

			if (AMPMScrollBlock != null)
			{
				AMPMScrollBlock.UpdateView();
			}
		}

		bool isInitedTime;

		/// <summary>
		/// Increase the current date on one hour.
		/// </summary>
		protected void IncreaseHours()
		{
			Date = LoopedHours(1);
		}

		/// <summary>
		/// Decrease the current date on one hour.
		/// </summary>
		protected void DecreaseHours()
		{
			Date = LoopedHours(-1);
		}

		/// <summary>
		/// Increase the current date on one minute.
		/// </summary>
		protected void IncreaseMinutes()
		{
			Date = LoopedMinutes(1);
		}

		/// <summary>
		/// Decrease the current date on one minute.
		/// </summary>
		protected void DecreaseMinutes()
		{
			Date = LoopedMinutes(-1);
		}

		/// <summary>
		/// Increase the current date on one second.
		/// </summary>
		protected void IncreaseSeconds()
		{
			Date = LoopedSeconds(1);
		}

		/// <summary>
		/// Decrease the current date on one second.
		/// </summary>
		protected void DecreaseSeconds()
		{
			Date = LoopedSeconds(-1);
		}

		/// <summary>
		/// Increase the current date on 12 hours.
		/// </summary>
		protected void IncreaseAMPM()
		{
			Date = LoopedHours(1, 12);
		}

		/// <summary>
		/// Decrease the current date on 12 hours.
		/// </summary>
		protected void DecreaseAMPM()
		{
			Date = LoopedHours(-1, 12);
		}

		/// <summary>
		/// Init.
		/// </summary>
		protected override void InitNestedWidgets()
		{
			base.InitNestedWidgets();

			if (isInitedTime)
			{
				return;
			}

			isInitedTime = true;

			if (HoursScrollBlock != null)
			{
				HoursScrollBlock.gameObject.SetActive(Hours);

				HoursScrollBlock.Value = HoursValue;
				HoursScrollBlock.Decrease = DecreaseHours;
				HoursScrollBlock.Increase = IncreaseHours;
				HoursScrollBlock.IsInteractable = IsActive;

				HoursScrollBlock.Init();
			}

			if (MinutesScrollBlock != null)
			{
				MinutesScrollBlock.gameObject.SetActive(Minutes);

				MinutesScrollBlock.Value = MinutesValue;
				MinutesScrollBlock.Decrease = DecreaseMinutes;
				MinutesScrollBlock.Increase = IncreaseMinutes;
				MinutesScrollBlock.IsInteractable = IsActive;

				MinutesScrollBlock.Init();
			}

			if (SecondsScrollBlock != null)
			{
				SecondsScrollBlock.gameObject.SetActive(Seconds);

				SecondsScrollBlock.Value = SecondsValue;
				SecondsScrollBlock.Decrease = DecreaseSeconds;
				SecondsScrollBlock.Increase = IncreaseSeconds;
				SecondsScrollBlock.IsInteractable = IsActive;

				SecondsScrollBlock.Init();
			}

			if (AMPMScrollBlock != null)
			{
				AMPMScrollBlock.gameObject.SetActive(AMPM);

				AMPMScrollBlock.Value = AMPMValue;
				AMPMScrollBlock.Decrease = DecreaseAMPM;
				AMPMScrollBlock.Increase = IncreaseAMPM;
				AMPMScrollBlock.IsInteractable = IsActive;

				SetSingleAMPM(SingleAMPM);

				AMPMScrollBlock.Init();
			}
		}

		/// <summary>
		/// Process interactable change.
		/// </summary>
		/// <param name="interactableState">Current interactable state.</param>
		protected override void OnInteractableChange(bool interactableState)
		{
		}

#if UNITY_EDITOR
		/// <summary>
		/// Modify default values for the date time.
		/// </summary>
		protected override void OnValidate()
		{
			base.OnValidate();

			if (format == "yyyy-MM-dd")
			{
				format = "yyyy-MM-dd HH:mm:ss";
				DefaultDateMin = DateTime.MinValue.ToString(Format);
				DefaultDateMax = DateTime.MaxValue.ToString(Format);
				DefaultDate = DateTime.Now.ToString(Format);
			}
		}
#endif

		#region IStylable implementation

		/// <inheritdoc/>
		public override bool SetStyle(Style style)
		{
			base.SetStyle(style);

			if (HoursScrollBlock != null)
			{
				HoursScrollBlock.SetStyle(style);
			}

			if (MinutesScrollBlock != null)
			{
				MinutesScrollBlock.SetStyle(style);
			}

			if (SecondsScrollBlock != null)
			{
				SecondsScrollBlock.SetStyle(style);
			}

			if (AMPMScrollBlock != null)
			{
				AMPMScrollBlock.SetStyle(style);
			}

			return true;
		}

		/// <inheritdoc/>
		public override bool GetStyle(Style style)
		{
			base.GetStyle(style);

			if (HoursScrollBlock != null)
			{
				HoursScrollBlock.GetStyle(style);
			}

			if (MinutesScrollBlock != null)
			{
				MinutesScrollBlock.GetStyle(style);
			}

			if (SecondsScrollBlock != null)
			{
				SecondsScrollBlock.GetStyle(style);
			}

			if (AMPMScrollBlock != null)
			{
				AMPMScrollBlock.GetStyle(style);
			}

			return true;
		}
		#endregion
	}
}