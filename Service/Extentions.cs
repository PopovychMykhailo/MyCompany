namespace MyCompany.Service
{
    public static class Extentions
    {
        public static string CutController(this string controllerName)
        {
            return controllerName.Replace("Controller", "");
        }
    }
}
