using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsTryWhenWeAreAlive
{
    public class ArrayQueue<T>: IEnumerable<T>
    {
        T[] _items = new T[0];

        // The current number of items in the stack.
        private int _size = 0;

        private int _head = 0, _tail = 0;

        public void Enqueue(T item)
        {
            // size == 0 ... first push
            // size == length ... growth boundary
            if (_size == _items.Length)
            {
                // initial size of 4, otherwise double the current length
                int newLength = _size == 0 ? 4 : _size * 2;

                // allocate, copy and assign the new array
                T[] newArray = new T[newLength];

                for (int i = _head, j =0; j < _size; i++, j++)
                {
                    if (i == _items.Length) i = 0;
                    newArray[j] = _items[i];
                }
                _head = 0;
                _tail = _size > 0 ? _size - 1 : 0;
                _items = newArray;
            }

            if (_tail >= _items.Length)
            {
                _tail = 0;
            }

            if (_size > 0) _tail++;
            if (_tail == _items.Length) _tail = 0;

            _items[_tail] = item;
            _size++;
        }

        public T Dequeue()
        {
            if (_size <= 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            var item = _items[_head];
            _items[_head] = default(T);
            _size--;
            _head = (_size > 0) ? ++_head : 0;
            return item;
        }

        public T Peek()
        {
            if (_items.Length <= 0)
            {
                throw new InvalidOperationException("The stack is empty");
            }

            return _items[_head];
        }




        public int Count
        {
            get { return _size; }
        }

        public void Clear()
        {
            _size = 0;
            // should clear the references too
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _head, j = 0; j < _size; i++, j++)
            {
                if (i == _items.Length) i = 0;
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
