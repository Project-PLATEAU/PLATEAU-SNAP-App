# Synesthesias.PLATEAU.Snap.Generated.Api.SurfacesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetVisibleSurfacesAsync**](SurfacesApi.md#getvisiblesurfacesasync) | **POST** /api/visible-surfaces | 現在の位置で撮影可能な面の情報を取得します。 |

<a id="getvisiblesurfacesasync"></a>
# **GetVisibleSurfacesAsync**
> VisibleSurfacesResponse GetVisibleSurfacesAsync (VisibleSurfacesRequest visibleSurfacesRequest = null)

現在の位置で撮影可能な面の情報を取得します。

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.PLATEAU.Snap.Generated.Model;

namespace Example
{
    public class GetVisibleSurfacesAsyncExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure API key authorization: ApiKey
            config.AddApiKey("X-API-KEY", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("X-API-KEY", "Bearer");

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new SurfacesApi(httpClient, config, httpClientHandler);
            var visibleSurfacesRequest = new VisibleSurfacesRequest(); // VisibleSurfacesRequest |  (optional) 

            try
            {
                // 現在の位置で撮影可能な面の情報を取得します。
                VisibleSurfacesResponse result = apiInstance.GetVisibleSurfacesAsync(visibleSurfacesRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SurfacesApi.GetVisibleSurfacesAsync: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetVisibleSurfacesAsyncWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // 現在の位置で撮影可能な面の情報を取得します。
    ApiResponse<VisibleSurfacesResponse> response = apiInstance.GetVisibleSurfacesAsyncWithHttpInfo(visibleSurfacesRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SurfacesApi.GetVisibleSurfacesAsyncWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **visibleSurfacesRequest** | [**VisibleSurfacesRequest**](VisibleSurfacesRequest.md) |  | [optional]  |

### Return type

[**VisibleSurfacesResponse**](VisibleSurfacesResponse.md)

### Authorization

[ApiKey](../README.md#ApiKey)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | リクエストが成功しました。 |  -  |
| **400** | リクエストが不正です。 |  -  |
| **401** | 認証に失敗しました。 |  -  |
| **404** | リソースが見つかりません。 |  -  |
| **500** | サーバー内部でエラーが発生しました。 |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

