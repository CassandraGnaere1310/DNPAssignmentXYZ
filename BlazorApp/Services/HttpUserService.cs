using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        using HttpResponseMessage response =
            await client.PostAsJsonAsync("users", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<UserDto>()
               ?? throw new Exception("The API did not return a user.");
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        using HttpResponseMessage response =
            await client.GetAsync($"users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<UserDto>()
               ?? throw new Exception("The API did not return a user.");
    }
}