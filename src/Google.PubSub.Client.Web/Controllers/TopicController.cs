using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accountbalanceproto;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Google.PubSub.Client.Web.Configs;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Google.PubSub.Client.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly ILogger<TopicController> _logger;
        private readonly PubSubConfig _pubSubConfig;

        public TopicController(
            ILogger<TopicController> logger,
            PubSubConfig pubSubConfig)
        {
            _logger = logger;
            _pubSubConfig = pubSubConfig;
        }

        [HttpGet("project/topics")]
        public async Task<IActionResult> List(string projectId, CancellationToken cancellationToken)
        {
            var client = await new PublisherServiceApiClientBuilder
            {
                Endpoint = _pubSubConfig.EmulatorHost,
                ChannelCredentials = ChannelCredentials.Insecure
            }.BuildAsync(cancellationToken);

            var projectName = ProjectName.FromProject(projectId);

            var topics = client.ListTopics(projectName).ToList();

            var model = new TopicListModel {ProjectId = projectId, Topics = topics};

            return View(model);
        }

        [HttpGet("project/topic/create")]
        public ActionResult Create(string projectId)
        {
            var model = new TopicCreateModel {ProjectId = projectId};

            return View(model);
        }

        [HttpGet("project/topic/detail")]
        public ActionResult Detail(string projectId, string topicName)
        {
            var model = new TopicDetailModel
            {
                ProjectId = projectId,
                TopicName = topicName
            };

            return View(model);
        }

        [HttpGet("project/topic/publish")]
        public ActionResult PublishMessage(string projectId, string topicName)
        {
            var model = new TopicDetailModel
            {
                ProjectId = projectId,
                TopicName = topicName
            };

            return View(model);
        }

        [HttpPost("project/topic/publish")]
        public async Task<ActionResult> PublishMessage(
            string projectId, 
            string topicName, 
            string message,
            bool useOrderKey,
            string orderKey,
            EncodingEnum encoding,
            ProtoModelEnum protoModel,
            CancellationToken cancellationToken)
        {
            // return RedirectToAction("Detail", new {projectId, topicName});
            var client = await new PublisherServiceApiClientBuilder
            {
                Endpoint = _pubSubConfig.EmulatorHost,
                ChannelCredentials = ChannelCredentials.Insecure
            }.BuildAsync(cancellationToken);

            ByteString data = null;
            if (encoding == EncodingEnum.Json)
            {
                data = ByteString.CopyFromUtf8(message);
            }
            else
            {
                switch (protoModel)
                {
                    case ProtoModelEnum.Balances:
                        var balances = Balances.Parser.ParseJson(message);
                        data = balances.ToByteString();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            var pubSubMessage = new PubsubMessage
            {
                MessageId = Guid.NewGuid().ToString("D"),
                Data = data,
                OrderingKey = useOrderKey ? orderKey ?? string.Empty : string.Empty
            };
            
            await client.PublishAsync(topicName, new[] {pubSubMessage}, cancellationToken);
            
            return RedirectToAction("Detail", new {projectId, topicName});
        }

        [HttpPost("project/topic/create")]
        public async Task<ActionResult> Create(string projectId, string topicName, CancellationToken cancellationToken)
        {
            var client = await new PublisherServiceApiClientBuilder
            {
                Endpoint = _pubSubConfig.EmulatorHost,
                ChannelCredentials = ChannelCredentials.Insecure
            }.BuildAsync(cancellationToken);

            var topic = new TopicName(projectId, topicName);
            await client.CreateTopicAsync(topic, cancellationToken);

            return RedirectToAction("List", new {projectId});
        }

        [HttpPost("project/topic/delete")]
        public async Task<ActionResult> Delete(string projectId, string topicName, CancellationToken cancellationToken)
        {
            var client = await new PublisherServiceApiClientBuilder
            {
                Endpoint = _pubSubConfig.EmulatorHost,
                ChannelCredentials = ChannelCredentials.Insecure
            }.BuildAsync(cancellationToken);

            await client.DeleteTopicAsync(topicName, cancellationToken);

            return RedirectToAction("List", new {projectId});
        }
    }

    public class TopicListModel
    {
        public string ProjectId { get; set; }

        public IReadOnlyCollection<Topic> Topics { get; set; }
    }

    public class TopicCreateModel
    {
        public string ProjectId { get; set; }
    }

    public class TopicDetailModel
    {
        public string ProjectId { get; set; }

        public string TopicName { get; set; }
    }

    public enum EncodingEnum
    {
        Json,
        Proto,
    }

    public enum ProtoModelEnum
    {
        None,
        Balances,
    }
}