/*
	Software Developer: Fred Ekstrand 
    Copyright (C) 2016 by: Fred Ekstrand


    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
	to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE SOFTWARE DEVLOPER BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
	IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

    Except as contained in this notice, the name of the software developer shall not be used in advertising or otherwise to promote the sale, 
	use or other dealings in this "Software" without prior written authorization from the software developer.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EnigmasSecurity
{
    /// <summary>
    /// This class calculates the cryptographic strength of Enigma.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            BigInteger result = 1;
            result *= PlugboardStrength();
            Console.WriteLine("");
            result *= RotorStrength();
            Console.WriteLine("");
            result *= RotorInitialPositions();
            Console.WriteLine("");
            result *= ReflectorStrength();
            Console.WriteLine("");
            result *= RotorTurning();

            Console.WriteLine("");
            Console.WriteLine("Everything together: " + result.ToString("E"));
            Console.ReadLine();
        }

        /// <summary>
        /// Plugboard strength.
        /// </summary>
        /// <returns>Return the calculated result</returns>
        /// <remarks>Return value is all 256 byte values for maximum strength</remarks>
        public static BigInteger PlugboardStrength()
        {// cryptographic strength.

            BigInteger result = 0; 
            int plugPairs = 128;        // The maximum number of transposition pairs

            Console.WriteLine("Plugboard".PadCenter(28));
            Console.WriteLine("Num Pairs\tCombinations");

            for (int i = 0; i <= plugPairs; i++)
            {
                result += Combination(256, (2 * i)) * DoubleFactorial(((2 * i) - 1));
                int numPairs = ((2 * i) / 2);
                Console.WriteLine(numPairs.ToString().PadCenter(9) + "\t" + result.ToString("E").PadCenter(12));
            }
            return result;
        }

        /// <summary>
        /// Rotors strength.
        /// </summary>
        /// <returns>Return the calculated result</returns>
        public static BigInteger RotorStrength() 
        {
            Console.WriteLine("Single Rotor: " + Factorial(256).ToString("E") + " combinations.");
            BigInteger result = 1;
            for(int i = 0; i < 3; i++)
            {
                result *= (Factorial(256) - i);
            }
            Console.WriteLine("With Three Rotors: " + result.ToString("E") + " combinations.");
            return result;
        }

        /// <summary>
        /// Reflectors strength.
        /// </summary>
        /// <returns>Return calculated result</returns>
        /// <remarks>Similar to rotors</remarks>
        public static BigInteger ReflectorStrength() 
        {
            BigInteger result = Combination(256, 256) * DoubleFactorial((256 - 1));
            Console.WriteLine("Reflector: " + result.ToString("E") + " combinations");
            return result;
        }

        /// <summary>
        /// Rotors initial positions.
        /// </summary>
        /// <returns>Return calculated results</returns>
        /// <remarks>Starting positions of the rotor.</remarks>
        public static BigInteger RotorInitialPositions()
        {
            BigInteger result = Power(256, 3);
            Console.WriteLine("Initial rotor Positions: " + result.ToString("E"));
            return result;
        }

        // simple rotation after 256 encryption on the first rotor.        
        /// <summary>
        /// Rotors turning.
        /// </summary>
        /// <returns>Returns the calculated result</returns>
        /// <remarks>
        /// Calculation is based on simple cycled rotation for each rotor.
        /// </remarks>
        public static BigInteger RotorTurning() 
        {
            BigInteger result = 0;
            result = Power(256, 2);
            Console.WriteLine("Rotor turn: " + result.ToString("E"));
            return result;
        }

        /// <summary>
        /// Factorial the given integer value 'n'.
        /// </summary>
        /// <param name="n">Integer value</param>
        /// <returns>Returns the factorial of the given value</returns>
        /// <exception cref="System.ArgumentException">Non-negative integer only</exception>
        public static BigInteger Factorial(int n)
        {
            if(n < 0)
            {
                throw new ArgumentException("Non-negative integer only");
            }

            BigInteger result = 1;

            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        /// <summary>
        /// Double Factorial the given integer value 'n'.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns>Returns the double factorial of the given value.</returns>
        public static BigInteger DoubleFactorial(int n)
        {
            BigInteger result = 1;

            for (int i = n; i > 0; i -= 2)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Combinations 
        /// </summary>
        /// <param name="n">Integer number of elements.</param>
        /// <param name="k">Integer number to choose from 'n'.</param>
        /// <returns></returns>
        public static BigInteger Combination(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        /// <summary>
        /// Permutations
        /// </summary>
        /// <param name="n">Integer number of elements.</param>
        /// <param name="k">Integer number to choose from 'n'.</param>
        /// <returns>Returns the permutation from 'k' chooses from 'n'.</returns>
        public static BigInteger Permutation(int n, int k)
        {
            return Factorial(n) / Factorial((n - k));
        }

        /// <summary>
        /// Powers
        /// </summary>
        /// <param name="base">BigInteger base value.</param>
        /// <param name="exponent">BigInteger exponent value.</param>
        /// <returns>Return the calculated Power from base and exponent.</returns>
        public static BigInteger Power(BigInteger @base, int exponent)
        {
            BigInteger result = @base;
            for(int i = 0; i < exponent; i++)
            {
                result *= @base;
            }
            return result;
        }
    }
}
