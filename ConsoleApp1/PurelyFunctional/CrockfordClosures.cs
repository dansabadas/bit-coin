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

      Console.ReadLine();
    }
  }
}
