using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new Node<int>(19);
            tree = Node<int>.Insert(tree, 8);
            tree = Node<int>.Insert(tree, 13);
            tree = Node<int>.Insert(tree, 78);
            tree = Node<int>.Insert(tree, 10);
            tree = Node<int>.Insert(tree, 1);
            tree = Node<int>.Insert(tree, 11);
            tree = Node<int>.Insert(tree, 9);
            tree.Print();
            tree = Node<int>.Remove(tree, 8);
            Console.WriteLine("Removed 8:");
            tree.Print();
            Console.ReadKey();
        }
    }
}
