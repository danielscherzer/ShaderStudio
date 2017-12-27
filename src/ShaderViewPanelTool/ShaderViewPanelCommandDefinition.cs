using Gemini.Framework.Commands;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace ShaderStudio.ShaderViewPanelTool
{
	[CommandDefinition]
	public class ShaderViewPanelCommandDefinition : CommandDefinition
	{
		public const string CommandName = "View.ShaderViewPanel";

		public override string Name => CommandName;

		public override string Text => "Shader View Panel";

		public override string ToolTip => Text;

		public override Uri IconSource
		{
			get { return new Uri("pack://application:,,,/Gemini;component/Resources/Icons/Undo.png"); } //TODO: add icon
		}

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ShaderViewPanelCommandDefinition>(new KeyGesture(Key.V, ModifierKeys.Control));
	}
}
