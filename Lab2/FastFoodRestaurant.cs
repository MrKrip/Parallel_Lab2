using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    class FastFoodRestaurant : UntypedActor
    {
        private List<IActorRef>[] Queues;
        private string name;

        public FastFoodRestaurant(int QueuesCount, string name)
        {
            this.name = name;
            Queues = new List<IActorRef>[QueuesCount];
            for (int i = 0; i < QueuesCount; i++)
            {
                Queues[i] = new List<IActorRef>();
            }
        }

        protected override void OnReceive(object message)
        {
            var Message = message.ToString().Split(':');
            switch (Message[0])
            {
                case "I am hungry":
                    NewVisitor(Message[1]);
                    break;
                case "I order":
                    VisitorOrders(Message[1]);
                    break;
                case "Queue of visitors":
                    QueueOfVisitors();
                    break;
            }

        }

        private void QueueOfVisitors()
        {
            Console.WriteLine(new string('-', 40));
            for (int i = 0; i < Queues.Length; i++)
            {
                if (Queues[i] != null)
                {
                    Console.WriteLine($"Queue {i} ");
                    foreach (var visitor in Queues[i])
                    {
                        Console.Write($"->{visitor.Path.Name}");
                    }
                    Console.WriteLine();
                }

            }
            Console.WriteLine(new string('-', 40));
        }

        private void VisitorOrders(string Visitor)
        {
            int index = -1;
            for (int i = 0; i < Queues.Length; i++)
            {
                if (Queues[i].Count>0 && Queues[i].First().Path.Name == Visitor)
                {
                    index = i;
                }
            }
            if (index < 0)
            {
                Console.WriteLine($"The visitor [{Visitor}] cannot make an order because he is not at the head of the line");
                return;
            }
            Thread.Sleep(500);
            Context.Stop(Queues[index].First());
            Queues[index].Remove(Queues[index].First());
            int from = 0;
            int to = 0;
            for (int i = 0; i < Queues.Length - 1; i++)
            {                
                if (Queues[i].Count > Queues[i + 1].Count)
                {
                    to = i + 1;
                }
                if (Queues[i].Count < Queues[i + 1].Count)
                {
                    from = i + 1;
                }
            }
            if (from != to)
            {
                var TempVisitor = Queues[from].Last();
                Queues[to].Add(TempVisitor);
                Queues[from].Remove(TempVisitor);
            }
        }

        private void NewVisitor(string Visitor)
        {
            int minindex = 0;

            for (int i = 0; i < Queues.Length - 1; i++)
            {
                if (Queues[i].Count > Queues[i + 1].Count)
                {
                    minindex = i + 1;
                }
            }
            Queues[minindex].Add(Context.ActorOf(Props.Create<Visitor>(Visitor), Visitor));
            Queues[minindex].Last().Tell("I am hungry");
        }

        protected override void PreStart() => Console.WriteLine($"{name} started");

        protected override void PostStop() => Console.WriteLine($"{name} stopped");
    }
}
