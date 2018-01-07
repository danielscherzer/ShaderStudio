using Gemini.Framework.Commands;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace ShaderStudio.ShaderViewPanelTool
{
	[CommandDefinition]
	public class ShaderViewPanelCommandDefinition : CommandDefinition
	{
		public override string Name => "View.ShaderViewPanel";

		public override string Text => "Shader View Panel";

		public override string ToolTip => Text;

		public override Uri IconSource => new Uri("/Resources/ShaderViewPanel.png", UriKind.Relative);

		[Export]
		public static CommandKeyboardShortcut KeyGesture = 
			new CommandKeyboardShortcut<ShaderViewPanelCommandDefinition>(
			new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift));
	}
}
