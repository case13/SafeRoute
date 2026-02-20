using MudBlazor;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Components.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static async Task<bool> ShowConfirmDialogAsync(
            this IDialogService dialogService,
            string title,
            string message,
            string confirmText = "Confirmar",
            string cancelText = "Cancelar",
            Color confirmColor = Color.Error,
            DialogOptions? options = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ConfirmDialog.Title)] = title,
                [nameof(ConfirmDialog.Message)] = message,
                [nameof(ConfirmDialog.ConfirmText)] = confirmText,
                [nameof(ConfirmDialog.CancelText)] = cancelText,
                [nameof(ConfirmDialog.ConfirmColor)] = confirmColor,
            };

            options ??= new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true
            };

            var reference = await dialogService.ShowAsync<ConfirmDialog>(title, parameters, options);
            var result = await reference.Result;

            return !result.Canceled && result.Data is true;
        }
    }
}
