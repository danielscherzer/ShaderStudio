using System.Collections.Generic;
using System.Linq;

namespace ShaderStudio.ShaderDocument
{
	public static class GlslFileExtensions
	{
		public static readonly IEnumerable<string> FileExtensions = new [] { ".glsl", ".frag", ".vert", };

		public static bool Contains(string fileExtension) => FileExtensions.Contains(fileExtension);
	}
}
