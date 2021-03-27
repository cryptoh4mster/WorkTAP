using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;
using Microsoft.EntityFrameworkCore;
using WorkTAP.Services;
using Microsoft.Extensions.Logging;

namespace WorkTAP.Controllers   
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase 
    {
        private IWorkTAPService WorkTAPService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IWorkTAPService workTAPService, ILogger<PersonsController> logger)
        {
            _logger = logger;
            WorkTAPService = workTAPService;
        }
        // GET: api/v1/persons Получение всех сотрудниковa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {   
            //Чтобы убрать лишнее поле Result
            return (ActionResult<IEnumerable<Person>>)Ok(await WorkTAPService.Get()).Value;
        }

        // GET: api/v1/person/id Получить конкретного сотрудника
        [Route("~/api/v1/person/{id?}")]
        [HttpGet]
        public async Task<ActionResult<Person>> Get(int id)
        {
            try
            {
                return (ActionResult<Person>)Ok(await WorkTAPService.Get(id)).Value;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                return NotFound("Пользователя с таким id не существует");
            }
        }

        // PUT: api/v1/person/id Обновление данных конкретного сотрудника
        [Route("~/api/v1/person")]
        [HttpPut]
        public async Task<ActionResult<Person>> Put(Person updatedPerson)
        {
            try
            {
                return (ActionResult<Person>)Ok(await WorkTAPService.Update(updatedPerson)).Value;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                return NotFound("Сущность не найдена");
            }
        }

        // POST: api/v1/person Добавление нового сотрудника
        [Route("~/api/v1/person")]
        [HttpPost]
        public async Task<ActionResult<Person>> Post(Person person)
        {
            return (ActionResult<Person>)Ok(await WorkTAPService.Create(person)).Value;
        }

        // DELETE: api/v1/person/id  Удаление существующего сотрудника
        [Route("~/api/v1/person/{id?}")]
        [HttpDelete]
        public async Task<ActionResult<Person>> Delete(int id)
        {
            try
            {
                return (ActionResult<Person>)Ok(await WorkTAPService.Delete(id)).Value;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                return NotFound("Пользователя с данным id не существует");
            }
        }
    }
}
