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
        public static void PrintMemory(StreamWriter MemoryFile, LinkedList<Mem> MemoryMap, int LoadAddress, int ExecutionAddress)
        {
            int MemoryIndex = MemoryMap.First.Value.Location;
            int ValueCount = 16;
            Console.Write("\n       0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F");
            MemoryFile.Write("\n       0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F");
            for (LinkedListNode<Mem>? CurrentNode = MemoryMap.First; CurrentNode != null; CurrentNode = CurrentNode.Next)
            {
                if (ValueCount == 16)
                {
                    ValueCount = 0;
                    string MemoryBuffer = (LoadAddress + MemoryIndex).ToString("X");
                    while(MemoryBuffer.Length < 5)
                    {
                        MemoryBuffer = "0" + MemoryBuffer;
                    }
                    Console.Write("\n" + MemoryBuffer);
                    MemoryFile.Write("\n" + MemoryBuffer);

                    MemoryIndex += 16;
                }

                string CurrentContents = CurrentNode.Value.Contents;
                Console.Write(" " + CurrentContents);
                MemoryFile.Write(" " + CurrentContents);
                ValueCount++;
            }
            while (ValueCount < 16)
            {
                Console.Write(" XX");
                MemoryFile.Write(" XX");
                ValueCount++;
            }
            Console.WriteLine("\n\nExecution begins at " + (LoadAddress+ExecutionAddress).ToString("X"));
        }
        public static void ModifyNode(LinkedList<Mem> MemoryMap, int LocStart, string Contents)
        {
            LinkedListNode<Mem> CurrentNode = MemoryMap.First;
            //Console.WriteLine("Location = " + LocStart);
            //Console.WriteLine("Contents = " + Contents);
            while (CurrentNode != null)
            {
                //Console.WriteLine("Node Location = " + CurrentNode.Value.Location);
                //Console.WriteLine("Node Contents = " + CurrentNode.Value.Contents);
                if (CurrentNode.Value.Location == LocStart)
                {

                    CurrentNode.Value.Contents = Contents;
                    return;
                }
                CurrentNode = CurrentNode.Next;
            }

        }
        public static void Estab(LinkedList<CSect> CSectList,LinkedList<Symbol> SymTab)
        {
            Console.WriteLine("CSECT  SYMBOL ADDR   CSADDR    LDADDR  LENGTH");
            for (LinkedListNode<CSect>? CurrentNode = CSectList.First; CurrentNode != null; CurrentNode = CurrentNode.Next)
            {
                Console.WriteLine(CurrentNode.Value.Name + "               " + CurrentNode.Value.CSAddress.ToString("X") + "              " + CurrentNode.Value.ProgramLength.ToString("X"));
                for(LinkedListNode<Symbol>? SymbolNode = SymTab.First; SymbolNode != null; SymbolNode = SymbolNode.Next)
                {
                    if(CurrentNode.Value.Name.Equals(SymbolNode.Value.CSect))
                    {
                        Console.WriteLine("       " + SymbolNode.Value.Name + " " + SymbolNode.Value.Value.ToString("X") + "               " + SymbolNode.Value.LoadAddress.ToString("X"));
                    }
                }
            }

        }
        public static void pause()
        {
            Console.ReadKey(false);
        }

    }

}