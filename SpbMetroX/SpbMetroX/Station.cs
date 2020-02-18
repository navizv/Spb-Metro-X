using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpbMetroX
{
    class Station : IComparable
    {
        public string id;
        public string name;

        public Station(string v1, string v2)
        {
            this.id = v1;
            this.name = v2;
        }

        public int CompareTo(object obj)
        {
            return ToString().CompareTo(obj.ToString());
        }

        public override string ToString()
        {
            return name;
        }
    }
}
