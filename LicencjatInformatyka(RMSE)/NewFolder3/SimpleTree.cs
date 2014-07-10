﻿using System.Collections.Generic;

namespace LicencjatInformatyka_RMSE_.NewFolder3
{
    class SimpleTree
    {
          public string Name { get; set; }
        public bool Dopytywalny { get; set; }
            public SimpleTree _parent { get; set; }
    List<SimpleTree> _children = new List<SimpleTree>();

    public List<SimpleTree> Children {
        get { return _children; }
    }
        public SimpleTree Parent 
        {
            get{return _parent; }
        }


        public int Depth
        {
            get
            {
                //return (Parent == null ? -1 : Parent.Depth) + 1;

                int depth = 0;
                SimpleTree node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                    depth++;
                }
                return depth;
            }
        }

    }

       //static IEnumerable<SimpleTree> SimpleTreeToEnumerable(SimpleTree f)
       // {
       //     Stack<SimpleTree> stack = new Stack<SimpleTree>();
       //     stack.Push(f);
       //     while (stack.Count > 0)
       //     {
       //         var SimpleTree = stack.Pop();
       //         yield return SimpleTree;
       //         foreach (var child in SimpleTree.Children)
       //             stack.Push(child);
       //     }
       // }

    }

