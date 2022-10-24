using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelExample
{
    class Program
    {
        static void Main(string[] args)
        {

            Parallel.For(1, 10, p =>
            {
                Console.WriteLine(p);
            });

            List<int> numberlist1 = Enumerable.Range(0, 100).ToList();
            Parallel.ForEach(numberlist1, item =>
            {
                Console.WriteLine(item);
            });

            List<int> numberlist = Enumerable.Range(0, 100).ToList();

            var option = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };

            Parallel.ForEach(numberlist, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount -1  }, item =>
            {
                Console.WriteLine(item);
            });


            List<Action> taskList = new List<Action>();
            foreach (var item in numberlist)
            {
                taskList.Add(() => ProcessStudent(item));
            }
            var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            Parallel.Invoke(options, taskList.ToArray());

            Parallel.Invoke(
                Method1, Method2, Method3
           );
        }

        private static void ProcessStudent(int item)
        {
            Thread threadToKill = null;
            int timeOutMinute = 5;
            Action action = () =>
            {
                try
                {
                    threadToKill = Thread.CurrentThread;
                }
                catch (ThreadAbortException ex)
                {
                    //Zaman aşımı sonrası düşeceği yer.
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            IAsyncResult result = action.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeOutMinute * 60 * 1000))
            {
                action.EndInvoke(result); //Normal bitti
            }
            else
            {
                threadToKill.Abort(); //Zaman aşımına uğradı
            }
        }

        static void Method1()
        {
            Task.Delay(200);
            Console.WriteLine($"Method 1 Completed");
        }
        static void Method2()
        {
            Task.Delay(200);
            Console.WriteLine($"Method 2 Completed");
        }
        static void Method3()
        {
            Task.Delay(200);
            Console.WriteLine($"Method 3 Completed");
        }
    }

}
