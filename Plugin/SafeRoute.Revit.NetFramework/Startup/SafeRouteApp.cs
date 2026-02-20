using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SafeRoute.Revit.NetFramework.Startup
{
    public class SafeRouteApp : IExternalApplication
    {
        private const string TabName = "Case13 Tecnology";
        private const string PanelName = "Safe Route";

        public Result OnStartup(UIControlledApplication app)
        {
            try
            {
                TryCreateTab(app, TabName);

                var panel = GetOrCreatePanel(app, TabName, PanelName);
                
                TaskDialog.Show("g.resources keys", ResourcesDebug.DumpGResources());

                var assemblyPath = Assembly.GetExecutingAssembly().Location;
                var commandType = "SafeRoute.Revit.NetFramework.Commands.OpenSafeRouteCommand";

                var buttonData = new PushButtonData(
                    "SafeRoute_Open",
                    " Analyze Project ",
                    assemblyPath,
                    commandType);

                var btn = panel.AddItem(buttonData) as PushButton;

                if (btn != null)
                {
                    btn.ToolTip = "Abre o SafeRoute.";

                    // Pega o nome real do assembly carregado
                    var asmName = Assembly.GetExecutingAssembly().GetName().Name;

                    btn.Image = LoadPackImageSource(asmName, "resources/logo-16x16.png", 16);
                    btn.LargeImage = LoadPackImageSource(asmName, "resources/logo-32x32.png", 32);

                    // tooltip menor e bonito (64 por exemplo)
                    btn.ToolTipImage = LoadPackImageSource(asmName, "resources/logo-32x32.png", 64);
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("SafeRoute - Erro", ex.ToString());
                return Result.Failed;
            }
        }

        private static ImageSource LoadPackImageSource(string assemblyName, string relativePath, int decodePixels)
        {
            var uri = new Uri($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.DecodePixelWidth = decodePixels;
            bitmap.DecodePixelHeight = decodePixels;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        private static BitmapImage LoadPack(string assemblyName, string relativePath)
        {
            var uri = new Uri($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);

            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = uri;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();

            return img;
        }

        private static void TryCreateTab(UIControlledApplication app, string tabName)
        {
            try { app.CreateRibbonTab(tabName); }
            catch { }
        }

        private static RibbonPanel GetOrCreatePanel(UIControlledApplication app, string tabName, string panelName)
        {
            var panels = app.GetRibbonPanels(tabName);
            foreach (var p in panels)
            {
                if (string.Equals(p.Name, panelName, StringComparison.OrdinalIgnoreCase))
                    return p;
            }

            return app.CreateRibbonPanel(tabName, panelName);
        }
    }
}
