using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    public sealed class Atom<T> where T : class
    {
        private volatile T _value;
        public Atom(T value)
        {
            _value = value;
        }

        public T Value => _value;

        public T Swap(Func<T, T> factory)
        {
            T original, temp;
            do
            {
                original = _value;
                temp = factory(original);
            }
            while (Interlocked.CompareExchange(ref _value, temp, original) != original);
            return original;
        }
    }

    static class FuncExtensionMethods
    {
        static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
          => (n) => g(f(n));

        private static readonly Dictionary<Tuple<Type, Type>, object> __cache = new Dictionary<Tuple<Type, Type>, object>();
        public static Func<T, R> Memoize<T, R>(this Func<T, R> func)
            where T : IComparable
        {
            Dictionary<T, R>  cache;
            var types = Tuple.Create(typeof(T), typeof(R));
            if (!__cache.ContainsKey(types))
            {
                cache = new Dictionary<T, R>();
                __cache.Add(types, cache);
            }
            else
            {
                cache = (Dictionary<T, R>)__cache[types];
            }

            return arg => {
                if (cache.ContainsKey(arg))
                    return cache[arg];
                return (cache[arg] = func(arg));
            };
        }

        public static Func<T, R> MemoizeThreadSafe<T, R>(this Func<T, R> func)
            where T : IComparable
        {
            ConcurrentDictionary<T, R>  cache;
            var types = Tuple.Create(typeof(T), typeof(R));
            if (!__cache.ContainsKey(types))
            {
                cache = new ConcurrentDictionary<T, R>();
                __cache.Add(types, cache);
            }
            else
            {
                cache = (ConcurrentDictionary<T, R>)__cache[types];
            }

            return arg => cache.GetOrAdd(arg, func);
        }

        private static readonly Dictionary<Tuple<Type, Type>, object> __lazyCache = new Dictionary<Tuple<Type, Type>, object>();
        public static Func<T, R> MemoizeLazyThreadSafe<T, R>(this Func<T, R> func)
            where T : IComparable
        {
            ConcurrentDictionary<T, Lazy<R>>  cache;
            var types = Tuple.Create(typeof(T), typeof(R));
            if (!__lazyCache.ContainsKey(types))
            {
                cache = new ConcurrentDictionary<T, Lazy<R>>();
                __lazyCache.Add(types, cache);
            }
            else
            {
                cache = (ConcurrentDictionary<T, Lazy<R>>)__lazyCache[types];
            }

            return arg => cache.GetOrAdd(arg, a => new Lazy<R>(() => func(a))).Value;
        }
    }
}
