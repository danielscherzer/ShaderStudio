using Gemini.Modules.UndoRedo;
using ICSharpCode.AvalonEdit.Document;

namespace ShaderStudio.ShaderDocument
{
	class TextDocumentInputAction : IUndoableAction
	{
		private TextDocument document;

		public TextDocumentInputAction(TextDocument document)
		{
			this.document = document;
		}

		public string Name
		{
			get { return "My Action"; }
		}

		public void Execute()
		{
			if (document.UndoStack.CanRedo) document.UndoStack.Redo();
		}

		public void Undo()
		{
			if (document.UndoStack.CanUndo) document.UndoStack.Undo();
		}
	}
}
