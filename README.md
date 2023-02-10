# ASP.NET Core/C#: Starter Web App Code Sample

This C# code sample demonstrates how to build web applications using ASP.NET Core.

Visit the ["ASP.NET Core/C# Code Samples: App Security in Action"](https://developer.auth0.com/resources/code-samples/web-app/aspnet-core) section of the ["Auth0 Developer Resources"](https://developer.auth0.com/resources) to explore how you can secure ASP.NET Core applications written in C# by implementing user authentication with Auth0.

## Why Use Auth0?

Auth0 is a flexible drop-in solution to add authentication and authorization services to your applications. Your team and organization can avoid the cost, time, and risk that come with building your own solution to authenticate and authorize users. We offer tons of guidance and SDKs for you to get started and [integrate Auth0 into your stack easily](https://developer.auth0.com/resources/code-samples/full-stack).

## Set Up and Run the ASP.NET Core Project

Create a `.env` file under the root project directory:

```bash
touch .env
```

Populate it with the following environment variables:

```bash
PORT=4040
```

Make sure you have the [dotnet cli tool](https://docs.microsoft.com/en-us/dotnet/core/tools/) installed and configured on your system and execute the following command from the root directory:

```bash
dotnet run
```
