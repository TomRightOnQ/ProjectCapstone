namespace UIWidgets
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Snap lines.
	/// </summary>
	public class SnapLines : SnapGridBase
	{
		[SerializeField]
		List<LineX> linesX = new List<LineX>();

		ObservableList<LineX> xAxisLines;

		/// <summary>
		/// Lines at X axis.
		/// </summary>
		public ObservableList<LineX> XAxisLines
		{
			get
			{
				if (xAxisLines == null)
				{
					xAxisLines = new ObservableList<LineX>(linesX);
					xAxisLines.OnChangeMono.AddListener(UpdateLines);
				}

				return xAxisLines;
			}

			set
			{
				if (xAxisLines != null)
				{
					xAxisLines.OnChangeMono.RemoveListener(UpdateLines);
				}

				xAxisLines = value;

				if (xAxisLines != null)
				{
					xAxisLines.OnChangeMono.AddListener(UpdateLines);

					UpdateLines();
				}
			}
		}

		[SerializeField]
		List<LineY> linesY = new List<LineY>();

		ObservableList<LineY> yAxisLines;

		/// <summary>
		/// Lines at Y axis.
		/// </summary>
		public ObservableList<LineY> YAxisLines
		{
			get
			{
				if (yAxisLines == null)
				{
					yAxisLines = new ObservableList<LineY>(linesY);
				}

				return yAxisLines;
			}

			set
			{
				if (yAxisLines != null)
				{
					yAxisLines.OnChangeMono.RemoveListener(UpdateLines);
				}

				yAxisLines = value;

				if (yAxisLines != null)
				{
					yAxisLines.OnChangeMono.AddListener(UpdateLines);

					UpdateLines();
				}
			}
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();

			XAxisLines = null;
			YAxisLines = null;
		}

		/// <inheritdoc/>
		protected override void UpdateLines()
		{
			LinesX.Clear();
			LinesX.AddRange(XAxisLines);

			LinesY.Clear();
			LinesY.AddRange(YAxisLines);

			OnLinesChanged.Invoke();
		}

		#if UNITY_EDITOR

		/// <inheritdoc/>
		protected override void OnValidate()
		{
			XAxisLines = null;
			YAxisLines = null;

			base.OnValidate();
		}

		#endif
	}
}