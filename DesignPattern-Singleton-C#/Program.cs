using System;
using System.Threading;

namespace DesignPattern_Singleton
{


    /// <summary>
    /// Main program class that uses DatabaseConnection to simulate a database connection.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and start multiple threads to test thread safety
            Thread[] threads = new Thread[5];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    // Retrieve the Singleton instance
                    DatabaseConnection instance = DatabaseConnection.GetInstance();

                    // Unique key per thread using ManagedThreadId
                    int key = Thread.CurrentThread.ManagedThreadId;

                    // Save and retrieve data
                    instance.SaveData(key, $"Data from thread {key}");
                    Console.WriteLine(instance.GetData(key));
                });

                threads[i].Start();
            }

            // Wait for all threads to finish
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
