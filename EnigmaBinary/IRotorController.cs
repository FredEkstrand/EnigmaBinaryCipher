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
using System.Collections.ObjectModel;
using System.Text;

namespace Ekstrand.Encryption.Ciphers
{
    /// <summary>
    /// Defines methods for EntryRotor, Rotors, Reflector, and Turns operations
    /// </summary>
    public interface IRotorController
	{
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Encoding/Decoding of byte.
        /// </summary>
        /// <param name="value">Byte to be encoded/decoded</param>
        /// <param name="encode">Bool <c>true</c> to encode byte or <c>false</c> to decode byte.</param>
        /// <returns>Return encoded/decoded byte</returns>
        byte ProcessByte(byte value, bool encode = true );

        /// <summary>
        /// Resets controller back to starting position
        /// </summary>
        void Reset();

        /// <summary>
        /// Clears all settings for this controller
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets or sets the entry rotor.
        /// </summary>
        /// <value>
        /// The entry rotor.
        /// </value>
        IRotor EntryRotor
		{
			get;
			set;
		}

        /// <summary>
        /// Gets the collection of rotors
        /// </summary>
        /// <value>
        /// The rotors.
        /// </value>
        Collection<IRotor> Rotors
		{
			get;
		}

        /// <summary>
        /// Gets or sets the reflector.
        /// </summary>
        /// <value>
        /// The reflector.
        /// </value>
        IRotor Reflector
		{
			get;
			set;
		}

        /// <summary>
        /// Gets the turns.
        /// </summary>
        /// <value>
        /// The turns.
        /// </value>
        Collection<ITurn> Turns
		{
			get;
		}
	}
}
