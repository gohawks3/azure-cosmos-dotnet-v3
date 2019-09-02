//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------
namespace Microsoft.Azure.Cosmos
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    internal class ReadFeedResponse<T> : FeedResponse<T>
    {
        protected ReadFeedResponse(
            HttpStatusCode httpStatusCode,
            ICollection<T> resource,
            CosmosHeaders responseMessageHeaders)
        {
            this.Count = resource.Count;
            this.CosmosHeaders = responseMessageHeaders;
            this.Resource = resource;
            this.StatusCode = httpStatusCode;
        }

        public override int Count { get; }

        public override string ContinuationToken => this.CosmosHeaders?.ContinuationToken;

        internal override CosmosHeaders CosmosHeaders { get; }

        public override IEnumerable<T> Resource { get; }

        public override HttpStatusCode StatusCode { get; }

        public override IEnumerator<T> GetEnumerator()
        {
            return this.Resource.GetEnumerator();
        }

        internal static ReadFeedResponse<TInput> CreateResponse<TInput>(
            ResponseMessage responseMessage,
            CosmosSerializer jsonSerializer)
        {
            using (responseMessage)
            {
                ICollection<TInput> resources = default(ICollection<TInput>);
                if (responseMessage.Content != null)
                {
                    CosmosFeedResponseUtil<TInput> response = jsonSerializer.FromStream<CosmosFeedResponseUtil<TInput>>(responseMessage.Content);
                    resources = response.Data;
                }

                ReadFeedResponse<TInput> readFeedResponse = new ReadFeedResponse<TInput>(
                    httpStatusCode: responseMessage.StatusCode,
                    resource: resources,
                    responseMessageHeaders: responseMessage.CosmosHeaders);

                return readFeedResponse;
            }
        }
    }
}