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
using System.Text;

namespace Ekstrand.Encryption.Ciphers
{
    /// <summary>
    /// Defines an end point pair.
    /// </summary>
    /// <typeparam name="T1">Type T1</typeparam>
    /// <typeparam name="T2">Type T2</typeparam>
    [Serializable]
    public class EndPointPair<T1, T2> where T1 : IComparable<T1>, IComparable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointPair{T1, T2}"/> class.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public EndPointPair()
		{
            SideA = default(T1);
            SideB = default(T2);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointPair{T1, T2}"/> class.
        /// </summary>
        /// <param name="sideA">Type T1</param>
        /// <param name="sideB">Type T2</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public EndPointPair(T1 sideA, T2 sideB):this()
		{
            SideA = sideA;
            SideB = sideB;
		}

        /// <summary>
        /// Gets or sets the SideA
        /// </summary>
        /// <value>
        /// Type T1
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public T1 SideA
		{
            get; set;
		}

        /// <summary>
        /// Gets or sets the SideB.
        /// </summary>
        /// <value>
        /// Type T2
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public T2 SideB
		{
            get; set;
		}

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns -1 if less than or null, 0 if equal or 1 if greater than.</returns>
        public int CompareTo(object obj)
        {
            byte item = (byte)obj;
            return this.SideA.CompareTo(item);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">Type EndPointPair&lt;T1, T2&gt;</param>
        /// <returns>Returns -1 if less than or null, 0 if equal or 1 if greater than.</returns>
        public int CompareTo(T1 other)
        {  
            if (other == null)
            {
                return -1;
            }
            return other.CompareTo(this.SideA);
        }
    }
}
