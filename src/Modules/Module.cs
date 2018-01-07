using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.Inspector;
using ShaderStudio.ShaderDocument;
using ShaderStudio.ShaderViewPanelTool;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;

namespace ShaderStudio.Modules
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void PreInitialize()
		{
			base.PreInitialize();
			MainWindow.Title = "ShaderStudio";
			MainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/ShaderIcon.png"));
			Shell.ToolBars.Visible = true;
			Shell.ActiveDocumentChanged += Shell_ActiveDocumentChanged;
		}

		public override void PostInitialize()
		{
		}

		private void Shell_ActiveDocumentChanged(object sender, EventArgs e)
		{
			var shaderDoc = Shell.ActiveItem as ShaderDocumentViewModel;
			if (shaderDoc is null) return;
			ConnectPanel(shaderDoc);
			ConnectInspector(shaderDoc);
		}

		private static void ConnectInspector(ShaderDocumentViewModel shaderDoc)
		{
			var inspectorTool = IoC.Get<IInspectorTool>();
			var objBuilder = new InspectableObjectBuilder().WithObjectProperties(shaderDoc, pd => true);
			inspectorTool.SelectedObject = objBuilder.ToInspectableObject();
		}

		private static void ConnectPanel(ShaderDocumentViewModel shaderDoc)
		{
			var shaderPanel = IoC.Get<IShaderViewPanelViewModel>();
			BindProperties(shaderDoc, nameof(shaderDoc.Text)
				, shaderPanel, nameof(shaderPanel.SelectedShader));
		}

		private static void BindProperties(INotifyPropertyChanged source, string sourcePropertyName
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
