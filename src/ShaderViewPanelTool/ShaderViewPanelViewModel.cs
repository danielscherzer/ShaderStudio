using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.ErrorList;
using System.ComponentModel.Composition;
using Zenseless.HLGL;

namespace ShaderStudio.ShaderViewPanelTool
{
	[Export]
	public class ShaderViewPanelViewModel : Tool
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
		{
			errorList.Items.Clear();
			var log = new ShaderLog(shaderLog);
			foreach (var line in log.Lines)
			{
				//line.Type
				errorList.AddItem(ErrorListItemType.Error, line.Message, "", line.LineNumber, null,
					() =>//TODO: filePath
					{
						//var openDocumentResult = new OpenDocumentResult(FilePath);
						//IoC.BuildUp(openDocumentResult);
						//openDocumentResult.Execute(null);
					});
			}
			shell.ShowTool(errorList);
		}

		private string _selectedShader = string.Empty;
		private IErrorList errorList;
		private IShell shell;
	}
}
