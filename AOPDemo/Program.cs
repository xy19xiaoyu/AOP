using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskFactory tf = new TaskFactory();


            Task[] tasks = new Task[10];



            for (int i = 0; i < 10; i++)
            {

                tasks[i] = tf.StartNew((o) =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Calc1 c = new Calc1();
                    var index = int.Parse(o.ToString());
                    if (index % 2 == 1)
                    {
                        c.add(0, 1);
                    }
                    else
                    {
                        c.diff(1, 2);
                    }

                    sw.Stop();

                    Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
                }, i);

            }

            Task.WaitAll(tasks);

            Console.Read();

            List<Action> actions = new List<Action>();
            actions.Add((Action)delegate ()
            {
                Console.WriteLine("action1");
            });
            actions.Add((Action)delegate ()
            {
                Console.WriteLine("action2");
            });
            ExecAction(actions);
        }

        static void ExecAction(List<Action> actions)
        {
            foreach (var action in actions)
            {
                action.Invoke();
            }
        }
    }
}
