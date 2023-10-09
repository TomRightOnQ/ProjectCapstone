namespace UIThemes.Samples
{
	using System;
	using UnityEngine;
	using UnityEngine.Scripting;

	/// <summary>
	/// Sample of the extended theme.
	/// </summary>
	// [CreateAssetMenu(fileName = "UI Theme Extended", menuName = "UI Themes/Create ThemeExtended")]
	[Serializable]
	public class ThemeExtended : Theme
	{
		/// <summary>
		/// Rotated sprites.
		/// </summary>
		[SerializeField]
		protected ValuesTable<RotatedSprite> RotatedSpritesTable = new ValuesTable<RotatedSprite>();

		/// <summary>
		/// Rotated sprites.
		/// </summary>
		[UIThemes.Property(typeof(RotatedSpriteView), "UI Themes: Change Rotated Sprite")]
		public ValuesWrapper<RotatedSprite> RotatedSprites => new ValuesWrapper<RotatedSprite>(this, RotatedSpritesTable);

		/// <summary>
		/// Get Theme target type.
		/// </summary>
		/// <returns>Target type.</returns>
		public override Type GetTargetType() => typeof(ThemeTargetExtended);

		/// <summary>
		/// Copy variation values.
		/// </summary>
		/// <param name="source">Source variation.</param>
		/// <param name="destination">Destination variation.</param>
		public override void Copy(Variation source, Variation destination)
		{
			base.Copy(source, destination);
			RotatedSpritesTable.Copy(source.Id, destination.Id);
		}

		/// <summary>
		/// Add properties.
		/// </summary>
		[PropertiesRegistry]
		[Preserve]
		public static void AddProperties()
		{
			PropertyWrappers<RotatedSprite>.Add(new RotatedSpriteWrapper());
		}
	}
}