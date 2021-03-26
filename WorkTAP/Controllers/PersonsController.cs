using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;
using Microsoft.EntityFrameworkCore;
using WorkTAP.Services;

namespace WorkTAP.Controllers   
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase 
    {
        IWorkTAPService WorkTAPService;

        public PersonsController(IWorkTAPService workTAPService)
        {
            WorkTAPService = workTAPService;
        }
        /// GET: api/v1/persons Получение всех сотрудниковa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return Ok(await WorkTAPService.Get());
        }

        /// GET: api/v1/person/id Получить конкретного сотрудника
        [Route("~/api/v1/person/{id?}")]
        [HttpGet]
        public async Task<ActionResult<Person>> Get(int id)
        {
            try
            {
                return Ok(await WorkTAPService.Get(id));
            }
            catch
            {
                return NotFound("Пользователя с таким id не существует");
            }
        }

        /// PUT: api/v1/person/id Обновление данных конкретного сотрудника
        [Route("~/api/v1/person")]
        [HttpPut]
        public async Task<ActionResult<Person>> Put(Person updatedPerson)
        {
            /*try
            {*/
            try
            {
                return Ok(await WorkTAPService.Update(updatedPerson));
            }
            catch
            {
                return NotFound("Сущность не найдена");
            }
        }

        /// POST: api/v1/person Добавление нового сотрудника
        [Route("~/api/v1/person")]
        [HttpPost]
        public async Task<ActionResult<Person>> Post(Person person)
        {
            return Ok(await WorkTAPService.Create(person));
        }

        /// DELETE: api/v1/person/id  Удаление существующего сотрудника
        [Route("~/api/v1/person/{id?}")]
        [HttpDelete]
        public async Task<ActionResult<Person>> Delete(int id)
        {
            try
            {
                return Ok(await WorkTAPService.Delete(id));
            }
            catch
            {
                return NotFound("Пользователя с данным id не существует");
            }
        }
    }
}
