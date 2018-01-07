using Gemini.Framework.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShaderStudio.ShaderDocument
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ShaderDocumentViewModel : CodeDocument
	{
		public ShaderDocumentViewModel()
		{
			TextChanged += (s, text) => Parse(text);
		}

		private void Parse(string text)
		{
			text = text.Replace('\r', ' ');
			text = RemoveMultiLineCstyleComments(text);
			text = RemoveSingleLineCstyleComments(text);
			//text = text.Replace('\n', ' ');
		}

		//private IEnumerable<string> ParseIncludes(string text)
		//{
		//	var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
		//	var pattern = @"^\s*#include\s+""([^""]+)"""; //match everything inside " except " so we get shortest ".+" match 
		//	foreach (var line in lines)
		//	{
		//		// Search for include pattern e.g. #include "raycast.glsl" 
		//		foreach (Match match in Regex.Matches(line, pattern, RegexOptions.Singleline))
		//		{
		//			string sFullMatch = match.Value;
		//			string sIncludeFileName = match.Groups[1].ToString(); // get the filename to include
		//		}
		//	}
		//}

		private string RemoveMultiLineCstyleComments(string text)
		{
			while (true)
			{
				var startComment = text.IndexOf("/*");
				if (-1 == startComment)
				{
					break;
				}
				else
				{
					//found comment start -> remove till end of comment
					var eoc = text.IndexOf("*/", startComment);
					if (-1 == eoc) eoc = text.Length;
					text = text.Remove(startComment, eoc - startComment + 2);
					continue;
				}
			}
			return text;
		}

		private static string RemoveSingleLineCstyleComments(string text)
		{
			while (true)
			{
				var startComment = text.IndexOf("//");
				if (-1 == startComment)
				{
					break;
				}
				else
				{
					//found line comment -> remove till end of line
					var eol = text.IndexOf('\n', startComment);
					if (-1 == eol) eol = text.Length;
					text = text.Remove(startComment, eol - startComment);
					continue;
				}
			}
			return text;
		}

		protected override Task DoNew()
		{
			ResetText("void main() \n{ \n\tgl_FragColor = vec4(1.0);\n}");
			return TaskUtility.Completed;
		}
	}
}
