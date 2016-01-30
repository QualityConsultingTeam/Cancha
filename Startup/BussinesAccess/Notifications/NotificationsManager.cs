using BussinesAccess.Extensions;
using BussinesAccess.Metrics;
using Microsoft.Azure.NotificationHubs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BussinesAccess.Notifications
{
    public static class NotificationsManager
    {
        public static async Task SendTestNotification()
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(Constants.HubConnectionString, Constants.HubName);

            var notification = new GcmNotification("{ \"data\" : {\"message\":\"Test Notification Yakiris Makiris!\"}}");

            var r = await hub.SendNotificationAsync(notification);


        }

        public static async Task SendNotificationAsync(string message)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(Constants.HubConnectionString, Constants.HubName);

            var notification = new GcmNotification("{ \"data\" : {\"message\":\"" + message + "\"}}");

            var r = await hub.SendNotificationAsync(notification);


        }

        public static async Task SendNotificationAsync(string title, string author, int id)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(Constants.HubConnectionString, Constants.HubName);

            var content = "{ \"data\" : {\"title\":\"" + title + "\", \"author\":\"" + author + "\", \"id\":\"" + id + "\" }}";

            var notification = new GcmNotification(content);

            var r = await hub.SendNotificationAsync(notification);


        }
        public static async Task GetAllRegistrations()
        {
            var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.HubConnectionString, Constants.HubName);

            var registrations = await hub.GetAllRegistrationsAsync(0);

            var notificationsJobs = await hub.GetNotificationHubJobsAsync();


        }


        #region Metrics


        public static void GetMetrics()
        {
            // string uri = @"https://management.core.windows.net/{subscriptionId}/services/ServiceBus/namespaces/{namespaceName}/NotificationHubs/{hubName}/metrics/outgoing.allpns.success/rollups/PT5M/Values?$filter={}'";

            string strFromDate = String.Format("{0:s}", DateTime.Now.Date);
            string strToDate = String.Format("{0:s}", DateTime.Now);

            // See - http://msdn.microsoft.com/library/azure/dn163590.aspx for details
            //string filterExpression =String.Format("Timestamp%20gt%20datetime'{0}Z'%20and%20Timestamp%20lt%20datetime'{1}Z'",strFromDate, strToDate);
            string filterExpression = String.Format("Timestamp%20gt%20datetime'{0}Z'", strFromDate);


            var uri = GetUri();
            uri = uri.Replace("thefilter", filterExpression);
            HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(uri);
            sendNotificationRequest.Method = "GET";
            sendNotificationRequest.ContentType = "application/xml";
            sendNotificationRequest.Headers.Add("x-ms-version", "2015-01");//"2011-02-25"); 

            X509Certificate2 certificate = CertLoader.Load(); //new X509Certificate2(@"{pathToPfxCert}", "{certPassword}");
            sendNotificationRequest.ClientCertificates.Add(certificate);




            List<MetricValue> data = new List<MetricValue>();
            try
            {
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();

                using (XmlReader reader = XmlReader.Create(response.GetResponseStream(),
                    new XmlReaderSettings { CloseInput = true }))
                {
                    SyndicationFeed feed = SyndicationFeed.Load<SyndicationFeed>(reader);

                    foreach (SyndicationItem item in feed.Items)
                    {
                        XmlSyndicationContent syndicationContent = item.Content as XmlSyndicationContent;
                        MetricValue value = syndicationContent.ReadContent<MetricValue>();

                        data.Add(value);

                        Console.WriteLine("Timestamp: {0} -> Total: {1}", value.Timestamp, value.Total);
                    }
                }
            }
            catch (WebException exception)
            {
                string error = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(error);
            }
        }

        private static string GetUri()
        {

            string uri = @"https://management.core.windows.net/{subscriptionId}/services/ServiceBus/namespaces/{namespaceName}/NotificationHubs/{hubName}/metrics/{metricName}/rollups/P1D/Values?$filter={filterExpression}";

            uri = uri.Replace("{subscriptionId}", Constants.AzureSubscriptionId);
            uri = uri.Replace("{namespaceName}", Constants.HubNamespace);
            uri = uri.Replace("{hubName}", Constants.HubName);
            uri = uri.Replace("{metricName}", "outgoing.allpns.sucess");

            string strFromDate = String.Format("{0:s}", DateTime.Now.Date);
            string strToDate = String.Format("{0:s}", DateTime.Now);

            // See - http://msdn.microsoft.com/library/azure/dn163590.aspx for details
            string filterExpression = String.Format
                ("Timestamp%20gt%20datetime'{0}Z'%20and%20Timestamp%20lt%20datetime'{1}Z'",
                    strFromDate, strToDate);
            uri = uri.Replace("{filterExpression}", filterExpression);

            return uri;

            return string.Format(@"https://management.core.windows.net/{0}/services/ServiceBus/namespaces/{1}/NotificationHubs/{2}/metrics/outgoing.allpns.success/rollups/P1D/Values?$filter=thefilter"
                //return string.Format(@"https://management.core.windows.net/{0}/services/ServiceBus/namespaces/{1}/NotificationHubs/{2}/metrics/Size/rollups/P1D/Values"

                , Constants.AzureSubscriptionId
                , Constants.HubNamespace
                , Constants.HubName);


        }

        public static async Task GetAllMetrics()
        {

            await MetricInfo.GetMetricInfoListAsync(Constants.HubNamespace, "EnLacanchaHub");


            var data = MetricInfo.EntityMetricDictionary.FirstOrDefault();


        }

        #endregion
    }
}
