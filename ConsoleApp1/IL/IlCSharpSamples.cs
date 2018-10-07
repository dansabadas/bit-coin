using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ConsoleApp1.IL
{
    public static class IlCSharpSamples
    {
        public static T CreateDelegate<T>(this DynamicMethod dynamicMethod) //: where T : Delegate
        {
            return (T)(object)dynamicMethod.CreateDelegate(typeof(T));
        }

        public static void Run()
        {
            //Assign();
            //Console.WriteLine(Calculate(5));
            //Console.WriteLine(CalculateRecur(5));

            //var filip = new Person { Name = "Dan Sabadis" };
            //Console.WriteLine(filip.Speak());

            //var type = filip.GetType();
            //var methods = type.GetMethods();
            //foreach (var method in methods)
            //{
            //    Console.WriteLine(method.Name);
            //}

            //type.GetMethod("set_Name").Invoke(filip, new[] {"Danson"});
            //var result = type.GetMethod("Speak").Invoke(filip, null);
            //Console.WriteLine(result);

            //IlGenerator();
            //IlGenerator2();
            //IlGenerator3();
            //CallingMethods();
            //CallingDynamicMethods();
            //FactorialIL();
            //SwitchIL();
            TypeBuilder();
        }

        public static void TypeBuilder()
        {
            var assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Demo"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PersonModule");
            var typeBuilder = moduleBuilder.DefineType("Person", TypeAttributes.Public);

            var nameField = typeBuilder.DefineField("name", typeof(string), FieldAttributes.Private);
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,
                new[] {typeof(string)});
            var ctorIL = ctor.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);   // the first parameter sent to a constructor is the instance 'this' itself
            ctorIL.Emit(OpCodes.Ldarg_1);   // the second argument is the first classic one for a constructor!
            ctorIL.Emit(OpCodes.Stfld, nameField);
            ctorIL.Emit(OpCodes.Ret);

            var nameProperty = typeBuilder.DefineProperty("Name", PropertyAttributes.HasDefault, typeof(string), null);
            var namePropertyGetMethod = typeBuilder.DefineMethod("get_Name",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(string), Type.EmptyTypes);

            nameProperty.SetGetMethod(namePropertyGetMethod);

            var namePropertyGetMethodIL = namePropertyGetMethod.GetILGenerator();
            namePropertyGetMethodIL.Emit(OpCodes.Ldarg_0);
            namePropertyGetMethodIL.Emit(OpCodes.Ldfld, nameField);
            namePropertyGetMethodIL.Emit(OpCodes.Ret);

            var t = typeBuilder.CreateType();
            var nProperty = t.GetProperty("Name");

            var instance = Activator.CreateInstance(t, "Filip");
            var result = nProperty.GetValue(instance, null);
            Console.WriteLine(result);
        }

        public static void FactorialIL()
        {
            var factorial = new DynamicMethod(
                "Factorial",
                typeof(int),
                new[] { typeof(int) },
                typeof(Program).Module);

            var il = factorial.GetILGenerator();
            var methodEnd = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Beq, methodEnd);

            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);

            il.Emit(OpCodes.Call, factorial);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);

            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ret);

            var factorialDelegate = factorial.CreateDelegate<Func<int, int>>();
            var result = factorialDelegate(4);
            Console.WriteLine(result);
        }

        public static void SwitchIL()
        {
            var switchMethod = new DynamicMethod(
                "SwitchMethod",
                typeof(int),
                new[] { typeof(int), typeof(int), typeof(int) },
                typeof(Program).Module);

            var il = switchMethod.GetILGenerator();

            var jumpTable = new[]
            {
                il.DefineLabel(),   //add
                il.DefineLabel(),    //mult
                il.DefineLabel(),   //div
                il.DefineLabel(),   //substract
                il.DefineLabel()    //default
            };

            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Switch, jumpTable);
            il.Emit(OpCodes.Br, jumpTable.Last());

            il.MarkLabel(jumpTable[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[1]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[2]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[3]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[4]);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var calcMethodDelegate = switchMethod.CreateDelegate<Func<int, int, int, int>>();
            Console.WriteLine(calcMethodDelegate(1, 2, 0));
        }

        public static int GetResult(int a, int b, int operation)
        {
            switch (operation)
            {
                case 0:
                    return a + b;
                case 1:
                    return a * b;
                case 2:
                    return a / b;
                case 3:
                    return a - b;
                default:
                    return 0;
            }
        }

        public static int Factorial(int i)
        {
            if (i == 1)
            {
                return i;
            }

            return i * Factorial(i - 1);

        }

        static void CallingDynamicMethods()
        {
            var multiplyMethod = new DynamicMethod(
                "MultiplyMethod",
                typeof(int),
                new[] { typeof(int) },
                typeof(Program).Module);

            var il = multiplyMethod.GetILGenerator();
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, 42);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            var nultiplyMethodDelegate = (Func<int, int>)multiplyMethod.CreateDelegate(typeof(Func<int, int>));
            var result = nultiplyMethodDelegate(10);
            Console.WriteLine(result);

            var calculateMethod = new DynamicMethod(
                "CalculateMethod",
                typeof(int),
                new[] { typeof(int), typeof(int) },
                typeof(Program).Module);

            var calcMethodIL = calculateMethod.GetILGenerator();

            calcMethodIL.Emit(OpCodes.Ldarg_0);
            calcMethodIL.Emit(OpCodes.Ldarg_1);
            calcMethodIL.Emit(OpCodes.Mul);
            calcMethodIL.Emit(OpCodes.Call, multiplyMethod);
            calcMethodIL.Emit(OpCodes.Ret);

            var calcMethodDelegate = calculateMethod.CreateDelegate<Func<int, int, int>>();
            result = calcMethodDelegate(100, 100);
            Console.WriteLine(result);
        }

        static void CallingMethods()
        {
            var calcMethod = new DynamicMethod(
                "myMethod",
                typeof(void),
                null,
                typeof(Program).Module);

            var il = calcMethod.GetILGenerator();

            il.Emit(OpCodes.Ldc_I4, 42);
            il.Emit(OpCodes.Call, typeof(Program).GetMethod("Print"));
            il.Emit(OpCodes.Ret);

            var myMethodDelegate = (Action)calcMethod.CreateDelegate(typeof(Action));
            myMethodDelegate();
        }

        public static void Print(int i) => Console.WriteLine("The value passed to Print is {0}", i);

        static void IlGenerator3()
        {
            //Console.WriteLine(Calculate3(1000));
            var calcMethod = new DynamicMethod(
                "calcMethod",
                typeof(int),
                new[] { typeof(int) },
                typeof(Program).Module);

            var il = calcMethod.GetILGenerator();

            //define the 2 labels used for jumping
            var loopStart = il.DefineLabel();
            var methodEnd = il.DefineLabel();

            // variable 0 : result = 0
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_0);

            // variable 1 : i = 0
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_1);

            il.MarkLabel(loopStart);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldc_I4, 10);
            il.Emit(OpCodes.Bge, methodEnd); //if il > 10 => jump

            // i*x
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);

            // result += 
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_1);

            il.Emit(OpCodes.Br, loopStart);

            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<int, int>) calcMethod.CreateDelegate(typeof(Func<int, int>));
            var result = method(1000);
            Console.WriteLine(result);
        }

        static int Calculate3(int x)
        {
            var result = 0;
            for (int i = 0; i < 10; i++)
            {
                result += i * x;
            }
            return result;
        }

        static void IlGenerator2()
        {
            var calcMethod = new DynamicMethod(
                "calcMethod", 
                typeof(int), 
                new []{ typeof(int), typeof(int), typeof(int) },
                typeof(Program).Module);

            var il = calcMethod.GetILGenerator();
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Stloc_0);//this is wrong if not using DeclareLocal above
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            var method = (Func<int, int, int, int>)calcMethod.CreateDelegate(typeof(Func<int, int, int, int>));
            var result = method(10, 10, 5);
            Console.WriteLine(result);
        }

        static int Calculate(int a, int b, int c)
        {
            var result = a * b;
            return result - c;
        }

        static void IlGenerator()
        {
            var myMethod = new DynamicMethod("DividerMethod",
                typeof(double),
                new[] { typeof(int), typeof(int) },
                typeof(Program).Module);

            var il = myMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Div);   // 2 pop and one push from/into the evaluation stack
            il.Emit(OpCodes.Ret);

            var result = myMethod.Invoke(null, new object[] {10, 2});
            Console.WriteLine(result);

            var method = (DivideDelegate)myMethod.CreateDelegate(typeof(DivideDelegate));
            var result2 = method(6, 2);
            Console.WriteLine(result2);
        }

        delegate double DivideDelegate(int a, int b);

        static double Divider(int a, int b)
        {
            return a / b;
        }

        static void Assign()
        {
            var x = 10;
            var y = 20;
            var result = x + y;

            //if (x == 10 && y == 20)
            //{

            //}
        }

        static int Calculate(int x)
        {
            var result = x;
            do
            {
                result += 5;
            } while (result < 15);

            return result;
        }

        static int CalculateRecur(int n)
        {
            if (n <= 1)
            {
                return n;
            }

            return n * CalculateRecur(n - 1);
        }
    }
}
