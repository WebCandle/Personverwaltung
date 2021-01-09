using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personenverwaltung
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
    }
}
