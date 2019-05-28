using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsTryWhenWeAreAlive
{
    public class ArrayStack<T> : IEnumerable<T>
    {
        T[] _items = new T[0];

        // The current number of items in the stack.
        int _size;

        public void Push(T item)
        {
            // size == 0 ... first push
            // size == length ... growth boundary
            if (_size == _items.Length)
            {
                // initial size of 4, otherwise double the current length
                int newLength = _size == 0 ? 4 : _size * 2;

                // allocate, copy and assign the new array
                T[] newArray = new T[newLength];
                _items.CopyTo(newArray, 0);
                _items = newArray;
            }

            // add the item ro the stack array and increase the size
            _items[_size] = item;
            _size++;
        }

        public T Pop()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("The stack is empty");
            }

            //_size--;
            //return _items[_size];

            // should clear the reference too
            var item = _items[--_size];
            _items[_size] = null; // _size is reduced by 1 already
            return item;
        }

        public T Peek()
        {
            if (_size <= 0)
            {
                throw new InvalidOperationException("The stack is empty");
            }

            return _items[_size - 1];
        }

        public int Count
        {
            get { return _size; }
        }

        public void Clear()
        {
            _size = 0;
            //Array.Clear(_items, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
            // should clear the references too
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _size - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
