using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities(){
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id){
            var result =  CitiesDataStore.Current.Cities
                .Where(x => x.Id == id)
                .FirstOrDefault();
            if(result == null){
                return NotFound();
            }

            return Ok(result);
        }

    }
}