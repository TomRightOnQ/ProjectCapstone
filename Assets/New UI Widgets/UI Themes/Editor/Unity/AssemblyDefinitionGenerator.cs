#if UNITY_EDITOR
namespace UIThemes.Editor
{
	using System.Collections.Generic;
	using System.IO;
	using UnityEditor;

	/// <summary>
	/// Assembly definitions generator.
	/// </summary>
	public class AssemblyDefinitionGenerator
	{
		struct AssemblyDefinition
		{
			public readonly string Name;

			public readonly string References;

			public readonly bool Editor;

			public AssemblyDefinition(string name, bool editor, IReadOnlyList<string> references)
			{
				Name = name;
				Editor = editor;
				References = references.Count > 0
					? '"' + string.Join("\", \"", references) + '"'
					: string.Empty;
			}

			public void Save(string folder)
			{
				var path = folder + Path.DirectorySeparatorChar + Name + ".asmdef";
				if (File.Exists(path))
				{
					return;
				}

				File.WriteAllText(path, Content());
			}

			string Content()
			{
				var platforms = Editor
					? "\"includePlatforms\": [\"Editor\"], \"excludePlatforms\": [],"
					: string.Empty;

				return string.Format(ReferencesGUIDs.AsmdefTemplate, Name, References, platforms);
			}
		}

		/// <summary>
		/// Create assembly definitions.
		/// </summary>
		public static void Create()
		{
			var type = typeof(AssemblyDefinitionGenerator);
			if (!type.Module.Name.Contains("Assembly-CSharp"))
			{
				return;
			}

			if (string.IsNullOrEmpty(ReferencesGUIDs.ScriptsFolder)
				|| string.IsNullOrEmpty(ReferencesGUIDs.EditorFolder)
				|| string.IsNullOrEmpty(ReferencesGUIDs.SamplesFolder)
				|| string.IsNullOrEmpty(ReferencesGUIDs.AsmdefTemplateFile))
			{
				return;
			}

			var assemblies = ProjectSettings.GetAssemblies();
			var main = new AssemblyDefinition("UIThemes", false, assemblies);

			assemblies.Add("UIThemes");
			var samples = new AssemblyDefinition("UIThemes.Samples", false, assemblies);
			var editor = new AssemblyDefinition("UIThemes.Editor", true, assemblies);

			main.Save(ReferencesGUIDs.ScriptsFolder);
			samples.Save(ReferencesGUIDs.SamplesFolder);
			editor.Save(ReferencesGUIDs.EditorFolder);
		}

		/// <summary>
		/// Delete assembly definitions.
		/// </summary>
		public static void Delete()
		{
			Delete(ReferencesGUIDs.ScriptsFolder);
			Delete(ReferencesGUIDs.SamplesFolder);
			Delete(ReferencesGUIDs.EditorFolder);

			AssetDatabase.Refresh();
		}

		static void Delete(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			var files = Directory.GetFiles(path, "*.asmdef", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				AssetDatabase.DeleteAsset(file);
			}
		}
	}
}
#endif