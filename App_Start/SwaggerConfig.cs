using System.Web.Http;
using WebActivatorEx;
using DefaultWebProject;
using Swashbuckle.Application;
using DefaultWebProject.App_Start;

namespace DefaultWebProject
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "DefaultWebProject");//Versão da documentação e Nome 
                    c.IncludeXmlComments(GetXmlCommentsPath());//Permite colocar uma descrição nos Metodos
                  
                })
                .EnableSwaggerUi(c =>
                    {

                    });
        }
        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\DefaultWebProject.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
