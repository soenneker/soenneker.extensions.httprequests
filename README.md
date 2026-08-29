[![](https://img.shields.io/nuget/v/soenneker.extensions.httprequests.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequests/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequests/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequests/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.httprequests.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequests/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequests/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequests/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpRequests
Extension methods for inspecting and processing ASP.NET Core `HttpRequest` data while respecting request-stream behavior.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpRequests
```

## Usage

```csharp
using Soenneker.Extensions.HttpRequests;

request.EnableBuffering();

string? body = await request.ReadBody(maxBytes: 16_384, cancellationToken);
```

`ReadBody()` reads from position zero as UTF-8, then restores the stream's original position—even when reading fails. Call `EnableBuffering()` earlier in the pipeline; a non-seekable body returns `null`.

When `maxBytes` is set and the declared `Content-Length` is larger, the result ends with a notice such as ` [truncated 240 bytes]`. An absent or zero `Content-Length` returns `""` without reading. A body larger than `int.MaxValue` also returns `null` unless `maxBytes` brings the buffered size below that limit.

This method relies on the declared `Content-Length`; it is not intended for unknown-length/chunked bodies.
