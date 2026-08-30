using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Soenneker.Tests.Unit;

namespace Soenneker.Extensions.HttpRequests.Tests;

public sealed class HttpRequestsExtensionTests : UnitTest
{
    [Test]
    public async System.Threading.Tasks.Task Non_positive_limit_returns_a_zero_byte_preview()
    {
        byte[] body = Encoding.UTF8.GetBytes("hello");
        var context = new DefaultHttpContext();
        context.Request.ContentLength = body.Length;
        context.Request.Body = new MemoryStream(body);

        await Assert.That(await context.Request.ReadBody(-1)).IsEqualTo(" [truncated 5 bytes]");
    }
}
