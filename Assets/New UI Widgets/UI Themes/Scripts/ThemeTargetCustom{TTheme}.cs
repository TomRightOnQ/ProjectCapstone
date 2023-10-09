namespace UIThemes
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Base class for the ThemeTarget.
	/// </summary>
	/// <typeparam name="TTheme">Type of Theme.</typeparam>
	public class ThemeTargetCustom<TTheme> : ThemeTargetBase, IThemeTarget
		where TTheme : Theme
	{
		/// <summary>
		/// Type of Theme.
		/// </summary>
		public override Type ThemeType
		{
			get
			{
				return typeof(TTheme);
			}
		}

		/// <summary>
		/// Theme.
		/// </summary>
		[SerializeField]
		protected TTheme theme;

		/// <summary>
		/// Theme.
		/// </summary>
		public TTheme Theme
		{
			get
			{
				return theme;
			}

			set
			{
				if (theme != null)
				{
					theme.OnChange -= ThemeChanged;
				}

				theme = value;

				if (theme != null)
				{
					theme.OnChange += ThemeChanged;
					ThemeChanged(Theme.ActiveVariationId);
				}
			}
		}

		/// <inheritdoc/>
		public override void Init()
		{
			Theme = theme;
		}

		/// <inheritdoc/>
		public override Theme GetTheme()
		{
			return Theme;
		}

		/// <inheritdoc/>
		public override void SetTheme(Theme theme)
		{
			if (theme is TTheme t)
			{
				Theme = t;
			}
		}
	}
}