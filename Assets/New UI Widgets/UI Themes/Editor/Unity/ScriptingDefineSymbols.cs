﻿#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;

	/// <summary>
	/// Scripting Define Symbols.
	/// </summary>
	public static class ScriptingDefineSymbols
	{
		/// <summary>
		/// State of ScriptingDefineSymbol.
		/// </summary>
		public struct State
		{
			/// <summary>
			/// Count of build targets with ScriptingDefineSymbol.
			/// </summary>
			public readonly int Has;

			/// <summary>
			/// Total count of build targets with ScriptingDefineSymbol.
			/// </summary>
			public readonly int Total;

			/// <summary>
			/// Is any build target has ScriptingDefineSymbol.
			/// </summary>
			public bool Any
			{
				get
				{
					return Has > 0;
				}
			}

			/// <summary>
			/// All build targets has ScriptingDefineSymbol.
			/// </summary>
			public bool All
			{
				get
				{
					return Has >= Total;
				}
			}

			/// <summary>
			/// Some build targets does not have ScriptingDefineSymbol.
			/// </summary>
			public bool HasMissing
			{
				get
				{
					return Any && !All;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="State"/> struct.
			/// </summary>
			/// <param name="has">Count of build targets with ScriptingDefineSymbol.</param>
			/// <param name="total">Total count of build targets with ScriptingDefineSymbol.</param>
			public State(int has, int total)
			{
				Has = has;
				Total = total;
			}
		}

		static List<BuildTargetGroup> targets;

		static IReadOnlyList<BuildTargetGroup> Targets
		{
			get
			{
				if (targets != null)
				{
					return targets;
				}

				targets = new List<BuildTargetGroup>();
				foreach (var v in Enum.GetValues(typeof(BuildTarget)))
				{
					var target = (BuildTarget)v;
					var group = BuildPipeline.GetBuildTargetGroup(target);
					if (IsBuildTargetSupported(group, target) && !targets.Contains(group))
					{
						targets.Add(group);
					}
				}

				return targets;
			}
		}

		static bool IsBuildTargetSupported(BuildTargetGroup group, BuildTarget target)
		{
#if UNITY_2018_1_OR_NEWER
			return BuildPipeline.IsBuildTargetSupported(group, target);
#else
			var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
			var buildTargetSupported = typeof(BuildPipeline).GetMethod("IsBuildTargetSupported", flags);
			return Convert.ToBoolean(buildTargetSupported.Invoke(null, new object[] { group, target }));
#endif
		}

		/// <summary>
		/// Add scripting define symbols.
		/// </summary>
		/// <param name="symbol">Symbol to add.</param>
		public static void Add(string symbol)
		{
			foreach (var target in Targets)
			{
				var symbols = Symbols(target);

				if (symbols.Contains(symbol))
				{
					continue;
				}

				symbols.Add(symbol);

				Save(symbols, target);
			}
		}

		/// <summary>
		/// Remove scripting define symbols.
		/// </summary>
		/// <param name="symbol">Symbol to remove.</param>
		public static void Remove(string symbol)
		{
			foreach (var target in Targets)
			{
				var symbols = Symbols(target);

				if (!symbols.Contains(symbol))
				{
					continue;
				}

				symbols.Remove(symbol);

				Save(symbols, target);
			}
		}

		/// <summary>
		/// Get scripting define symbols.
		/// </summary>
		/// <param name="targetGroup">Target group.</param>
		/// <returns>Scripting define symbols.</returns>
		static HashSet<string> Symbols(BuildTargetGroup targetGroup)
		{
			return UtilitiesEditor.GetScriptingDefineSymbols(targetGroup);
		}

		/// <summary>
		/// Get symbol state.
		/// </summary>
		/// <param name="symbol">Scripting Define Symbol.</param>
		/// <returns>Symbol state.</returns>
		public static State GetState(string symbol)
		{
			var has = 0;
			foreach (var target in Targets)
			{
				var a = EditorUserBuildSettings.selectedBuildTargetGroup;
				if (Symbols(target).Contains(symbol))
				{
					has += 1;
				}
			}

			return new State(has, Targets.Count);
		}

		static void Save(HashSet<string> symbols, BuildTargetGroup target)
		{
			UtilitiesEditor.SetScriptingDefineSymbols(target, symbols);
			AssetDatabase.Refresh();
		}
	}
}
#endif