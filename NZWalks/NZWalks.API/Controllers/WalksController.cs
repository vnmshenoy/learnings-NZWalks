﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            //Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            //convert domain walks to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            // return response
            return Ok(walksDTO)
;
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk domain object from db
            var walkDomain = await walkRepository.GetAsync(id);
            //conver domain object to dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //return response
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //convert dto to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            // pass domain object to repository to persist
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //convert domian object back to dto
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDiffcultyId = walkDomain.WalkDifficultyId
            };
            //send dto response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new
            {
                id = walkDTO.Id,

            }, walkDTO);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> updateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to domain object
            var walkDomain= new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            // pass details to repository -get domain object in response (or null)
            walkDomain= await walkRepository.UpdateAsync(id, walkDomain);

            //handle null
            if (walkDomain == null)
            {
                return NotFound();
            }
            //if found something, convert domain to dto
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDiffcultyId = walkDomain.WalkDifficultyId

            };
            //return response
            return Ok(walkDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //call repo to delete walk
            var walkDomain= await walkRepository.DeleteAsync(id);
            //check for null
            if (walkDomain == null){// if domain not found
                return NotFound();
            }
            var walkDTO=mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
    }
}
