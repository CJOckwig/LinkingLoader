using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkingOckwig
{
    internal class Symbol
    {
        public Symbol(string name, string val)
        {
            Name = name;
            Value = Convert.ToInt32(val, 16);
        }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
