using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_RealtorAssistant.Entities;
using RestAPI_RealtorAssistant.Models;
using RestAPI_RealtorAssistant.Services;

namespace RestAPI_RealtorAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private IRealtorRepository _realtorRepository;
        private readonly IMapper _mapper;
        public AssetsController(IRealtorRepository realtorRepository, IMapper mapper)
        {
            _realtorRepository = realtorRepository;
            _mapper = mapper;
        }


        // GET: api/<controller>/1/asset
        [HttpGet("{realtorId}/asset")]
        public async Task<ActionResult<Asset>> GetAssets(int realtorId)
        {
            if (!(await _realtorRepository.RealtorExists(realtorId)))
            {
                return NotFound();
            }
            var assetForRealtor = await _realtorRepository.GetAssetsForRealtor(realtorId);
            var assetForRealtorResults = _mapper.Map<IEnumerable<AssetDto>>(assetForRealtor);
            return Ok(assetForRealtorResults);
        }


        // GET api/<controller>/5/asset/1
        [HttpGet("{realtorId}/asset/{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int realtorId, int id)
        {
            if (!await _realtorRepository.RealtorExists(realtorId))
            {
                return NotFound();
            }
            var asset = await _realtorRepository.GetAssetForRealtorById(realtorId, id);
            if (asset == null)
            {
                return NotFound();
            }
            var assetResult = _mapper.Map<AssetDto>(asset);
            return Ok(assetResult);
        }


        // GET: api/<controller>/{realtorId}/assetType?assetType={assetType}
        [HttpGet("{realtorId}/assetType")]
        public async Task<ActionResult<Asset>> GetAssetWithSpecificType(int realtorId, string assetType = "Condo")
        {
            if (!(await _realtorRepository.RealtorExists(realtorId)))
            {
                return NotFound();
            }
            if (assetType == "Condo")
            {
                await _realtorRepository.GetAssetsForRealtorByType(realtorId, "Condo");
            }
            var assetForRealtorWithType = await _realtorRepository.GetAssetsForRealtorByType(realtorId, assetType);
            var assetForRealtorResults = _mapper.Map<IEnumerable<AssetDto>>(assetForRealtorWithType);
            return Ok(assetForRealtorResults);
        }


        // GET: api/<controller>/{realtorId}/assetOwnershipStatus?isAssetOwned={isAssetOwned}
        [HttpGet("{realtorId}/assetOwnershipStatus")]
        public async Task<ActionResult<Asset>> GetAssetWithSpecificOwnership(int realtorId, bool isAssetOwned = false)
        {
            if (!(await _realtorRepository.RealtorExists(realtorId)))
            {
                return NotFound();
            }
            if (isAssetOwned)
            {
                await _realtorRepository.GetSoldAssetsOfRealtor(realtorId, false);
            }
            var assetForRealtorWithOwnership = await _realtorRepository.GetAssetsForRealtorByOwnership(realtorId, isAssetOwned);
            var assetForRealtorResults = _mapper.Map<IEnumerable<AssetDto>>(assetForRealtorWithOwnership);
            return Ok(assetForRealtorResults);
        }


        // GET: api/<controller>/{realtorId}/assetSoldStatus?isAssetSold={isAssetSold}
        [HttpGet("{realtorId}/assetSoldStatus")]
        public async Task<ActionResult<Asset>> GetAssetWithSoldStatus(int realtorId, bool isAssetSold = false)
        {
            if (!(await _realtorRepository.RealtorExists(realtorId)))
            {
                return NotFound();
            }
            if (isAssetSold)
            {
                await _realtorRepository.GetSoldAssetsOfRealtor(realtorId, false);
            }
            var assetForRealtorWithSold = await _realtorRepository.GetSoldAssetsOfRealtor(realtorId, isAssetSold);
            var assetForRealtorResults = _mapper.Map<IEnumerable<AssetDto>>(assetForRealtorWithSold);
            return Ok(assetForRealtorResults);
        }


        // POST api/<controller>/1/asset
        [HttpPost("{realtorId}/asset")]
        public async Task<ActionResult<AssetDto>> CreateAsset(int realtorId, [FromBody] AssetForCreationDto assetForCreation)
        {
            if (assetForCreation == null)
            {
                return BadRequest();
            }
            if (assetForCreation.Address == assetForCreation.Name)
            {
                ModelState.AddModelError("Address", "The address of the property should be diferrent from the name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _realtorRepository.RealtorExists(realtorId)) 
            { 
                return NotFound(); 
            }
            var finalAsset = _mapper.Map<Asset>(assetForCreation);
            await _realtorRepository.AddAssetForRealtor(realtorId, finalAsset);
            if (!await _realtorRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            var createdAssetToReturn = _mapper.Map<AssetDto>(finalAsset);
            return CreatedAtAction("GetAsset", new { realtorId = realtorId, id = createdAssetToReturn.Id }, createdAssetToReturn);
        }


        // PUT api/<controller>/5/asset/1
        [HttpPut("{realtorId}/asset/{id}")]
        public async Task<ActionResult> UpdateAsset(int realtorId, int id, [FromBody] AssetForUpdateDto assetForUpdate)
        {
            if (assetForUpdate == null)
            {
                return BadRequest();
            }
            if (assetForUpdate.Address == assetForUpdate.Name)
            {
                ModelState.AddModelError("Address", "The address of the property should be diferrent from the name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _realtorRepository.RealtorExists(realtorId))
            {
                return NotFound();
            }
            Asset oldAssetEntity = await _realtorRepository.GetAssetForRealtorById(realtorId, id);
            if (oldAssetEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(assetForUpdate, oldAssetEntity);
            if (!await _realtorRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }


        // DELETE api/<controller>/1/asset/2
        [HttpDelete("{realtorId}/asset/{id}")]
        public async Task<ActionResult> DeleteAsset(int realtorId, int id)
        {
            if (!await _realtorRepository.RealtorExists(realtorId))
            {
                return NotFound();
            }
            Asset assetEntityForDelete = await _realtorRepository.GetAssetForRealtorById(realtorId, id);
            if (assetEntityForDelete == null)
            {
                return NotFound();
            }
            _realtorRepository.DeleteAsset(assetEntityForDelete);
            if (!await _realtorRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }
    }
}
