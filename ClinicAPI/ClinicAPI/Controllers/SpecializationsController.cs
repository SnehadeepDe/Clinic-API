using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using ClinicAPI.Services.Interfaces;
using ClinicAPI.CustomException;
using System.Linq;

namespace ClinicAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var specializations = _specializationService.GetAll();
                if (specializations.Any())
                    return Ok(specializations);
                else
                    return Ok(new { message = "No specializations found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<SpecializationResponse> GetById([FromRoute] int id)
        {
            try
            {
                var specialization = _specializationService.GetById(id);
                return Ok(specialization);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] SpecializationRequest specializationRequest)
        {
            try
            {
                var specializationId = _specializationService.Create(specializationRequest);
                return CreatedAtAction(nameof(GetById), new { id = specializationId }, new { id = specializationId });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] SpecializationRequest specializationRequest)
        {
            try
            {
                _specializationService.Update(id, specializationRequest);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _specializationService.Delete(id);
                return Ok(new { message = $"Specialization with Id {id} deleted" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
