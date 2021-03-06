﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace DBFiller
{
    public static class DBSpammer
    {
        private static Mutex mutex = new Mutex();
        private static Queue<string> queue = new Queue<string>();
        private static bool entriesLeft = true;

        private static string getFromQueue()
        {
            mutex.WaitOne();

            string ret = null;

            try
            {
                if (queue.Count > 0)
                    ret = queue.Dequeue();

                mutex.ReleaseMutex();
            }
            catch(Exception)
            {
                mutex.ReleaseMutex();
            }

            return ret;
        }

        public static void setToQueue(string input)
        {
            mutex.WaitOne();
            
            try
            {
                queue.Enqueue(input);

                mutex.ReleaseMutex();
            }
            catch (Exception)
            {
                mutex.ReleaseMutex();
            }
        }

        private static void spammer()
        {
            NpgsqlConnection connection = new NpgsqlConnection(Master.connection);
            connection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("", connection);
            string s = "";

            while (entriesLeft || queue.Count > 0)
            {
                s = getFromQueue();

                if (s != null)
                {
                    cmd.CommandText = s;

                    redo:

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        goto redo;
                    }
                }
                else
                    Thread.Sleep(1);
            }
        }

        static Thread[] threads;

        public static async void spamDB(int count)
        {
            threads = new Thread[count];

            for (int i = 0; i < count; i++)
            {
                threads[i] = new Thread(new ThreadStart(spammer));
                threads[i].Start();
            }
        }

        public static void waitForFinished()
        {
            Console.WriteLine("\nWaitung for the threads to finish...\n");

            for (int i = 0; i < threads.Length; i++)
            {
                while (threads[i].ThreadState != ThreadState.Stopped)
                {
                    Console.WriteLine("Waiting for threads to finish... (" + queue.Count + " Entries left)");
                    Thread.Sleep(500);
                }
            }
        }

        public static void setNoEntriesLeft()
        {
            entriesLeft = false; 
        }
    }
}
