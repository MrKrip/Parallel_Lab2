using Akka.Actor;
using System;
using System.Threading;

namespace Lab2
{
    class Program
    {

        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("actor-system");
            IActorRef actor = actorSystem.ActorOf(Props.Create<FastFoodRestaurant>(3,"FastFood"));
            for (int i=0;i<10;i++)
            {
                string Command = $"I am hungry:Visitor{i}";
                actor.Tell(Command);
            }
            Thread.Sleep(500);
            actor.Tell("Queue of visitors:");
            for (int i = 0; i < 10; i++)
            {
                string Command = $"I order:Visitor{i}";
                actor.Tell(Command);
                Thread.Sleep(500);
            }            
            actor.Tell("Queue of visitors:");
            Console.ReadLine();
        }
    }
}
