using System;
using System.Security.Cryptography.MD5;

public class CompareFiles
{

	public CompareFiles()
	{

    }

    public static Boolean FilesAreEqual(string pathFile1, string pathFile2)
    {
        var md5_1 = MD5.Create();
        var md5_2 = MD5.Create();
        var stream1 = File.OpenRead(pathFile1);
        var stream2 = File.OpenRead(pathFile2);
        var hash1 = BitConverter.ToString(md5_1.ComputeHash(stream1));
        var hash2 = BitConverter.ToString(md5_2.ComputeHash(stream2));

        if (hash1 == hash2)
        {
            return true;
        }
        else
        {
            return false;
        }
 
    }
}
