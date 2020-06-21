using System;
using System.IO;
using System.Text;
using Dolanan.Controller;
using Dolanan.Core.Utility;
using Dolanan.Editor.ImGui;
using Dolanan.Engine;

namespace Dolanan.Editor
{
	using ig = ImGuiNET.ImGui;

	public static class EditorSettings
	{
#if DEBUG
		/// <summary>
		/// 	Project path contains all main script and assets content. If singleplatform, you don't need to modify in
		/// 	Editor Window, but if multiplatform, you need to change the directory path to the main shared project
		/// </summary>
		public static string ProjectPath
		{
			get
			{
				if (_projectPath == "")
				{
					_projectPath = Directory.GetParent(
						Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName).ToString();
				}

				return _projectPath;
			}
			internal set => _projectPath = value;
		}

		public static string EditorSettingPath = "EditorSettings.cfg";
	
		private static string _projectPath = "";
		
		public static bool IsSetupDone = false;

		public static void Init()
		{
			if (!File.Exists(EditorSettingPath))
			{
				Save();
			}

			Load();
			Console.WriteLine(_projectPath);
			
			if(_projectPath == "")
				_projectPath = Directory.GetParent(
					Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName).ToString();
			
			if(!IsSetupDone) GameMgr.Game.OnImGuiDraw += DrawImGuiWindow;
		}

		public static bool ShowWindow
		{
			get => _showWindow;
			set => _showWindow = value;
		}
		private static bool _showWindow = true;
		private static void DrawImGuiWindow()
		{
			if (ShowWindow)
			{
				ig.SetNextWindowSize(new System.Numerics.Vector2(650, 200), ImGuiNET.ImGuiCond.Appearing);
				ig.Begin("Editor Settings", ref _showWindow);
				ImGuiMg.InputText("Project Path", ref _projectPath, 100);
				
				if (ig.Button("Save"))
				{
					FileDirectory.WriteFile(EditorSettingPath, ToStrCfg());
				}
				ig.End();
			}
		}

		public static void Load()
		{
			var configs = DolananParser.ParseCfg(File.ReadAllText(EditorSettingPath));

			foreach (var s in configs)
			{
				var keyVal = s.Split('=');
				switch (keyVal[0])
				{
					case "ProjectPath":
						if(keyVal.Length > 1)
							ProjectPath = keyVal[1];
						break;
				}
			}
		}
		
		public static void Save()
		{
			File.WriteAllText(EditorSettingPath, ToStrCfg());
		}

		static string ToStrCfg()
		{
			string newCfg = "";
			var oldCfg = DolananParser.ParseCfg(File.ReadAllText(EditorSettingPath));

			foreach (var s in oldCfg)
			{
				var keyVal = s.Split('=');
				newCfg += keyVal[0] + " = ";
				switch (keyVal[0])
				{
					case "ProjectPath":
						newCfg += ProjectPath;
						break;
					
				}
				
				newCfg += "\n";
			}

			return newCfg.Remove(newCfg.Length - 1);
		}
#endif
	}
}