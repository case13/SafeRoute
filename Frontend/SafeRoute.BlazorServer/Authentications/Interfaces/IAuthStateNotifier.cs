namespace SafeRoute.BlazorServer.Authentications.Interfaces
{
    public interface IAuthStateNotifier
    {
        void NotifyUserAuthentication(string accessToken);
        void NotifyUserLogout();
    }
}
