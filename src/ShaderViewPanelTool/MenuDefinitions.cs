using Gemini.Framework.Menus;
using System.ComponentModel.Composition;

namespace ShaderStudio.ShaderViewPanelTool
{
	public static class MenuDefinitions
	{
		[Export]
		public static MenuItemDefinition ShaderViewPanelMenuItem = new CommandMenuItemDefinition<ShaderViewPanelCommandDefinition>(
			Gemini.Modules.MainMenu.MenuDefinitions.ViewPropertiesMenuGroup, 0);
	}
}
