using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsTryWhenWeAreAlive
{
    public class Set<T>: IEnumerable<T>
        where T: IComparable<T>
    {
        List<T> _items = new List<T>();

        public Set() { } 
        public Set(IEnumerable<T> items) { AddRange(items); } 

        public void Add(T item)
        {
            if (_items.Contains(item))
            {
                throw  new ArgumentException("Item already exists in the set");
            }

            _items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        // {2,3,4} & {3,4,5} returns {2,3,4,5}
        public Set<T> Union(Set<T> other)
        {
            var result = new Set<T>(_items);
            result.AddRangeSkipDuplicates(other._items);
            return result;
        }

        // {2,3,4} & {3,4,5} returns {3,4}
        public Set<T> Intersection(Set<T> other)
        {
            var result = new Set<T>();
            foreach (var item in other._items)
            {
                if (_items.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // {2,3,4} & {3,4,5} returns {2}
        public Set<T> Difference(Set<T> other)
        {
            var result = new Set<T>(_items);
            foreach (var item in other._items)
            {
                result.Add(item);
            }
            return result;
        }

        // {1,2,3} & {2,3,4} returns {1,4}
        public Set<T> SymetricDifference(Set<T> other)
        {
            Set<T> intersection = Intersection(other);
            Set<T> union = Union(other);

            return union.Difference(intersection);
        }

        private void AddRangeSkipDuplicates(List<T> itemsToAdd)
        {
            foreach (var itemToAdd in itemsToAdd)
            {
                if (_items.Contains(itemToAdd) == false)
                {
                    _items.Add(itemToAdd);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
