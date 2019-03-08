using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public class Constructor : IConstructor
    {
        IDecompositor _Decompositor;
        private string _Class;
        private Dictionary<string, string> ClassNames = new Dictionary<string, string>();

        private Constructor()
        {

        }

        public static IConstructor Create()
        {
            return new Constructor();
        }

        public IConstructor SetDecompositor(IDecompositor Decompositor)
        {
            _Decompositor = Decompositor;
            return this;
        }

        private string PrintCommonObject(IClassPart part)
        {
            StringBuilder sb = new StringBuilder();
            if(ClassNames.Count > 0 ) sb.Append("public class ClassName" + ClassNames[part.PartName] +   "\n{ \n");
            else sb.Append("public class RootClass\n{ \n");
            var InnerParts = part.InnerParts;
            for(int i = 0; i < InnerParts.Count; i++)
            {
              var TextPart = TypeClass(InnerParts[i]);
              sb.Append(TextPart);
            }
            sb.Append("}\n");

            for (int i = 0; i < InnerParts.Count; i++)
            {
                if(InnerParts[i].PartType == ClassPartType.Object)
                {
                   var InnerText = PrintCommonObject(ClassPart.Create(ClassPartType.Object, ClassPartDataType.Object, "", InnerParts[i].PartName, InnerParts[i].InnerParts));
                   sb.Append(InnerText);
                }
            }
            return sb.ToString();
        }

        private string PrintProperty(IClassPart part)
        {
            StringBuilder sb = new StringBuilder();
            if (part.PartType == ClassPartType.Property)
            {
                if (part.PartDataType == ClassPartDataType.Double) sb.Append("\tpublic double " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.Int) sb.Append("\tpublic int " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.String) sb.Append("\tpublic string " + part.PartName.ToString());
                sb.Append(" { get; set;}\n");
            }
            else if (part.PartType == ClassPartType.Array)
            {
                if (part.PartDataType == ClassPartDataType.Double) sb.Append("\tpublic double[] " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.Int) sb.Append("\tpublic int[] " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.String) sb.Append("\tpublic string[] " + part.PartName.ToString());
                sb.Append(" { get; set;}\n");
            }
            return sb.ToString();
        }

        private string TypeClass(IClassPart part)
        {
            StringBuilder sb = new StringBuilder();
            if (part.PartType == ClassPartType.Property)
            {
                if (part.PartDataType == ClassPartDataType.Double) sb.Append("\tpublic double " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.Int) sb.Append("\tpublic int " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.String) sb.Append("\tpublic string " + part.PartName.ToString());
                sb.Append(" { get; set;}\n");
            }
            else if (part.PartType == ClassPartType.Array)
            {
                if (part.PartDataType == ClassPartDataType.Double) sb.Append("\tpublic double[] " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.Int) sb.Append("\tpublic int[] " + part.PartName.ToString());
                else if (part.PartDataType == ClassPartDataType.String) sb.Append("\tpublic string[] " + part.PartName.ToString());
                sb.Append(" { get; set;}\n");
            }
            else if (part.PartType == ClassPartType.Object)
            {
                if (part.PartDataType == ClassPartDataType.Object)
                {
                    sb.Append("\tpublic ClassName" + ClassNames.Count.ToString() + " " + part.PartName.ToString());
                    ClassNames.Add(part.PartName, ClassNames.Count.ToString());
                }
                sb.Append(" { get; set;}\n");
            }
            else
            { }
            return sb.ToString();
        }

        public IConstructor Construct()
        {
            var Parts = _Decompositor.GetParts();
            var PrimePart = ClassPart.Create(ClassPartType.Object, ClassPartDataType.Object, "", "", Parts);
            _Class = PrintCommonObject(PrimePart);
            return this;
        }

        public string GetClass()
        {
            return _Class;
        }
    }
}
