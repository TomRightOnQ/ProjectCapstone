namespace UIWidgets
{
	using System;
	using UIWidgets.Attributes;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Border effect.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Graphic))]
	[AddComponentMenu("UI/New UI Widgets/Effects/Border Effect")]
	public class BorderEffect : UVEffect
	{
		/// <summary>
		/// IDs of ring shader properties.
		/// </summary>
		protected struct BorderShaderIDs : IEquatable<BorderShaderIDs>
		{
			/// <summary>
			/// Line color ID.
			/// </summary>
			public readonly int BorderColor;

			/// <summary>
			/// Borders ID.
			/// </summary>
			public readonly int Borders;

			/// <summary>
			/// Resolution X ID.
			/// </summary>
			public readonly int ResolutionX;

			/// <summary>
			/// Resolution Y ID.
			/// </summary>
			public readonly int ResolutionY;

			/// <summary>
			/// Transparent ID.
			/// </summary>
			public readonly int Transparent;

			private BorderShaderIDs(int lineColor, int borders, int resolutionX, int resolutionY, int transparent)
			{
				BorderColor = lineColor;
				Borders = borders;
				ResolutionX = resolutionX;
				ResolutionY = resolutionY;
				Transparent = transparent;
			}

			/// <summary>
			/// Get BorderShaderIDs instance.
			/// </summary>
			public static BorderShaderIDs Instance
			{
				get
				{
					return new BorderShaderIDs(
						Shader.PropertyToID("_BorderColor"),
						Shader.PropertyToID("_Borders"),
						Shader.PropertyToID("_ResolutionX"),
						Shader.PropertyToID("_ResolutionY"),
						Shader.PropertyToID("_Transparent"));
				}
			}

			/// <summary>
			/// Determines whether the specified object is equal to the current object.
			/// </summary>
			/// <param name="obj">The object to compare with the current object.</param>
			/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
			public override bool Equals(object obj)
			{
				if (obj is BorderShaderIDs)
				{
					return Equals((BorderShaderIDs)obj);
				}

				return false;
			}

			/// <summary>
			/// Determines whether the specified object is equal to the current object.
			/// </summary>
			/// <param name="other">The object to compare with the current object.</param>
			/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
			public bool Equals(BorderShaderIDs other)
			{
				return BorderColor == other.BorderColor
					&& Borders == other.Borders
					&& ResolutionX == other.ResolutionX
					&& ResolutionY == other.ResolutionY
					&& Transparent == other.Transparent;
			}

			/// <summary>
			/// Hash function.
			/// </summary>
			/// <returns>A hash code for the current object.</returns>
			public override int GetHashCode()
			{
				return BorderColor ^ Borders ^ ResolutionX ^ Transparent;
			}

			/// <summary>
			/// Compare specified instances.
			/// </summary>
			/// <param name="left">Left instance.</param>
			/// <param name="right">Right instances.</param>
			/// <returns>true if the instances are equal; otherwise, false.</returns>
			public static bool operator ==(BorderShaderIDs left, BorderShaderIDs right)
			{
				return left.Equals(right);
			}

			/// <summary>
			/// Compare specified instances.
			/// </summary>
			/// <param name="left">Left instance.</param>
			/// <param name="right">Right instances.</param>
			/// <returns>true if the instances are now equal; otherwise, false.</returns>
			public static bool operator !=(BorderShaderIDs left, BorderShaderIDs right)
			{
				return !left.Equals(right);
			}
		}

		[SerializeField]
		Color borderColor = Color.white;

		/// <summary>
		/// Border color.
		/// </summary>
		public Color BorderColor
		{
			get
			{
				return borderColor;
			}

			set
			{
				borderColor = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		[Tooltip("Make the background transparent.")]
		bool transparentBackground = false;

		/// <summary>
		/// Make the background transparent.
		/// </summary>
		public bool TransparentBackground
		{
			get
			{
				return transparentBackground;
			}

			set
			{
				transparentBackground = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		Vector2 horizontalBorders = new Vector2(1f, 1f);

		/// <summary>
		/// Horizontal borders.
		/// </summary>
		public Vector2 HorizontalBorders
		{
			get
			{
				return horizontalBorders;
			}

			set
			{
				horizontalBorders = value;
				UpdateMaterial();
			}
		}

		[SerializeField]
		Vector2 verticalBorders = new Vector2(1f, 1f);

		/// <summary>
		/// Thickness.
		/// </summary>
		public Vector2 VerticalBorders
		{
			get
			{
				return verticalBorders;
			}

			set
			{
				verticalBorders = value;
				UpdateMaterial();
			}
		}

		/// <summary>
		/// Borders.
		/// </summary>
		protected Vector4 Borders
		{
			get
			{
				return new Vector4(horizontalBorders.x, horizontalBorders.y, verticalBorders.x, verticalBorders.y);
			}
		}

		/// <summary>
		/// Ring shader ids.
		/// </summary>
		[DomainReloadExclude]
		protected static readonly BorderShaderIDs ShaderIDs = BorderShaderIDs.Instance;

		/// <inheritdoc/>
		protected override void OnEnable()
		{
			base.OnEnable();

			Mode = UVMode.One;
		}

		/// <summary>
		/// Set material properties.
		/// </summary>
		protected override void SetMaterialProperties()
		{
			if (EffectMaterial != null)
			{
				var size = RectTransform.rect.size;

				EffectMaterial.SetColor(ShaderIDs.BorderColor, BorderColor);
				EffectMaterial.SetVector(ShaderIDs.Borders, Borders);
				EffectMaterial.SetFloat(ShaderIDs.ResolutionX, size.x);
				EffectMaterial.SetFloat(ShaderIDs.ResolutionY, size.y);
				EffectMaterial.SetFloat(ShaderIDs.Transparent, transparentBackground ? 1 : 0);
			}
		}
	}
}