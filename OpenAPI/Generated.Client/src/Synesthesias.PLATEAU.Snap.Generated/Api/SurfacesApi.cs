/*
 * PLATEAU.Snap.Server
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.PLATEAU.Snap.Generated.Model;

namespace Synesthesias.PLATEAU.Snap.Generated.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISurfacesApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。
        /// </summary>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <returns>VisibleSurfacesResponse</returns>
        VisibleSurfacesResponse GetVisibleSurfacesAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest));

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <returns>ApiResponse of VisibleSurfacesResponse</returns>
        ApiResponse<VisibleSurfacesResponse> GetVisibleSurfacesAsyncWithHttpInfo(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest));
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISurfacesApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of VisibleSurfacesResponse</returns>
        System.Threading.Tasks.Task<VisibleSurfacesResponse> GetVisibleSurfacesAsyncAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest), System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken));

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (VisibleSurfacesResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<VisibleSurfacesResponse>> GetVisibleSurfacesAsyncWithHttpInfoAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest), System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISurfacesApi : ISurfacesApiSync, ISurfacesApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class SurfacesApi : IDisposable, ISurfacesApi
    {
        private Synesthesias.PLATEAU.Snap.Generated.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <returns></returns>
        public SurfacesApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public SurfacesApi(string basePath)
        {
            this.Configuration = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.MergeConfigurations(
                Synesthesias.PLATEAU.Snap.Generated.Client.GlobalConfiguration.Instance,
                new Synesthesias.PLATEAU.Snap.Generated.Client.Configuration { BasePath = basePath }
            );
            this.ApiClient = new Synesthesias.PLATEAU.Snap.Generated.Client.ApiClient(this.Configuration.BasePath);
            this.Client =  this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            this.ExceptionFactory = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class using Configuration object.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="configuration">An instance of Configuration.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public SurfacesApi(Synesthesias.PLATEAU.Snap.Generated.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.MergeConfigurations(
                Synesthesias.PLATEAU.Snap.Generated.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.ApiClient = new Synesthesias.PLATEAU.Snap.Generated.Client.ApiClient(this.Configuration.BasePath);
            this.Client = this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            ExceptionFactory = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class.
        /// </summary>
        /// <param name="client">An instance of HttpClient.</param>
        /// <param name="handler">An optional instance of HttpClientHandler that is used by HttpClient.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        /// <remarks>
        /// Some configuration settings will not be applied without passing an HttpClientHandler.
        /// The features affected are: Setting and Retrieving Cookies, Client Certificates, Proxy settings.
        /// </remarks>
        public SurfacesApi(HttpClient client, HttpClientHandler handler = null) : this(client, (string)null, handler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class.
        /// </summary>
        /// <param name="client">An instance of HttpClient.</param>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <param name="handler">An optional instance of HttpClientHandler that is used by HttpClient.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        /// <remarks>
        /// Some configuration settings will not be applied without passing an HttpClientHandler.
        /// The features affected are: Setting and Retrieving Cookies, Client Certificates, Proxy settings.
        /// </remarks>
        public SurfacesApi(HttpClient client, string basePath, HttpClientHandler handler = null)
        {
            if (client == null) throw new ArgumentNullException("client");

            this.Configuration = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.MergeConfigurations(
                Synesthesias.PLATEAU.Snap.Generated.Client.GlobalConfiguration.Instance,
                new Synesthesias.PLATEAU.Snap.Generated.Client.Configuration { BasePath = basePath }
            );
            this.ApiClient = new Synesthesias.PLATEAU.Snap.Generated.Client.ApiClient(client, this.Configuration.BasePath, handler);
            this.Client =  this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            this.ExceptionFactory = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class using Configuration object.
        /// </summary>
        /// <param name="client">An instance of HttpClient.</param>
        /// <param name="configuration">An instance of Configuration.</param>
        /// <param name="handler">An optional instance of HttpClientHandler that is used by HttpClient.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        /// <remarks>
        /// Some configuration settings will not be applied without passing an HttpClientHandler.
        /// The features affected are: Setting and Retrieving Cookies, Client Certificates, Proxy settings.
        /// </remarks>
        public SurfacesApi(HttpClient client, Synesthesias.PLATEAU.Snap.Generated.Client.Configuration configuration, HttpClientHandler handler = null)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (client == null) throw new ArgumentNullException("client");

            this.Configuration = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.MergeConfigurations(
                Synesthesias.PLATEAU.Snap.Generated.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.ApiClient = new Synesthesias.PLATEAU.Snap.Generated.Client.ApiClient(client, this.Configuration.BasePath, handler);
            this.Client = this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            ExceptionFactory = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacesApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SurfacesApi(Synesthesias.PLATEAU.Snap.Generated.Client.ISynchronousClient client, Synesthesias.PLATEAU.Snap.Generated.Client.IAsynchronousClient asyncClient, Synesthesias.PLATEAU.Snap.Generated.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = Synesthesias.PLATEAU.Snap.Generated.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Disposes resources if they were created by us
        /// </summary>
        public void Dispose()
        {
            this.ApiClient?.Dispose();
        }

        /// <summary>
        /// Holds the ApiClient if created
        /// </summary>
        public Synesthesias.PLATEAU.Snap.Generated.Client.ApiClient ApiClient { get; set; } = null;

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public Synesthesias.PLATEAU.Snap.Generated.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public Synesthesias.PLATEAU.Snap.Generated.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Synesthesias.PLATEAU.Snap.Generated.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public Synesthesias.PLATEAU.Snap.Generated.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。 
        /// </summary>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <returns>VisibleSurfacesResponse</returns>
        public VisibleSurfacesResponse GetVisibleSurfacesAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest))
        {
            Synesthesias.PLATEAU.Snap.Generated.Client.ApiResponse<VisibleSurfacesResponse> localVarResponse = GetVisibleSurfacesAsyncWithHttpInfo(visibleSurfacesRequest);
            return localVarResponse.Data;
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。 
        /// </summary>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <returns>ApiResponse of VisibleSurfacesResponse</returns>
        public Synesthesias.PLATEAU.Snap.Generated.Client.ApiResponse<VisibleSurfacesResponse> GetVisibleSurfacesAsyncWithHttpInfo(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest))
        {
            Synesthesias.PLATEAU.Snap.Generated.Client.RequestOptions localVarRequestOptions = new Synesthesias.PLATEAU.Snap.Generated.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json",
                "text/json",
                "application/*+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Synesthesias.PLATEAU.Snap.Generated.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Synesthesias.PLATEAU.Snap.Generated.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = visibleSurfacesRequest;


            // make the HTTP request
            var localVarResponse = this.Client.Post<VisibleSurfacesResponse>("/api/visible-surfaces", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetVisibleSurfacesAsync", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。 
        /// </summary>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of VisibleSurfacesResponse</returns>
        public async System.Threading.Tasks.Task<VisibleSurfacesResponse> GetVisibleSurfacesAsyncAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest), System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
        {
            Synesthesias.PLATEAU.Snap.Generated.Client.ApiResponse<VisibleSurfacesResponse> localVarResponse = await GetVisibleSurfacesAsyncWithHttpInfoAsync(visibleSurfacesRequest, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得します。 
        /// </summary>
        /// <exception cref="Synesthesias.PLATEAU.Snap.Generated.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="visibleSurfacesRequest"> (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (VisibleSurfacesResponse)</returns>
        public async System.Threading.Tasks.Task<Synesthesias.PLATEAU.Snap.Generated.Client.ApiResponse<VisibleSurfacesResponse>> GetVisibleSurfacesAsyncWithHttpInfoAsync(VisibleSurfacesRequest visibleSurfacesRequest = default(VisibleSurfacesRequest), System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
        {

            Synesthesias.PLATEAU.Snap.Generated.Client.RequestOptions localVarRequestOptions = new Synesthesias.PLATEAU.Snap.Generated.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/*+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Synesthesias.PLATEAU.Snap.Generated.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Synesthesias.PLATEAU.Snap.Generated.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = visibleSurfacesRequest;


            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PostAsync<VisibleSurfacesResponse>("/api/visible-surfaces", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetVisibleSurfacesAsync", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

    }
}
