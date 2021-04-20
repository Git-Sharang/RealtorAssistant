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
    public class RealtorsController : ControllerBase
    {

        private IRealtorRepository _realtorRepository;
        private readonly IMapper _mapper;
        public RealtorsController(IRealtorRepository realtorRepository, IMapper mapper)
        {
            _realtorRepository = realtorRepository;
            _mapper = mapper;
        }


        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<Realtors>> GetRealtors()
        {
            var realtorEntities = await _realtorRepository.GetRealtors();

            var results = _mapper.Map<IEnumerable<ReatorWithoutAssetsDto>>(realtorEntities);

            return Ok(results);
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Realtors>> GetRealtor(int id, bool includeAssets = false)
        {
            var realtor = await _realtorRepository.GetRealtorById(id, includeAssets);

            if (realtor == null)
            {
                return NotFound();
            }

            if (includeAssets)
            {
                var realtorResult = _mapper.Map<RealtorDto>(realtor);
                return Ok(realtorResult);
            }

            var realtorWithoutAssets = _mapper.Map<ReatorWithoutAssetsDto>(realtor);
            return Ok(realtorWithoutAssets);
        }


        // GET api/<controller>/realtorusername?username
        [HttpGet("realtorusername")]
        public async Task<ActionResult<Realtors>> GetRealtorByUsername(string username, bool includeAssets = false)
        {
            var realtor = await _realtorRepository.GetRealtorByUsername(username, includeAssets);

            if (realtor == null)
            {
                return NotFound();
            }

            if (includeAssets)
            {
                var realtorResult = _mapper.Map<RealtorDto>(realtor);
                return Ok(realtorResult);
            }

            var realtorWithoutAssets = _mapper.Map<ReatorWithoutAssetsDto>(realtor);
            return Ok(realtorWithoutAssets);
        }


        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<RealtorDto>> CreateRealtor(RealtorForCreationDto realtorForCreation)
        {
            if (realtorForCreation == null)
            {
                return BadRequest();
            }
            if (realtorForCreation.Username == realtorForCreation.Firstname)
            {
                ModelState.AddModelError("Username", "The provided username should be different from the firstname");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _realtorRepository.RealtorUsernameExists(realtorForCreation.Username))
            {
                return StatusCode(400, "The Username already exists");
            }
            var finalRealtor = _mapper.Map<Realtors>(realtorForCreation);
            await _realtorRepository.AddRealtor(finalRealtor);
            if (!await _realtorRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            var createdReatorToReturn = _mapper.Map<RealtorDto>(finalRealtor);
            return CreatedAtAction("GetRealtor", new { id = createdReatorToReturn.Id }, createdReatorToReturn);
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRealtor(int id)
        {
            if (!await _realtorRepository.RealtorExists(id))
            {
                return NotFound();
            }
            Realtors realtorEntityDelete = await _realtorRepository.GetRealtorById(id, true);
            if (realtorEntityDelete == null)
            {
                return NotFound();
            }
            _realtorRepository.DeleteRealtor(realtorEntityDelete);
            if (!await _realtorRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }
    }
}
