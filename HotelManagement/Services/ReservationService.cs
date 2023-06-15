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
            string requestUrl = string.IsNullOrEmpty(queryString) ? _reservationDetailApiUrl : $"{_reservationDetailApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LCount<ReservationDetail>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }

        public async Task UpdateAsync(ReservationDetail reservationDetail)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(reservationDetail), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_reservationDetailApiUrl}/{reservationDetail.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update RoomType with id {reservationDetail.Id} failed with status code: {response.StatusCode}");
            }
        }
    }
}
