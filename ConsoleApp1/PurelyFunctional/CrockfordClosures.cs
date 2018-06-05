using System;

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
      log(yell());
      log(yell());

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

      var x1 = new int[0];

      funky(x1);
      log(x1); //a. throw exception b. =null c. = int[0] 

      //2. what is value of x?
      void swap(int a, int b)
      {
        var temp = a;
        a = b;
        b = temp;
      }

      int x2 = 1, y2 = 2;
      swap(x2, y2);
      log(x2); //a. 1 b.2 c.null d. throw exc

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
        double func(){
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
        double innerAddFunction(double b)
         {
          return add(a, b); // first rule of functional programming
          // or return a + b
        };

        return innerAddFunction;
      }

      log(addf(3)(4));  // 7

      //6. closure with 3 args: 2 numerical and one function which handles the 2 numbers - higher-order function (that receives other functions as params)
      // write a function liftf that takes a binary function and makes it callable with two invocations
      Func<double, Func<double, double>> liftf(Func<double, double, double>f)
      {
        Func<double, double> outerFunction(double a1){
          double innerFunction(double b1){
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
      Func<double, double> curry(Func<double, double, double>f, double a)
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
        double innerFunction(double b){
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
        double innerFunction(double a, double b){
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
        double function(double a, double b, double c){
          var s = bf1(a, b);
          return bf2(s, c);
        }

        return function;
      }

      log(composeb(add, mul)(2, 3, 7)); //35

      //13. return undefined: 2 schools of thought: either save space, or provide documented code for the contract to return undefined
      // write a limit func that allows a binary func to be called a limited number of times
      Func<double, double, double?> limit(Func<double, double, double> bf1, int l)
      {
        double? function(double a, double b) {
          if (l >= 1)
          {
            l -= 1;
            return bf1(a, b);
          }

          return null;
        }

        return function;
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
        int function(){
          return seed++;
        }

        return function;
      }

      var index = from(0);
      log(index()); // 0
      log(index()); // 1
      log(index()); // 2

      //15. write a 'to' func which takes a generator (see pb. above) and an end val and returns a generator that will produce numbers up to that limit
      Func<int?> to(Func<int> generator, int endVal)
      {
        int? function(){
          var nextVal = generator();
          if (nextVal < endVal)
          {
            return nextVal;
          }

          return null;
        }

        return function;
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
        double? function(){
          var nextVal = generatorFunc();
          if (nextVal != null)
          {
            return arr[nextVal.Value];
          }

          return null;
        }

        return function;
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
        double? function(){
          var nextVal = generator();
          if (nextVal != null)
          {
            return arr[nextVal.Value];
          }

          return null;
        }

        return function;
      }

      ele = element(new double[] { 'a', 'b', 'c', 'd' });
      log((char)ele());//a
      log((char)ele());//b
      log((char)ele());//c
      log((char)ele());//d
      log(ele());//NULL

      Console.ReadLine();
    }
  }
}
