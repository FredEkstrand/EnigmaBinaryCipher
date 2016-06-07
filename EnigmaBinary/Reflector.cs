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
    /// Definedes Reflector operations.
    /// </summary>
    /// <seealso cref="AbstractRotor" />
    [Serializable]
    public class Reflector : AbstractRotor
	{

        #region Constructor    

        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector"/> class.
        /// </summary>
        public Reflector():base()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector"/> class.
        /// </summary>
        /// <param name="values">Array of EndPointPair&lt;byte, byte&gt;</param>
        public Reflector(EndPointPair<byte, byte>[] values):base()
        {
            PopulateTransulationPoints(values);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Endcode/Decode byte
        /// </summary>
        /// <param name="value">The value.</param>
        /// /// <param name="encode">Bool mode of operation "true" is encode and "false" is decode</param>
        /// <returns>
        /// Return encode/decode byte
        /// </returns>
        public override byte ProcessByte(byte value, bool encode = true)
        {
            if (m_Translation.Count == 0)
            {
                throw new InvalidOperationException(SUBSTITUTIONSET_NULL);
            }
            // encode mode not used
            m_SettingsInUse = true;
            return m_Translation[value];
        }

        /// <summary>
        /// Resets back to its start state. 
        /// </summary>
        public override void Reset()
        {
            m_SettingsInUse = false;
            Position = StartPosition;
            Cycled = false;
            m_LstPosition = LAST_POSITION;
        }

        /// <summary>
        /// Clear all settings
        /// </summary>
        public override void Clear()
        {
            m_Translation = new Dictionary<byte, byte>();
            m_SettingsInUse = false;
        }

        /// <summary>
        /// Turn a rotor
        /// </summary>
        /// <remarks>Not used</remarks>
        public override void Turn()
        {
            throw new InvalidOperationException("Reflector does not turn.");
        }

        /// <summary>
        /// Determines whether this instance has cycled.
        /// </summary>
        /// <remarks>Not used</remarks>
        protected override void HasCycled()
        {
            Cycled = false;
        }

        #endregion
    }
}
