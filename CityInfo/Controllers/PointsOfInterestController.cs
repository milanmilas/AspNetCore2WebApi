using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost("{cityId}/pointsofinterest", Name="GetPointOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // demo purposes - to be improved
            var maxPointOfInterestId = CitiesDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterest)
                .Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                                    new { cityId = cityId,
                                        id = finalPointOfInterest.Id},
                                        finalPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if(pointOfInterest == null){
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == cityId);
            if(city == null){ return NotFound(); };

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(x => x.Id == id);
            if(pointOfInterestFromStore == null){ return NotFound(); };

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent(); // 201
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(
            int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            // patch should never change id !!!
            if(patchDoc == null){
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == cityId);
            if(city == null){ return NotFound(); };

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(x => x.Id == id);
            if(pointOfInterestFromStore == null){ return NotFound(); };

            var pointOfInterestToPatch =
                    new PointOfInterestForUpdateDto(){
                        Name = pointOfInterestFromStore.Name,
                        Description = pointOfInterestFromStore.Description
                    };
            
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            
            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {

        }
    }
}