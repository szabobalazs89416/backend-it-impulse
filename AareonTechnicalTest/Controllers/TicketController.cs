using AareonTechnicalTest.Data;
using AareonTechnicalTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Controllers
{
    [Route("")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private ITicketData _ticketData;
        private IPersonData _personData;
        private ILogger<TicketController> _logger;

        public TicketController(ITicketData ticketData, IPersonData personData, ILogger<TicketController> logger)
        {
            _ticketData = ticketData;
            _personData = personData;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetTickets()
        {
            _logger.LogInformation("GetTickets method was called");

            try
            {
                var tickets = await _ticketData.GetTickets();

                _logger.LogInformation("All tickets are queried");
                return Ok(tickets);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured by querying all Ticket Data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            _logger.LogInformation("GetTicketById method was called with the following id paramater: {p1}", id);

            try
            {
                var ticket = await _ticketData.GetTicketById(id);

                if (ticket != null)
                {
                    _logger.LogInformation("Ticket was queried");
                    return Ok(ticket);
                }

                _logger.LogWarning("Ticket with Id: {id} was not found", id);
                return NotFound($"Ticket with Id: {id} was not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured by querying a Ticket Data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> AddTicket(Ticket ticket)
        {
            _logger.LogInformation("AddTicket method was called with the following params- Content: {p1}; PersonId: {p2}; Note: {p3}", ticket.Content, ticket.PersonId, ticket.Note);

            try
            {
                _logger.LogInformation("GetPersonById method was called, id: {p1}", ticket.PersonId);
                var person = await _personData.GetPersonById(ticket.PersonId);

                if (person != null)
                {
                    await _ticketData.AddTicket(ticket);

                    _logger.LogInformation("Ticket was added");
                    return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + ticket.Id, ticket);
                }

                _logger.LogWarning("Person with Id: {id} was not found", ticket.PersonId);
                return NotFound($"PersonId is not valid: {ticket.PersonId}, it was not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured by adding of Ticket Data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> DeleteTicketById(int id)
        {
            _logger.LogInformation("DeleteTicketById method was called with the following id paramater: {p1}", id);

            try
            {
                var ticket = await _ticketData.GetTicketById(id);

                if (ticket != null)
                {
                    await _ticketData.DeleteTicket(ticket);

                    _logger.LogInformation("Ticket was deleted");
                    return Ok($"Ticket with Id: {id} was deleted");
                }

                _logger.LogWarning("Ticket with Id: {id} was not found", id);
                return NotFound($"Ticket with Id: {id} was not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured by deletion of Ticket Data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> UpdateTicket(int id, Ticket ticket)
        {
            _logger.LogInformation("UpdateTicket method was called with the following id paramater- id: {p1}; Content: {p2}; PersonId: {p3}; Note: {p4}", id, ticket.Content, ticket.PersonId, ticket.Note);

            try
            {
                _logger.LogInformation("GetPersonById method was called, id: {p1}", ticket.PersonId);
                var person = await _personData.GetPersonById(ticket.PersonId);

                if (person == null)
                {
                    return NotFound($"PersonId is not valid: {ticket.PersonId}, it was not found");
                }

                var existingTicket = await _ticketData.GetTicketById(id);

                if (existingTicket != null)
                {
                    existingTicket.Content = ticket.Content;
                    existingTicket.PersonId = ticket.PersonId;
                    existingTicket.Note = ticket.Note;

                    await _ticketData.EditTicket(existingTicket);

                    _logger.LogInformation("Ticket was updated");
                    return Ok($"Ticket with Id: {id} was updated");
                }

                _logger.LogWarning("Ticket with Id: {id} was not found", id);
                return NotFound($"Ticket with Id: {id} was not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured by updating of Ticket Data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
