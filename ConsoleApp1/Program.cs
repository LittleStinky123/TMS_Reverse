using System;
using System.IO;

public class Example
{
    public static void Main(string[] args)
    {
        // Ranges of offsets
        //0 is copying data 1:1 from input to the output file
        //1 reverses every 2 bytes
        //2 reverses every 4 bytes
        //3 reverses every 8 bytes
        int[,] array2D = new int[,] { 
            { 0x00000, 0x0000F, 1}, {0x00010, 0x00017, 3}, {0x00018, 0x0009F, 2}, {0x000A0, 0x0209F, 0}, {0x020A0, 0x020A5, 1}, {0x020A6, 0x020FF, 2}, 
             {0x02100, 0x34EFF, 0}, {0x34F00, 0x3541F, 2}, {0x35420, 0x4191F, 1}, {0x41920, 0x44CDF, 2}, {0x44CE0, 0x450DF, 1}, 
             {0x450E0, 0x4519F, 2}, {0x451A0, 0x451BF, 0}, {0x451C0, 0x4529F, 2}, {0x452A0, 0x452BF, 0}, {0x452C0, 0x4539F, 2},
             {0x453A0, 0x453BF, 0}, {0x453C0, 0x4549F, 2}, {0x454A0, 0x454BF, 0}, {0x454C0, 0x4559F, 2}, {0x455A0, 0x455BF, 0}, 
             {0x455C0, 0x4569F, 2}, {0x456A0, 0x456BF, 0}, {0x456C0, 0x4579F, 2}, {0x457A0, 0x457BF, 0}, {0x457C0, 0x4589F, 2}, 
             {0x458A0, 0x459BF, 0}, {0x459C0, 0x45A9F, 2}, {0x45AA0, 0x45ABF, 0}, {0x45AC0, 0x45B9F, 2}, {0x45BA0, 0x45BBF, 0}, 
             {0x45BC0, 0x45C9F, 2}, {0x45CA0, 0x45CBF, 0}, {0x45CC0, 0x45D9F, 2}, {0x45DA0, 0x45DBF, 0}, {0x45DC0, 0x45E9F, 2}, 
             {0x45EA0, 0x45EBF, 0}, {0x45EC0, 0x45F9F, 2}, {0x45FA0, 0x45FBF, 0}, {0x45FC0, 0x4609F, 2}, {0x460A0, 0x460BF, 0}, 
             {0x460C0, 0x4619F, 2}, {0x461A0, 0x461BF, 0}, {0x461C0, 0x4629F, 2}, {0x462A0, 0x462CF, 0 },{0x462D0, 0x4A06F, 2 },
             {0x4A070, 0x51DAF, 1}, {0x51DB0, 0x520BF, 2}, {0x520C0, 0x535BF, 0}};
        byte[] bytesOrig;
        if (args.Length > 0 && File.Exists(args[0]))
            bytesOrig = System.IO.File.ReadAllBytes(args[0]);
        else
            bytesOrig = System.IO.File.ReadAllBytes("L:/save_2_wiiu.bin");
        // Offset after which endian is swapped?
        //int pos = 0x535BF;
        FileStream file = File.Create(Path.GetFileNameWithoutExtension(args[0]) + "_reversed.bin");
        byte[] twobyteswap = new byte[2];
        byte[] fourbyteswap = new byte[4];
        byte[] eightbyteswap = new byte[8];
        byte[] bytescopy = new byte[2];
        int start, end = 0;
        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            Console.WriteLine(i);
            switch (array2D[i, 2])
            {
                //Copy bytes
                case 0:
                    Console.WriteLine(i);
                    Console.WriteLine((array2D[i, 1]- array2D[i, 1])%2);
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, bytescopy, 0, 2);
                        file.Write(bytescopy, 0, bytescopy.Length);
                        start += 2;
                    }
                    break;
                // Swap every 2 bytes
                case 1:
                    Console.WriteLine(i);
                    Console.WriteLine((array2D[i, 1] - array2D[i, 1]) % 2);
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, twobyteswap, 0, 2);
                        Array.Reverse(twobyteswap);
                        file.Write(twobyteswap, 0, twobyteswap.Length);
                        start += 2;
                    }
                    break;
                // Swap every 4 bytes
                case 2:
                    Console.WriteLine(i);
                    Console.WriteLine((array2D[i, 1] - array2D[i, 1]) % 4);
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, fourbyteswap, 0, 4);
                        Array.Reverse(fourbyteswap);
                        file.Write(fourbyteswap, 0, fourbyteswap.Length);
                        start += 4;
                    }
                    break;
                // Swap every 8 bytes
                case 3:
                    Console.WriteLine(i);
                    Console.WriteLine((array2D[i, 1] - array2D[i, 1]) % 8);
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, eightbyteswap, 0, 8);
                        Array.Reverse(eightbyteswap);
                        file.Write(eightbyteswap, 0, eightbyteswap.Length);
                        start += 4;
                    }
                    break;
                default:
                    break;
            }
        }
        file.Close();
    }
    }