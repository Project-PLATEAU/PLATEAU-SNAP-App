# Synesthesias.PLATEAU.Snap.Generated.Api.ImagesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateBuildingImageAsync**](ImagesApi.md#createbuildingimageasync) | **POST** /api/building-image | 撮影した建物面の画像を登録します。 |

<a id="createbuildingimageasync"></a>
# **CreateBuildingImageAsync**
> BuildingImageResponse CreateBuildingImageAsync (FileParameter file, string metadata)

撮影した建物面の画像を登録します。

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
    public class CreateBuildingImageAsyncExample
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
            var apiInstance = new ImagesApi(httpClient, config, httpClientHandler);
            var file = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // FileParameter | 建物面を撮影した画像ファイル
            var metadata = "metadata_example";  // string | 画像に関連するメタデータ

            try
            {
                // 撮影した建物面の画像を登録します。
                BuildingImageResponse result = apiInstance.CreateBuildingImageAsync(file, metadata);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ImagesApi.CreateBuildingImageAsync: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateBuildingImageAsyncWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // 撮影した建物面の画像を登録します。
    ApiResponse<BuildingImageResponse> response = apiInstance.CreateBuildingImageAsyncWithHttpInfo(file, metadata);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ImagesApi.CreateBuildingImageAsyncWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **file** | **FileParameter****FileParameter** | 建物面を撮影した画像ファイル |  |
| **metadata** | **string** | 画像に関連するメタデータ |  |

### Return type

[**BuildingImageResponse**](BuildingImageResponse.md)

### Authorization

[ApiKey](../README.md#ApiKey)

### HTTP request headers

 - **Content-Type**: multipart/form-data
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

