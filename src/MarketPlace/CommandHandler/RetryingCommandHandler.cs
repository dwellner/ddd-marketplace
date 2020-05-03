using System;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace MarketPlace.CommandHandler
{
    public class RetryableException : Exception
    {
        public RetryableException(Exception innerException) : base($"Retryable failure: {innerException.Message}", innerException) { }
    }

    public class RetryingCommandHandler : ICommandHandler
    {
        static AsyncRetryPolicy policy = Policy
            .Handle<RetryableException>()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        private readonly ICommandHandler innerHandler;

        public RetryingCommandHandler(ICommandHandler innerHandler) {
            this.innerHandler = innerHandler;
        }


        public async Task Handle(object command)
        {
            await policy.ExecuteAsync(async () => {

                await innerHandler.Handle(command);
            });
        }
    }
}
