using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SafeRoute.Revit.NetFramework.Services;
using SafeRoute.Shared.Enums.Normas;

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
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;

            if (uiDoc == null)
            {
                message = "No active document.";
                return Result.Failed;
            }

            Document document = uiDoc.Document;

            // Fase 1: norma fixa (depois vem da UI)
            var norma = NormaTypeEnum.NBR_9050;

            var ingestionService = new ModelIngestionService(document);

            var doors = ingestionService.GetDoors(norma);
            var ramps = ingestionService.GetRamps(norma);

            // Fase 1: apenas validação visual
            TaskDialog.Show(
                "SafeRoute",
                $"Extraction completed:\n\nDoors: {doors.Count}\nRamps: {ramps.Count}"
            );

            return Result.Succeeded;
        }
    }
}
