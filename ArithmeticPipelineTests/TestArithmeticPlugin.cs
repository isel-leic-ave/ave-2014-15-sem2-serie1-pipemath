using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Collections.Generic;
using PipeMath;

namespace UnitTestsPipeMath
{
    [TestClass]
    public class TestArithmeticPlugin
    {
        [ClassInitialize]
        public static void InitAssemblyFiles(TestContext context)
        {
            DynamicTypesUtils.CreateAssemblyFileWithTypes();
            DynamicTypesUtils.CreateAssemblyFileWithMethods();
        }

        [ClassCleanup]
        public static void DestroyAssemblyFiles()
        {
            File.Delete(DynamicTypesUtils.assemblyFileNameTypes);
            File.Delete(DynamicTypesUtils.assemblyFileNameMethods);
        }

        [TestMethod]
        public void Test_Loader_Of_Types()
        {
            Loader loader = new LoaderOfTypes(DynamicTypesUtils.assemblyFileNameTypes + ".dll");
            ArithmeticPipeline pipeline = ArithmeticPipeline.LoadPipeline(loader);
            Assert.AreEqual(2, pipeline.Count);
        }

        [TestMethod]
        public void Test_Two_Types_Add_Mul()
        {
            Loader loader = new LoaderOfTypes(DynamicTypesUtils.assemblyFileNameTypes + ".dll");
            ArithmeticPipeline pipeline = ArithmeticPipeline.LoadPipeline(loader);

            double[] start = new double[] { 2.0 };
            pipeline.ExecuteAll(start);

            Assert.AreEqual((2.0 + 2.0) * 2.0, start[0]);
        }

        [TestMethod]
        public void Test_Loader_Of_Methods()
        {
            Loader loader = new LoaderOfMethods(DynamicTypesUtils.assemblyFileNameMethods + ".dll");
            ArithmeticPipeline pipeline = ArithmeticPipeline.LoadPipeline(loader);
            
            Assert.AreEqual(3, pipeline.Count);
        }

        [TestMethod]
        public void Test_Three_Methods_Add_Mul_Mul()
        {
            Loader loader = new LoaderOfMethods(DynamicTypesUtils.assemblyFileNameMethods + ".dll");
            ArithmeticPipeline pipeline = ArithmeticPipeline.LoadPipeline(loader);

            double[] start = new double[] { 2.0 };
            pipeline.ExecuteAll(start);

            Assert.AreEqual(((2 + 2) * 2 * 3), start[0]);
        }


    }
}
