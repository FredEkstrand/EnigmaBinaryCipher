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
    /// Abstract base class for cipher operations with the Plugboard, Rotors and Turns
    /// </summary>
    /// <seealso cref="ICipherController" />
    [Serializable]
    public abstract class AbstractCipherController : ICipherController
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected bool m_Initialized = false;

        protected const string ROTORCONTROLLER_INIT_ERROR = "RotorController";
        protected const string NOTINITIALIZED = "Not Initialized";
        protected const string PLUGBOARD_INIT_ERROR = "Plugboard";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCipherController"/> class.
        /// </summary>
        internal AbstractCipherController()
        {
            RotorController = null;
            Plugboard = null;
        }

        #endregion

        #region Methods      

        /// <summary>
        /// Initializes cipher controller sub-components
        /// </summary>
        /// <exception cref="System.NullReferenceException">RotorController</exception>
        public abstract void Initialize();

        /// <summary>
        /// Encode/Decode given byte
        /// </summary>
        /// <param name="value">Byte value to be encoded/decoded</param>
        /// <param name="encode">Boolean <c>true</c> to encode and <c>false</c> to decode.</param>
        /// <returns>
        /// Return an encoded/decoded byte
        /// </returns>
        public abstract byte ProcessByte(byte value, bool encode = true);

        /// <summary>
        /// Resets its components back to their start state.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public abstract void Clear();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rotor controller.
        /// </summary>
        /// <value>
        /// The rotor controller.
        /// </value>
        public IRotorController RotorController
		{
            get; set;
		}

        /// <summary>
        /// Gets or sets the plugboard.
        /// </summary>
        /// <value>
        /// The plugboard.
        /// </value>
        public IPlugboard Plugboard
		{
            get; set;
		}

        #endregion
    }
}
