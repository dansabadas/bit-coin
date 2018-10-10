using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.IL
{
    class Person
    {
        public Person()
        {
            Name = "Danson";
        }

        public string Name { get; set; }

        public string Speak()
        {
            return $"My name is {Name}";
        }
    }
}
