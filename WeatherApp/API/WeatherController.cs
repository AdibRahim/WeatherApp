using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp;

[Route("api/Weather")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly MyDbContext _context;

    public WeatherController(MyDbContext context)
    {
        _context = context;
    }

    #region Standard
    // GET: api/<WeatherController>
    [HttpGet]
    public async Task<ActionResult<List<Weather>>> Get()
    {
        return await _context.Weathers.ToListAsync();
    }

    // GET api/<WeatherController>/5
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        return Ok();
    }

    // POST api/<WeatherController>
    [HttpPost]
    public ActionResult Post([FromBody] string value)
    {
        return Ok();
    }

    // PUT api/<WeatherController>/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] string value)
    {
        return Ok();
    }

    // DELETE api/<WeatherController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
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

