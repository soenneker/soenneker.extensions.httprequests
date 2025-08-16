using Microsoft.AspNetCore.Http;
using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;

namespace Soenneker.Extensions.HttpRequests;

/// <summary>
/// A collection of helpful HttpRequest (from HttpContext) extension methods
/// </summary>
public static class HttpRequestsExtension
{
    /// <summary>
    /// Reads the request body as a UTF-8 string with optional truncation.
    /// </summary>
    /// <param name="request">
    /// The <see cref="HttpRequest"/> whose body should be read.
    /// </param>
    /// <param name="maxBytes">
    /// An optional maximum number of bytes to read from the request body.  
    /// If specified, only up to this many bytes are buffered and returned, and the
    /// string will be annotated with a truncation notice if the actual body length
    /// exceeds the limit.  
    /// If <c>null</c>, the entire body is read (subject to <see cref="int.MaxValue"/>).
    /// </param>
    /// <returns>
    /// A <see cref="string"/> containing the decoded request body if successfully read.  
    /// Returns <c>string.Empty</c> when the body is empty.  
    /// Returns <c>null</c> if the body is non-seekable or too large to buffer safely.
    /// </returns>
    /// <remarks>
    /// The request stream position is reset to its original value after reading.
    /// Callers should ensure <see cref="HttpRequest.EnableBuffering"/> has been invoked
    /// earlier in the pipeline so that the request body stream is seekable.
    /// </remarks>
    public static async ValueTask<string?> ReadBody(this HttpRequest request, int? maxBytes = null)
    {
        long? cl = request.ContentLength;

        if (cl is null or 0)
            return "";

        if (!request.Body.CanSeek)
            return null;

        long originalPos = request.Body.Position;
        request.Body.Position = 0;

        try
        {
            long toReadLong = maxBytes.HasValue ? Math.Min(cl.Value, maxBytes.Value) : cl.Value;

            if (toReadLong > int.MaxValue)
                return null; // too large to buffer

            var toRead = (int) toReadLong;

            byte[] rented = ArrayPool<byte>.Shared.Rent(toRead);

            try
            {
                var readTotal = 0;
                while (readTotal < toRead)
                {
                    int read = await request.Body.ReadAsync(rented, readTotal, toRead - readTotal).NoSync();

                    if (read == 0) 
                        break;
                    readTotal += read;
                }

                if (readTotal == 0)
                    return "";

                string text = Encoding.UTF8.GetString(rented, 0, readTotal);

                if (maxBytes.HasValue && cl.Value > readTotal)
                    text += $" [truncated {cl.Value - readTotal} bytes]";

                return text;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
        finally
        {
            request.Body.Position = originalPos;
        }
    }
}