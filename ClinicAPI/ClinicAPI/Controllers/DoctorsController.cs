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
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? specializationId )
        {
            try
            {
                var doctors = _doctorService.GetAll(specializationId);
                if (doctors.Any())
                    return Ok(doctors);
                else
                    return Ok(new { message = "No doctors found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DoctorResponse> GetById([FromRoute] int id)
        {
            try
            {
                var doctor = _doctorService.GetById(id);
                return Ok(doctor);
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
        public IActionResult Create([FromBody] DoctorRequest doctorRequest)
        {
            try
            {
                var doctorId = _doctorService.Create(doctorRequest);
                return CreatedAtAction(nameof(GetById), new { id = doctorId }, new { id = doctorId });
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
        public IActionResult Update([FromRoute] int id, [FromBody] DoctorRequest doctorRequest)
        {
            try
            {
                _doctorService.Update(id, doctorRequest);
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
                _doctorService.Delete(id);
                return Ok(new { message = $"Doctor with Id {id} deleted" });
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

        [HttpGet("best-doctor")]
        public IActionResult GetBestDoctor([FromQuery] int specializationId, [FromQuery] string day)
        {
            try
            {
                var bestDoctor = _doctorService.GetBestDoctorForSpecialization(specializationId, day);
                if(bestDoctor!=null)
                    return Ok(bestDoctor);
                else
                    return Ok(new { message = "No doctors found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
