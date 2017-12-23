using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gemini.Modules.ErrorList;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zenseless.HLGL;

namespace ShaderStudio
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	class ShaderDocumentViewModel : PersistedDocument
	{
		public string ShaderSourceCode
		{
			get => _source;
			set
			{
				_source = value;
				NotifyOfPropertyChange(nameof(ShaderSourceCode));
			}
		}

		[ImportingConstructor]
		public ShaderDocumentViewModel(IErrorList errorList, IShell shell)
		{
			DisplayName = "Default Shader";
			this.errorList = errorList;
			this.shell = shell;
		}

		public override void CanClose(Action<bool> callback)
		{
			//if(!ReferenceEquals(null, shader)) shader.Dispose();
			callback(true);
		}

		//public override bool Equals(object obj)
		//{
		//	var other = obj as ShaderViewModel;
		//	if (ReferenceEquals(null, other)) return false;
		//	return string.Equals(FilePath, other.FilePath, StringComparison.InvariantCultureIgnoreCase);
		//}

		protected override Task DoNew()
		{
			return TaskUtility.Completed;
		}

		protected override Task DoLoad(string filePath)
		{
			fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), FileName);
			fileWatcher.Changed += FileNotification;
			//visual studio does not change a file, but saves a copy and later deletes the original and renames
			fileWatcher.Created += FileNotification;
			fileWatcher.Renamed += FileNotification;
			fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName;
			fileWatcher.EnableRaisingEvents = true;

			LoadShaderFromFile(filePath);
			return TaskUtility.Completed;
		}

		private void FileNotification(object sender, FileSystemEventArgs e)
		{
			Thread.Sleep(1000);
			LoadShaderFromFile(FilePath);
		}

		private void LoadShaderFromFile(string filePath)
		{
			ShaderSourceCode = //ShaderLoader.ShaderStringFromFileWithIncludes(filePath, true);
				File.ReadAllText(filePath);
		}

		//private void LogShaderException(ShaderException e)
		//{
		//	if (string.IsNullOrEmpty(e.ShaderLog))
		//	{
		//		errorList.AddItem(ErrorListItemType.Error, e.Message);
		//		shell.ShowTool(errorList);
		//	}
		//	else
		//	{
		//		UpdateLog(e.ShaderLog);
		//	}
		//}

		public void UpdateLog(string shaderLog)
		{
			errorList.Items.Clear();
			var log = new ShaderLog(shaderLog);
			foreach (var line in log.Lines)
			{
				//line.Type
				errorList.AddItem(ErrorListItemType.Error, line.Message, FilePath, line.LineNumber, null,
					() =>
					{
						//var openDocumentResult = new OpenDocumentResult(FilePath);
						//IoC.BuildUp(openDocumentResult);
						//openDocumentResult.Execute(null);
					});
			}
			shell.ShowTool(errorList);
		}

		protected override Task DoSave(string filePath)
		{
			return TaskUtility.Completed;
		}

		private IErrorList errorList;
		private IShell shell;
		private FileSystemWatcher fileWatcher;
		private string _source;
	}
}
