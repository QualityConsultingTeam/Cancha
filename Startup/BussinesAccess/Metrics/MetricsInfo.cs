using BussinesAccess.Extensions;
using BussinesAccess.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesAccess.Metrics
{
    public class MetricInfo
    {
        #region Private Constants
        //***************************
        // Formats
        //***************************
        private const string ExceptionFormat = "Exception: {0}";
        private const string InnerExceptionFormat = "InnerException: {0}";
        private const string RetrievingMetricsFormat = "Retrieving metrics for the [{0}] entity...";
        private const string NoMetricRetrievedFormat = "No metric was retrieved for the [{0}] entity. Check the subscriptionId and certificateThumbprint settings in the configuration file.";
        private const string MetricSuccessfullyRetrievedFormat = "[{0}] metrics successfully retrieved for the [{1}] entity.";
        private const string NoSubscriptionIdOrCertificateThumbprint = "Warning: the subscriptionId or certificateThumbprint settings are not defined in the configuration file.\n\rSpecify your Azure subscription ID and the thumbprint of an Azure management certificate in the local machine or current user store, if you want to use entity metrics.";

        //***************************
        // Entities
        //***************************
        private const string QueueEntity = "Queue";
        private const string TopicEntity = "Topic";
        private const string SubscriptionEntity = "Subscription";
        private const string EventHubEntity = "Event Hub";
        private const string ConsumerGroupEntity = "Consumer Group";
        private const string NotificationHubEntity = "Notification Hub";
        private const string RelayEntity = "Relay";

        //***************************
        // Constants
        //***************************
        private const string QueueEntities = "queues";
        private const string TopicEntities = "topics";
        private const string EventHubEntities = "eventhubs";
        private const string NotificationHubEntities = "notificationhubs";
        private const string RelayEntities = "relays";
        #endregion

        #region Public Instance Properties
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string PrimaryAggregation { get; set; }
        public List<Rollup> Rollups { get; set; }
        #endregion

        #region Public Static Constructor
        static MetricInfo()
        {
            EntityMetricDictionary = new Dictionary<string, BindingList<MetricInfo>>();
        }
        #endregion

        #region Private Static Fields

        private static readonly Dictionary<string, string> entityToUrlSegmentMapDictionary = new Dictionary<string, string>
        {
            {QueueEntity, QueueEntities},
            {TopicEntity, TopicEntities},
            {SubscriptionEntity, TopicEntities},
            {EventHubEntity, EventHubEntities},
            {ConsumerGroupEntity, EventHubEntities},
            {NotificationHubEntity, NotificationHubEntities},
            {RelayEntity, RelayEntities}
        };

        private static bool warningShown;
        private static bool retrieveMetrics = true;
        #endregion

        #region Public Static Properties
        public static Dictionary<string, BindingList<MetricInfo>> EntityMetricDictionary { get; private set; }
        #endregion

        #region Public Static Properties

        public async static Task GetMetricInfoListAsync(string ns, string entityPath)
        {
            try
            {
                if (!retrieveMetrics)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(ns)) return;
                //string.IsNullOrEmpty(entityType) ||
                //string.IsNullOrWhiteSpace(entityPath) ||
                //!entityToUrlSegmentMapDictionary.ContainsKey(entityType) ||
                //EntityMetricDictionary.ContainsKey(entityType))
                //{
                //    return;
                //}
                if (string.IsNullOrWhiteSpace(Constants.AzureSubscriptionId))
                {
                    if (!warningShown)
                    {
                        Trace.WriteLine(NoSubscriptionIdOrCertificateThumbprint);
                        warningShown = true;
                    }
                    return;
                }
                var uri = MetricHelper.BuildUriForDataPointDiscoveryQuery(Constants.AzureSubscriptionId,
                                                                          ns,
                                                                          NotificationHubEntities,//entityToUrlSegmentMapDictionary[entityType],
                                                                          entityPath);
                var enumerable = await MetricHelper.GetSupportedMetricsAsync(uri);
                Trace.WriteLine(string.Format(RetrievingMetricsFormat, NotificationHubEntities));
                if (enumerable == null)
                {
                    retrieveMetrics = false;
                    Trace.WriteLine(string.Format(NoMetricRetrievedFormat, NotificationHubEntities));
                    return;
                }
                var metricInfoList = enumerable.ToList();
                var count = metricInfoList.Count;
                metricInfoList.Insert(0, new MetricInfo { DisplayName = "All", Name = "all", Unit = "Requests", PrimaryAggregation = "Total" });
                foreach (var item in metricInfoList)
                {
                    item.DisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.DisplayName.Replace('.', ' '));
                }
                if (!metricInfoList.Any())
                {
                    return;
                }
                EntityMetricDictionary[NotificationHubEntity] = new BindingList<MetricInfo>(metricInfoList.ToList())
                {
                    AllowEdit = true,
                    AllowNew = true,
                    AllowRemove = true
                };
                Trace.WriteLine(string.Format(MetricSuccessfullyRetrievedFormat, count, NotificationHubEntity));
            }
            catch (ArgumentException ex)
            {
                if (string.Compare(ex.ParamName, "certificateThumbprint", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    HandleException(ex);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion

        #region Private Static Methods
        private static void HandleException(Exception ex)
        {
            if (ex == null || string.IsNullOrWhiteSpace(ex.Message))
            {
                return;
            }
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, ExceptionFormat, ex.Message));
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, InnerExceptionFormat, ex.InnerException.Message));
            }
        }
        #endregion
    }

    public class Rollup
    {
        public TimeSpan TimeGrain { get; set; }
        public TimeSpan Retention { get; set; }
        public ICollection<MetricValue> Values { get; set; }
    }
}
