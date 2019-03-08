using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace JSONIOLIB
{
    public class TestJSONConverter
    {
        [Fact]
        public void TestObjectToJSON()
        {
            RootClass newObject = new RootClass();
            newObject.ArrayDouble = new double[] { 1.0, 2.0 };
            newObject.ArrayInt = new int[] { 1, 2 };
            newObject.Hour = 12D;
            newObject.Seconds = 15;
            newObject.Say = 5.ToString() + " = Число";
            newObject.Object1 = new ClassName0() { Exp = 2.718D, PI = 3.14D };
            newObject.Object2 = new ClassName1() { Speed = 5.0D };
            newObject.Object3 = new ClassName2() { Power = new ClassName4() { Wattage = 45.0D } };
            newObject.Object4 = new ClassName3() { Current = new ClassName5() { Active = new ClassName6() { Sin = "90.0" } } };
            var result = JSONConverter.Serialize(newObject);
            string TestJSONString = "\n{\n\"Hour\" : 12.0,\n\"Seconds\" : 15,\n\"Say\" : \"5 = Число\",\n\"ArrayInt\" : [1, 2],\n\"ArrayDouble\" : [1.0, 2.0],\n\"Object1\" : {\n\"PI\" : 3.14,\n\"Exp\" : 2.718},\n\"Object2\" : {\n\"Speed\" : 5.0},\n\"Object3\" : {\n\"Power\" : {\n\"Wattage\" : 45.0}},\n\"Object4\" : {\n\"Current\" : {\n\"Active\" : {\n\"Sin\" : \"90.0\"}}}\n}";
            Assert.Equal(TestJSONString, result);
        }

        [Fact]
        public void TestJSONToObject()
        {
            string TestJSONString = "\n{\n\"Hour\" : 12.0,\n\"Seconds\" : 15,\n\"Say\" : \"5 = Число\",\n\"ArrayInt\" : [1, 2],\n\"ArrayDouble\" : [1.0, 2.0],\n\"Object1\" : {\n\"PI\" : 3.14,\n\"Exp\" : 2.718},\n\"Object2\" : {\n\"Speed\" : 5.0},\n\"Object3\" : {\n\"Power\" : {\n\"Wattage\" : 45.0}},\n\"Object4\" : {\n\"Current\" : {\n\"Active\" : {\n\"Sin\" : \"90.0\"}}}\n}";
            var result = JSONConverter.Deserialize<RootClass>(TestJSONString);
            Assert.Equal(new double[] { 1, 2 }, result.ArrayDouble);
            Assert.Equal(new int[] { 1, 2 }, result.ArrayInt);
            Assert.Equal(12D, result.Hour);
            Assert.Equal(2.718D, result.Object1.Exp);
            Assert.Equal(3.14D, result.Object1.PI);
            Assert.Equal(5.0D, result.Object2.Speed);
            Assert.Equal(45.0D, result.Object3.Power.Wattage);
            Assert.Equal("90.0", result.Object4.Current.Active.Sin);
            Assert.Equal("5 = Число", result.Say);
            Assert.Equal(15, result.Seconds);
        }

    }
}
