namespace MarketPlace.Service
{
    public interface IFailoverPolicyProvider
    {
        IFailoverPolicy CommandRetryPolicy { get; }
    }
}
