namespace SafeRoute.Revit.NetFramework.Startup
{
    public static class SessionContext
    {
        public static string BaseUrl { get; set; } = "https://localhost:7030/";
        public static string UserEmail { get; set; } = "";
        public static string AccessToken { get; set; } = "";

        public static bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(AccessToken);
    }
}
