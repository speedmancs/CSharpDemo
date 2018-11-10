using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDemo
{
    public class Stack<T>
    {
        int position;
        T[] data = new T[100];
        public void Push(T obj) => data[position++] = obj;
        public T Pop() => data[--position];
    }

    public class StackDemo
    {
        static void P<T>(T x)
        {
            Console.WriteLine(x);
        }

        static void Main(string[] args)
        {
            var stack = new Stack<int>();
            stack.Push(5);
            stack.Push(10);
            int x = stack.Pop();        // x is 10
            int y = stack.Pop();        // y is 5
            P(x); P(y);
        }
    }
}
