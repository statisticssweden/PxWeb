namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// Interface for table response serializers
    /// </summary>
    interface IWebSerializer
    {
        void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse);
    }
}
