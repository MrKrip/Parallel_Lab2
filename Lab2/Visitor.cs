using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Visitor : UntypedActor
    {
        private string name;
        public Visitor(string name)
        {
            this.name = name;

        }
        public void Send(string message)
        {
            Console.WriteLine($"Check: {message}");
        }

        protected override void OnReceive(object message)
        {
            Console.WriteLine($"Comand: {message} -- {name}");
        }
        protected override void PreStart() => Console.WriteLine($"{name} came");

        protected override void PostStop() => Console.WriteLine($"{name} is gone");
    }
}
