using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace searchDub
{
    class GetFiles
    {
        public void getFilesFromDirectory(string path, int depth,List<string> allFilters, ConcurrentDictionary<long,string> files)
        {
            try
            {
                if (depth == 0)
                {
                    getFiles(path, allFilters, files, SearchOption.TopDirectoryOnly);
                }
                else if (depth == -2)
                {
                    getFiles(path, allFilters, files, SearchOption.AllDirectories);
                }
                else
                {
                    getFilesFromDirectoryMaxDepth(path, depth, allFilters, files);
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private void getFilesFromDirectoryMaxDepth(string path, int depth, List<string> allFilters, ConcurrentDictionary<long, string> files)
        {
            try
            {
                if (depth < 0)
                {
                    return;
                }

                getFiles(path, allFilters, files, SearchOption.TopDirectoryOnly);

                var directories = GetDirectories(path);
                foreach(string direct in directories)
                {
                    getFilesFromDirectoryMaxDepth(direct, depth-1, allFilters, files);
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private void getFiles(string path, List<string> allFilters, ConcurrentDictionary<long, string> files, SearchOption options)
        {
            foreach (string filter in allFilters)
            {
                var filesEnum = EnumerateFiles(path, filter, options);
                foreach (string currentFile in filesEnum)
                {
                    try
                    {
                        long length = new FileInfo(currentFile).Length;
                        if (length > 0 && !files.TryAdd(length, currentFile))
                        {
                            string temp;
                            if (files.TryGetValue(length, out temp))
                            {
                                if (!temp.Contains(currentFile))
                                {
                                    files.TryUpdate(length, temp + ";" + currentFile, temp);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message + "\n->of File: " + currentFile);
                    }

                }
            }
        }

        private static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOpt)
        {
            try
            {
                var dirFiles = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories)
                {
                    dirFiles = Directory.EnumerateDirectories(path)
                                        .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
                }
                return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<string>();
            }
            catch (PathTooLongException)
            {
                return Enumerable.Empty<string>();
            }
            catch (IOException)
            {
                return Enumerable.Empty<string>();
            }
        }

        public static List<string> GetDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
            catch (PathTooLongException)
            {
                return new List<string>();
            }
            catch (IOException)
            {
                return new List<string>();
            }
        }
    }
}
