using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.ErrorList;
using System.ComponentModel.Composition;
using System.Linq;
using Zenseless.HLGL;

namespace ShaderStudio.ShaderViewPanelTool
{
	[Export(typeof(IShaderViewPanelViewModel))]
	public class ShaderViewPanelViewModel : Tool, IShaderViewPanelViewModel
	{
		public override PaneLocation PreferredLocation => PaneLocation.Right;

		public string SelectedShader
		{
			get => _selectedShader;
			set
			{
				_selectedShader = value;
				NotifyOfPropertyChange(() => SelectedShader);
			}
		}

		[ImportingConstructor]
		public ShaderViewPanelViewModel(IErrorList errorList, IShell shell)
		{
			DisplayName = "Shader View Panel";
			this.errorList = errorList;
			this.shell = shell;
		}

		public void UpdateLog(string shaderLog)
		{//TODO: no error logs when closed -> no closing or split
			errorList.Items.Clear();
			var log = new ShaderLog(shaderLog);
			foreach (var line in log.Lines)
			{
				var type = ShaderLogLine.WellKnownTypeError == line.Type ? ErrorListItemType.Error : ErrorListItemType.Warning;
				errorList.AddItem(type, line.Message, "", line.LineNumber, null,
					() =>//TODO: filePath
					{
						//var openDocumentResult = new OpenDocumentResult(FilePath);
						//IoC.BuildUp(openDocumentResult);
						//openDocumentResult.Execute(null);
					});
			}
			if (log.Lines.Count() > 0)
			{
				shell.ShowTool(errorList);
			}
			else
			{
				errorList.IsVisible = false;
			}
		}

		private string _selectedShader = string.Empty;
		private IErrorList errorList;
		private IShell shell;
	}
}
