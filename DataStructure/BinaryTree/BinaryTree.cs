using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LetsTryWhenWeAreAlive
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {

        private BinaryTreeNode<T> _head;
        private int _count;

        #region Add

        public void Add(T value)
        {
            if (_head == null)
            {
                _head = new BinaryTreeNode<T>(value);
            }
            else
            {
                AddTo(_head, value);
            }
            _count++;
        }

        public void AddTo(BinaryTreeNode<T> node, T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                }
                else
                {
                    AddTo(node.Left, value);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                }
                else
                {
                    AddTo(node.Right, value);
                }
            }
        }

        #endregion

        public bool Contains(T value)
        {
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> current = _head;
            parent = null;

            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }

            return current;
        }

        #region Remove

        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;
            current = FindWithParent(value, out parent);
            if (current == null) return false;

            // Case 1: If current has no right child, then current's left replaces current
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _head = current.Left;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Left;
                    }
                }
            }
            // CAse 2: If current's right child has no left child, then currnet's right child replaces current
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    _head = current.Right;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Right;
                    }
                }
            }
            // Case 3: If current's right child has a left child, replace current with current's right child's left-most child
            else
            {
                BinaryTreeNode<T> leftMost = current.Right.Left;
                BinaryTreeNode<T> leftMostParent = current.Right;

                while (leftMost.Left != null)
                {
                    leftMostParent = leftMost;
                    leftMostParent = leftMost.Left;
                }

                // the parent's left subtree becomes leftmost's right subtree
                leftMostParent.Left = leftMostParent.Right;

                // assign leftmost's left and right to current's left and right children
                leftMost.Left = current.Left;
                leftMost.Right = current.Right;

                if (parent == null)
                {
                    _head = leftMost;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        parent.Left = leftMost;
                    }
                    else if (result < 0)
                    {
                        parent.Right = leftMost;
                    }
                }
            }

            _count--;
            return true;
        }

        #endregion


        #region Pre-Order Traversal

        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, _head);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                action(node.Value);
                PreOrderTraversal(action, node.Left);
                PreOrderTraversal(action, node.Right);
            }
        }

        #endregion

        #region Post-Order Traversal

        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, _head);
        }

        private void PostOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                PostOrderTraversal(action, node.Left);
                PostOrderTraversal(action, node.Right);
                action(node.Value);
            }
        }

        #endregion


        #region In-Order Traversal

        public void InOrderTraversal(Action<T> action)
        {
            InOrderTraversal(action, _head);
        }

        private void InOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);
                action(node.Value);
                InOrderTraversal(action, node.Right);
            }
        }

        public IEnumerator<T> InOrderTraversal()
        {
            // This is a non-recursive algorithm using a stack to demonstrate removing  recursion to make using the yield syntax easier
            if (_head != null)
            {
                // Store the nodes we've skipped in this track (avoids recursion)
                var stack = new Stack<BinaryTreeNode<T>>();
                BinaryTreeNode<T> current = _head;

                // when removing recursion we need to keep track whether or not we should be going to the left node or the right nodes next
                bool goLeftNext = true;

                // start by pushing the Head onto the stack
                stack.Push(current);


                while (stack.Count > 0)
                {
                    // If we're heading left
                    if (goLeftNext)
                    {
                        // push everything but the left-most node to the stack we'll yield the left-most after this block
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // in-order is left->yield->right
                    yield return current.Value;

                    // if we can go right then do so
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // once we've gone right once, we need to start going left again
                        goLeftNext = true;
                    }
                    else
                    {
                        // if we can't go right then we need to pop off the parent node so we can process it and then go to it's right node
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _head = null;
            _count = 0;
            // release memory
        }

        public int Count
        {
            get { return _count;  }
        }
    }
}
