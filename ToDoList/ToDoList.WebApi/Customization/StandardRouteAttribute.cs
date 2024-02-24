using Microsoft.AspNetCore.Mvc.Routing;

namespace ToDoListManager.WebApi.Customization
{
    public class StandardRouteAttribute : Attribute, IRouteTemplateProvider
    {
        public StandardRouteAttribute()
        {
        }
        public StandardRouteAttribute(string controllerName)
        {
            Controller = controllerName;
        }
        public string Template => $"api/to-do-list-manager/{Version ?? "v1"}/{Controller ?? "[controller]"}";

        public int? Order => 1;

        public string Name { get; set; }
        public string Version { get; set; }
        public string Controller { get; set; }
    }
}
