using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ADS
{
    public class Node<T> where T: IComparable
    {
        public T Key { get; set; }
        private int _height; //высота поддерева с корнем в данном узле
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public Node(T key)
        {
            Key = key;
            Left = Right = null;
            _height = 1;
        }

        public static int GetHeight(Node<T> node) => node?._height ?? 0;

        public int GetBalanceFactor() => GetHeight(Right) - GetHeight(Left);

        public void FixHeight()//восстановить правильную высоту при известных корректных
        {
            _height = Math.Max(GetHeight(Left), GetHeight(Right)) + 1;
        }

        private static Node<T> RotateRight(Node<T> node)
        {
            var q = node.Left;
            node.Left = q.Right;
            q.Right = node;
            node.FixHeight();
            q.FixHeight();
            return q;
        }

        private static Node<T> RotateLeft(Node<T> node)
        {
            var p = node.Right;
            node.Right = p.Left;
            p.Left = node;
            node.FixHeight();
            p.FixHeight();
            return p;
        }

        private static Node<T> Balance(Node<T> node)
        {
            node.FixHeight();
            if (node.GetBalanceFactor() == 2)
            {
                if (node.Right.GetBalanceFactor() < 0)
                {
                    node.Right = RotateRight(node.Right);
                }
                return RotateLeft(node);
            }
            if (node.GetBalanceFactor() == -2)
            {
                if (node.Left.GetBalanceFactor() > 0)
                {
                    node.Left = RotateLeft(node.Left);
                }
                return RotateRight(node);
            }
            return node;
        }

        public static Node<T> Insert(Node<T> node, T key)
        {
            if(node == null) return new Node<T>(key);
            if (key.CompareTo(node.Key) < 0)
            {
                node.Left = Insert(node.Left, key);
            }
            if (key.CompareTo(node.Key) == 0)
            {
                throw new ArgumentException("Дерево уже содержит такой ключ");
            }
            if (key.CompareTo(node.Key) > 0)
            {
                node.Right = Insert(node.Right, key);
            }
            return Balance(node);
        }
        
        //поиск узла с минимальным ключом
        private static Node<T> FindMin(Node<T> node) => (node.Left == null) ? node : FindMin(node.Left);

        //служебная: удаление узла с мин. ключом (узел остаётся)
        private static Node<T> RemoveMin(Node<T> node)
        {
            if (node.Left == null)
                return node.Right;
            node.Left = RemoveMin(node.Left);
            return Balance(node);
        }

        public static Node<T> Remove(Node<T> node, T key)
        {
            if (node == null) return null;
            if (key.CompareTo(node.Key) < 0)
            {
                node.Left = Remove(node.Left, key);
            }
            if (key.CompareTo(node.Key) > 0)
            {
                node.Right = Remove(node.Right, key);
            }
            if (key.CompareTo(node.Key) != 0) return Balance(node);
            var l = node.Left;
            var r = node.Right;
            node = null;
            if (r == null) return l;
            var min = FindMin(r);
            min.Right = RemoveMin(r);
            min.Left = l;
            return Balance(min);
        }

        public static Node<T> Search(Node<T> node, T key)
        {
            if (node == null) return null;
            if (key.CompareTo(node.Key) < 0)
            {
                return Search(node.Left, key);
            }
            if (key.CompareTo(node.Key) > 0)
            {
                return Search(node.Right, key);
            }
            return node;
        }

        private static Node<T> FindFirstMin(Node<T> node, Node<T> firstmin, T key)
        {
            if (node == null) return null;
            if (key.CompareTo(node.Key) < 0)
            {                
                return FindFirstMin(node.Left, firstmin, key);
            }
            if (key.CompareTo(node.Key) > 0)
            {
                firstmin = node;
                return FindFirstMin(node.Right, firstmin, key);
            }
            return firstmin;
        }

        private static Node<T> FindFirstMax(Node<T> node, Node<T> firstmax, T key)
        {
            if (node == null) return null;
            if (key.CompareTo(node.Key) < 0)
            {
                firstmax = node;
                return FindFirstMax(node.Left, firstmax, key);
            }
            if (key.CompareTo(node.Key) > 0)
            {
                return FindFirstMax(node.Right, firstmax, key);
            }
            return firstmax;
        }

        public static Node<T> Predecessor(Node<T> root, Node<T> node)
        {
            if (node.Left != null)
            {
                var pred = node.Left;
                while (pred.Right != null)
                    pred = pred.Right;
                return pred;
            }
            return FindFirstMin(root, null, node.Key);
        }

        public static Node<T> Successor(Node<T> root, Node<T> node)
        {
            if (node.Right != null)
            {
                var succ = node.Right;
                while (succ.Left != null)
                    succ = succ.Left;
                return succ;
            }
            return FindFirstMax(root, null, node.Key);
        }

        private static void Print(Node<T> node, int padding)
        {
            if (node == null) return;
            if (node.Right != null)
            {
                Print(node.Right, padding + 4);
            }
            if (padding > 0)
            {
                Console.Write(" ".PadLeft(padding));
            }
            if (node.Right != null)
            {
                Console.WriteLine("/");
                Console.Write(" ".PadLeft(padding));
            }
            Console.WriteLine(node.Key);
            if (node.Left != null)
            {
                Console.WriteLine(" ".PadLeft(padding) + "\\");
                Print(node.Left, padding + 4);
            }
        }

        public void Print()
        {
            Print(this, 4);
        }
    }
}
