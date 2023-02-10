using App.Models;

namespace App.Services;

public class MessageService : IMessageService
{
    private HttpClient _client;
    private readonly IConfiguration _config;

    public MessageService(IConfiguration config) : this(new HttpClient(), config)
    {
    }

    public MessageService(HttpClient client, IConfiguration config)
    {
        this._client = client;
        this._config = config;
    }

    public Message GetPublicMessage()
    {
        return new Message { text = "This is a public message." };
    }

    public Message GetProtectedMessage()
    {
        return new Message { text = "This is a protected message." };
    }

    public async Task<ApiResponse> GetAdminMessage(string accessToken)
    {
        return await CallApi(GetApiUrl("/api/messages/admin"), accessToken);
    }

    private string GetApiUrl(string uri)
    {
        return _config.GetValue<string>("API_SERVER_URL") + '/' + uri.TrimStart('/');
    }

    private async Task<ApiResponse> CallApi(string url, string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (accessToken != null)
        {
            request.Headers.Add("Authorization", "Bearer " + accessToken);
        }

        try
        {
            var response = _client.Send(request);
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse
                {
                    message = await response.Content.ReadFromJsonAsync<Message>()
                };
            }

            return new ApiResponse
            {
                error = await response.Content.ReadFromJsonAsync<object>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                error = new {
                    message = ex.Message
                }
            };
        }
    }
}
