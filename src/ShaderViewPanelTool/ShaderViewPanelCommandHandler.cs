using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace ShaderStudio.ShaderViewPanelTool
{
	[CommandHandler]
	public class ShaderViewPanelCommandHandler : CommandHandlerBase<ShaderViewPanelCommandDefinition>
	{
		[ImportingConstructor]
		public ShaderViewPanelCommandHandler(IShell shell, IShaderViewPanelViewModel shaderPanel)
		{
			_shell = shell;
			_shaderPanel = shaderPanel;
		}

		public override Task Run(Command command)
		{
			_shell.ShowTool<IShaderViewPanelViewModel>();
			return TaskUtility.Completed;
		}

		private readonly IShell _shell;
		private readonly IShaderViewPanelViewModel _shaderPanel;
	}
}
