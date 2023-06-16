using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;

namespace HotelManagement.Services
{
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _reservationApiUrl = "Reservation";

        public ReservationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LCount<Reservation>?> GetAsync(string? roomId, bool? checkedOut)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["roomId"] = roomId;
            queryBuilder["checkedOut"] = checkedOut.ToString();

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _reservationApiUrl : $"{_reservationApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LCount<Reservation>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }

		public async Task UpdateAsync(Reservation reservation)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_reservationApiUrl}/{reservation.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update RoomType with id {reservation.Id} failed with status code: {response.StatusCode}");
            }
        }
        public async Task<Reservation?> GetByIdAsync(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_reservationApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Reservation>(jsonResponse);
            }

            throw new HttpRequestException(
                $"Request to get RoomType by id {id} failed with status code: {response.StatusCode}");
        }

		public async Task<Reservation?> CreateAsync(Reservation reservation)
		{
			StringContent content =
				new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync(_reservationApiUrl, content);
			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Reservation>(jsonResponse);
			}

			throw new HttpRequestException(
				$"Request to create Room failed with status code: {response.StatusCode}");
		}
	}
}
