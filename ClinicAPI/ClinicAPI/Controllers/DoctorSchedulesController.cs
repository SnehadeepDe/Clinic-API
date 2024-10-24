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
    [Route("/doctors/{doctorId}/schedule")]
    [ApiController]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorSchedulesController(IDoctorScheduleService doctorScheduleService)
        {
            _doctorScheduleService = doctorScheduleService;
        }

        [HttpGet]
        public IActionResult GetAll([FromRoute] int doctorId)
        {
            try
            {
                var schedules = _doctorScheduleService.GetAll(doctorId);
                if (schedules.Any())
                    return Ok(schedules);
                else
                    return Ok(new { message = "No doctor schedules found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DoctorScheduleResponse> GetById([FromRoute] int doctorId, [FromRoute] int id)
        {
            try
            {
                var schedule = _doctorScheduleService.GetById(doctorId,id);
                return Ok(schedule);
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
        public IActionResult Create([FromRoute] int doctorId, [FromBody] DoctorScheduleRequest scheduleRequest)
        {
            try
            {
                var scheduleId = _doctorScheduleService.Create(doctorId,scheduleRequest);
                return CreatedAtAction(nameof(GetById), new { doctorId = doctorId, id = scheduleId }, new { id = scheduleId });
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
        public IActionResult Update([FromRoute]int doctorId,[FromRoute]int id, [FromBody] DoctorScheduleRequest scheduleRequest)
        {
            try
            {
                _doctorScheduleService.Update(doctorId, id, scheduleRequest);
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
        public IActionResult Delete([FromRoute]int doctorId, [FromRoute]int id)
        {
            try
            {
                _doctorScheduleService.Delete(doctorId, id);
                return Ok(new { message = $"Doctor schedule with Id {id} deleted" });
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
