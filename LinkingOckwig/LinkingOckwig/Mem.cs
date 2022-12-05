using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkingOckwig
{
    public class Mem
    {
        public Mem(int location, string contents)
        {
            this.Location = location;
            Contents = contents;
        }
        public int Location { get; set; }
        public string Contents { get; set; }
    }
}
