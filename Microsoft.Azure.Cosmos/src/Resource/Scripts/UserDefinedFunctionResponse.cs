//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Scripts
{
    using System.Net;
    using Microsoft.Azure.Documents;

    /// <summary>
    /// The cosmos user defined function response
    /// </summary>
    public class UserDefinedFunctionResponse : Response<UserDefinedFunctionProperties>
    {
        /// <summary>
        /// Create a <see cref="UserDefinedFunctionResponse"/> as a no-op for mock testing
        /// </summary>
        protected UserDefinedFunctionResponse()
            : base()
        {
        }

        /// <summary>
        /// A private constructor to ensure the factory is used to create the object.
        /// This will prevent memory leaks when handling the HttpResponseMessage
        /// </summary>
        internal UserDefinedFunctionResponse(
          HttpStatusCode httpStatusCode,
          CosmosHeaders headers,
          UserDefinedFunctionProperties userDefinedFunctionProperties)
        {
            this.StatusCode = httpStatusCode;
            this.CosmosHeaders = headers;
            this.Resource = userDefinedFunctionProperties;
        }

        /// <inheritdoc/>
        internal override CosmosHeaders CosmosHeaders { get; }

        /// <inheritdoc/>
        public override UserDefinedFunctionProperties Resource { get; }

        /// <inheritdoc/>
        public override HttpStatusCode StatusCode { get; }

        /// <inheritdoc/>
        public override double RequestCharge => this.CosmosHeaders?.RequestCharge ?? 0;

        /// <inheritdoc/>
        public override string ActivityId => this.CosmosHeaders?.ActivityId;

        /// <inheritdoc/>
        public override string ETag => this.CosmosHeaders?.ETag;

        /// <inheritdoc/>
        internal override string MaxResourceQuota => this.CosmosHeaders?.GetHeaderValue<string>(HttpConstants.HttpHeaders.MaxResourceQuota);

        /// <inheritdoc/>
        internal override string CurrentResourceQuotaUsage => this.CosmosHeaders?.GetHeaderValue<string>(HttpConstants.HttpHeaders.CurrentResourceQuotaUsage);

        /// <summary>
        /// Get <see cref="UserDefinedFunctionProperties"/> implicitly from <see cref="UserDefinedFunctionResponse"/>
        /// </summary>
        /// <param name="response">UserDefinedFunctionResponse</param>
        public static implicit operator UserDefinedFunctionProperties(UserDefinedFunctionResponse response)
        {
            return response.Resource;
        }
    }
}