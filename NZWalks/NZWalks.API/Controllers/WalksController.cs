using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

     public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
     [HttpGet]
     public async Task<IActionResult> GetAllWalksAsync()
        {
            // fetch data from database - domain walks
           var walksDomain = await walkRepository.GetAllAsync();

            // convert domain walks to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response
            return Ok(walksDTO);
        }

     [HttpGet]
     [Route("{id:guid}")]
     [ActionName("GetWalkAsync")]
     public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain Object From Database
            var walkDomain = await walkRepository.GetAsync(id);

            // Convert Domain Object To DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // return

            return Ok(walkDTO);
        }


     [HttpPost]

     public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain Object
            //var walkDomain =  mapper.Map<Models.Domain.Walk>(addWalkRequest);

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass Domain Object TO Reoisitory to Persist THIS

            walkDomain = await  walkRepository.AddAsync(walkDomain);

            // Convert domain object back to DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //var walkDTO = new Models.DTO.Walk
            //{
            //    Id = walkDomain.Id,
            //    Name = walkDomain.Name,
            //    Length = walkDomain.Length,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficultyId = walkDomain.WalkDifficultyId,

            //};


            // Send DTO response back to client

            return CreatedAtAction(nameof(GetWalkAsync),new { id = walkDTO.Id} , walkDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to domain models
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // pass Details to repository - get domain object in response (or null)

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // Handle null (not Found)

            if(walkDomain == null)
            {
                return NotFound();
            }
                       
                // convert domain to DTO

                var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //var walkDTO = new Models.DTO.Walk
            //{
            //    Id = walkDomain.Id,
            //    Name = walkDomain.Name,
            //    Length = walkDomain.Length,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficultyId = walkDomain.WalkDifficultyId,
            //};
           

            // return response
            return Ok(walkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // call repository to delete walk
            var walkDomain =await walkRepository.DeleteAsync(id);

            if(walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }
    }
}
