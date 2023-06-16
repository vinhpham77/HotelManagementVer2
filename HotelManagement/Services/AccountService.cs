using System.Net;
using System.Text;
using HotelManagement.Models;
using System.Web;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using HotelManagement.ViewModels;

namespace HotelManagement.Services;

public class AccountService
{
    private readonly HttpClient _httpClient;
    private readonly string _accountApiUrl = "Accounts";

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LCount<Account>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
    {
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder["keyword"] = keyword;
        queryBuilder["sort"] = sort;
        queryBuilder["order"] = order;
        if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
        if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

        string? queryString = queryBuilder.ToString();
        string requestUrl = string.IsNullOrEmpty(queryString) ? _accountApiUrl : $"{_accountApiUrl}?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<Account>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
    }
    
    public async Task<AccountDto?> LoginAsync(LoginViewModel request)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_accountApiUrl + "/login", content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountDto>(jsonResponse);
        } 
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException("Tên tài khoản hoặc mật khẩu không chính xác");
        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa");
        

        throw new HttpRequestException($"Có lỗi xảy ra, vui lòng thử lại sau!");
    }
    
    public async Task<List<string>?> GetUsernamesAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_accountApiUrl + "/usernames");
    
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(jsonResponse);
        }
    
        throw new HttpRequestException($"Request to get all RoomTypes failed with status code: {response.StatusCode}");
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_accountApiUrl}/{username}");

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to get Account by username {username} failed with status code: {response.StatusCode}");
    }

    public async Task<Account?> CreateAsync(Account account)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_accountApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to create Account failed with status code: {response.StatusCode}");
    }

    public async Task UpdateAsync(string username, JsonPatchDocument<Account> account)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PatchAsync($"{_accountApiUrl}/{username}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to update Account with username {username} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task ChangeStatus(string username, JsonPatchDocument<Account> account)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PatchAsync($"{_accountApiUrl}/{username}/status", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to change status with username {username} failed with status code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(string username)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{_accountApiUrl}/{username}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to delete Account with username {username} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task DeleteManyAsync(string[] usernames)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(usernames), Encoding.UTF8, "application/json");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _accountApiUrl)
        {
            Content = content
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to delete multiple Accounts failed with status code: {response.StatusCode}");
        }
    }
}