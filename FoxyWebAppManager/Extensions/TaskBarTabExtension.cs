using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Extensions
{
    public static class TaskBarTabExtension
    {
        extension(TaskbarTab taskbarTab) 
        {
            public Uri GetStartUrl()
                => new(taskbarTab.startUrl);
            
            public string DomainName()
                => taskbarTab.GetStartUrl().GetDomainNameWithoutExtension();
        
        }
    }
}
