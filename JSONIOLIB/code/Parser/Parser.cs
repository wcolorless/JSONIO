using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public class Parser : IParser
    {
        private string _Input;
        private IDecompositor _Decompositor;
        private IConstructor _Constructor;


        private Parser()
        {
             
        }

        public static IParser Create()
        {
            return new Parser();
        }


        public string GetClass(string json)
        {
            _Input = json;
            _Decompositor = Decompositor.Create().SetJSONString(json).Go();
            _Constructor = Constructor.Create().SetDecompositor(_Decompositor).Construct();
            return _Constructor.GetClass();
        }
    }
}
