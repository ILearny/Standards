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
        private int _size = 0, _head = 0, _tail = -1;

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

                if (_size > 0)
                {
                    int targetIndex = 0;

                    if (_tail < _head)
                    {
                        // copy the _items[head] ... _items[end]
                        for (int i = _head; i < _items.Length; i++)
                        {
                            newArray[targetIndex] = _items[i];
                            targetIndex ++;
                        }

                        // copy the _items[0] ... _items[tail]
                        for (int i = 0; i <= _tail; i++)
                        {
                            newArray[targetIndex] = _items[i];
                            targetIndex++;
                        }
                    }
                    else
                    {
                        // copy the _items[head] ... _items[tail]
                        for (int i = _head; i <= _tail; i++)
                        {
                            newArray[targetIndex] = _items[i];
                            targetIndex++;
                        }
                    }

                    _head = 0;
                    _tail = targetIndex - 1;

                }
                else
                {
                    _head = 0;
                    _tail = -1;
                }

                _items = newArray;
            }

            if (_tail == _items.Length - 1)
            {
                _tail = 0;
            }
            else
            {
                _tail++;
            }

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

            if (_head == _items.Length - 1)
            {
                _head = 0;
            }
            else
            {
                _head++;
            }

            _items[_head] = default(T);
            _size--;
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
            if (_size > 0)
            {
                if (_tail < _head)
                {
                    // copy the _items[head] ... _items[end]
                    for (int i = _head; i < _items.Length; i++)
                    {
                        yield return _items[i];
                    }

                    // copy the _items[0] ... _items[tail]
                    for (int i = 0; i <= _tail; i++)
                    {
                        yield return _items[i];
                    }
                }
                else
                {
                    // copy the _items[head] ... _items[tail]
                    for (int i = _head; i <= _tail; i++)
                    {
                        yield return _items[i];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
