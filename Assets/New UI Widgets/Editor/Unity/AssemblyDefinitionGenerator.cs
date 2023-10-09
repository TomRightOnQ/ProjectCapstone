#if UNITY_EDITOR
namespace UIWidgets
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

				return string.Format(ReferenceGUID.AsmdefTemplate, Name, References, platforms);
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

			if (string.IsNullOrEmpty(ReferenceGUID.ScriptsFolder)
				|| string.IsNullOrEmpty(ReferenceGUID.EditorFolder)
				|| string.IsNullOrEmpty(ReferenceGUID.SamplesFolder)
				|| string.IsNullOrEmpty(ReferenceGUID.AsmdefTemplateFile))
			{
				return;
			}

			var assemblies = ProjectSettings.GetAssemblies();
			var main = new AssemblyDefinition("UIWidgets", false, assemblies);

			assemblies.Add("UIWidgets");
			var samples = new AssemblyDefinition("UIWidgets.Samples", false, assemblies);

			assemblies.Add("UIThemes.Editor");
			var editor = new AssemblyDefinition("UIWidgets.Editor", true, assemblies);

			main.Save(ReferenceGUID.ScriptsFolder);
			samples.Save(ReferenceGUID.SamplesFolder);
			editor.Save(ReferenceGUID.EditorFolder);

			UIThemes.Editor.AssemblyDefinitionGenerator.Create();
		}

		/// <summary>
		/// Delete assembly definitions.
		/// </summary>
		public static void Delete()
		{
			Delete(ReferenceGUID.ScriptsFolder);
			Delete(ReferenceGUID.SamplesFolder);
			Delete(ReferenceGUID.EditorFolder);

			UIThemes.Editor.AssemblyDefinitionGenerator.Delete();

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