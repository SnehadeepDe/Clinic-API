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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery]int? patientId, [FromQuery]int? doctorId)
        {
            try
            {
                var appointments = _appointmentService.GetAll(patientId,doctorId);
                if (appointments.Any())
                    return Ok(appointments);
                else
                    return Ok(new { message = "No appointments found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AppointmentResponse> GetById([FromRoute] int id)
        {
            try
            {
                var appointment = _appointmentService.GetById(id);
                return Ok(appointment);
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

        [HttpPost("book")]
        public IActionResult Create([FromBody] AppointmentRequest appointmentRequest)
        {
            try
            {
                var appointmentId = _appointmentService.Create(appointmentRequest);
                return CreatedAtAction(nameof(GetById), new { id = appointmentId }, new { id = appointmentId });
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
        public IActionResult Update([FromRoute] int id, [FromBody] AppointmentRequest appointmentRequest)
        {
            try
            {
                _appointmentService.Update(id, appointmentRequest);
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
                _appointmentService.Delete(id);
                return Ok(new { message = $"Appointment with Id {id} deleted" });
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
