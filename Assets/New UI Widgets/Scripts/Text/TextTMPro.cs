#if UIWIDGETS_TMPRO_SUPPORT
namespace UIWidgets
{
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// TextTMPro.
	/// </summary>
	public class TextTMPro : ITextProxy
	{
		/// <summary>
		/// Is enum value has specified flag?
		/// </summary>
		/// <param name="value">Enum value.</param>
		/// <param name="flag">Flag.</param>
		/// <returns>true if enum has flag; otherwise false.</returns>
		public static bool IsSet(FontStyles value, FontStyles flag)
		{
			return (value & flag) == flag;
		}

		/// <summary>
		/// Text component.
		/// </summary>
		protected TextMeshProUGUI Component;

		/// <summary>
		/// GameObject.
		/// </summary>
		public GameObject GameObject
		{
			get
			{
				return Component.gameObject;
			}
		}

		/// <summary>
		/// Graphic component.
		/// </summary>
		public Graphic Graphic
		{
			get
			{
				return Component;
			}
		}

		/// <summary>
		/// Color.
		/// </summary>
		public Color color
		{
			get
			{
				return Component.color;
			}

			set
			{
				Component.color = value;
			}
		}

		/// <summary>
		/// Font size.
		/// </summary>
		public float fontSize
		{
			get
			{
				return Component.fontSize;
			}

			set
			{
				Component.fontSize = value;
			}
		}

		/// <summary>
		/// Font style.
		/// </summary>
		public FontStyle fontStyle
		{
			get
			{
				if (Bold && Italic)
				{
					return FontStyle.BoldAndItalic;
				}

				if (Bold)
				{
					return FontStyle.Bold;
				}

				if (Italic)
				{
					return FontStyle.Italic;
				}

				return FontStyle.Normal;
			}

			set
			{
				Bold = (value == FontStyle.Bold) || (value == FontStyle.BoldAndItalic);
				Italic = (value == FontStyle.Italic) || (value == FontStyle.BoldAndItalic);
			}
		}

		/// <summary>
		/// Text alignment.
		/// </summary>
		public TextAnchor alignment
		{
			get
			{
				return ConvertAlignment(Component.alignment);
			}

			set
			{
				Component.alignment = ConvertAlignment(value);
			}
		}

		TextAnchor ConvertAlignment(TextAlignmentOptions alignment)
		{
			switch (alignment)
			{
				case TextAlignmentOptions.TopLeft:
					return TextAnchor.UpperLeft;
				case TextAlignmentOptions.Top:
					return TextAnchor.UpperCenter;
				case TextAlignmentOptions.TopRight:
					return TextAnchor.UpperRight;
				case TextAlignmentOptions.MidlineLeft:
					return TextAnchor.MiddleLeft;
				case TextAlignmentOptions.Center:
					return TextAnchor.MiddleCenter;
				case TextAlignmentOptions.MidlineRight:
					return TextAnchor.MiddleRight;
				case TextAlignmentOptions.BottomLeft:
					return TextAnchor.LowerLeft;
				case TextAlignmentOptions.Bottom:
					return TextAnchor.LowerCenter;
				case TextAlignmentOptions.BottomRight:
					return TextAnchor.LowerRight;
			}

			return TextAnchor.UpperLeft;
		}

		TextAlignmentOptions ConvertAlignment(TextAnchor alignment)
		{
			switch (alignment)
			{
				case TextAnchor.UpperLeft:
					return TextAlignmentOptions.TopLeft;
				case TextAnchor.UpperCenter:
					return TextAlignmentOptions.Top;
				case TextAnchor.UpperRight:
					return TextAlignmentOptions.TopRight;
				case TextAnchor.MiddleLeft:
					return TextAlignmentOptions.MidlineLeft;
				case TextAnchor.MiddleCenter:
					return TextAlignmentOptions.Center;
				case TextAnchor.MiddleRight:
					return TextAlignmentOptions.MidlineRight;
				case TextAnchor.LowerLeft:
					return TextAlignmentOptions.BottomLeft;
				case TextAnchor.LowerCenter:
					return TextAlignmentOptions.Bottom;
				case TextAnchor.LowerRight:
					return TextAlignmentOptions.BottomRight;
			}

			return TextAlignmentOptions.TopLeft;
		}

		/// <summary>
		/// Bold.
		/// </summary>
		public bool Bold
		{
			get
			{
				return IsSet(Component.fontStyle, FontStyles.Bold);
			}

			set
			{
				if (Bold == value)
				{
					return;
				}

				if (value)
				{
					Component.fontStyle |= FontStyles.Bold;
				}
				else
				{
					Component.fontStyle &= ~FontStyles.Bold;
				}
			}
		}

		/// <summary>
		/// Italic.
		/// </summary>
		public bool Italic
		{
			get
			{
				return IsSet(Component.fontStyle, FontStyles.Italic);
			}

			set
			{
				if (Italic == value)
				{
					return;
				}

				if (value)
				{
					Component.fontStyle |= FontStyles.Italic;
				}
				else
				{
					Component.fontStyle &= ~FontStyles.Italic;
				}
			}
		}

		/// <summary>
		/// Text.
		/// </summary>
		public string text
		{
			get
			{
				return Component.text;
			}

			set
			{
				Component.text = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTMPro"/> class.
		/// </summary>
		/// <param name="component">Component.</param>
		public TextTMPro(TextMeshProUGUI component)
		{
			Component = component;
		}
	}
}
#endif