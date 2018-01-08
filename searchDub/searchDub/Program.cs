using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace searchDub
{
    class Program
    {
        static void Main(string[] args)
        {

            int depth = 0;
            bool printProcesTime = false;
            bool extendedOutput = false;
            bool waitBeforeTerminate = false;
            uint maxThreads = 0;
            List<string> path = new List<string>();
            List<string> filter = new List<string>();

            if (args.Length < 2)
            {
                printHelp();
                return ;
            }

            //handle input arguments
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    string argument = args[i];
                    if (argument == "-r")
                    {
                        try
                        {
                            depth = int.Parse(args[i + 1]);
                            if (depth < 0)
                            {
                                Console.WriteLine("Error: folder depth must be > 0");
                            }
                            i++;
                        }
                        catch (Exception e)
                        {
                            depth = -2;     //search in all subdirectories
                        }
                    }
                    else if (argument == "-f")
                    {
                        string[] temp = args[i + 1].Split(';');
                        filter.AddRange(temp);
                        i++;
                    }
                    else if (argument == "-t")
                    {
                        maxThreads = uint.Parse(args[i + 1]);
                        if (depth <= 0)
                        {
                            Console.WriteLine("Error: there must be at least 1 Thread");
                        }
                        i++;
                    }
                    else if (argument == "-s")
                    {
                        path.Add(args[i + 1]);
                        i++;
                    }
                    else if (argument == "-h")
                    {
                        printHelp();
                        break;
                    }
                    else if (argument == "-p")
                    {
                        printProcesTime = true;
                    }
                    else if (argument == "-v")
                    {
                        extendedOutput = true;
                    }
                    else if (argument == "-w")
                    {
                        waitBeforeTerminate = true;
                    }
                    else
                    {
                        if (i == (args.Length - 1))
                        {
                            path.Add(argument);
                        }
                        else
                        {
                            Console.WriteLine("wrong argument: " + argument);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: in interpreting arguments "+ args[i]);
                }

            }

            //if no filter is set add filter to all
            if (filter.Count == 0)
            {
                filter.Add("*");
            }

            Console.WriteLine("Started finding multibe files files...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //get all files and their size
            GetFiles getFiles = new GetFiles();
            ConcurrentDictionary<long, string> files = new ConcurrentDictionary<long, string>();
            foreach (string entry in path)
            {
                getFiles.getFilesFromDirectory(entry, depth, filter,files);
            }


            // get all files that have the same size
            ConcurrentDictionary<long, string> doubleFiles = new ConcurrentDictionary<long, string>();
            foreach (KeyValuePair<long, string> entry in files)
            {
                if (entry.Value.Contains(";"))
                {
                    doubleFiles.TryAdd(entry.Key, entry.Value);
                }
            }

            //TODO Compare files with byte by byte

            timer.Stop();
            if (extendedOutput)
            {
                Console.WriteLine("\nAnzahl gefundener Dateien mit unterschiedlicher Dateigröße: "+ files.Count());
                Console.WriteLine("Gefundene Dateien mit gleicher Dateigröße: " + doubleFiles.Count());
            }
            if (printProcesTime)
            {
                TimeSpan ts = timer.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:000}",ts.TotalSeconds, ts.Milliseconds );
                Console.WriteLine("\nRunTime: " + elapsedTime+" seconds\n");
            }
            if (waitBeforeTerminate)
            {
                Console.Write("Press any key to exit...");
                //Console.ReadLine();
                Console.ReadKey();
            }

        }

        private static void printHelp()
        {
            Console.WriteLine("cntFileBits.exe [-r[n]] [-f FileFilter] [-t maxThreads] -[h] [-v] -[w] [-p] [-s] path");
            Console.WriteLine("options:");
            Console.WriteLine("    -r[n]\tactivates the the recursion in folders with n a max depth can be set");
            Console.WriteLine("    -f FileFilter\tactivates a filter for files (e.g *.img)");
            Console.WriteLine("    -t maxThreads\tsets the max number of threads");
            Console.WriteLine("    -h \tprints help");
            Console.WriteLine("    -v \tactivate additional outputs");
            Console.WriteLine("    -w \twait before ending the programm for key press");
            Console.WriteLine("    -s path \tset one or more path to execute the programm");
            Console.WriteLine("    -p \tshow processing time");
        }
    }
}
