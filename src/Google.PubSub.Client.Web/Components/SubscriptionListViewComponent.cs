using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Google.PubSub.Client.Web.Configs;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Google.PubSub.Client.Web.Components
{
    public class SubscriptionListViewComponent : ViewComponent
    {
        private readonly PubSubConfig _pubSubConfig;

        public SubscriptionListViewComponent(
            PubSubConfig pubSubConfig)
        {
            _pubSubConfig = pubSubConfig;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(
            string projectId, string topicName)
        {
            var client = await new PublisherServiceApiClientBuilder
            {
                Endpoint = _pubSubConfig.EmulatorHost,
                ChannelCredentials = ChannelCredentials.Insecure
            }.BuildAsync();

            var subscriptions = client.ListTopicSubscriptions(topicName).AsRawResponses().ToList();

            var model = new SubscriptionListModel
            {
                ProjectId = projectId,
                TopicName = topicName,
                SubscriptionResponses = subscriptions
            };

            return View(model);
        }
    }
    
    public class SubscriptionListModel
    {
        public string ProjectId { get; set; }
        
        public string TopicName { get; set; }
        
        public IReadOnlyCollection<ListTopicSubscriptionsResponse> SubscriptionResponses { get; set; }
    }
}