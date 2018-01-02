using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Xml;
using Gemini.Framework.Themes;
using Gemini.Modules.CodeEditor;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace ShaderStudio.ShaderDocument
{
	//[Export(typeof(ILanguageDefinition))]
	//[PartCreationPolicy(CreationPolicy.Shared)]
	public class LanguageDefinitionGlsl : ILanguageDefinition
	{
		[ImportingConstructor]
		public LanguageDefinitionGlsl(IThemeManager themeManager)
		{
			FileExtensions = new[] { ".glsl", ".frag" };
			_themeManager = themeManager;
			themeManager.CurrentThemeChanged += ThemeManager_CurrentThemeChanged;
			SelectStyle();
		}

		public string Name => SyntaxHighlighting.Name;

		public IEnumerable<string> FileExtensions { get; set; }

		public IHighlightingDefinition SyntaxHighlighting { get; private set; }

		public string CustomSyntaxHighlightingFileName { get => ""; set => System.Linq.Expressions.Expression.Empty(); }

		private readonly IThemeManager _themeManager;

		private void LoadHighlighting(Uri uri)
		{
			using (var stream = Application.GetResourceStream(uri).Stream)
			{
				using (var xml = XmlReader.Create(stream))
				{
					var xshd = HighlightingLoader.LoadXshd(xml);
					SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
				}
			}
		}

		private void SelectStyle()
		{
			string prefix = StylePrefix();
			LoadHighlighting(new Uri($"pack://application:,,,/Resources/glsl{prefix}.xshd"));
		}

		private string StylePrefix()
		{
			if (_themeManager.CurrentTheme is null) return "";
			if(_themeManager.CurrentTheme.Name.ToLowerInvariant().Contains("dark")) return "Dark";
			return "";
		}

		private void ThemeManager_CurrentThemeChanged(object sender, EventArgs e)
		{
			SelectStyle();
		}
	}
}
