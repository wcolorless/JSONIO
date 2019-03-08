using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using Microsoft.Win32;

namespace JSONIOLIB
{
    public class TestElementFinder
    {

        [Fact]
        public void TestStartAndStopExist()
        {
            string TestJSONString = "{ \"Property\" : 1.0 }";
            var result = ElementsFinder.StartAndStopExist(TestJSONString);
            Assert.True(result);
        }

        [Fact]
        public void TestGetNextPropertyIndex()
        {
            string TestJSONString = "{ \"Property\" : 1.0, \"Property2\" : 1 }";
            var result = ElementsFinder.GetNextPropertyIndex(TestJSONString, 0);
            Assert.Equal(2, result);
        }

        [Fact]
        public void TestGetPropertyFragment()
        {
            string TestJSONString = "{ \"Property\" : 1.0, \"Property2\" : 1 }";
            var result1 = ElementsFinder.GetPropertyFragment(TestJSONString, ElementsFinder.GetNextPropertyIndex(TestJSONString, 0));
            Assert.Equal("\"Property\" : 1.0", result1);
        }

        [Fact]
        public void TestGetFragmentName()
        {
            string TestJSONtring = "\"Property\" : 1.0";
            var Name = ElementsFinder.GetFragmentName(TestJSONtring);
            Assert.Equal("Property", Name);
        }

        [Fact]
        public void TestGetFragmentDataType()
        {
            ClassPartDataType ResultType = ClassPartDataType.Empty;
            ResultType = ElementsFinder.GetFragmentDataType("\"Property\" : 1.0");
            Assert.Equal(ClassPartDataType.Double, ResultType);
            ResultType = ElementsFinder.GetFragmentDataType("\"Property\" : 1");
            Assert.Equal(ClassPartDataType.Int, ResultType);
            ResultType = ElementsFinder.GetFragmentDataType("\"Property\" : \"String\"");
            Assert.Equal(ClassPartDataType.String, ResultType);
            ResultType = ElementsFinder.GetFragmentDataType("\"Property\" : { }");
            Assert.Equal(ClassPartDataType.Object, ResultType);
            ResultType = ElementsFinder.GetFragmentDataType("\"Property\" :");
            Assert.Equal(ClassPartDataType.Empty, ResultType);
        }

        [Fact]
        public void TestGetFragmentType()
        {
            ClassPartType ResultType = ClassPartType.Empty;
            ResultType = ElementsFinder.GetFragmentType("\"Property\" : 1.0");
            Assert.Equal(ClassPartType.Property, ResultType);
            ResultType = ElementsFinder.GetFragmentType("\"Property\" : { }");
            Assert.Equal(ClassPartType.Object, ResultType);
            ResultType = ElementsFinder.GetFragmentType("\"Property\" : [ ]");
            Assert.Equal(ClassPartType.Array, ResultType);
        }

        [Fact]
        public void TestGetFragmentValue()
        {
            Assert.Equal(1D, (double)ElementsFinder.GetFragmentValue("\"Property\" : 1.0"));
            Assert.Equal(1, (int)ElementsFinder.GetFragmentValue("\"Property\" : 1"));
            Assert.Equal("String", (string)ElementsFinder.GetFragmentValue("\"Property\" : \"String\""));
            Assert.Equal(new double[] { 1, 2, 3 }, (double[])ElementsFinder.GetFragmentValue("\"Property\" : [1.0, 2.0, 3.0]"));
            Assert.Equal(new int[] { 1, 2, 3 }, (int[])ElementsFinder.GetFragmentValue("\"Property\" : [1, 2, 3]"));
            List<IClassPart> TestObject = new List<IClassPart>();
            TestObject.Add(ClassPart.Create(ClassPartType.Property, ClassPartDataType.Double, "\"InnerProperty\" : 1.0 ", "InnerProperty", 1.0D));
            Assert.Equal(TestObject.GetType(), ElementsFinder.GetFragmentValue("\"Property\" : { \"InnerProperty\" : 1.0 }").GetType());
        }
    }
}
