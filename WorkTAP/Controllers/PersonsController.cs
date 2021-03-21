using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkTAP.Controllers   
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        PersonsContext db;
        public PersonsController(PersonsContext context)
        {
            db = context;
        }

        // GET: api/v1/persons Получение всех сотрудников
        [Route("~/api/v1/persons")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return Ok(await db.Persons.ToListAsync());
        }

        // GET: api/v1/person/id Получить конкретного сотрудника
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            try
            {
                Person person = await db.Persons.FirstAsync(x => x.Id == id);
                return Ok(person);
            }
            catch
            {
                return NotFound("Пользователя с таким id не существует");
            }
        }

        // PUT: api/v1/person/id Обновление данных конкретного сотрудника
        [HttpPut]
        public async Task<ActionResult<Person>> Put(int id, Person updatedPerson)
        {   
            try
            {
                var person = await db.Persons.FindAsync(id);

                db.Entry(person).CurrentValues.SetValues(updatedPerson);
                var personSkills = person.Skills.ToList();

                foreach (var personSkill in personSkills)
                {
                    var skill = updatedPerson.Skills.SingleOrDefault(s => s.Name == personSkill.Name);//ищем навыки которые были до изменений и остались после изменений
                    if (skill != null)
                    {
                        //обновляем поле у навыка сотрудника
                        db.Entry(personSkill).CurrentValues.SetValues(skill);
                    }
                    else
                    {
                        //удаляем, если навыка нет
                        db.Remove(personSkill);
                    }
                }
                //добавляем новые навыки
                foreach (var skill in updatedPerson.Skills)
                {
                    if (personSkills.All(s => s.Name != skill.Name))
                    {
                        person.Skills.Add(skill);
                    }
                }
                await db.SaveChangesAsync();
                return Ok(person);
            }
            catch 
            {
                if (id != updatedPerson.Id)
                {
                    return BadRequest("Неверный запрос");
                }
                else if (!PersonExists(id))
                {
                    return NotFound("Сущность не найдена");
                }
                else
                    return StatusCode(500);
            }
        }

        // POST: api/v1/person Добавление нового сотрудника
        [HttpPost]
        public async Task<ActionResult<Person>> Post(Person person)
        {
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return Ok(person);
        }

        // DELETE: api/v1/person/id  Удаление существующего сотрудника
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> Delete(int id)
        {
            try
            {
                var person = await db.Persons.FindAsync(id);
                db.Persons.Remove(person);
                await db.SaveChangesAsync();
                return Ok(person);
            }
            catch
            {
                return NotFound("Пользователя с данным id не существует");
            }
        }

        private bool PersonExists(int id)//проверка существования пользователя с заданным id
        {
            return db.Persons.Any(p => p.Id == id);
        }

    }
}
