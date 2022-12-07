using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkingOckwig;
internal class Program
{
    static void Main(string[] args)
    {
        string path =  "LinkingOckwig" + Path.DirectorySeparatorChar;
        string ProgramLabel = "";
        int ProgramCount = args.Length;
        int executionAddress = 0;
        int ProgramBuffer = 0, ProgLength = 0;
        int MemoryCounter = 0;
        string[] ProgramLines = { };
        string[] R_Records = { };
        int LoadAddress = 8560;//02170H
        LinkedList<Mem> MemoryMap = new LinkedList<Mem>();
        Queue<Mods> ModificationRecords = new Queue<Mods>();
        LinkedList<string> AllProgramLines = new LinkedList<string>();
        LinkedList<CSect> CSectList = new LinkedList<CSect>();
        LinkedList<Symbol> SymbolTable = new LinkedList<Symbol>();
        for (int i = 0; i < ProgramCount; i++)
        {
            ProgramLines = Methods.GetFileLines(path + args[i]);
            foreach (string line in ProgramLines)
            {
                AllProgramLines.AddLast(line);
            }

        }
        //All program lines are now in one linkedlist
        for (LinkedListNode<string> CurrentLine = AllProgramLines.First; CurrentLine != null; CurrentLine = CurrentLine.Next)
        {
            string line = CurrentLine.Value.Trim();
            line = line.Substring(1);
            // line = line.Trim();
            if (CurrentLine.Value.StartsWith('H'))
            {
                ProgramLabel = line.Substring(0, 6);
                line = line.Substring(6);
                string ProgramStart = line.Substring(0, 6);
                line = line.Substring(6);
                string ProgramLength = line.Substring(0, 6);
                ProgLength = Convert.ToInt32(ProgramLength, 16);
                int ProgStartAddress = Convert.ToInt32(ProgramStart, 16);

                CSectList.AddLast(new CSect(ProgramLabel, ProgStartAddress,ProgLength, ProgramBuffer));
                //MemoryCounter = MemoryCounter + 
                for (int i = 0; i < ProgLength; i++)
                {
                    MemoryMap.AddLast(new Mem(MemoryCounter, "UU"));
                    MemoryCounter++;
                }
                SymbolTable.AddLast(new Symbol(ProgramLabel, ProgramStart, ProgramLabel, ProgramBuffer));


            }
            else if (CurrentLine.Value.StartsWith('T'))
            {
                string stringLineStartAddress = line.Substring(0, 6);
                int LineStartAddress = Convert.ToInt32(stringLineStartAddress, 16);
                line = line.Substring(6);

                string InstructionLength = line.Substring(0, 2);
                line = line.Substring(2);
                int length = Convert.ToInt32(InstructionLength, 16);

                string contents;
                for (int i = 0; i < length; i++)
                {
                    contents = line.Substring(0, 2);
                    line = line.Substring(2);
                    Methods.ModifyNode(MemoryMap, LineStartAddress + i + ProgramBuffer, contents);
                }
            }
            else if (CurrentLine.Value.StartsWith('E'))
            {
                ProgramBuffer += ProgLength;
                if(line.Length == 6)
                    {
                        executionAddress = Convert.ToInt32(line, 16);
                    }
            }
            else if (CurrentLine.Value.StartsWith('R'))
            {


            }
            else if (CurrentLine.Value.StartsWith('D'))
            {
                while (line.Length > 0)
                {
                    SymbolTable.AddLast(new Symbol(line.Substring(0, 6), line.Substring(6, 6), ProgramLabel, ProgramBuffer));
                    line = line.Substring(12);
                }
            }
            else if (CurrentLine.Value.StartsWith('M'))
            {
                string stringLineStartAddress = line.Substring(0, 6);
                int LineStartAddress = Convert.ToInt32(stringLineStartAddress, 16);
                ModificationRecords.Enqueue(new Mods(line, ProgramBuffer));
            }
        }
        while(ModificationRecords.Count>0)
        {
            Mods Node =  ModificationRecords.Dequeue();
            string line = Node.line, originalContents = "";
            string stringLineStartAddress = line.Substring(0, 6);
            int LineStartAddress = Convert.ToInt32(stringLineStartAddress, 16);
            LineStartAddress += Node.ProgramBuffer;
            line = line.Substring(6);
            string bytesChangeS = line.Substring(0,2);
            line = line.Substring(2);
            int SymbolValue = 0;
            int halfBytes = Convert.ToInt32(bytesChangeS, 16);
            int counterBytes = halfBytes;
            line = line.Substring(1);
            while(line.Length < 6)
            {
                line = line + " ";
            }
            for(LinkedListNode<Symbol> SymNode = SymbolTable.First; SymNode !=null; SymNode = SymNode.Next)
            {
                if(line.Equals(SymNode.Value.Name))
                {
                    SymbolValue = SymNode.Value.LoadAddress;
                }
            }
            for(LinkedListNode<Mem> MemNode = MemoryMap.First; MemNode != null; MemNode = MemNode.Next)
                {
                    if(MemNode.Value.Location == LineStartAddress)
                    {
                        while(counterBytes>0)
                        {
                            originalContents+=MemNode.Value.Contents;
                            counterBytes-=2;
                            MemNode = MemNode.Next;
                        }
                    }
                }
            int NewContents = Convert.ToInt32(originalContents, 16);
            string ContentsForMemory = (NewContents + SymbolValue).ToString("X");
            while(ContentsForMemory.Length < halfBytes)
            {
                ContentsForMemory = "0" + ContentsForMemory;
            }
            int i = 0;
            while(ContentsForMemory.Length >= 2)
            {
                string contents = ContentsForMemory.Substring(0, 2);
                ContentsForMemory = ContentsForMemory.Substring(2);
                Console.WriteLine((LineStartAddress + i + Node.ProgramBuffer).ToString("X") + "Location");
                Console.WriteLine("Program Buffer: " + Node.ProgramBuffer.ToString("X") +"  Contents:" + contents);
                Methods.ModifyNode(MemoryMap, LineStartAddress + i, contents);
                i++;
            }
        }
        Methods.Estab(CSectList, SymbolTable);
        using (StreamWriter writer = new StreamWriter(path + "memory.txt"))
        {
            Methods.PrintMemory(writer, MemoryMap, LoadAddress, executionAddress);
        }
    }
}
