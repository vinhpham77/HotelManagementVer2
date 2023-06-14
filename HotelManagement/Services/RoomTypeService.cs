using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;

namespace HotelManagement.Services;

public class RoomTypeService
{
    private readonly HttpClient _httpClient;
    private readonly string _roomTypeApiUrl = "RoomTypes";

    public RoomTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LCount<RoomType>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
    {
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder["keyword"] = keyword;
        queryBuilder["sort"] = sort;
        queryBuilder["order"] = order;
        if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
        if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

        string? queryString = queryBuilder.ToString();
        string requestUrl = string.IsNullOrEmpty(queryString) ? _roomTypeApiUrl : $"{_roomTypeApiUrl}?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<RoomType>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
    }
    
    public async Task<LCount<RoomType>?> GetAllAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_roomTypeApiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<RoomType>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to get all RoomTypes failed with status code: {response.StatusCode}");
    }

    public async Task<RoomType?> GetByIdAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_roomTypeApiUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoomType>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to get RoomType by id {id} failed with status code: {response.StatusCode}");
    }

    public async Task<RoomType?> CreateAsync(RoomType roomType)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(roomType), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_roomTypeApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoomType>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to create RoomType failed with status code: {response.StatusCode}");
    }

    public async Task UpdateAsync(string id, RoomType roomType)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(roomType), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync($"{_roomTypeApiUrl}/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to update RoomType with id {roomType.Id} failed with status code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{_roomTypeApiUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to delete RoomType with id {id} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task DeleteManyAsync(string[] roomTypeIds)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(roomTypeIds), Encoding.UTF8, "application/json");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _roomTypeApiUrl)
        {
            Content = content
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to delete multiple RoomTypes failed with status code: {response.StatusCode}");
        }
    }
}