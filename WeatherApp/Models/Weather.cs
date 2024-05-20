namespace WeatherApp.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public double TemperatureC { get; set; }
        public string Summary { get; set; }

    }
}
