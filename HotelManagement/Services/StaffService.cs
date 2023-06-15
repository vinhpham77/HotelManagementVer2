using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;

namespace HotelManagement.Services;

public class StaffService
{
    private readonly HttpClient _httpClient;
    private readonly string _staffApiUrl = "Staff";

    public StaffService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LCount<Personnel>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
    {
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder["keyword"] = keyword;
        queryBuilder["sort"] = sort;
        queryBuilder["order"] = order;
        if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
        if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

        string? queryString = queryBuilder.ToString();
        string requestUrl = string.IsNullOrEmpty(queryString) ? _staffApiUrl : $"{_staffApiUrl}?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<Personnel>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
    }
    
    public async Task<LCount<Personnel>?> GetAllAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_staffApiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<Personnel>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to get all Staff failed with status code: {response.StatusCode}");
    }

    public async Task<Personnel?> GetByIdAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_staffApiUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personnel>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to get Personnel by id {id} failed with status code: {response.StatusCode}");
    }

    public async Task<Personnel?> CreateAsync(Personnel personnel)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(personnel), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_staffApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personnel>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to create Personnel failed with status code: {response.StatusCode}");
    }

    public async Task UpdateAsync(string id, Personnel personnel)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(personnel), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync($"{_staffApiUrl}/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to update Personnel with id {personnel.Id} failed with status code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{_staffApiUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to delete Personnel with id {id} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task DeleteManyAsync(string[] personnelIds)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(personnelIds), Encoding.UTF8, "application/json");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _staffApiUrl)
        {
            Content = content
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to delete multiple Staff failed with status code: {response.StatusCode}");
        }
    }
}