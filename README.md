# DoLess.Rest

**DoLess.Rest** is another type-safe REST API client library.
It is heavily inspired by [Refit](https://github.com/paulcbetts/refit), which is inspired by [Retrofit](http://square.github.io/retrofit), but does not works exactly the same way.

In fact **DoLess.Rest** is built by keeping in mind that reflection is slow. When we build beautiful apps we don't want them to be slow down because of an external library, that's why all the **DoLess.Rest** REST clients are fully generated during the compilation.

This library fully supports the uri template defined by the [RFC6570](http://tools.ietf.org/html/rfc6570) thanks to [DoLess.UriTemplates](https://github.com/letsar/DoLess.UriTemplates).

## How it works?

Install the NuGet package called DoLess.Rest and one of the extra package (like DoLess.Rest.Newtonsoft.Json) into your project.
A DoLess.Rest folder with a file named RestClient.g.dl.rest.cs will be inserted in your project.
This file will allow you to create the Rest client from your interface without relection.

As in Refit, you have to create an interface representing your REST API and use attributes to indicate what to do.
During the project compilation, all REST clients will be generated.

## Install

Available on NuGet.

Install DoLess.Rest

Install DoLess.Rest.Newtonsoft.Json (if you want to serialize/deserialize using Json.Net)

## Quick start

### 1°) Create your REST API interface

```csharp
[Header("User-Agent", "DoLess.Rest")]
public interface IGitHubApi
{
    [Get("users{/userId}")]
    Task<User> GetUserAsync(string userId);
}
```

### 2°) Get the actual REST client

```csharp
IGitHubApi gitHubApi = RestClient.For<IGitHubApi>("https://api.github.com");
```

### 3°) Make the call

```csharp
User user = await gitHubApi.GetUserAsync("lestar");
```

## Attributes

### HTTP Method attributes

In order to be identified as a REST interface, all methods of the interface must have a HTTP Method attribute that provides the request method and relative URL.

There are 8 built-in attributes: **Delete**, **Get**, **Head**, **Options**, **Patch**, **Post**, **Put** and **Trace**.
The relative URL of the resource must be specified as the argument of this attribute and it have to respect the [RFC6570 Uri template specification](http://tools.ietf.org/html/rfc6570).

*Note: You can use a literal string, or any expression (like a constant) as the argument.*

```csharp
[Get("/users{/userId}")]
[Get("/users{?since}")]
```

