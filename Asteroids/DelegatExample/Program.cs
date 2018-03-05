using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatExample
{
    class Element : IComparable
    {
        private string _fio;
        private int _ball;
        public Element(string fio, int ball)
        {
            _fio = fio;
            _ball = ball;
        }
        public string FIO => _fio;
        public int Ball => _ball;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Element elm = obj as Element;
            if (elm != null)
                return Ball > elm.Ball ? 1 : -1;
            else
                throw new ArgumentException("");
        }

    }
    class Program
    {

        static int MyDelegat<T>(T el1, T el2) where T : IComparable
        {
            return el1.CompareTo(el2);
        }

        static void Main(string[] args)
        {
            List<Element> list = new List<Element>();
            using (StreamReader sr = new StreamReader("data.txt"))
            {
                int n;
                if (int.TryParse(sr.ReadLine(), out n))
                    for (int i = 0; i < n; i++)
                    {
                        string[] s = sr.ReadLine()?.Split(' ');
                        int ball = int.Parse(s[2]) + int.Parse(s[3]) + int.Parse(s[4]);
                        list.Add(new Element(s[0] + " " + s[1], ball)); 
                    }
                else throw new InvalidOperationException();
            }
            list.Sort(MyDelegat);
            foreach (var v in list)
            {
                Console.WriteLine($"{v.FIO,20}{v.Ball,10}");
            }
            Console.WriteLine();
            int ball2 = list[2].Ball;
            foreach (var v in list)
            {
                if (v.Ball <= ball2) Console.WriteLine($"{v.FIO,20}{v.Ball,10}");
            }
        }
    }
}
