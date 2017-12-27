using Gemini.Framework;

namespace ShaderStudio.ShaderViewPanelTool
{
	public interface IShaderViewPanelViewModel : ITool
	{
		string SelectedShader { get; set; }

		void UpdateLog(string shaderLog);
	}
}