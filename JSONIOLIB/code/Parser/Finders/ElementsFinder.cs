using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace JSONIOLIB
{
    public class ElementsFinder
    {
        public static bool StartAndStopExist(string json)
        {
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                return true;
            }
            else return false;
        }

        public static int GetNextPropertyIndex(string json, int lastindex)
        {
            if (lastindex == -1 || lastindex > json.Length || string.IsNullOrEmpty(json)) return -1;
            int tmpIndex = lastindex;
            int resultIndexPosition = -1;
            char[] array = json.ToArray();
            for (;;)
            {
                if(tmpIndex >= json.Length)
                {
                    break;
                }
                if(array[tmpIndex] == '"')
                {
                    resultIndexPosition = tmpIndex;
                    break;
                }
                else
                {
                    tmpIndex++;
                }
            }
            return resultIndexPosition;
        }

        public static string GetPropertyFragment(string json, int lastindex)
        {
            if (lastindex == -1 || lastindex > json.Length || string.IsNullOrEmpty(json)) return string.Empty;
            int tmpIndex = lastindex;
            int endIndex = -1;
            int startArrayBlock = 0;
            int endArrayBlock = 0;
            int startObjectBlock = 0;
            int endObjectBlock = 0;
            int mode = -1; // 0 - [Array]; 1 - {Object}
            string resultString = string.Empty;
            char[] array = json.ToArray();
            for (;;)
            {
                if (tmpIndex >= json.Length)
                {
                    break;
                }
                var SelectedChar = array[tmpIndex];
                if(SelectedChar == '[')
                {
                    startArrayBlock++;
                    if (startObjectBlock == 0 && mode == -1) mode = 0;
                }
                if(SelectedChar == ']')   endArrayBlock++;
                if (SelectedChar == '{')
                {
                    startObjectBlock++;
                    if (startArrayBlock == 0 && mode == -1) mode = 1;
                }
                if (SelectedChar == '}') endObjectBlock++;
                if (SelectedChar == ',' || SelectedChar == '}')
                {
                    if (mode == 0)
                    {
                        if (startArrayBlock == 0 || (startArrayBlock == endArrayBlock && startArrayBlock > 0))
                        {
                            endIndex = tmpIndex;
                            break;
                        }
                        else tmpIndex++;
                    }
                    else if (mode == 1)
                    {
                        if (startObjectBlock == 0 || (startObjectBlock == endObjectBlock && startObjectBlock > 0))
                        {
                            endIndex = tmpIndex + 1;
                            break;
                        }
                        else tmpIndex++;
                    }
                    else
                    {
                        endIndex = tmpIndex;
                        break;
                    }

                }
                else
                {
                    tmpIndex++;
                }
            }
            resultString = json.Substring(lastindex, endIndex - lastindex); // !!!
            return resultString;
        }

        public static string GetFragmentName(string Fragment)
        {
            if (string.IsNullOrEmpty(Fragment)) return string.Empty;
            if (Fragment.Contains(":"))
            {
                string[] parts = Fragment.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    var listChar = parts[0].ToList();
                    listChar.RemoveAll(x => x == '"' || x == 32);
                    var name =  new string(listChar.ToArray());
                    return name;
                }
                else return string.Empty;
            }
            else return string.Empty;
        }

        public static ClassPartDataType GetFragmentDataType(string Fragment)
        {
            if (string.IsNullOrEmpty(Fragment)) return ClassPartDataType.Empty;
            if (Fragment.Contains(":"))
            {
                string[] parts = Fragment.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    if(Fragment.Contains("{") && Fragment.Contains("}"))
                    {
                        return ClassPartDataType.Object;
                    }
                    else
                    {
                        if (parts[1].Contains(".") && !(parts[1].Contains("\""))) return ClassPartDataType.Double;
                        else if (parts[1].Contains("\"")) return ClassPartDataType.String;
                        else return ClassPartDataType.Int;
                    }
                }
                else return ClassPartDataType.Empty;
            }
            else return ClassPartDataType.Empty;
        }

        public static ClassPartType GetFragmentType(string Fragment)
        {
            if (string.IsNullOrEmpty(Fragment)) return ClassPartType.Empty;
            if (Fragment.Contains(":"))
            {
                string[] parts = Fragment.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    if (parts[1].Contains("[") && parts[1].Contains("]")) return ClassPartType.Array;
                    else if (Fragment.Contains("{") && Fragment.Contains("}")) return ClassPartType.Object;
                    else return ClassPartType.Property;
                }
                else return ClassPartType.Empty;
            }
            else return ClassPartType.Empty;
        }

        public static object GetFragmentValue(string Fragment)
        {
            if (string.IsNullOrEmpty(Fragment)) return null;
            object resultObj = null;
            if (Fragment.Contains(":"))
            {
                string[] parts = Fragment.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    var culture = CultureInfo.GetCultureInfo(CultureInfo.CurrentCulture.Name);
                    if(Fragment.Contains("{") && Fragment.Contains("}"))
                    {
                        var listChars = Fragment.ToList().FindIndex(x => x == '{');
                        int indexStartObject = listChars;
                        string obj = Fragment.Substring(indexStartObject);
                        IDecompositor decompositor = Decompositor.Create().SetJSONString(obj).Go();
                        var localParts = decompositor.GetParts();
                        return localParts;
                    }
                    else if (parts[1].Contains("\""))
                    {
                        var cleanPart = parts[1].Trim().Trim(new char[] { '\t', '\r', '\n', '\"' });
                        return cleanPart;
                    }
                    else if ((parts[1].Contains(".") || parts[1].Contains(",")) && !(parts[1].Contains("[") || parts[1].Contains("]")) && !(parts[1].Contains("\"")))
                    {
                        var cleanPart = parts[1].Trim(new char[] {'\t', '\r', '\n' });
                        if(culture.Name == "ru-RU")
                        {
                            cleanPart = cleanPart.Replace('.', ',');
                        }
                        resultObj = Convert.ToDouble(cleanPart);
                        return resultObj;
                    }
                    else if(parts[1].Contains("[") || parts[1].Contains("]"))
                    {
                        var cleanPart = parts[1].Trim(new char[] { '\t', '\r', '\n' });
                        cleanPart = cleanPart.Replace('[', ' ').Replace(']', ' ');
                        var numbers = cleanPart.Split(new char[] { ',' });
                        if (numbers.Length > 0)
                        {
                            if(numbers[0].Contains('.'))
                            {
                                if (culture.Name == "ru-RU")
                                {
                                    for(int i = 0; i < numbers.Length; i++) numbers[i] = numbers[i].Replace('.', ',');
                                }
                                var doubleArray = new double[numbers.Length];
                                for (int i = 0; i < numbers.Length; i++)
                                {
                                    doubleArray[i] = Convert.ToDouble(numbers[i]);
                                }
                                return doubleArray;
                            }
                            else
                            {
                                var array = new int[numbers.Length];
                                for (int i = 0; i < numbers.Length; i++)
                                {
                                    array[i] = Convert.ToInt32(numbers[i]);
                                }
                                return array;
                            }
                        }
                        else return resultObj;
                    }
                    else
                    {
                        var cleanPart = parts[1].Trim(new char[] { '\t', '\r', '\n' });
                        return Convert.ToInt32(cleanPart);
                    }
                }
                else return null;
            }
            else return null;

        }
    }
}
