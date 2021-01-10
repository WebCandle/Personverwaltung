using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace WebUI.Controllers
{
    public class PersonController : Controller
    {
        public string WebAPIHost { get; set; }
        public List<Person> Persons { get; set; }
        public PersonController()
        {
            WebAPIHost = "https://localhost:44357/";
            loadPersons();
        }
        public void loadPersons()
        {
            string URL = WebAPIHost+ "api/person";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                Persons = JsonConvert.DeserializeObject<List<Person>>(reader.ReadToEnd());
            }
        }
        // GET: PersonController
        public ActionResult Index()
        {
            return View(Persons);
        }

        // GET: PersonController/Details/5
        public ActionResult Details(int id)
        {
            Person person = Persons.Where(x => x.PersonId == id).FirstOrDefault();
            return View(person);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            Person person = new Person();
            person.PersonId = 0;
            person.Name = collection["Name"];
            person.Vorname = collection["Vorname"];
            person.Geburtsdatum = Convert.ToDateTime(collection["Geburtsdatum"]);
            try
            {
                string URL = WebAPIHost + "api/person";
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(person);
                    streamWriter.Write(json);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(person);
                    }
                }
            }
            catch
            {
                return View(person);
            }
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id)
        {
            Person person = Persons.Where(x => x.PersonId == id).FirstOrDefault();
            return View(person);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Person person = new Person();
                person.PersonId = id;
                person.Name = collection["Name"];
                person.Vorname = collection["Vorname"];
                person.Geburtsdatum = Convert.ToDateTime(collection["Geburtsdatum"]);

                string URL = WebAPIHost + "api/person/"+id.ToString();
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json";
                request.Method = "PUT";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(person);
                    streamWriter.Write(json);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(person);
                    }
                }
            }
            catch
            {
                return View(Persons.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id)
        {
            Person person = Persons.Where(x => x.PersonId == id).FirstOrDefault();
            return View(person);
        }

        // POST: PersonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                string URL = WebAPIHost + "api/person/"+id.ToString();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "DELETE";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    string state = reader.ReadToEnd();
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(Persons.Where(x=> x.PersonId == id).FirstOrDefault());
                    }
                }
            }
            catch
            {
                return View(Persons.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }
    }
}
