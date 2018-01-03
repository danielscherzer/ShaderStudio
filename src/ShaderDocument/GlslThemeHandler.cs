using Gemini.Framework.Themes;
using ICSharpCode.AvalonEdit.Highlighting;
using System.ComponentModel.Composition;
using System.Linq;

namespace ShaderStudio.ShaderDocument
{
	[Export]
	public class GlslThemeHandler
	{
		[ImportingConstructor]
		public GlslThemeHandler(IThemeManager themeManager)
		{
			highlightingDefault = HighlightingLoaderEx.FromResource("Resources/glsl.xshd");
			highlightingDark = HighlightingLoaderEx.FromResource("Resources/glsl_dark.xshd");

			_themeManager = themeManager;
			_themeManager.CurrentThemeChanged += (s, e) => SelectStyle();
			SelectStyle();
		}

		private readonly IHighlightingDefinition highlightingDefault;
		private readonly IHighlightingDefinition highlightingDark;
		private readonly IThemeManager _themeManager;

		private void SelectStyle()
		{
			IHighlightingDefinition SelectHighlightingDef()
			{
				if (_themeManager.CurrentTheme is null) return highlightingDefault;
				if (_themeManager.CurrentTheme.Name.ToLowerInvariant().Contains("dark")) return highlightingDark;
				return highlightingDefault;
			}
			var def = SelectHighlightingDef();
			var hlManager = HighlightingManager.Instance;
			hlManager.RegisterHighlighting(def.Name, GlslFileExtensions.FileExtensions.ToArray(), def);
			//TODO: set new style for all docs
		}
	}
}
