using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SafeRoute.Revit.NetFramework.Auth;
using SafeRoute.Revit.NetFramework.Services;
using SafeRoute.Revit.NetFramework.Settings;
using SafeRoute.Revit.NetFramework.Views;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using SafeRoute.Shared.Enums.Normas;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.Revit.NetFramework.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ExtractModelCommand : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;

                if (uiDoc == null)
                {
                    message = "No active document.";
                    return Result.Failed;
                }

                var document = uiDoc.Document;
                var norma = NormaTypeEnum.NBR_9050;

                var ingestionService = new ModelIngestionService(document);

                var doors = ingestionService.GetDoors(norma).ToList();
                var ramps = ingestionService.GetRamps(norma).ToList();

                if (doors.Count == 0 && ramps.Count == 0)
                {
                    TaskDialog.Show("SafeRoute", "No elements found.");
                    return Result.Succeeded;
                }

                string baseUrl;
                string accessToken;

                if (!AuthSession.IsLogged)
                {
                    var login = new LoginWindow(uiApp);
                    var ok = login.ShowDialog();

                    if (ok != true)
                        return Result.Cancelled;

                    baseUrl = login.BaseUrl;
                    accessToken = login.AccessToken;

                    AuthSession.SetToken(accessToken);
                }
                else
                {
                    var settings = PluginSettingsProvider.Load();
                    baseUrl = settings.BaseUrl ?? "https://localhost:7030/";
                    accessToken = AuthSession.AccessToken;
                }

                var dto = new IngestProjectElementsRequestDto
                {
                    ProjectExternalId = document.ProjectInformation.UniqueId,
                    ProjectName = document.Title,
                    Doors = doors,
                    Ramps = ramps
                };

                var apiClient = new SafeRouteApiClient(baseUrl, accessToken);

                var result = Task.Run(() => apiClient.ReplaceProjectAsync(dto))
                    .GetAwaiter().GetResult();

                if (result == null)
                {
                    TaskDialog.Show("SafeRoute", "Error sending data to API.");
                    return Result.Failed;
                }

                TaskDialog.Show(
                    "SafeRoute",
                    "Extraction completed successfully.\n\n" +
                    "Deleted: " + result.Deleted + "\n" +
                    "Inserted: " + result.Inserted
                );

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("SafeRoute", ex.Message);
                return Result.Failed;
            }
        }
    }
}
