using Caliburn.Micro;
using Gemini.Framework.Services;
using System.Windows.Controls;

namespace ShaderStudio.ShaderDocument
{
	/// <summary>
	/// View logic for ShaderDocumentView.xaml
	/// </summary>
	public partial class ShaderDocumentView : UserControl
	{
		public ShaderDocumentView()
		{
			InitializeComponent();
			var options = textEditor.Options;
			options.HighlightCurrentLine = true;
			options.ShowSpaces = true;
			options.ShowTabs = true;
			//textEditor.DocumentChanged += (se, ev) =>
			//{
			//	textEditor.Document.Changed += (s, e) =>
			//	{
			//		var undoRedoManager = IoC.Get<IShell>().ActiveItem.UndoRedoManager;
			//		undoRedoManager.ExecuteAction(new AvalonEditInputAction(textEditor));
			//	};
			//};
		}

		private void UndoCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			textEditor.Undo();
		}

		private void UndoCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (textEditor is null) ? false : textEditor.CanUndo;
		}
	}
}
