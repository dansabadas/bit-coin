using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    struct Complex
    {
        public Complex(float real, float imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public float Imaginary { get; }

        public float Real { get; }

        public float Magnitude => (float)Math.Sqrt(Real * Real + Imaginary * Imaginary);
        public static Complex operator +(Complex c1, Complex c2) => new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        public static Complex operator *(Complex c1, Complex c2)
            => new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c1.Imaginary * c2.Real);
    }

    public sealed class FList<T>
    {
        private FList(T head, FList<T> tail)
        {
            Head = head;
            Tail = tail.IsEmpty
                ? Empty
                : tail;
            IsEmpty = false;
        }

        private FList()
        {
            IsEmpty = true;
        }

        public T Head { get; }

        public FList<T> Tail { get; }

        public bool IsEmpty { get; }

        public static FList<T> Cons(T head, FList<T> tail)
        {
            return tail.IsEmpty
                ? new FList<T>(head, Empty)
                : new FList<T>(head, tail);
        }

        public FList<T> Cons(T element)
        {
            return Cons(element, this);
        }

        public static readonly FList<T> Empty = new FList<T>();
    }

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
        static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g) => (n) => g(f(n));

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

        static IEnumerable<R>  Map<T, R>(IEnumerable<T> sequence, Func<T, R> projection)
        {
            return sequence.Aggregate(new List<R>(), (acc, item) => {
                acc.Add(projection(item));
                //acc.AddRange(new List<R> { projection(item) });
                return acc;
            });
        }

        static int Max(IEnumerable<int> sequence)
        {
            return sequence.Aggregate(0, (acc, item) => Math.Max(item, acc));
        }

        static IEnumerable<T> Filter<T>(IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            return sequence.Aggregate(new List<T>(), (acc, item) =>
            {
                if (predicate(item))
                {
                    acc.Add(item);
                }

                return acc;
            });
        }

        static int Length<T>(IEnumerable<T> sequence) => sequence.Aggregate(0, (acc, _) => acc + 1);

        static TSource Reduce<TSource>(this ParallelQuery<TSource> source, Func<TSource, TSource, TSource> reduce) =>
            ParallelEnumerable.Aggregate(source, (item1, item2) => reduce(item1, item2));

        static TValue Reduce<TValue>(this IEnumerable<TValue> source, TValue seed, Func<TValue, TValue, TValue> reduce) =>
            source.AsParallel()
                .Aggregate(
                    seed: seed,
                    updateAccumulatorFunc: (local, value) => reduce(local, value),
                    combineAccumulatorsFunc: (overall, local) => reduce(overall, local),
                    resultSelector: overall => overall);
    }
}
