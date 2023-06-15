using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;

namespace HotelManagement.Services;

public class CustomerService
{
    private readonly HttpClient _httpClient;
    private readonly string _customerApiUrl = "Customers";

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LCount<Customer>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
    {
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder["keyword"] = keyword;
        queryBuilder["sort"] = sort;
        queryBuilder["order"] = order;
        if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
        if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

        string? queryString = queryBuilder.ToString();
        string requestUrl = string.IsNullOrEmpty(queryString) ? _customerApiUrl : $"{_customerApiUrl}?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<Customer>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
    }
    
    public async Task<LCount<Customer>?> GetAllAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_customerApiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LCount<Customer>>(jsonResponse);
        }

        throw new HttpRequestException($"Request to get all Customers failed with status code: {response.StatusCode}");
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_customerApiUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to get Customer by id {id} failed with status code: {response.StatusCode}");
    }

    public async Task<Customer?> CreateAsync(Customer customer)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_customerApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(jsonResponse);
        }
        
        throw new HttpRequestException(
            $"Request to create Customer failed with status code: {response.StatusCode}");
    }

    public async Task UpdateAsync(string id, Customer customer)
    {
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync($"{_customerApiUrl}/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to update Customer with id {customer.Id} failed with status code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"{_customerApiUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to delete Customer with id {id} failed with status code: {response.StatusCode}");
        }
    }
    
    public async Task DeleteManyAsync(string[] customerIds)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(customerIds), Encoding.UTF8, "application/json");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _customerApiUrl)
        {
            Content = content
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to delete multiple Customers failed with status code: {response.StatusCode}");
        }
    }
}