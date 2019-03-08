using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONIOLIB
{
    public class Decompositor : IDecompositor
    {
        List<IClassPart> _Parts = new List<IClassPart>();
        string JSONInput;
        public bool IsComplited { get; private set; }



        private Decompositor()
        {

        }

        public static IDecompositor Create()
        {
            return new Decompositor();
        }

        public IDecompositor SetJSONString(string json)
        {
            var arrayChar = json.ToList();
            arrayChar.RemoveAll(x => x == '\n' || x == '\t' || x == '\r');
            JSONInput = new string(arrayChar.ToArray());
            return this;
        }


        public IDecompositor Go()
        {
            int strIndex = 0;
            if (!string.IsNullOrEmpty(JSONInput))
            {
                if(ElementsFinder.StartAndStopExist(JSONInput))
                {
                    for(;;)
                    {
                        var index = ElementsFinder.GetNextPropertyIndex(JSONInput, strIndex);
                        if (index == -1)
                        {
                            IsComplited = false;
                            break;
                        }
                        var NextPropertyFragment = ElementsFinder.GetPropertyFragment(JSONInput, index);
                        if(!string.IsNullOrEmpty(NextPropertyFragment))
                        {
                            strIndex = index + NextPropertyFragment.Length;
                        }
                        else
                        {
                            IsComplited = false;
                            break;
                        }
                        var PropertyName = ElementsFinder.GetFragmentName(NextPropertyFragment);
                        var PropertyDataType = ElementsFinder.GetFragmentDataType(NextPropertyFragment);
                        var PropertyType = ElementsFinder.GetFragmentType(NextPropertyFragment);
                        var ProperyValue = ElementsFinder.GetFragmentValue(NextPropertyFragment);
                        _Parts.Add(ClassPart.Create(PropertyType, PropertyDataType, NextPropertyFragment, PropertyName, ProperyValue));
                    }
                }
            }
            else
            {
                IsComplited = false;
            }
            return this;
        }

        public List<IClassPart> GetParts()
        {
            return _Parts;
        }
    }
}
