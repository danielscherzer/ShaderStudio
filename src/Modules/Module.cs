using Caliburn.Micro;
using Gemini.Framework;
using ShaderStudio.ShaderViewPanelTool;
using System;
using System.ComponentModel;
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
			Shell.ToolBars.Visible = true;
			Shell.ActiveDocumentChanged += Shell_ActiveDocumentChanged;
		}

		public override void PostInitialize()
		{
			var shaderDoc = IoC.Get<ShaderDocumentViewModel>();
			shaderDoc.Load(@"D:\Daten\FH Ravensburg\Framework\ACG\Examples\2D\HelloWorld.glsl");
			Shell.OpenDocument(shaderDoc);

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

		private void Shell_ActiveDocumentChanged(object sender, System.EventArgs e)
		{
			var shaderDoc = Shell.ActiveItem as ShaderDocumentViewModel;
			if (shaderDoc is null) return;
			var shaderPanel = IoC.Get<ShaderViewPanelViewModel>();
			BindProperties(shaderDoc, nameof(shaderDoc.ShaderSourceCode)
				, shaderPanel, nameof(shaderPanel.SelectedShader));
		}

		private void BindProperties(INotifyPropertyChanged source, string sourcePropertyName
			, object destination, string destinationPropertyName)
		{
			var propertySource = source.GetType().GetProperty(sourcePropertyName);
			if(propertySource is null) throw new ArgumentException($"Invalid source property name '{sourcePropertyName}'");
			var propertyDestination = destination.GetType().GetProperty(destinationPropertyName);
			if (propertyDestination is null) throw new ArgumentException($"Invalid destination property name '{destinationPropertyName}'");
			//TODO: remove old link from here first
			source.PropertyChanged += (s, a) =>
			{
				if (a.PropertyName == sourcePropertyName)
				{
					var value = propertySource.GetValue(source);
					propertyDestination.SetValue(destination, value);
				}
			};
			propertyDestination.SetValue(destination, propertySource.GetValue(source));
		}
	}
}
