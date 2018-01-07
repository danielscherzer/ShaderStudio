using Gemini.Modules.UndoRedo;
using ICSharpCode.AvalonEdit;

namespace ShaderStudio.ShaderDocument
{
	public class AvalonEditInputAction : IUndoableAction
	{
		private TextEditor textEditor;

		public AvalonEditInputAction(TextEditor textEditor)
		{
			this.textEditor = textEditor;
		}

		public string Name
		{
			get { return "My Action"; }
		}

		public void Execute()
		{
			if (textEditor.CanRedo) textEditor.Redo();
		}

		public void Undo()
		{
			if (textEditor.CanUndo) textEditor.Undo();
		}
	}
}
