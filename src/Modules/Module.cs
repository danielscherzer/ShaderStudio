using Caliburn.Micro;
using Gemini.Framework;
using System.ComponentModel.Composition;

namespace ShaderStudio.Modules
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void PreInitialize()
		{
			base.PreInitialize();
			MainWindow.Title = "ShaderStudio";
			MainWindow.Icon = null;
		}

		public override void PostInitialize()
		{
			//var shaderDoc = IoC.Get<ShaderDocumentViewModel>();
			//shaderDoc.Load(@"D:\Daten\FH Ravensburg\Framework\ACG\Examples\2D\HelloWorld.glsl");
			//Shell.OpenDocument(shaderDoc);
			//var graph = IoC.Get<GraphViewModel>();

			//var element1 = graph.AddElement<ImageSource>(10, 10);
			//var element2 = graph.AddElement<ColorInput>(10, 200);
			//var element3 = graph.AddElement<Multiply>(300, 100);
			//element2.Color = Colors.Green;

			//graph.Connections.Add(new ConnectionViewModel(
			//	element1.OutputConnector,
			//	element3.InputConnectors[0]));

			//graph.Connections.Add(new ConnectionViewModel(
			//	element2.OutputConnector,
			//	element3.InputConnectors[1]));

			////element1.IsSelected = true;

			////IoC.Get<IPropertyGrid>().SelectedObject = graph;
			//Shell.OpenDocument(graph);
		}


	}
}
