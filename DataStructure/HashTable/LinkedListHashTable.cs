// NOT WORKING AS EXPECTED - STRANGE COMPILATION ERROR
using System;
using System.Collections.Generic;

namespace LetsTryWhenWeAreAlive
{

    public class HashTableNodePair<TKey, TValue>
    {
        public HashTableNodePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        // The key cannot be changed because it would affect the indexing in the hash table
        public TKey Key { get; private set; }
        public TValue Value { get; set; }

    }

    public class HashTableArrayNode<TKey, TValue>
    {
        // the list contains the actual data in the hash table. It contains together data collisions. It would be possible to use a list only when there is a collision
        // to avoid allocating the list unnecessarily but this approach makes the implementation easier to follow for this sample
        LinkedList<HashTableNodePair<TKey, TValue>> _items = new LinkedList<HashTableNodePair<TKey, TValue>>();
        public void Add<TKey, TValue>(TKey key, TValue value)
        {
            // laxy init the linked list
            foreach (HashTableNodePair<TKey, TValue> pair in _items)
            {
                if (pair.Key.Equals(key))
                {
                    throw  new ArgumentException("The collection already contains the key");
                }
            }
            _items.AddFirst(new HashTableNodePair<TKey, TValue>(key, value));
        }

        public void Update<TKey, TValue>(TKey key, TValue value)
        {
            bool updated = false;
            if (_items != null)
            {
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        pair.Value = value;
                        updated = true;
                        break;
                    }
                }
            }
            if (!updated)
            {
                throw new ArgumentException("The collection does not contain the key");
            }
        }

        public bool Remove<TKey>(TKey key)
        {
            bool removed = false;
            if (_items != null)
            {
                LinkedListNode<HashTableNodePair<TKey, TValue>> current = _items.First;
                while (current != null)
                {
                    if (current.Value.Key.Equals(key))
                    {
                        _items.Remove(current);
                        removed = true;
                        break;
                    }
                    current = current.Next;
                }
            }
            return removed;
        }

        public bool TryGetvalue<TKey, TValue>(TKey key, out TValue value)
        {
            value = default(TValue);
            bool found = false;

            if (_items != null)
            {
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        public void Clear()
        {
            _items?.Clear();
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Value;
                    }
                }
            }
        }
        public IEnumerable<TKey> Keys
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Key;
                    }
                }
            }
        }

        public IEnumerable<HashTableNodePair<TKey, TValue>> Items
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node;
                    }
                }

            }
        }

    }

    public class HashTableArray<TKey, TValue>
    {
        HashTableArrayNode<TKey, TValue>[] _array;

        public HashTableArray(int capacity)
        {
            _array = new HashTableArrayNode<TKey, TValue>[capacity];
            // to safegaurd null exception - not for prod
            for (int i = 0; i < capacity; i++)
            {
                _array[i] = new HashTableArrayNode<TKey, TValue>();
            }
        }

        public void Add(TKey key, TValue value)
        {
            _array[GetIndex(key)].Add(key, value);
        }

        public void Update(TKey key, TValue value)
        {
            _array[GetIndex(key)].Update(key, value);
        }

        public bool Remove(TKey key)
        {
            return _array[GetIndex(key)].Remove(key);
        }

        public bool TryGetvalue(TKey key, out TValue value)
        {
            return _array[GetIndex(key)].TryGetvalue(key, out value);
        }

        public int Capacity
        {
            get { return _array.Length; }
        }

        public void Clear()
        {
            foreach (HashTableArrayNode<TKey, TValue> node in _array)
            {
                node.Clear();
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in _array)
                {
                    foreach (TValue value in node.Values)
                    {
                        yield return value;
                    }
                }
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in _array)
                {
                    foreach (TKey key in node.Keys)
                    {
                        yield return key;
                    }
                }
            }
        }

        public IEnumerable<HashTableArrayNode<TKey, TValue>> Items
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in _array)
                {
                    yield return node;
                }
            }
        }

        private int GetIndex(TKey key)
        {
            throw new NotImplementedException();
        }
    }

    public class HashTable<TKey, TValue>
    {
        // if the array axeeds this fill % it will grow
        const double _fillfactor = 0.75;

        // the maximum items to store before growing.
        private int _maxItemsAtCurrentSize;
        // the no. of items in the hash table
        int _count;
        // array where items are stored
        HashTableArray<TKey, TValue> _array;

        public HashTable() : this(1000)
        {
            
        }

        public HashTable(int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }
            _array = new HashTableArray<TKey, TValue>(initialCapacity);

            // when the count exeeds this value, the next Addd will cause the array to grow
            _maxItemsAtCurrentSize = (int) (initialCapacity * _fillfactor) + 1;
        }

        public void Add(TKey key, TValue value)
        {
            // if we are at capacity, the array needs to grow
            if (_count >= _maxItemsAtCurrentSize)
            {
                // allocate a larger array
                HashTableArray<TKey, TValue> largeArray = new HashTableArray<TKey, TValue>(_array.Capacity * 2);
                // and re-add each item to the new array
                foreach (HashTableNodePair<TKey, TValue> node in _array.Items)
                {
                    largeArray.Add(node.Key, node.Value);
                }
                // the larger array is now the hash table storage
                _array = largeArray;
                // update the new max items cached value
                _maxItemsAtCurrentSize = (int)(_array.Capacity * _fillfactor) + 1;
            }

            _array.Add(key, value);
            _count++;
        }

        public bool Remove(TKey key)
        {
            bool removed = _array.Remove(key);
            if (removed)
            {
                _count--;
            }
            return removed;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_array.TryGetvalue(key, out value))
                {
                    throw new ArgumentException("key");
                }
                return value;
            }
            set
            {
                _array.Update(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _array.TryGetvalue(key, out value);
        }

        public void Clear()
        {
            _array.Clear();
            _count = 0;
        }

        public int Count => _count;
    }
}
