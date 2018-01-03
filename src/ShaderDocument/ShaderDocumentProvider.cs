using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShaderStudio.ShaderDocument
{
	[Export(typeof(IEditorProvider))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ShaderDocumentProvider : IEditorProvider
	{
		[ImportingConstructor]
		public ShaderDocumentProvider(GlslHighlightingHandler themeHandler) //make sure the GLSL theme handler is instantiated
		{
			FileTypes = from ext in GlslFileExtensions.List
						select new EditorFileType("Shader", ext);
		}

		public IEnumerable<EditorFileType> FileTypes { get; private set; }

		public IDocument Create() => IoC.Get<ShaderDocumentViewModel>();

		public bool Handles(string path) => GlslFileExtensions.Contains(Path.GetExtension(path));

		public async Task New(IDocument document, string name) => await ((ShaderDocumentViewModel)document).New(name);

		public async Task Open(IDocument document, string path) => await ((ShaderDocumentViewModel)document).Load(path);
	}
}
