using Gemini.Framework;
using Gemini.Framework.Threading;
using ICSharpCode.AvalonEdit.Document;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
				IsDirty = string.Compare(_originalText, ShaderSourceCode) != 0;
				NotifyOfPropertyChange(nameof(Document));
				NotifyOfPropertyChange(nameof(ShaderSourceCode));
			};
		}

		//public override bool Equals(object obj)
		//{
		//	var other = obj as ShaderDocumentViewModel;
		//	if (other is null) return false;
		//	return string.Equals(FilePath, other.FilePath, StringComparison.InvariantCultureIgnoreCase)
		//		&& string.Equals(FileName, other.FileName, StringComparison.InvariantCultureIgnoreCase);
		//}

		public override bool ShouldReopenOnStart => true;

		public override void SaveState(BinaryWriter writer)
		{
			writer.Write(FilePath);
		}

		public override void LoadState(BinaryReader reader)
		{
			Load(reader.ReadString());
		}

		protected override Task DoNew()
		{
			_originalText = "void main() { gl_FragColor = vec4(1.0); }";
			ShaderSourceCode = _originalText;
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

		protected override Task DoSave(string filePath)
		{
			File.WriteAllText(filePath, ShaderSourceCode);
			_originalText = ShaderSourceCode;
			return TaskUtility.Completed;
		}

		private void FileNotification(object sender, FileSystemEventArgs e)
		{
			Thread.Sleep(1000);
			var dispatcher = Application.Current.Dispatcher;
			if (!dispatcher.CheckAccess())
			{
				dispatcher.Invoke(() =>
				{
					LoadShaderFromFile(FilePath);
				});
			}
			else
			{
				LoadShaderFromFile(FilePath);
			}
		}

		private void LoadShaderFromFile(string filePath)
		{
			_originalText = //ShaderLoader.ShaderStringFromFileWithIncludes(filePath, true);
				File.ReadAllText(filePath);
			ShaderSourceCode = _originalText;
		}

		private string _originalText;
		private FileSystemWatcher fileWatcher;
		private TextDocument _document = new TextDocument();
	}
}
