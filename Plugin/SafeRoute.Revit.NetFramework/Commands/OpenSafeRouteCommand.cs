using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SafeRoute.Revit.NetFramework.Startup;
using SafeRoute.Revit.NetFramework.Views;
using System;
using System.Windows.Forms;

namespace SafeRoute.Revit.NetFramework.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class OpenSafeRouteCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;

                if (!SessionContext.IsAuthenticated)
                {
                    var login = new LoginWindow(uiApp);
                    var ok = login.ShowDialog();

                    if (ok != true)
                        return Result.Cancelled;

                    SessionContext.BaseUrl = login.BaseUrl;
                    SessionContext.AccessToken = login.AccessToken;
                    SessionContext.UserEmail = login.Email;
                }

                var menu = new MainMenuWindow(SessionContext.UserEmail);
                menu.ShowDialog();

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
