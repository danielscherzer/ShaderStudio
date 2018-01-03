using Gemini.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace ShaderStudio.ShaderDocument
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class ShaderDocumentViewModel : PersistedDocumentEx
	{
		protected override Task DoNew()
		{
			ResetText("void main() \n{ \n\tgl_FragColor = vec4(1.0);\n}");
			return TaskUtility.Completed;
		}
	}
}
