namespace DL.AccountChecker.Framework
{
    public static class EndPoints
    {
        public const string ExcelService = "AccountCheckerExcelServiceEndPoint";
        public const string GuiService = "AccountCheckerGuiServiceEndPoint";
        public const string SubscriptionService = "AccountCheckerSubscriptionServiceEndPoint";
    }

    public static class Settings
    {
        public const string CacheDestinations = "CacheDestinations";
        public const string ExcelController = "ExcelController";
        public const string MessagePublisher = "RabbitMqPublisher";
    }
}
