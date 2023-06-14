using System.Text;
using System.Web;
using HotelManagement.Models;
using Newtonsoft.Json;

namespace HotelManagement.Services;

public class MenuService
{
    private readonly HttpClient _httpClient;
    private readonly string _menuApiUrl = "Menu";

    public MenuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LCount<MenuItem>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
    {
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder["keyword"] = keyword;
        queryBuilder["sort"] = sort;
        queryBuilder["order"] = order;
        if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
        if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

        string? queryString = queryBuilder.ToString();
        string requestUrl = string.IsNullOrEmpty(queryString) ? _menuApiUrl : $"{_menuApiUrl}?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<MenuItem>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
    }

    public async Task<MenuItem?> GetByIdAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_menuApiUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MenuItem>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to get MenuItem by id {id} failed with status code: {response.StatusCode}");
    }

    public async Task<MenuItem?> CreateAsync(MenuItem menuItem)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(menuItem), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_menuApiUrl, content);
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MenuItem>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to create MenuItem failed with status code: {response.StatusCode}");
    }

    public async Task UpdateAsync(string id, MenuItem menuItem)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(menuItem), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync($"{_menuApiUrl}/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to update MenuItem with id {menuItem.Id} failed with status code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{_menuApiUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to delete MenuItem with id {id} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task DeleteManyAsync(string[] menuItemIds)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(menuItemIds), Encoding.UTF8, "application/json");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _menuApiUrl)
        {
            Content = content
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to delete multiple MenuItems failed with status code: {response.StatusCode}");
        }
    }
}