using Gemini.Framework;
using Gemini.Framework.Threading;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ShaderStudio
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	class ShaderDocumentViewModel : PersistedDocument
	{
		public string ShaderSourceCode
		{
			get => Document.Text;
			set
			{
				Document.Text = value;
			}
		}

		public TextDocument Document
		{
			get => _document;
			set
			{
				_document = value;
				NotifyOfPropertyChange(nameof(Document));
				NotifyOfPropertyChange(nameof(ShaderSourceCode));
			}
		}

		[ImportingConstructor]
		public ShaderDocumentViewModel()
		{
			DisplayName = "Default Shader";
			_document.Changed += (s, e) =>
			{
				NotifyOfPropertyChange(nameof(Document));
				NotifyOfPropertyChange(nameof(ShaderSourceCode));
			};
		}

		//public override void CanClose(Action<bool> callback)
		//{
		//	//if(!ReferenceEquals(null, shader)) shader.Dispose();
		//	callback(true);
		//}

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

		protected override Task DoSave(string filePath)
		{
			return TaskUtility.Completed;
		}

		private FileSystemWatcher fileWatcher;
		private TextDocument _document = new TextDocument("void main() { gl_FragColor = vec4(1.0); }");
	}
}
