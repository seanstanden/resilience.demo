using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace resilience.demo
{
    public static class RetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry: LogRetry);
        }

        private static void LogRetry(DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<IAsyncPolicy<HttpResponseMessage>>();
            logger.LogWarning(result.Exception,
                @$"Request failed with: {result.Result.ReasonPhrase}. Retrying request after {timeSpan.TotalSeconds}.");
        }
    }
}