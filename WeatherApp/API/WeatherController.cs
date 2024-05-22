using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using WeatherApp.Data;
using WeatherApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp;

[Route("api/Weather")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly MyDbContext _database;

    private static readonly HttpClient client = new HttpClient();
    private const string ApiKey = "b1c170d869bc4bbeb8bbbea074e6ea1a"; 
    private const string OpenWeatherURL = "http://api.openweathermap.org/data/2.5/weather";

    public WeatherController(MyDbContext database)
    {
        _database = database;
    }

    #region Standard
    // GET: api/<WeatherController>
    [HttpGet]
    public async Task<ActionResult<List<Weather>>> Get()
    {
        // There should be filtering for pagination here
        // Just take 20 so that in future doesn't cause high memory take a lot of data
        return await _database.Weathers.ToListAsync();
    }

    // GET api/<WeatherController>/5
    [HttpGet("{id}")]
    public ActionResult Get([FromRoute] int id)
    {
        // Should be validation for query parameter
        // 0 is the default value for integer
        if (id == 0)
        {
            throw new ArgumentNullException();
        } 

        Weather? weather = _database.Weathers.FirstOrDefault(x => x.Id == id);
        if (weather is null)
        {
            return NotFound();
        }
        return Ok(weather);
    }
        
    // POST api/<WeatherController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Weather weather)
    {
        // Validate 
        await _database.Weathers.AddAsync(weather);
        await _database.SaveChangesAsync();
        return Ok();
    }

    // PUT api/<WeatherController>/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Weather weather)
    {
        // Validate id
        if (id != weather.Id)
        {
            return BadRequest("Id is invalid.");
        }
        _database.Weathers.Update(weather);
        _database.SaveChanges();
        return Ok();
    }

    // DELETE api/<WeatherController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute]int id)
    {
        Weather? weather = _database.Weathers.FirstOrDefault(x => x.Id == id);
        if (weather == null)
        {
            return NotFound();
        }
        _database.Weathers.Remove(weather);
        await _database.SaveChangesAsync();
        return Ok();
    }
    #endregion

    #region Custom
    [HttpGet]
    [Route(nameof(GetWeatherForeCastByLatitudeAndLongitude))]
    public async Task<ActionResult> GetWeatherForeCastByLatitudeAndLongitude([FromQuery] double latitude,[FromQuery] double longitude)
    {
        string url = $"{OpenWeatherURL}?lat={latitude}&lon={longitude}&appid={ApiKey}";
        try
        {
            var response = await client.GetStringAsync(url);
            WeatherResponse? weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(response);
            if (weatherResponse == null)
            {
                throw new InvalidOperationException();
            }
            return Ok(weatherResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}

