﻿namespace UIThemes.Wrappers
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Theme property for the normal color of the Selectable.
	/// </summary>
	public class SelectableNormalColor : SelectableColor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectableNormalColor"/> class.
		/// </summary>
		public SelectableNormalColor()
		{
			Name = nameof(Selectable.colors.normalColor);
		}

		/// <inheritdoc/>
		protected override Color Get(ColorBlock colors)
		{
			return colors.normalColor;
		}

		/// <inheritdoc/>
		protected override void Set(ref ColorBlock colors, Color value)
		{
			colors.normalColor = value;
		}

		/// <inheritdoc/>
		protected override bool ShouldAttachValue(Selectable widget)
		{
			return widget.colors.normalColor != ColorBlock.defaultColorBlock.normalColor;
		}
	}
}