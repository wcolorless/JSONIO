using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public interface IDecompositor
    {

        IDecompositor SetJSONString(string json);
        IDecompositor Go();
        List<IClassPart> GetParts();

    }
}
