using System;
using System.Collections.Generic;

namespace WebService
{
    public class KeyEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        protected readonly Func<T, TKey> keyExtractor;

        public KeyEqualityComparer(Func<T, TKey> keyExtractor)
        {
            this.keyExtractor = keyExtractor;
        }

        public virtual bool Equals(T x, T y)
        {
            TKey tKey = this.keyExtractor(x);
            return tKey.Equals(this.keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {
            return this.keyExtractor(obj).GetHashCode();
        }
    }
}