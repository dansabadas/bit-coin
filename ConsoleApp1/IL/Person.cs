using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.IL
{
    class Person
    {
        public string Name { get; set; }

        public string Speak()
        {
            return $"My name is {Name}";
        }
    }
}
