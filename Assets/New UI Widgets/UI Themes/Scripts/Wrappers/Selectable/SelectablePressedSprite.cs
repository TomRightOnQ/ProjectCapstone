namespace UIThemes.Wrappers
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Theme property for the pressed sprite of Selectable.
	/// </summary>
	public class SelectablePressedSprite : SelectableSprite
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectablePressedSprite"/> class.
		/// </summary>
		public SelectablePressedSprite()
		{
			Name = nameof(Selectable.spriteState.pressedSprite);
		}

		/// <inheritdoc/>
		protected override Sprite Get(SpriteState sprites)
		{
			return sprites.pressedSprite;
		}

		/// <inheritdoc/>
		protected override void Set(ref SpriteState sprites, Sprite value)
		{
			sprites.pressedSprite = value;
		}

		/// <inheritdoc/>
		protected override bool ShouldAttachValue(SpriteState sprites)
		{
			return sprites.pressedSprite != null;
		}
	}
}