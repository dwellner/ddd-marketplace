using System;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace MarketPlace.Service
{
    public class RetryableException : Exception
    {
        public RetryableException(Exception innerException) : base($"Retryable failure: {innerException.Message}", innerException) { }
    }

    public class PollyFailoverPolicy : IFailoverPolicy
    {
        private readonly AsyncRetryPolicy policy;

        public PollyFailoverPolicy(AsyncRetryPolicy policy) =>
            this.policy = policy;

        public async Task ExecuteAsync(Func<Task> action) =>
            await policy.ExecuteAsync(async () => await action());
    }

    public class FailoverPolicyFactory : IFailoverPolicyProvider
    {
        static readonly IFailoverPolicy commandRetryPolicy = new PollyFailoverPolicy(Policy
            .Handle<RetryableException>()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        public IFailoverPolicy CommandRetryPolicy => commandRetryPolicy;
    }
}
