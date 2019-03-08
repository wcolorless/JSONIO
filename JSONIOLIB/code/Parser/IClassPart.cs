using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public interface IClassPart
    {
        ClassPartType PartType { get; }
        ClassPartDataType PartDataType { get; }
        string PartName { get; }
        object Value { get; }
        List<IClassPart> InnerParts { get; set; }
    }
}
