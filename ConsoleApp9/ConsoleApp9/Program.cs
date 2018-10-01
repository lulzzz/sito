using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Program
    {
        static IEnumerable<BigInteger> IntGenerator()
        {
            var l = new BigInteger(2);

            while (true)
            {
                yield return l;
                l++;
            }
        }

        static IEnumerable<BigInteger> SquareGenerator()
        {
            return IntGenerator().Select(n => n * n);
        }

        static IEnumerable<BigInteger> ListTo(BigInteger n)
        {
            for (BigInteger i = 1; i <= n; i++)
            {
                yield return i;
            }
        }

        static BigInteger Factorial(BigInteger n) => ListTo(n).Aggregate((x, y) => x * y);

        static BigInteger FastFactorial(BigInteger n)
        {
            var half = n / 2;
            BigInteger r = n % 2 == 1 ? (half + 1) : 1;

            for (BigInteger k = 1; k <= half; k++)
            {
                r = r * k * (n - k + 1);
            }

            return r;
        }

        static void Main(string[] args)
        {
            // !N + 1 = M ^ 2;
            // !N = MOLTIPLICAZIONE DI (KN - K^2 + K) CON K DA 1 A N/2  
            // K (N - K + 1)

            for (int i = 2; i < int.MaxValue; i++)
            {
                if (Factorial(i) != FastFactorial(i))
                {
                    Console.WriteLine("FAILED !");
                }
            }

            foreach (var N in IntGenerator())
            {
                foreach (var M in IntGenerator())
                {
                    if (Factorial(N) + 1 == M * M)
                    {
                        Console.WriteLine($"{N} {M}");
                    }
                }
            }
            foreach (var v in IntGenerator())
            {
                Console.WriteLine(Factorial(v));
            }
            

            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
