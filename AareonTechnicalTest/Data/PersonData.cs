using AareonTechnicalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AareonTechnicalTest.Data
{
    public class PersonData : IPersonData
    {
        private ApplicationContext _applicationContext;

        public PersonData(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<Person> GetPersonById(int id)
        {
            return await _applicationContext.Persons.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
