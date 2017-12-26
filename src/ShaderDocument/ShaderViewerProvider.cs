using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace ShaderStudio
{
	[Export(typeof(IEditorProvider))]
	class ShaderViewerProvider : IEditorProvider
	{
		public IEnumerable<EditorFileType> FileTypes
		{
			get { yield return new EditorFileType("Fragment Shader", extension); }
		}

		public IDocument Create() => IoC.Get<ShaderDocumentViewModel>();

		public bool Handles(string path) => extension == Path.GetExtension(path);

		public async Task New(IDocument document, string name)
		{
			await((ShaderDocumentViewModel)document).New(name);
		}

		public async Task Open(IDocument document, string path)
		{
			await ((ShaderDocumentViewModel)document).Load(path);
		}

		private const string extension = ".glsl";
	}
}
