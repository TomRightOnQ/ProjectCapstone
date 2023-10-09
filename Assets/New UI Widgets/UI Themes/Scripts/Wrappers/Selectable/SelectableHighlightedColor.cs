﻿namespace UIThemes.Wrappers
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Theme property for the highlighted color of Selectable.
	/// </summary>
	public class SelectableHighlightedColor : SelectableColor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectableHighlightedColor"/> class.
		/// </summary>
		public SelectableHighlightedColor()
		{
			Name = nameof(Selectable.colors.highlightedColor);
		}

		/// <inheritdoc/>
		protected override Color Get(ColorBlock colors)
		{
			return colors.highlightedColor;
		}

		/// <inheritdoc/>
		protected override void Set(ref ColorBlock colors, Color value)
		{
			colors.highlightedColor = value;
		}

		/// <inheritdoc/>
		protected override bool ShouldAttachValue(Selectable widget)
		{
			return widget.colors.highlightedColor != ColorBlock.defaultColorBlock.highlightedColor;
		}
	}
}