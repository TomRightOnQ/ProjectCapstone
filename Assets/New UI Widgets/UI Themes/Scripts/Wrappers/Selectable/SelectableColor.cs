namespace UIThemes.Wrappers
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for the theme property for the colors of Selectable.
	/// </summary>
	public abstract class SelectableColor : Wrapper<Color, Selectable>
	{
		/// <inheritdoc/>
		protected override Color Get(Selectable widget)
		{
			return Get(widget.colors);
		}

		/// <summary>
		/// Get color.
		/// </summary>
		/// <param name="colors">Colors.</param>
		/// <returns>Color.</returns>
		protected abstract Color Get(ColorBlock colors);

		/// <inheritdoc/>
		protected override void Set(Selectable widget, Color value)
		{
			var colors = widget.colors;
			Set(ref colors, value);
			widget.colors = colors;
		}

		/// <summary>
		/// Set color.
		/// </summary>
		/// <param name="colors">Colors.</param>
		/// <param name="value">Color.</param>
		protected abstract void Set(ref ColorBlock colors, Color value);

		/// <inheritdoc/>
		protected override bool Active(Selectable widget)
		{
			return widget.transition == Selectable.Transition.ColorTint;
		}

		/// <inheritdoc/>
		protected override bool ShouldAttachValue(Selectable widget)
		{
			return widget.colors != ColorBlock.defaultColorBlock;
		}
	}
}