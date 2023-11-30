// TicketController.cs
using Countries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Countries.Controllers // Corrected namespace
{
    public class TicketController : Controller
    {
        private static List<Ticket> tickets = new List<Ticket>();

        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Assign a unique ID to the ticket (for simplicity, you might want to use a database-generated ID)
                ticket.Id = tickets.Count + 1;

                // Set the initial status to 'todo'
                ticket.Status = Ticket.TicketStatus.Todo; // Assuming TicketStatus is an enum

                // Add the ticket to the list
                tickets.Add(ticket);

                // Return CreatedAtAction with the new ticket and the route values
                return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
            }

            // Return BadRequest with model state errors
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetTicket(int id)
        {
            var ticket = tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, Ticket updatedTicket)
        {
            var existingTicket = tickets.Find(t => t.Id == id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            // Update the existing ticket
            existingTicket.Name = updatedTicket.Name;
            existingTicket.Description = updatedTicket.Description;
            existingTicket.SprintNumber = updatedTicket.SprintNumber;
            existingTicket.PointValue = updatedTicket.PointValue;
            existingTicket.Status = updatedTicket.Status;

            return Ok(existingTicket);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            tickets.Remove(ticket);

            return NoContent(); // Indicates successful deletion with no response body
        }

        public ActionResult Index(int? id)
        {
            return View(tickets);
        }
    }
}
