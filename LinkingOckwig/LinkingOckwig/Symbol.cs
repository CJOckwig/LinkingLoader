using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkingOckwig
{
    public class Symbol
    {
        public Symbol(string Name, string val, string CSect, int ProgramBuffer)
        {
            this.Name = Name;
            Value = Convert.ToInt32(val, 16);
            this.CSect = CSect;
            LoadAddress = Value + ProgramBuffer + 8560;
        }
        public string Name { get; set; }
        public string CSect {get; set;}
        public int Value { get; set; }
        public int LoadAddress { get; set; }

    }
}
