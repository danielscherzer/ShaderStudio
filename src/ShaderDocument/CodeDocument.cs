using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ShaderStudio.ShaderDocument
{
	public abstract class CodeDocument : PersistedDocument
	{
		public event EventHandler<string> TextChanged;

		public string Text
		{
			get => Document.Text;
			set
			{
				Document.Text = value;
				NotifyTextRelatedProperties();
			}
		}

		public TextDocument Document
		{
			get => _document;
			set
			{
				_document = value;
				NotifyTextRelatedProperties();
			}
		}

		private void NotifyTextRelatedProperties()
		{
			NotifyOfPropertyChange(nameof(Document));
			NotifyOfPropertyChange(nameof(Text));
			TextChanged?.Invoke(this, Text);
		}

		public CodeDocument()
		{
			//var def = IoC.Get<ICommandService>().GetCommandDefinition(typeof(SaveFileAsCommandDefinition));
			//var command = IoC.Get<ICommandService>().GetCommand(def);
			//saveAsCommand = IoC.Get<ICommandService>().GetTargetableCommand(command);

			_document.Changed += (s, e) =>
			{
				IsDirty = string.Compare(_originalText, Text) != 0;
				NotifyTextRelatedProperties();
				//var undoRedoManager = IoC.Get<IShell>().ActiveItem.UndoRedoManager;
				//undoRedoManager.ExecuteAction(new TextDocumentInputAction(Document));
			};
		}

		public override void CanClose(Action<bool> callback)
		{
			var result = MessageBoxResult.No;
			if (IsDirty)
			{
				result = MessageBox.Show($"Do you want to save the changes made to '{FileName}'?", "Confirm", MessageBoxButton.YesNoCancel);
				if (MessageBoxResult.Yes == result)
				{
					Save(FilePath);
				}
			}
			callback(MessageBoxResult.Cancel != result);
		}

		public override bool Equals(object obj)
		{
			var other = obj as ShaderDocumentViewModel;
			if (other is null) return false;
			return string.Equals(FilePath, other.FilePath, StringComparison.InvariantCultureIgnoreCase)
				&& string.Equals(FileName, other.FileName, StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode()
		{
			return 35528798 + EqualityComparer<string>.Default.GetHashCode(FilePath);
		}

		public override bool ShouldReopenOnStart => true;

		public override void SaveState(BinaryWriter writer)
		{
			writer.Write(FilePath);
		}

		public override void LoadState(BinaryReader reader)
		{
			Load(reader.ReadString());
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

			LoadTextFromFile(filePath);
			return TaskUtility.Completed;
		}

		protected override Task DoNew()
		{
			ResetText();
			return TaskUtility.Completed;
		}

		protected override Task DoSave(string filePath)
		{
			File.WriteAllText(filePath, Text);
			_originalText = Text;
			return TaskUtility.Completed;
		}

		protected void ResetText(string text = "")
		{
			_originalText = text;
			Text = _originalText;
			Document.UndoStack.ClearAll();
		}

		private TextDocument _document = new TextDocument();
		private string _originalText;
		private FileSystemWatcher fileWatcher;

		private void FileNotification(object sender, FileSystemEventArgs e)
		{
			Thread.Sleep(1000); //TODO: hack; make more stable
			var dispatcher = Application.Current.Dispatcher;
			if (!dispatcher.CheckAccess())
			{
				dispatcher.Invoke(() =>
				{
					LoadTextFromFile(FilePath);
				});
			}
			else
			{
				LoadTextFromFile(FilePath);
			}
		}

		private void LoadTextFromFile(string filePath)
		{
			ResetText(File.ReadAllText(filePath));
		}
	}
}
