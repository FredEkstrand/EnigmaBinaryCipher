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
    /// Cipher operations encompassing Plugboard, Rotors and Turns
    /// </summary>
    /// <seealso cref="AbstractCipherController" />
    [Serializable]
    public class CipherController : AbstractCipherController
	{
        #region Constructor   
             
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherController"/> class.
        /// </summary>
        public CipherController():base()
		{

		}

        #endregion

        #region Methods 

        /// <summary>
        /// Initializes cipher controller sub-components
        /// </summary>
        /// <exception cref="System.NullReferenceException">RotorController</exception>
        public override void Initialize()
        {
            if (RotorController == null)
            {
                throw new NullReferenceException(ROTORCONTROLLER_INIT_ERROR);
            }

            if (Plugboard == null)
            {
                throw new NullReferenceException(PLUGBOARD_INIT_ERROR);
            }

            this.RotorController.Initialize();
            m_Initialized = true;
        }

        /// <summary>
        /// Encode/Decode given byte
        /// </summary>
        /// <param name="value">Byte value to be encoded/decoded</param>
        /// <param name="encode">Boolean <c>true</c> to encode and <c>false</c> to decode.</param>
        /// <returns>
        /// Return an encoded/decoded byte
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// RotorController
        /// or
        /// Plugboard
        /// </exception>
        public override byte ProcessByte(byte value, bool encode = true)
		{
            if(!m_Initialized)
            {
                throw new InvalidOperationException(NOTINITIALIZED);
            }

            byte result;

            if (encode)
            {
                result = RotorController.ProcessByte(value);            
                return Plugboard.Transpose(result);
            }
            else
            {
                result = Plugboard.Transpose(value);             
                return RotorController.ProcessByte(result, encode);
            }
        }

        /// <summary>
        /// Resets back to its start state
        /// </summary>
        public override void Reset()
		{
            RotorController.Reset();
            this.Plugboard.Reset();
		}

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public override void Clear()
		{
            RotorController.Clear();
            Plugboard.Clear();
            m_Initialized = false;
		}

        #endregion
    }
}
