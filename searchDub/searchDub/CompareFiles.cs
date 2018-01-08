using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

public class CompareFiles
{

	public CompareFiles()
	{

    }

    //public static Boolean FilesAreEqual(string pathFile1, string pathFile2)
    //{
    //    var md5_1 = MD5.Create();
    //    var md5_2 = MD5.Create();
    //    var stream1 = File.OpenRead(pathFile1);
    //    var stream2 = File.OpenRead(pathFile2);
    //    var hash1 = BitConverter.ToString(md5_1.ComputeHash(stream1));
    //    var hash2 = BitConverter.ToString(md5_2.ComputeHash(stream2));

    //    return hash1 == hash2;
    //}

    public void compareFileTest(string data, List<string> files)
    {
        string[] splitedData = data.Split(';');
        string[] hashedData = new string[splitedData.Length];
        for (int i =0;i< splitedData.Length;i++)
        {
            try
            {
                var md5 = MD5.Create();
                var stream = File.OpenRead(splitedData[i]);
                hashedData[i] = BitConverter.ToString(md5.ComputeHash(stream));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message + "\n->of File: " + splitedData[i]);
            }

        }
        var distHasedData = hashedData.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);
        foreach (string hash in distHasedData)
        {
            int[] indexes = Helper.FindAllIndexof(hashedData, hash);
            string tempFiles="";
            foreach(int pathIndex in indexes)
            {
                tempFiles = tempFiles +";"+ splitedData[pathIndex];
            }
            files.Add(tempFiles);
        }
    }
}
