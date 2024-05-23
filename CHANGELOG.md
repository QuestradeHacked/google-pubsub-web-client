# Changelog

Here you'll see all release notes for this Google PubSub Web client.

> NOTE: The topmost should be the latest version published.

## 1.0.0

* Functionalities
  * Spin up Emulator and WEB app with a single docker compose file
  * Create a project, pubsub topic, or pubsub subscription
  * Control contifuration for retries
  * Control message order attribute
  * Fetch messages from a topic using a subscription
  * Purge messages from a subscription
* Known Issues
  * Pulling messages from subscriptions may not return messages sometimes - This is caused by the synchounous way that was used to fetch messages. On the future we might change that to be on a streaming base, which will help on mitigating this issue.
* Frontend
  * MVC with Razor pages views and controllers
  * Design built using `Bootstrap v4.3.1`
* Internals
  * dotnet core `3.1`
  * pubsub library `Google.Cloud.PubSub.V1` version `3.5.0`
  * gRPC library `Grpc.Tools` version `2.47.0`
