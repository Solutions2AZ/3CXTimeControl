using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Models._3CX
{


    public class GenericDataRS
    {
        public string? odatacontext { get; set; }
        public ValueData[]? value { get; set; }
    }

    public class ValueData
    {
        //public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
    }


}
