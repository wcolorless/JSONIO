using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public interface IConstructor
    {
        IConstructor SetDecompositor(IDecompositor Decompositor);
        IConstructor Construct();
        string GetClass();
    }
}
