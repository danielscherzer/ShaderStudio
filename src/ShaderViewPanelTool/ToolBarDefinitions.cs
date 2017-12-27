using Gemini.Framework.ToolBars;
using System.ComponentModel.Composition;

namespace ShaderStudio.ShaderViewPanelTool
{
	public static class ToolBarDefinitions
	{
		[Export]
		public static ToolBarDefinition ShaderToolBar = new ToolBarDefinition(8, "Shader");

		[Export]
		public static ToolBarItemGroupDefinition ShaderToolBarGroup = new ToolBarItemGroupDefinition(
			ShaderToolBar, 8);

		[Export]
		public static ToolBarItemDefinition ShaderViewPanelToolBarItem = new CommandToolBarItemDefinition<ShaderViewPanelCommandDefinition>(
		ShaderToolBarGroup, 0);
	}
}
