using System.Windows.Controls;

namespace ShaderStudio.ShaderDocument
{
	/// <summary>
	/// View logic for ShaderDocumentView.xaml
	/// </summary>
	public partial class ShaderDocumentView : UserControl
	{
		public ShaderDocumentView()
		{
			InitializeComponent();
			var options = textEditor.Options;
			options.HighlightCurrentLine = true;
			options.ShowSpaces = true;
			options.ShowTabs = true;
		}
	}
}
