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
    /// Defines methods for Rotor operations
    /// </summary>
    public interface IRotor
	{
        /// <summary>
        /// Encode/Decode byte
        /// </summary>
        /// <param name="value">The value.</param>
        ///  /// <param name="encode">Bool mode of operation "true" is encode and "false" is decode</param>
        /// <returns>Return encode/decode byte</returns>
        byte ProcessByte(byte value, bool encode = true);

        /// <summary>
        /// Turn a rotor
        /// </summary>
        void Turn();

        /// <summary>
        /// Resets back to its start state. 
        /// </summary>
        void Reset();

        /// <summary>
        /// Clear all settings
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        int StartPosition
		{
			get;
			set;
		}

        /// <summary>
        /// Gets current position of the rotor.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        int Position
		{
			get;
		}

        /// <summary>
        /// Gets or sets the turn direction.
        /// </summary>
        /// <value>
        /// The turn direction.
        /// </value>
        RotorTurnDirection TurnDirection
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the substitution set.
        /// </summary>
        /// <value>
        /// The substitution set.
        /// </value>
        EndPointPair<byte,byte>[] SubstitutionSet
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the next rotor.
        /// </summary>
        /// <value>
        /// The next rotor.
        /// </value>
        IRotor NextRotor
		{
			get;
			set;
		}

        /// <summary>
        /// Gets a value indicating whether this <see cref="IRotor"/> is cycled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cycled; otherwise, <c>false</c>.
        /// </value>
        bool Cycled
		{
			get;
		}

        /// <summary>
        /// Gets the incrementation.
        /// </summary>
        /// <value>
        /// The incrementation.
        /// </value>
        int Incrementation
		{
			get; set;
		}
	}
}
