using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml;

namespace ShaderStudio.ShaderDocument
{
	public static class HighlightingLoaderEx
	{
		public static IHighlightingDefinition FromResource(string resourceName)
		{
			try
			{
				var uri = new Uri($"pack://application:,,,/{resourceName}");
				using (var stream = Application.GetResourceStream(uri).Stream)
				{
					using (var xml = XmlReader.Create(stream))
					{
						var xshd = HighlightingLoader.LoadXshd(xml);
						return HighlightingLoader.Load(xshd, null);
					}
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return HighlightingManager.Instance.HighlightingDefinitions.FirstOrDefault();
			}
		}
	}
}
