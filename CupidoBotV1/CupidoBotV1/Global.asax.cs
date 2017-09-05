using BotBuilder.Instrumentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace CupidoBotV1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public readonly BotFrameworkApplicationInsightsInstrumentation DefaultInstrumentation = DependencyResolver.Current.DefaultInstrumentationWithCognitiveServices as BotFrameworkApplicationInsightsInstrumentation;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
