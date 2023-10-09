namespace UIThemes.Samples
{
	using UnityEngine;
	using UnityEngine.UIElements;

	/// <summary>
	/// Field view for the rotated sprite.
	/// </summary>
	public class RotatedSpriteView : FieldView<RotatedSprite>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RotatedSpriteView"/> class.
		/// </summary>
		/// <param name="undoName">Undo name.</param>
		/// <param name="values">Theme values wrapper.</param>
		public RotatedSpriteView(string undoName, Theme.ValuesWrapper<RotatedSprite> values)
			: base(undoName, values)
		{
		}

		/// <inheritdoc/>
		protected override VisualElement CreateView(VariationId variationId, OptionId optionId, RotatedSprite value)
		{
			#if UNITY_EDITOR
			var block = new VisualElement();
			block.style.flexDirection = FlexDirection.Column;

			var input = new UnityEditor.UIElements.ObjectField();
			input.value = value.Sprite;
			input.objectType = typeof(Sprite);
			input.RegisterValueChangedCallback(x =>
			{
				value.Sprite = x.newValue as Sprite;
				Save(variationId, optionId, value);
			});
			block.Add(input);

			#if UNITY_2022_1_OR_NEWER
			var rotation = new UnityEngine.UIElements.FloatField("Rotation.Z");
			#else
			var rotation = new UnityEditor.UIElements.FloatField("Rotation.Z");
			#endif
			rotation.value = value.RotationZ;
			rotation.RegisterValueChangedCallback(x =>
			{
				value.RotationZ = x.newValue;
				Save(variationId, optionId, value);
			});
			block.Add(rotation);

			return block;
			#else
			return null;
			#endif
		}

		/// <inheritdoc/>
		public override void UpdateValue(VisualElement view, RotatedSprite value)
		{
			#if UNITY_EDITOR
			var block = new VisualElement();
			block.style.flexDirection = FlexDirection.Column;

			var input = view.ElementAt(0) as UnityEditor.UIElements.ObjectField;
			if (input != null)
			{
				input.value = value.Sprite;
				input.objectType = typeof(Sprite);
			}

			#if UNITY_2022_1_OR_NEWER
			var rotation = view.ElementAt(1) as UnityEngine.UIElements.FloatField;
			#else
			var rotation = view.ElementAt(1) as UnityEditor.UIElements.FloatField;
			#endif
			if (rotation != null)
			{
				rotation.value = value.RotationZ;
			}
			#endif
		}
	}
}