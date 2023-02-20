namespace HowHttpClientWorks.Clients
{
    public class OpenWeatherClient : IWeatherClient
    {
        private const string OpenWeatherApiKey = "";

        private readonly HttpClient _httpClient;

        public OpenWeatherClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponse> GetCurrentWeatherForCity(string city)
        {
            return await _httpClient.GetFromJsonAsync<WeatherResponse>($"weather?q={city}&app={OpenWeatherApiKey}");
        }
    }

    public interface IWeatherClient
    {

    }
}
