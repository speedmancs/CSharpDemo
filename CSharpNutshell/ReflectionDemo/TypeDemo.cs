using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionDemo
{
    class TypeDemo
    {
        static void P<T>(T x)
        {
            Console.WriteLine(x);
        }
        static public void Test()
        {
            Type t1 = DateTime.Now.GetType();
            Type t2 = typeof(DateTime);

            Type t3 = typeof(DateTime[]);          // 1-d Array type
            Type t4 = typeof(DateTime[,]);         // 2-d Array type
            Type t5 = typeof(Dictionary<int, int>); // Closed generic type
            Type t6 = typeof(Dictionary<,>);       // Unbound generic type

            Type t = Assembly.GetExecutingAssembly().GetType("ReflectionDemo.TypeDemo");

            Type t_system = Type.GetType("System.Int32, mscorlib, Version=2.0.0.0, " +
                       "Culture=neutral, PublicKeyToken=b77a5c561934e089");

            Type stringType = typeof(string);
            string name = stringType.Name;          // String
            Type baseType = stringType.BaseType;      // typeof(Object)
            Assembly assem = stringType.Assembly;      // mscorlib.dll
            bool isPublic = stringType.IsPublic;      // true
        }

        static public void TestArrayTypes()
        {
            Type type = typeof(int).MakeArrayType();
            Console.WriteLine(type == typeof(int[]));
            Type cubeType = typeof(int).MakeArrayType(3);       // cube shaped
            Console.WriteLine(cubeType == typeof(int[,,]));     // True
            type = typeof(int[]).GetElementType();
            int rank = typeof(int[,,]).GetArrayRank();
        }

        
        static public void TestTypeInfo()
        {
            Type stringType = typeof(string);
            string name = stringType.Name;
            Type baseType = stringType.GetTypeInfo().BaseType;
            Assembly assem = stringType.GetTypeInfo().Assembly;
            bool isPublic = stringType.GetTypeInfo().IsPublic;
        }

        static void Main(string[] args)
        {
            TestBaseInterfaces();
        }

        private static void TestBaseInterfaces()
        {
            Type base1 = typeof(System.String).BaseType;
            Type base2 = typeof(System.IO.FileStream).BaseType;

            Console.WriteLine(base1.Name);     // Object
            Console.WriteLine(base2.Name);     // Stream

            foreach (Type iType in typeof(Guid).GetInterfaces())
                Console.WriteLine(iType.Name);
        }

        private static void TestRefType()
        {
            Type t = typeof(bool).GetMethod("TryParse").GetParameters()[1]
                                             .ParameterType;
            Console.WriteLine(t.Name);    // Boolean&
        }

        private static void TestGenericType()
        {
            Type t = typeof(Dictionary<,>); // Unbound
            Console.WriteLine(t.Name);      // Dictionary'2
            Console.WriteLine(t.FullName);  // System.Collections.Generic.Dictionary'2
            Console.WriteLine(typeof(Dictionary<int, string>).FullName);

            Console.WriteLine(typeof(int[]).Name);      // Int32[]
            Console.WriteLine(typeof(int[,]).Name);      // Int32[,]
            Console.WriteLine(typeof(int[,]).FullName);  // System.Int32[,]
        }


        private static void TestNestedTypes()
        {
            foreach (Type t in typeof(System.Environment).GetNestedTypes())
                P(t.FullName);

            foreach (TypeInfo t in typeof(System.Environment).GetTypeInfo()
                                                  .DeclaredNestedTypes)
                Debug.WriteLine(t.FullName);
        }
    }
}
