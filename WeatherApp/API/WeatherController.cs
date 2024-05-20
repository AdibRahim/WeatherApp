using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WeatherApp.Data;
using WeatherApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp;

[Route("api/Weather")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly MyDbContext _database;

    public WeatherController(MyDbContext database)
    {
        _database = database;
    }

    #region Standard
    // GET: api/<WeatherController>
    [HttpGet]
    public async Task<ActionResult<List<Weather>>> Get()
    {
        return await _database.Weathers.ToListAsync();
    }

    // GET api/<WeatherController>/5
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
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
        await _database.Weathers.AddAsync(weather);
        await _database.SaveChangesAsync();
        return Ok();
    }

    // PUT api/<WeatherController>/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Weather weather)
    {
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
    public ActionResult GetWeatherForeCastByLatitudeAndLongitude(double latitude, double longitude)
    {
        return Ok();
    }
    #endregion
}

