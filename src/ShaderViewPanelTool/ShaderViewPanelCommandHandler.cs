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
		public ShaderViewPanelCommandHandler(IShell shell)
		{
			_shell = shell;
		}

		public override Task Run(Command command)
		{
			_shell.ShowTool<ShaderViewPanelViewModel>(); //TODO: interface
			return TaskUtility.Completed;
		}

		private readonly IShell _shell;
	}
}
