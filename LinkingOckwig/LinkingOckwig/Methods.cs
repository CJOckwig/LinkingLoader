using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LinkingOckwig
{
    public class Methods
    {

        public static string[] GetFileLines(string FileInput)
        {
            string[] lines = { };
            try
            {
                lines = File.ReadAllLines(FileInput);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            return lines;
        }
        public static void PrintMemory(StreamWriter MemoryFile, LinkedList<Mem> MemoryMap)
        {
            int MemoryIndex = MemoryMap.First.Value.Location;
            int ValueCount = 16;
            Console.Write("          0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F");


            for (LinkedListNode<Mem>? CurrentNode = MemoryMap.First; CurrentNode != null; CurrentNode = CurrentNode.Next)
            {
                if (ValueCount == 16)
                {
                    ValueCount = 0;
                    //MemoryFile.Write(MemoryIndex);
                    Console.Write("\n" + MemoryIndex);
                    MemoryIndex += 16;
                }

                string CurrentContents = CurrentNode.Value.Contents;
                Console.Write(" " + CurrentContents);
                ValueCount++;
            }
            while (ValueCount <= 16)
            {
                Console.Write(" XX");
                ValueCount++;
            }
        }
        public static void ModifyNode(LinkedList<Mem> MemoryMap, int LocStart, string Contents)
        {
            LinkedListNode<Mem> CurrentNode = MemoryMap.First;
            while (CurrentNode != null)
            {
                if (CurrentNode.Value.Location == LocStart)
                {
                    CurrentNode.Value.Contents = Contents;
                    return;
                }
                CurrentNode = CurrentNode.Next;
            }

        }

    }

}