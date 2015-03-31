using PipeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsPipeMath
{
    class DynamicTypesUtils
    {
        internal static String assemblyFileNameTypes = "DynamicAssemblySeveralTypes";
        internal static String assemblyFileNameMethods = "DynamicAssemblySeveralOperations";

        private static void Generate(ILGenerator gen, OpCode instr, double value)
        {
            gen.DeclareLocal(typeof(Double));
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem, typeof(Double));
            gen.Emit(OpCodes.Ldc_R8, value);
            gen.Emit(instr);
            gen.Emit(OpCodes.Stloc_0);   // local = a[0] <op> 2
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Stelem, typeof(Double));    // a[0] = local
            gen.Emit(OpCodes.Ret);
        }

        private static void DefineOperations(ModuleBuilder mb, string typeName)
        {
            TypeBuilder tb = mb.DefineType(typeName, TypeAttributes.Public);

            // Define a method that accepts an array of double and returns void.
            MethodBuilder op1 = tb.DefineMethod(
                "Op1",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(void),
                new Type[] { typeof(double[]) });
            Generate(op1.GetILGenerator(), OpCodes.Add, 2.0);

            MethodBuilder op2 = tb.DefineMethod(
                "Op2",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(void),
                new Type[] { typeof(double[]) });
            Generate(op2.GetILGenerator(), OpCodes.Mul, 2.0);

            MethodBuilder op3 = tb.DefineMethod(
                "Op3",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(void),
                new Type[] { typeof(double[]) });
            Generate(op3.GetILGenerator(), OpCodes.Mul, 3.0);

            tb.CreateType();
        }

        internal static void CreateAssemblyFileWithMethods()
        {
            AssemblyName aName = new AssemblyName(assemblyFileNameMethods);
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.Save);

            // For a single-module assembly, the module name is usually 
            // the assembly name plus an extension.
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            DefineOperations(mb, "OneTypeSeveralOperations");

            ab.Save(aName.Name + ".dll");
        }

        internal static void CreateAssemblyFileWithTypes()
        {
            /*
            public class Operation_Add : IArrayOperation
            {
                public void Execute(double[] values)
                {
                    values[0] = values[0] + 2;
                }
            }

            public class Operation_Mul: IArrayOperation
            {
                public void Execute(double[] values)
                {
                    values[0] = values[0] * 2;
                }
            }
            */

            AssemblyName aName = new AssemblyName(assemblyFileNameTypes);
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.Save);

            // For a single-module assembly, the module name is usually 
            // the assembly name plus an extension.
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder tb = mb.DefineType("Operation_Add", TypeAttributes.Public);

            tb.AddInterfaceImplementation(typeof(IArrayOperation));

            // Define a method that accepts an array of double and returns void.
            MethodBuilder meth = tb.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(void),
                new Type[] { typeof(double[]) });

            Generate(meth.GetILGenerator(), OpCodes.Add, 2.0);

            // Finish the type.
            tb.CreateType();

            tb = mb.DefineType("Operation_Mul", TypeAttributes.Public);

            tb.AddInterfaceImplementation(typeof(IArrayOperation));

            // Define a method that accepts an array of double and returns void.
            meth = tb.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(void),
                new Type[] { typeof(double[]) });

            Generate(meth.GetILGenerator(), OpCodes.Mul, 2.0);

            // Finish the type.
            tb.CreateType();

            ab.Save(aName.Name + ".dll");
        }

    }
}
