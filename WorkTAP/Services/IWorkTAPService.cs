using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;

namespace WorkTAP.Services
{
    public interface IWorkTAPService
    {
        Task<ActionResult<IEnumerable<Person>>> Get();
        Task<ActionResult<Person>> Get(int id);
        Task<ActionResult<Person>> Create(Person person);
        Task<ActionResult<Person>> Update(Person updatedPerson);
        Task<ActionResult<Person>> Delete(int id);

        bool PersonExists(int id);
    }
}
