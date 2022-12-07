using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkingOckwig
{
    public class CSect
    {
        public CSect(string Name, int CSAddress, int ProgramLength, int ProgramBuffer)
        {
            this.Name = Name;
            this.CSAddress = CSAddress + ProgramBuffer + 8560;
            this.ProgramLength = ProgramLength;
        }
        public string Name { get; set; }
        public int CSAddress { get; set; }
        public int ProgramLength {get;set;}
    }
}