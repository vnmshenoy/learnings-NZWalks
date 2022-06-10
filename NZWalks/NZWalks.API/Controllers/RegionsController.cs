﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    /*You could have done in two ways
     *1) Route("[Regions]") or say Route("[nz-regions]")
         - It can be anything like "Regions","nz-regions" etc. It will
            following url of our application (eg /regions,/nz-regions)
       
     *2) Route("[controller]") - we just specify the controller keyword 
     *So it will auto populate controller name to "Region" since it is a 
     * RegionController
     **/

    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {

            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id=Guid.NewGuid(),
            //        Name="Wellingon",
            //        Code = "WLG",
            //        Area= 227755,
            //        Lat = -1.8822,
            //        Long = 299.88,
            //        Population =500000
            //    },
            //     new Region
            //    {
            //        Id=Guid.NewGuid(),
            //        Name="Auckland",
            //        Code = "AUCK",
            //        Area= 227755,
            //        Lat = -1.8822,
            //        Long = 299.88,
            //        Population =500000
            //    }
            //};
            //var regions = regionRepository.GetAll();
            var regions = await regionRepository.GetAllAsync();
            // return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            //
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")] //restricting route to only take guid values
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region= await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest){
            //Convert request which is a dto to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };
            //pass details to repository
            region=await regionRepository.AddAsync(region);
            //convert data (domain modal) back to dto
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id }, regionDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from database
            var region = await regionRepository.DeleteAsync(id);
            //if no region found, then say null NotFound
            if (region == null)
            {
                return NotFound();
            }
            //Convert the response back to DTO 
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //Give response back to client by saying Ok
            return Ok(regionDTO);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };
            //update region using repository
            region = await regionRepository.UpdateAsync(id,region);
            //if null return NotFound
            if (region == null)
            {
                return NotFound();
            }
            // convert domain back to dto
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //return Ok response
            return Ok(regionDTO);
        }
    }
}
