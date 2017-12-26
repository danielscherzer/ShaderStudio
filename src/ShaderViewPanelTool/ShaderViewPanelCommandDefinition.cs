using Gemini.Framework.Commands;

namespace ShaderStudio.ShaderViewPanelTool
{
	[CommandDefinition]
	public class ShaderViewPanelCommandDefinition : CommandDefinition
	{
		public const string CommandName = "View.ShaderViewPanel";

		public override string Name => CommandName;

		public override string Text => "Shader View Panel";

		public override string ToolTip => Text;
	}
}
