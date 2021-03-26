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
            person.Id = 0;//только ради того, если указывать в swagger любое id!=0, а так на фронте указать id при создании нельзя
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return person;
        }
        public async Task<ActionResult<Person>> Update(Person updatedPerson)
        {

            Person person = await db.Persons.FindAsync(updatedPerson.Id);


            db.Entry(person).CurrentValues.SetValues(updatedPerson);//Обновление всех данных кроме навыков

            var personSkills = person.Skills.ToList();//Навыки работника

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
            return (person);
        }
        public async Task<ActionResult<Person>> Delete(int id)
        {
            Person person = await db.Persons.FindAsync(id);
            if (person != null)
            {
                db.Persons.Remove(person);
                await db.SaveChangesAsync();
            }
            return person;
        }
        public bool PersonExists(int id)//проверка существования пользователя с заданным id
        {
            return db.Persons.Any(p => p.Id == id);
        }
    }
}
