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
        string path = "";// "LinkingOckwig" + Path.DirectorySeparatorChar;
        int ProgramCount = args.Length;
        int MemoryCounter = 0;
        string[] ProgramLines = { };
        string[] R_Records = { };
        int LoadAddress = 8560;//02170H
        LinkedList<Mem> MemoryMap = new LinkedList<Mem>();
        LinkedList<string> AllProgramLines = new LinkedList<string>();
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
            line = line.Trim();
            if (CurrentLine.Value.StartsWith('H'))
            {
                string ProgramLabel = line.Substring(0, 6);
                line = line.Substring(6);
                string ProgramStart = line.Substring(0, 6);
                line = line.Substring(6);
                string ProgramLength = line.Substring(0, 6);
                int ProgLength = Convert.ToInt32(ProgramLength, 16);
                int ProgStartAddress = Convert.ToInt32(ProgramStart, 16);


                //MemoryCounter = MemoryCounter + 
                for (int i = 0; i < ProgLength; i++)
                {
                    MemoryMap.AddLast(new Mem(MemoryCounter, "UU"));
                }

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
                    Methods.ModifyNode(MemoryMap, LineStartAddress + i, contents);
                }
            }
            else if (CurrentLine.Value.StartsWith('E'))
            {

            }
            else if (CurrentLine.Value.StartsWith('R'))
            {
                //External Symbols

            }
            else if (CurrentLine.Value.StartsWith('D'))
            {
                //D Records
                while (line.Length > 0)
                {
                    SymbolTable.AddLast(new Symbol(line.Substring(0, 6), line.Substring(6, 6)));
                    line = line.Substring(12);
                }
            }
            else if (CurrentLine.Value.StartsWith('M'))
            {
                string stringLineStartAddress = line.Substring(0, 6);
                int LineStartAddress = Convert.ToInt32(stringLineStartAddress, 16);
            }

        }
        using (StreamWriter writer = new StreamWriter(path + "memory.txt"))
        {
            Methods.PrintMemory(writer, MemoryMap);
        }
    }
}
