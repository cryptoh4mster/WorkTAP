using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTAP.Models;


namespace WorkTAP.Services
{
    public class WorkTAPService : IWorkTAPService
    {
        private PersonsContext db;
        public WorkTAPService(PersonsContext context)
        {
            db = context;
        }

        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return await db.Persons.ToListAsync();
        }
        public async Task<ActionResult<Person>> Get(int id)
        {
            return await db.Persons.FirstAsync(x => x.Id == id);
        }
        public async Task<ActionResult<Person>> Create(Person person)
        {
            //Только ради того, если указывать в swagger любое id!=0, а так на фронте указать id 
            //при создании нельзя(он генерируется автоматически).
            person.Id = 0;
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return person;
        }
        public async Task<ActionResult<Person>> Update(Person updatedPerson)
        {
            //Поиск существующего пользователя.
            Person person = await db.Persons.FindAsync(updatedPerson.Id);

            //Обновление всех данных кроме навыков.
            db.Entry(person).CurrentValues.SetValues(updatedPerson);

            //Навыки работника.
            var personSkills = person.Skills.ToList();

            foreach (var personSkill in personSkills)
            {
                //Ищем навыки которые были до изменений и остались после изменений.
                var skill = updatedPerson.Skills.SingleOrDefault(s => s.Name == personSkill.Name);
                if (skill != null)
                {
                    //Обновляем поле у навыка сотрудника.
                    db.Entry(personSkill).CurrentValues.SetValues(skill);
                }
                else
                {
                    //Удаляем, если навыка нет.
                    db.Remove(personSkill);
                }
            }

            //Добавляем новые навыки.
            foreach (var skill in updatedPerson.Skills)
            {
                if (personSkills.All(s => s.Name != skill.Name))
                {
                    person.Skills.Add(skill);
                }
            }
            await db.SaveChangesAsync();
            return (person);
        }
        public async Task<ActionResult<Person>> Delete(int id)
        {
            //Поиск существующего пользователя для удаления.
            Person person = await db.Persons.FindAsync(id);
            if (person != null)
            {
                db.Persons.Remove(person);
                await db.SaveChangesAsync();
            }
            return person;
        }
    }
}
