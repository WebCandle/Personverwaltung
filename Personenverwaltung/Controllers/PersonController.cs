using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Personenverwaltung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public MyDbContext Db { get; set; }
        public PersonController()
        {
            Db = new MyDbContext();
        }
        // GET: api/<PersonController>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return Db.Persons.ToList();
        }

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = Db.Persons.Where(x => x.PersonId == id);
            if(person == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(person);
            }
        }
        
        // POST api/<PersonController>
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if(person == null)
            {
                return this.BadRequest();
            }
            else
            {
                Db.Persons.Add(person);
                int state = Db.SaveChanges();
                return Ok(state);
            }
        }

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Person person_new)
        {
            Person person = Db.Persons.Where(x => x.PersonId == id).First();
            if(person == null)
            {
                return NotFound();
            }
            else
            {
                person.Name = person_new.Name;
                person.Vorname = person_new.Vorname;
                person.Geburtsdatum = person_new.Geburtsdatum;
                int state = Db.SaveChanges();
                return Ok(state);
            }
        }

        // DELETE api/<PersonController>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Person person = Db.Persons.Where(x => x.PersonId == id).First();
            if (person == null)
            {
                return NotFound();
            }
            else
            {
                Db.Persons.Remove(person);
                int state = Db.SaveChanges();
                return Ok(state);
            }
        }
    }
}
