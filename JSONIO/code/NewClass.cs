using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIO
{

    public class RootClass
    {
        public double Hour { get; set; }
        public int Seconds { get; set; }
        public string Say { get; set; }
        public int[] ArrayInt { get; set; }
        public double[] ArrayDouble { get; set; }
        public ClassName0 Object1 { get; set; }
        public ClassName1 Object2 { get; set; }
        public ClassName2 Object3 { get; set; }
        public ClassName3 Object4 { get; set; }
    }
    public class ClassName0
    {
        public double PI { get; set; }
        public double Exp { get; set; }
    }
    public class ClassName1
    {
        public double Speed { get; set; }
    }
    public class ClassName2
    {
        public ClassName4 Power { get; set; }
    }
    public class ClassName4
    {
        public double Wattage { get; set; }
    }
    public class ClassName3
    {
        public ClassName5 Current { get; set; }
    }
    public class ClassName5
    {
        public ClassName6 Active { get; set; }
    }
    public class ClassName6
    {
        public double Sin { get; set; }
    }





}
