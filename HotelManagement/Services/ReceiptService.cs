using System.Text;
using System.Web;
using HotelManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace HotelManagement.Services
{
    public class ReceiptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _receiptApiUrl = "Receipts";

        public ReceiptService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Receipt?> GetByIdAsync(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_receiptApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Receipt>(jsonResponse);
            }

            throw new HttpRequestException(
                $"Request to get RoomType by id {id} failed with status code: {response.StatusCode}");
        }

        public async Task<Receipt?> CreateAsync(Receipt receipt)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(receipt), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_receiptApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Receipt>(jsonResponse);
            }

            throw new HttpRequestException(
                $"Request to create RoomType failed with status code: {response.StatusCode}");
        }

        public async Task UpdateAsync(Receipt receipt)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(receipt), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_receiptApiUrl}/{receipt.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update RoomType with id {receipt.Id} failed with status code: {response.StatusCode}");
            }
        }
    }
}
