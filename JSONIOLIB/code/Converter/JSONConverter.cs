using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JSONIOLIB
{



    public class JSONConverter
    {

        private static object InitProperty(Type type, object obj, List<IClassPart> Parts)
        {
            for(int i = 0; i < Parts.Count; i++)
            {
                PropertyInfo property = type.GetProperty(Parts[i].PartName, BindingFlags.Public | BindingFlags.Instance);
                if(Parts[i].PartType == ClassPartType.Object)
                {
                    var InnerType = property.PropertyType;
                    var newInnerObj = Activator.CreateInstance(InnerType);
                    var InitedObj = InitProperty(property.PropertyType, newInnerObj, Parts[i].InnerParts);
                    property.SetValue(obj, InitedObj);
                }
                else
                {
                    property.SetValue(obj, Parts[i].Value);
                }
            }
            return obj;
        }

        public static T Deserialize<T>(string json)
        {
            var decompositor = Decompositor.Create().SetJSONString(json).Go();
            var obj = Activator.CreateInstance<T>();
            var type = obj.GetType();
            var Parts = decompositor.GetParts();
            for (int i = 0; i < Parts.Count; i++)
            {
                PropertyInfo property = type.GetProperty(Parts[i].PartName, BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    var value = Parts[i].Value;
                    if (Parts[i].PartType == ClassPartType.Object)
                    {

                        var InnerType = property.PropertyType;
                        var newInnerObj = Activator.CreateInstance(InnerType);
                        var InitedObj = InitProperty(property.PropertyType, newInnerObj, (List<IClassPart>)value);
                        property.SetValue(obj, InitedObj);
                    }
                    else
                    {
                        property.SetValue(obj, value);
                    }

                }
            }
            return obj;
        }

        private static string SerializePartObject(object obj)
        {
            StringBuilder sb = new StringBuilder();
            var type = obj.GetType();
            var Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < Properties.Length; i++)
            {
                var SelectedProperty = Properties[i];
                if (SelectedProperty.MemberType == MemberTypes.Property)
                {
                    sb.Append("\n\"" + SelectedProperty.Name + "\" : ");
                    if (SelectedProperty.PropertyType == typeof(double))
                    {
                        var Num = ((double)SelectedProperty.GetValue(obj)).ToString();
                        Num = Num.Replace(',', '.');
                        if (Num.Contains('.')) sb.Append(Num);
                        else sb.Append(Num + ".0");

                    }
                    else if (SelectedProperty.PropertyType == typeof(int))
                    {
                        sb.Append(((int)SelectedProperty.GetValue(obj)).ToString());
                    }
                    else if(SelectedProperty.PropertyType == typeof(Int32[]))
                    {
                        Int32[] array = (Int32[])SelectedProperty.GetValue(obj);
                        sb.Append("[");
                        for (int a = 0; a < array.Length; a++)
                        {
                            if(a == array.Length - 1)  sb.Append(array.GetValue(a).ToString());
                            else sb.Append(array.GetValue(a).ToString() + ", ");
                        }
                        sb.Append("]");
                    }
                    else if (SelectedProperty.PropertyType == typeof(Double[]))
                    {
                        Double[] array = (Double[])SelectedProperty.GetValue(obj);
                        sb.Append("[");
                        for (int a = 0; a < array.Length; a++)
                        {
                            var Num = array.GetValue(a).ToString();
                            Num = Num.Replace(',', '.');
                            if (a == array.Length - 1)
                            {
                                if (Num.Contains('.')) sb.Append(Num);
                                else sb.Append(Num + ".0");
                            }
                            else
                            {
                                if (Num.Contains('.')) sb.Append(Num + ", ");
                                else sb.Append(Num + ".0" + ", ");
                            }
                        }
                        sb.Append("]");
                    }
                    else if(SelectedProperty.PropertyType == typeof(string))
                    {
                       sb.Append("\"" + ((string)SelectedProperty.GetValue(obj)) + "\"");
                    }
                    else
                    {
                        sb.Append("{");
                        var Object = SerializePartObject(SelectedProperty.GetValue(obj));
                        sb.Append(Object);
                        sb.Append("}");
                    }

                    if(Properties.Length > 1 && i != (Properties.Length - 1)) sb.Append(",");
                }
            }
            return sb.ToString();
        }


        public static string Serialize(object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n{");
            var Object = SerializePartObject(obj);
            sb.Append(Object); 
            sb.Append("\n}");
            return sb.ToString();
        }
    }
}
