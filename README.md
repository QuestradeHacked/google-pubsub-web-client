# Google PubSub Web Client

## Getting Started
This client is intended to help developers and QAs to perform local testing using Google PubSub emulator and a Web Client interface by providing an easy way to publish and subscribe to messages, and in that way, facilitating such local tests with pubsub events. 

Check [CHANGELOG](CHANGELOG.md) file to check latest changes.

## Running the PubSub Client
If you need to run the emulator just to use to publish/subscribe to message, you first need `docker` installed. And then perform the following:

1. Building and running the emulator and PubSub web client
   ```
   docker compose build
   docker compose up
   ```
2. Access http://localhost:63420

After those steps you should be able to navigate to the home page of Pub/Sub client and start creating the projects, topics, subscriptions and publish the messages.

## Set up your application to point to emulator
The emulator container will spin up on the following endpoint `localhost:8681` and GCP project id defaulting to `qt-msa-local`. For instructions on setting up PUBSUB library to run with emulator. Questrade PubSub wrapper library already offer this option to use emulator, check its docs for details. Otherwise check google documentation for connecting to an emulator directly like [Google: Use emulator](https://cloud.google.com/pubsub/docs/samples/pubsub-use-emulator) .

---
## Running the PubSub Client on Debug mode for contributions
If you want to run the PubSub Client on debug mode to contribute to the code base or to test something new, follow the following steps.
### Prerequisites

What things you need to install to be able to run the PubSub Web client on debug mode

* Download all the following [.NET Core SDK](https://dotnet.microsoft.com/download):
    * .NET Core SDK 3.1
      When downloading those, please refer to latest LTS versions for each.
* Download Docker
  Make sure your computer has docker installed. Verify if you have it with the following command:
    ```
    $ docker --version
    Docker version 19.03.13, build 4484c46d9d
    ```
   - [How to install docker on Mac machines](https://docs.docker.com/docker-for-mac/install/);
   - [How to install docker on Windows machines](https://docs.docker.com/docker-for-windows/install/);
   
### Local Setup
To be able to test your code locally you'll need to have the following setup in your machine:

1. **Docker:** 
   
   To run just the pubsub emulator execute the following command from the root of client repository:
   ```
   docker compose up emulator
   ```

2. **Google.PubSub.Client:**
   
   To run Google.PubSub.Client execute the following command from the root of client repository:
   ```
   dotnet run --project ./src/Google.PubSub.Client.Web/Google.PubSub.Client.Web.csproj
   ```
   
Done, the debug setup is done and you are able to run the application on debug mode accessing the exposed localhost port.

## Contributors :tada:
- [Volodymyr Teodorovych](https://git.questrade.com/vteodorovych)
- [Kelvin Ferreira](https://git.questrade.com/kferreira)
- [Codebusters Team](https://git.questrade.com/codebusters)
