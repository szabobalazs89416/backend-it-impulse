using AareonTechnicalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AareonTechnicalTest.Data
{
    public class TicketData : ITicketData
    {
        private ApplicationContext _applicationContext;
        public TicketData(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task AddTicket(Ticket ticket)
        {
            await _applicationContext.Tickets.AddAsync(ticket);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task DeleteTicket(Ticket ticket)
        {
            _applicationContext.Tickets.Remove(ticket);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<Ticket> EditTicket(Ticket ticket)
        {
            _applicationContext.Tickets.Update(ticket);
            await _applicationContext.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> GetTicketById(int id)
        {
            return await _applicationContext.Tickets.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Ticket>> GetTickets()
        {
            return await _applicationContext.Tickets.ToListAsync();
        }
    }
}
