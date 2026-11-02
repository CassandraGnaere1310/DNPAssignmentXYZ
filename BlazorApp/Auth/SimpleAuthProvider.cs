using System.Net.Http.Json;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;

    private ClaimsPrincipal currentUser =
        new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(currentUser));
    }

    public async Task LoginAsync(string username, string password)
    {
        LoginRequest request = new()
        {
            UserName = username,
            Password = password
        };

        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Forkert brugernavn eller adgangskode.");
        }

        UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>();

        if (user is null)
        {
            throw new Exception("Serveren returnerede ingen bruger.");
        }

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Id", user.Id.ToString())
        };

        ClaimsIdentity identity = new(claims, "apiauth");
        currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }

    public void Logout()
    {
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }
}