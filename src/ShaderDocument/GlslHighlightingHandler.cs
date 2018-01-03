using Gemini.Framework.Themes;
using ICSharpCode.AvalonEdit.Highlighting;
using System.ComponentModel.Composition;
using System.Linq;

namespace ShaderStudio.ShaderDocument
{
	[Export]
	public class GlslHighlightingHandler
	{
		[ImportingConstructor]
		public GlslHighlightingHandler(IThemeManager themeManager)
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
			var def = highlightingDefault;
			if (!(_themeManager.CurrentTheme is null))
			{
				var themeName = _themeManager.CurrentTheme.Name;
				if (themeName.ToLowerInvariant().Contains("dark")) def = highlightingDark;
			}
			var hlManager = HighlightingManager.Instance;
			var exts = GlslFileExtensions.List.ToArray();
			hlManager.RegisterHighlighting(def.Name, exts, def);
			//TODO: set new style for all docs
		}
	}
}
