#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;

	/// <summary>
	/// Scripting Define Symbol.
	/// </summary>
	public class ScriptingDefineSymbol
	{
		/// <summary>
		/// Symbol.
		/// </summary>
		public readonly string Symbol;

		/// <summary>
		/// Required package is installed.
		/// </summary>
		public readonly bool Installed;

		/// <summary>
		/// State.
		/// </summary>
		public readonly ScriptingDefineSymbols.State State;

		bool enabled;

		/// <summary>
		/// Enabled.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return enabled;
			}

			set
			{
				if (enabled == value)
				{
					return;
				}

				enabled = value;

				if (enabled)
				{
					ScriptingDefineSymbols.Add(Symbol);
				}
				else
				{
					ScriptingDefineSymbols.Remove(Symbol);
				}

				OnChanged?.Invoke(enabled);
			}
		}

		/// <summary>
		/// Status text.
		/// </summary>
		public string Status
		{
			get
			{
				if (!Installed)
				{
					return "Not installed";
				}

				return Enabled ? "Enabled" : "Disabled";
			}
		}

		/// <summary>
		/// Action on enable/disable.
		/// </summary>
		protected Action<bool> OnChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptingDefineSymbol"/> class.
		/// </summary>
		/// <param name="symbol">Scripting Define Symbol.</param>
		/// <param name="installed">Required package is installed.</param>
		/// <param name="onChanged">Action on enable/disable.</param>
		public ScriptingDefineSymbol(string symbol, bool installed, Action<bool> onChanged = null)
		{
			Symbol = symbol;
			Installed = installed;
			OnChanged = onChanged;

			State = ScriptingDefineSymbols.GetState(Symbol);
			enabled = Installed && State.Has > 0;
		}

		/// <summary>
		/// Enable symbols for all build targets.
		/// </summary>
		public void EnableForAll()
		{
			enabled = true;
			ScriptingDefineSymbols.Add(Symbol);
			OnChanged?.Invoke(enabled);
		}
	}
}
#endif