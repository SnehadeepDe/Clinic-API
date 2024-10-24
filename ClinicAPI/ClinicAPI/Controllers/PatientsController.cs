using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using ClinicAPI.Services.Interfaces;
using System.Linq;
using ClinicAPI.CustomException;

namespace ClinicAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var patients = _patientService.GetAll();
                if (patients.Any())
                    return Ok(patients);
                else
                    return Ok(new { message = "No patients found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PatientResponse> GetById([FromRoute] int id)
        {
            try
            {
                var patient = _patientService.GetById(id);
                return Ok(patient);
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
        public IActionResult Create([FromBody] PatientRequest patientRequest)
        {
            try
            {
                var patientId = _patientService.Create(patientRequest);
                return CreatedAtAction(nameof(GetById), new { id = patientId }, new { id = patientId });
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
        public IActionResult Update([FromRoute] int id, [FromBody] PatientRequest patientRequest)
        {
            try
            {
                _patientService.Update(id, patientRequest);
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
                _patientService.Delete(id);
                return Ok(new { message = $"Patient with Id {id} deleted" });
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
