using AareonTechnicalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Data
{
    public interface ITicketData
    {
        public Task<List<Ticket>> GetTickets();

        public Task<Ticket> GetTicketById(int id);

        public Task AddTicket(Ticket ticket);

        public Task DeleteTicket(Ticket ticket);

        public Task<Ticket> EditTicket(Ticket ticket);
    }
}
