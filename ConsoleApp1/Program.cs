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
            {0x00000, 0x0000F, 1}, {0x00010, 0x00017, 3}, {0x00018, 0x0009F, 2}, {0x000A0, 0x0209F, 0}, {0x020A0, 0x020A7, 1}, 
            {0x020A8, 0x020FF, 2}, 
            // TODO Between 0x0 and 0x368B0 the wrong save file info lies
            // TODO Between 0x368B0 and 0x535BF the crash lies
            {0x02100, 0x34EFF, 0},
            {0x34F00, 0x350CF, 2}, {0x350D0, 0x3510F, 1 },
            //Characters
            {0x35110, 0x3512F, 2 },
            {0x35130, 0x3546F, 1 },
            {0x35470, 0x3548F, 2 },
            {0x35490, 0x357CF, 1 },
            {0x357D0, 0x357EF, 2 },
            {0x357F0, 0x35B2F, 1 },
            {0x35B30, 0x35B4F, 2 },
            {0x35B50, 0x35E8F, 1 },
            {0x35E90, 0x35EAF, 2 },
            {0x35EB0, 0x361EF, 1 },
            {0x361F0, 0x3620F, 2 },
            {0x36210, 0x3654F, 1 },
            {0x36550, 0x3656F, 2 },
            //Characters
            {0x36570, 0x368A3, 1 },
            {0x368A4, 0x368A7, 2},
            {0x368A8, 0x3E457, 1 },{0x3E458, 0x3E45B, 2 },
            // TODO region somewhere here that is copied, for stuff like levels
            {0x3E45C, 0x3E665, 1},
            {0x3E666, 0x3E6FB, 1},
            //0003EA07 - 0003EC01 maybe 1 byte vars as well?
            {0x3E6FC, 0x3EC5B, 1},{0x3EC5C, 0x3ED5B, 0},
            {0x3ED5C, 0x4191F, 1}, {0x41920, 0x44CDF, 2}, 
            {0x44CE0, 0x450DF, 1}, {0x450E0, 0x4519F, 2}, {0x451A0, 0x451BF, 0}, {0x451C0, 0x4529F, 2}, {0x452A0, 0x452BF, 0}, 
            {0x452C0, 0x4539F, 2}, {0x453A0, 0x453BF, 0}, {0x453C0, 0x4549F, 2}, {0x454A0, 0x454BF, 0}, {0x454C0, 0x4559F, 2}, 
            {0x455A0, 0x455BF, 0}, {0x455C0, 0x4569F, 2}, {0x456A0, 0x456BF, 0}, {0x456C0, 0x4579F, 2}, {0x457A0, 0x457BF, 0}, 
            {0x457C0, 0x4589F, 2}, {0x458A0, 0x459BF, 0}, {0x459C0, 0x45A9F, 2}, {0x45AA0, 0x45ABF, 0}, {0x45AC0, 0x45B9F, 2},
            {0x45BA0, 0x45BBF, 0}, {0x45BC0, 0x45C9F, 2}, {0x45CA0, 0x45CBF, 0}, {0x45CC0, 0x45D9F, 2}, {0x45DA0, 0x45DBF, 0}, 
            {0x45DC0, 0x45E9F, 2}, {0x45EA0, 0x45EBF, 0}, {0x45EC0, 0x45F9F, 2}, {0x45FA0, 0x45FBF, 0}, {0x45FC0, 0x4609F, 2}, 
            {0x460A0, 0x460BF, 0}, {0x460C0, 0x4619F, 2}, {0x461A0, 0x461BF, 0}, {0x461C0, 0x4629F, 2}, {0x462A0, 0x462CF, 0 },
            {0x462D0, 0x4A06F, 2 },{0x4A070, 0x51DAF, 1}, {0x51DB0, 0x520BF, 2}, {0x520C0, 0x535BF, 0}};
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
        int todo = 0;
        // Check continuity
        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            if (i> 0 && array2D[i, 0] != (array2D[i-1, 1]+1))
                Console.WriteLine("Bad region!" + i);
        }
            for (int i = 0; i < array2D.GetLength(0); i++)
        {
            Console.WriteLine(i);
            Console.WriteLine(array2D[i, 0]);
            Console.WriteLine(array2D[i, 1]);
            switch (array2D[i, 2])
            {
                //Copy bytes
                case 0:
                    Console.WriteLine(i);
                    todo = array2D[i, 1] - array2D[i, 1] + 1;
                    if (((array2D[i, 1] - array2D[i, 0] + 1) % 2) > 0)
                        Console.WriteLine("Bad region!");
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, bytescopy, 0, 2);
                        file.Write(bytescopy, 0, bytescopy.Length);
                        start += 2;
                        todo -= 2;
                    }
                    if(todo>0)
                        Console.WriteLine("Bad region!");
                    break;
                // Swap every 2 bytes
                case 1:
                    Console.WriteLine(i);
                    todo = array2D[i, 1] - array2D[i, 1] + 1;
                    if (((array2D[i, 1] - array2D[i, 0] + 1) % 2) > 0)
                        Console.WriteLine("Bad region!");
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, twobyteswap, 0, 2);
                        Array.Reverse(twobyteswap);
                        file.Write(twobyteswap, 0, twobyteswap.Length);
                        start += 2;
                        todo -= 2;
                    }
                    if (todo > 0)
                        Console.WriteLine("Bad region!");
                    break;
                // Swap every 4 bytes
                case 2:
                    Console.WriteLine(i);
                    todo = array2D[i, 1] - array2D[i, 1] + 1;
                    if (((array2D[i, 1] - array2D[i, 0] + 1) % 4) > 0)
                        Console.WriteLine("Bad region!");
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, fourbyteswap, 0, 4);
                        Array.Reverse(fourbyteswap);
                        file.Write(fourbyteswap, 0, fourbyteswap.Length);
                        start += 4;
                        todo -= 4;
                    }
                    if (todo > 0)
                        Console.WriteLine("Bad region!");
                    break;
                // Swap every 8 bytes
                case 3:
                    Console.WriteLine(i);
                    todo = array2D[i, 1] - array2D[i, 1] + 1;
                    if (((array2D[i, 1] - array2D[i, 0] + 1) % 8) > 0)
                        Console.WriteLine("Bad region!");
                    start = array2D[i, 0];
                    end = array2D[i, 1];
                    while (start < end)
                    {
                        Array.Copy(bytesOrig, start, eightbyteswap, 0, 8);
                        Array.Reverse(eightbyteswap);
                        file.Write(eightbyteswap, 0, eightbyteswap.Length);
                        start += 8;
                        todo -= 8;
                    }
                    if (todo > 0)
                        Console.WriteLine("Bad region!");
                    break;
                default:
                    break;
            }
        }
        file.Close();
    }
    }