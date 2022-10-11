using AareonTechnicalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Data
{
    public interface IPersonData
    {
        public Task<Person> GetPersonById(int id);
    }
}
