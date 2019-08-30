using System;
using System.Collections.Generic;

namespace ConsoleApp1.PurelyFunctional
{
  public class CrockfordClosures
  {
    public void Run()
    {
      void log(object messages)
      {
        if (messages == null)
        {
          Console.WriteLine("NULL");
        }

        Console.WriteLine(messages);
      }

      Action<object> log2 = (object messages) =>
      {
        if (messages == null)
        {
          Console.WriteLine("NULL");
        }

        Console.WriteLine(messages);
      };

      Func<int> green()
      {
        var a = 2;
        int yellow()
        {
          return a++;
        }

        return yellow;
      }

      var yell = green();
      yell();
      Console.WriteLine(yell());

      dynamic green2()  // or object
      {
        var a = 6;
        Func<int> yellow2 = () =>
        {
          return a++;
        };

        return yellow2;
      }

      var yell2 = green2();
      log(yell2());
      log(yell2());

      //1. ask people first their opinion! => funky coompletely useless
      void funky(int[] o)
      {
        o = null;
      }

      var x1 = new int[1] { 2 };

      funky(x1);
      Console.WriteLine(x1[0]); //a. throw exception b. =null c. = int[0] 


      //2. what is value of x?
      void swap(int a, int b)
      {
        var temp = a;
        a = b;
        b = temp;
      }

      int x2 = 1, y2 = 2;
      swap(x2, y2);
      Console.WriteLine(x2); //a. 1 b.2 c.null d. throw exc

      int[] a2 = new int[2] { 1, 2 };
      log(a2);
      a2 = null;
      log(a2);

      //3. write 3 binary functions: add, sub and mul functions (no tricks!)
      double add(double a, double b)
      {
        return a + b;
      }
      double sub(double a, double b)
      {
        return a - b;
      }
      double mul(double a, double b)
      {
        return a * b;
      }

      log(add(3, 4));
      log(sub(3, 4));
      log(mul(3, 4));

      //4. closure with 1 arg - first interesting pb! :) - who got it? don't be discouraged
      // write a func identityf that takes an argument
      // and returns a function that returns that argument
      Func<double> identityf(double o)
      {
        double func() {
          return o;
        }

        return func;
      }

      var three = identityf(3);
      log(three()); // 3

      //5. closure with 2 args - suggested mandatory for hiring interviews! RaphaelJS
      // write a function addf that addstwo numbers from two invocations 
      Func<double, double> addf(double a)
      {
        return (double b) =>
        {
          return add(a, b); // first rule of functional programming :) reuse the functions
          // or return a + b
        };

        //return innerAddFunction;
      }

      log(addf(3)(4));  // 7

      //6. closure with 3 args: 2 numerical and one function which handles the 2 numbers - higher-order function (that receives other functions as params)
      // write a function liftf that takes a binary function and makes it callable with two invocations
      Func<double, Func<double, double>> liftf(Func<double, double, double> f)
      {
        Func<double, double> outerFunction(double a1) {
          double innerFunction(double b1) {
            return f(a1, b1);
          };

          return innerFunction;
        };

        return outerFunction;
      }

      var addf1 = liftf(add);
      log(addf1(3)(4)); // 7
      log(liftf(mul)(5)(6));  // 30

      //7. curry function - Haskell curry
      // extra credit for liftf usage => transforming a function that takes multiple args into multiple functions that take one arg => is called currying

      // write a function curry that takes two arguments: a binary function and a 'regular' argument,
      // and returns a function that can take a second argument (and that invokes the binary function)
      Func<double, double> curry(Func<double, double, double> f, double a)
      {
        //return liftf(f)(a);
        double innerFunction(double b)
        {
          return f(a, b);
        }

        return innerFunction;
      }

      var add3 = curry(add, 3);
      log(add3(4));   // 7
      log(curry(mul, 5)(6));  //30

      //8. let the functions to the work! - the first rule of functional programming
      //  write increment function using existing functions
      // do not create any new function but use the existing functions
      var inc = addf(1);
      var inc2 = liftf(add)(1);
      var inc3 = curry(add, 1);

      log(inc(5));  // 6
      log(inc2(5)); // 6
      log(inc3(5)); // 6

      //9. write a func called "twice" 
      // that takes a binary func and return an unary func that passes its argument to the binary function twice
      Func<double, double> twice(Func<double, double, double> binaryFunc)
      {
        double innerFunction(double b) {
          return binaryFunc(b, b);
        }

        return innerFunction;
      }

      var doubl = twice(add);
      log(doubl(11));
      var square = twice(mul);
      log(square(11));

      //10. write reverse, a func which reverses the arguments of a binary function
      Func<double, double, double> reverse(Func<double, double, double> binary)
      {
        double innerFunction(double a, double b) {
          return binary(b, a);
        }

        return innerFunction;
      }

      var bus = reverse(sub);
      log(bus(3, 2));

      //11. like UNIX pipes
      // write a func composeu that takes two unary functions 
      // and returns a unary func that calls them both (composing them)
      Func<double, double> composeu(Func<double, double> unaryFunc1, Func<double, double> unaryFunc2)
      {
        double innerFunction(double a)
        {
          return unaryFunc2(unaryFunc1(a));
        }

        return innerFunction;
      }

      log(composeu(doubl, square)(5));  // 100

      //12. write a func composeb that takes two binary funcs 
      // and returns a func that calls them both (composing them)
      Func<double, double, double, double> composeb(Func<double, double, double> bf1, Func<double, double, double> bf2)
      {
        double innerFunction(double a, double b, double c)
        {
          var s = bf1(a, b);
          return bf2(s, c);
        }

        return innerFunction;
      }

      log(composeb(add, mul)(2, 3, 7)); //35

      //13. return undefined: 2 schools of thought: either save space, or provide documented code for the contract to return undefined
      // write a limit func that allows a binary func to be called a limited number of times
      Func<double, double, double?> limit(Func<double, double, double> bf1, int l)
      {
        double? innerFunction(double a, double b)
        {
          if (l >= 1)
          {
            l -= 1;
            return bf1(a, b);
          }

          return null;
        }

        return innerFunction;
      }

      var add_ltd = limit(add, 1);
      log(add_ltd(3, 4)); // 7
      log(add_ltd(3, 4)); // null
      var mul_ltd = limit(mul, 2);
      log(mul_ltd(3, 4)); //12
      log(mul_ltd(3, 4)); //12
      log(mul_ltd(3, 4)); //null

      //14. write a 'from' func that produces a generator that will produce a serioes of values
      Func<int> from(int seed)
      {
        int innerFunction()
        {
          return seed++;
        }

        return innerFunction;
      }

      var index = from(0);
      log(index()); // 0
      log(index()); // 1
      log(index()); // 2

      //15. write a 'to' func which takes a generator (see pb. above) and an end val and returns a generator that will produce numbers up to that limit
      Func<int?> to(Func<int> generator, int endVal)
      {
        int? innerFunction()
        {
          var nextVal = generator();
          if (nextVal < endVal)
          {
            return nextVal;
          }

          return null;
        }

        return innerFunction;
      }

      var index2 = to(from(1), 3);
      log(index2()); //1
      log(index2()); //2
      log(index2()); //NULL

      //16. write a 'fromTo' that produces a generator that will produce values in a range
      Func<int?> fromTo(int startVal, int endVal)
      {
        return to(from(startVal), endVal);
      }

      var index3 = fromTo(0, 3);
      log(index3()); //0
      log(index3()); //1
      log(index3()); //2
      log(index3()); //NULL

      //17. better to use this version than the more direct: var next = generatorFunc(); return arr[next]; because still accidentally works cause arr[undefined] does not exist
      // write an 'element' func that takes an array and a generator and returns a generator that will produce elements from the array
      Func<double?> element0(double[] arr, Func<int?> generatorFunc)
      {
        double? innerFunction()
        {
          var nextVal = generatorFunc();
          if (nextVal != null)
          {
            return arr[nextVal.Value];
          }

          return null;
        }

        return innerFunction;
      }

      var ele = element0(new double[] { 'a', 'b', 'c', 'd' }, fromTo(1, 3));
      log((char)ele()); //b
      log((char)ele()); //c
      log(ele()); //NULL

      //18. modify the 'element' function so that the generator argument is optional. 
      // If generator not provided => then each element of the array will be provided
      Func<double?> element(double[] arr, Func<int?> generatorFunc = null)
      {
        var generator = generatorFunc != null ? generatorFunc : fromTo(0, arr.Length);
        double? innerFunction()
        {
          var nextVal = generator();
          if (nextVal != null)
          {
            return arr[nextVal.Value];
          }

          return null;
        }

        return innerFunction;
      }

      ele = element(new double[] { 'a', 'b', 'c', 'd' });
      log((char)ele());//a
      log((char)ele());//b
      log((char)ele());//c
      log((char)ele());//d
      log(ele());//NULL

      //19. write a 'collect' func that takes a generator and an array and produces a function  
      // that will collect the results (made internally by the generator at every invocation of collect) in the array
      Func<int?> collect(Func<int?> generator, List<int> arr)
      {
        int? innerFunction()
        {
          var res = generator();
          if (res != null)
          {
            arr.Add(res.Value);
          }

          return res;
        }

        return innerFunction;
      }

      var array = new List<int>();
      var col = collect(fromTo(0, 2), array);

      log(col()); //0
      log(col()); //1
      log(col()); //NULL
      log(array.Count); //[0,1]

      //20. named third is optional - hint: you need a loop. also show the commented recursive version below properly implemented now in ES6 (recursive vs iterative approach)
      // write a filter func that takes a generator and a predicate 
      // and produces a generator that produces only the values approved by the predicate
      //Func<int?> filter(Func<int?> generator, Func<int, bool> predicate)
      //{
      //  int? innerFunction()
      //  {
      //    int? generatedValue;
      //    do
      //    {
      //      generatedValue = generator();
      //    } while (generatedValue != null && !predicate(generatedValue.Value));

      //    return generatedValue;
      //  }

      //  return innerFunction;
      //}

      Func<int?> filter(Func<int?> generator, Func<int, bool> predicate)
      {
        int? recur()
        {
          var value = generator();
          if (value == null || predicate(value.Value))
          {
            return value;
          }

          return recur();
        }

        return recur;
      }

      var fil = filter(fromTo(0, 5),
        (int value) => value % 3 == 0);
      log(fil()); // 0
      log(fil()); // 3
      log(fil()); // null

      //21. write a 'concat' function which takes 2 generators and produces a generator that combines the sequences
      Func<int?> concat(Func<int?> gen1, Func<int?> gen2)
      {
        var gen = gen1;
        int? innerFunction()
        {
          var value = gen();
          if (value != null)
          {
            return value;
          }
          gen = gen2;
          return gen();
        }

        return innerFunction;
      }

      var con = concat(fromTo(0, 3), fromTo(0, 2));
      log(con()); //0
      log(con()); //1
      log(con()); //2
      log(con()); //0
      log(con()); //1
      log(con()); //null

      //22. make a function 'gensymf' that makes a function that generates unique symbols
      Func<string> gensymf(string symbol)
      {
        var i = 0;
        string innerFunction()
        {
          i += 1;
          return symbol + i;
        }

        return innerFunction;
      }

      var geng = gensymf("G");  // these are prefixes (G series and H series)
      var genh = gensymf("H");

      log(geng());//G1
      log(genh());//H1
      log(geng());//G2
      log(genh());//H2

      //23. this is a factory fibonacci generator. this is hard. no recursivity! elegant iterativity!
      //first example is a little too big and we do not need the i. the second sample is the more optimal
      // make a fibonaccif that returns a generator function that will return the next fibonacci number
      Func<int> fibonaccif0(int i1, int i2)
      {
        int i = -1, next;
        int innerFunction()
        {
          i += 1;
          if (i == 0)
          {
            return i1;
          }
          else if (i == 1)
          {
            return i2;
          }

          next = i1 + i2;
          i1 = i2;
          i2 = next;
          return next;
        }

        return innerFunction;
      }

      var fib = fibonaccif0(0, 1);
      log(fib());//0
      log(fib());//1
      log(fib());//1
      log(fib());//2
      log(fib());//3
      log(fib());//5

      Func<int> fibonaccif(int i1, int i2)
      {
        int innerFunction()
        {
          var next = i1;
          i1 = i2;
          i2 += next;
          return next;
        }

        return innerFunction;
      }

      fib = fibonaccif(0, 1);
      log(fib());
      log(fib());
      log(fib());
      log(fib());
      log(fib());
      log(fib());

      //24. Hint: no 'this', no global variables! :) OOP combined with functional programming/closures
      // write a counter function that returns an object containing 2 functions that implement 
      // an up/down counter, hiding the counter
      dynamic counter(int seed)
      {
        return new
        {
          up = new Func<int>(() => {
            seed += 1;
            return seed;
          }),
          down = (Func<int>)(() => {
            seed -= 1;
            return seed;
          })
        };
      }

      var obj = counter(10);
      var up = obj.up;
      var down = obj.down;

      log(up());  //11
      log(down());//10
      log(down());//9
      log(up());  //10

      //25. this sounds much more complex than it actually is!!! :)
      // make a revocable function that takes a binary function and returns an object containing 
      // an invoke function that can invoke the binary function and a revoke function that disables the invoke function
      dynamic revocable(Func<double, double, double> binaryF)
      {
        return new
        {
          invoke = new Func<double, double, double?>((a, b) =>
          {
            return binaryF == null ? (double?)null : binaryF(a, b);
          }),
          revoke = new Action(() =>
          {
            binaryF = null;
          })
        };

        //alternate impl:
        //var isRevoked = false;
        //return new 
        //{
        //  invoke = new Func<double, double, double?>((a, b) => {
        //    if (!isRevoked)
        //    {
        //      return binaryF(a, b);
        //    }

        //    return null;
        //  }),
        //  revoke = new Action(() => {
        //    isRevoked = true;
        //  })
        //};
      }

      var rev = revocable(add);
      var add_rev = rev.invoke;
      log(add_rev(3, 4)); //7
      rev.revoke();
      log(add_rev(3, 4)); // null

      //26. 
      // write a function 'm' that takes a value and an optional source string and returns them in an object
      dynamic m(double value, dynamic source = null)
      {
        return new
        {
          value,
          source = source != null && source.GetType() == typeof(string) ? source : value.ToString()
        };
      }

      log(m(1));
      log(m(Math.PI, "pi"));

      //27. who did it? the hard way and with the first rule of func programming...
      // write a func 'addm' that takes two m objects and returns an m object 
      //(m have 2 properties. one is mandatory 'value' and the other is optional: 'source')
      dynamic addm(dynamic m1, dynamic m2)
      {
        return m(
            m1.value + m2.value,
            "(" + m1.source + "+" + m2.source + ")"
        );
      }

      log(addm(m(1), m(3)));  //{"value":4,"source":"(1+3)"}
      log(addm(m(1), m(Math.PI, "pi")));  //{"value":4.141592653589793,"source":"(1+pi)"}

      //28. this is a monad!
      // write a function 'liftm' that takes a binary function and a string and returns a function that acts on m objects
      Func<dynamic, dynamic, dynamic> liftm(Func<double, double, double> binary, string sign)
      {
        dynamic function(dynamic m1, dynamic m2)
        {
          return m(
              binary(m1.value, m2.value),
              "(" + m1.source + sign + m2.source + ")"
          );
        }

        return function;
      }

      var addm1 = liftm(add, "+");
      log(addm1(m(1), m(3)));  //{"value":4,"source":"(1+3)"}
      log(liftm(mul, "*")(m(4), m(3))); //{"value":12,"source":"(4*3)"

      //29. 
      // modify liftm so that the functions it produces can accept arguments that are either numbers or m objects
      Func<dynamic, dynamic, dynamic> liftm2(Func<double, double, double> binary, string sign)
      {
        dynamic function(dynamic a, dynamic b)
        {
          double num;
          if (double.TryParse(a.ToString(), out num))
          {
            a = m(a);
          }
          if (double.TryParse(b.ToString(), out num))
          {
            b = m(b);
          }

          return m(
              binary(a.value, b.value),
              "(" + a.source + sign + b.source + ")"
          );
        }

        return function;
      }

      var addm2 = liftm2(add, "+");
      log(addm2(1, 3));//{"value":4,"source":"(1+3)"}

      //30. a simple array expression is an array in which the first element is a function and the remaining elements are arguments to the function
      // assume the sae has always only 3 elements. two different implementations below
      // write a function 'exp' that evaluates simple array expressions.
      dynamic exp(dynamic value)
      {
        return value.GetType().IsArray
          ? value[0](value[1], value[2])
          : value;
      }

      Func<double, double, double> mulFunc = mul;
      var sae = new dynamic[] { mulFunc, 5, 11 };
      log(exp(sae));  // 55
      log(exp(42));   //42

      //31. there are 2 implementations below!. this time we can have 3 or 2 elements in the array
      // it resembles LISP!
      //31. Modify exp to evaluate nested array expressions
      dynamic exp2(dynamic value)
      {
        if (!value.GetType().IsArray) return value;
        return value.Length == 3
          ? value[0](exp2(value[1]), exp2(value[2]))
          : value[0](exp2(value[1]));
      }

      Func<double, double> sqrt = Math.Sqrt;
      Func<double, double, double> addFunc = add;
      var nae = new dynamic[] 
      {
          sqrt,
          new dynamic[]
          {
              addFunc,
              new dynamic[] 
              {
                square,
                3
              },
              new dynamic[] 
              {
                square,
                4
              }
          }
      };
      log(exp2(nae));  // 5

      //32. (hint: it involves a function which returns itself somehow)
      // compiler bug detected! mandatory to provide null to inner function!
      // write a function 'addg' which adds from many invocations until it sees an empty (or in C# a NULL) invocation 
      dynamic addg(double? first = null)
      {
        dynamic more(double? next = null)
        {
          if (next == null)
          {
            return first;
          }

          first += next;
          //Func<double?, dynamic> moreC = more;
          return (Func<double?, dynamic>)more;  //moreC
        }

        if (first != null)
        {
          //Func<double?, dynamic> moreC = more;
          return (Func<double?, dynamic>)more;  //moreC;// 
        }

        return null;
      }

      log(addg());            //null
      log(addg(2)(null));         //2 
      log(addg(2)(7)(null));      //9
      log(addg(3)(0)(4)(null));   //7
      log(addg(1)(2)(4)(8)(null));//15

      //33. 
      // write a func 'liftg' that will take a binary function and apply it to many invocations
      dynamic liftg(Func<double, double, double> binaryF)
      {
        dynamic function(double? first)
        {
          if (first == null)
          {
            return null;
          }

          dynamic more(double? next = null)
          {
            if (next == null)
            {
              return first;
            }
            first = binaryF(first.Value, next.Value);
            return (Func<double?, dynamic>)more;
          }

          return (Func<double?, dynamic>)more;
        }

        return (Func<double?, dynamic>)function;
      }

      log(liftg(mul)(null));              //null
      log(liftg(mul)(3)(null));           //3
      log(liftg(mul)(1)(0)(4)(8)(null));  //0
      log(liftg(mul)(1)(2)(4)(8)(null));  //64

      //34. the 2 impl
      // write a function 'arrayg' that will build a list from many invocations
      dynamic arrayg(double? first = null)
      {
        var returnArr = new List<double>();
        dynamic more(double? next = null)
        {
          if (next == null)
          {
            return returnArr;
          }
          returnArr.Add(next.Value);
          return (Func<double?, dynamic>)more;
        }

        return more(first);
      }

      log(arrayg().Count);           //[]
      log(arrayg(3)(null).Count);        //[3]
      log(arrayg(3)(4)(5)(null).Count);  //[3,4,5]

      //35. 
      // make a 'continuize' function that takes a unary function and returns
      // a function that takes a callback and an argument
      Func<Action<double>, double, double> continuize(Func<double, double> unaryF)
      {
        double function(Action<double> callback, double arg){
          var result = unaryF(arg);
          callback(result);
          return result;
        }

        return function;
      }

      Action<double> log1 = (double number) => log(number);
      var sqrtc = continuize(Math.Sqrt);
      sqrtc(log1, 81); //9

      Console.ReadLine();
    }
  }
}
