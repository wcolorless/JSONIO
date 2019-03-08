using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{

    public enum ClassPartType
    {
        Empty,
        Property,
        Array,
        Object
    }

    public enum ClassPartDataType
    {
        Empty,
        String,
        Double,
        Int,
        Object
    }


    public class ClassPart : IClassPart
    {
        ClassPartType _PartType;
        ClassPartDataType _PartDataType;
        string _PartName;
        object _Value;
        public List<IClassPart> InnerParts {get; set; } = new List<IClassPart>();
        public string Part { get; private set; }

        public object Value
        {
            get
            {
                return _Value;
            }
        }

       public ClassPartType PartType
        {
            get
            {
                return _PartType;
            }
        }

        public ClassPartDataType PartDataType
        {
            get
            {
                return _PartDataType;
            }
        }

        public string PartName
        {
            get
            {
                return _PartName;
            }
        }

        private ClassPart(ClassPartType PartType, ClassPartDataType PartDataType, string Part, string Name, object Value)
        {
            _PartDataType = PartDataType;
            _PartType = PartType;
            this.Part = Part;
            _PartName = Name;
            _Value = Value;
            if (Value.GetType() == typeof(List<IClassPart>))
            {
                InnerParts = (List<IClassPart>)Value;
            }
        }

        public static IClassPart Create(ClassPartType PartType, ClassPartDataType PartDataType, string Part, string Name, object Value)
        {
            return new ClassPart(PartType, PartDataType, Part, Name, Value);
        }

    }
}
