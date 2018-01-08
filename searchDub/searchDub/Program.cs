using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            for (int i = 1; i < args.Length; i++)
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
                        filter.Add(args[i + 1]);
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
