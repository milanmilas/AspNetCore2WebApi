using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId){
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == cityId);
            
            if(city == null){ return NotFound(); };

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsOfInterest/{id}")]
        public IActionResult GetPointsOfInterest(int cityId, int id){
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == cityId);
            if(city == null){ return NotFound(); };

            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(x => x.Id == id);
            if(pointOfInterest == null){ return NotFound(); };

            return Ok(pointOfInterest);
        }

    }
}