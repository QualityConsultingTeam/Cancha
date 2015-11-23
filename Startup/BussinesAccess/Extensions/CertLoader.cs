using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BussinesAccess.Extensions
{
    public class CertLoader
    {

        public static X509Certificate2 Load()
        {
            var raw = GetResourceFile(Constants.CertResourceName + ".pfx");
            return new X509Certificate2(raw, Constants.CertPassword);
        }
        static byte[] GetResourceFile(string partialPath)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            var targetResouceName = assembly.GetManifestResourceNames().
                    FirstOrDefault(f => f.Contains(partialPath));

            using (var resource = assembly.GetManifestResourceStream(targetResouceName))
            {
                if (resource == null)
                {
                    var error = string.Format("No se encontro el Certificado en Recursos . target resource : {0}",
                        targetResouceName);

                    throw new Exception(error);
                }


                byte[] b;

                using (var br = new System.IO.BinaryReader(resource))
                {
                    b = br.ReadBytes((int)resource.Length);
                }

                return b;
            }
        }



    }

    public static class Constants
    {
        public static string CertResourceName = ConfigurationManager.AppSettings["CertResourceName"];

        public static string CertPassword = ConfigurationManager.AppSettings["CertPassword"];

        public static string AzureSubscriptionId = ConfigurationManager.AppSettings["AzureSubscriptionId"];

        public static string HubNamespace = ConfigurationManager.AppSettings["NamespaceName"];

        public static string HubName = ConfigurationManager.AppSettings["HubName"];

        public static string HubConnectionString = ConfigurationManager.AppSettings["HubConnectionString"];



    }
}
