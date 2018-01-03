using System.Collections.Generic;
using System.Linq;

namespace ShaderStudio.ShaderDocument
{
	public static class GlslFileExtensions
	{
		public static readonly IEnumerable<string> List = new [] { ".glsl", ".frag", ".vert", };

		public static bool Contains(string fileExtension) => List.Contains(fileExtension);
	}
}
