[![](https://img.shields.io/nuget/v/soenneker.extensions.httprequests.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequests/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequests/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequests/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.httprequests.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequests/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequests/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequests/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpRequests
A collection of helpful HttpRequest (from HttpContext) extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpRequests
```

## Quick start

```csharp
using Soenneker.Extensions.HttpRequests;

// Given an existing HttpRequest named request:
var result = request.ReadBody();
```

## Common operations

- `ReadBody()` - Reads the request body as a UTF-8 string with optional truncation. Returns a `string` containing the decoded request body if successfully read. Returns `string.Empty` when the body is empty. Returns `null` if the body is non-seekable or too large to buffer safely.
