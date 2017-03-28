using System;

namespace WebService
{
    public class StrictKeyEqualityComparer<T, TKey> : KeyEqualityComparer<T, TKey>
    where TKey : IEquatable<TKey>
    {
        public StrictKeyEqualityComparer(Func<T, TKey> keyExtractor)
            : base(keyExtractor)
        {
        }

        public override bool Equals(T x, T y)
        {
            TKey tKey = this.keyExtractor(x);
            return tKey.Equals(this.keyExtractor(y));
        }
    }
}