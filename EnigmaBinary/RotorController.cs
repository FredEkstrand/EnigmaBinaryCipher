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
    /// Defines RotorControler operations.
    /// </summary>
    /// <seealso cref="AbstractRotorController" />
    [Serializable]
    public class RotorController : AbstractRotorController
	{
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RotorController"/> class.
        /// </summary>
        public RotorController():base()
		{
            m_Rotors.CollectionChanged += CollectionChanged;
            m_Turns.CollectionChanged += CollectionChanged;
		}

        #endregion

        #region Events

        /// <summary>
        /// Collections has changed.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.Rotors.Count == this.Turns.Count &&
                this.Rotors.Count > 0 && this.Turns.Count > 0 &&
                this.Reflector != null && this.EntryRotor != null)
            {
                this.Initialize();
                return;
            }

            m_Initialized = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializer this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Rotor and Turn Collection count do not match.</exception>
        /// <exception cref="System.NullReferenceException">
        /// Reflector
        /// or
        /// EntryRotor
        /// </exception>
        public override void Initialize()
		{// Setting up chaining of Rotors and Turn objects
           

            if(m_Rotors.Count != m_Turns.Count)
            {
                throw new InvalidOperationException("Rotor and Turn Collection count do not match.");
            }

            if(Reflector == null)
            {
                throw new NullReferenceException("Reflector");
            }

            if(EntryRotor == null)
            {
                throw new NullReferenceException("EntryRotor");
            }

            this.Reset();
            for(int i = 0; i < m_Turns.Count-1; i++)
            {
                m_Turns[i].NextTurn = m_Turns[i + 1];
                m_Turns[i].Rotor = m_Rotors[i];

                if (i == 0)
                {
                    m_Turns[i].StarterRotor = true;
                }

                m_Rotors[i].NextRotor = m_Rotors[i + 1];
            }

            m_Turns[m_Turns.Count - 1].Rotor = m_Rotors[m_Turns.Count - 1];
            EntryRotor.NextRotor = m_Rotors[0];
            m_Rotors[m_Rotors.Count - 1].NextRotor = Reflector;
            m_Initialized = true;
		}

        /// <summary>
        /// Encoding/Decoding of byte.
        /// </summary>
        /// <param name="value">Byte to be encoded/decoded</param>
        /// <param name="encode">Bool <c>true</c> to encode byte or <c>false</c> to decode byte.</param>
        /// <returns>
        /// Return encoded/decoded byte
        /// </returns>
        public override byte ProcessByte(byte value, bool encode = true)
		{
            if(!m_Initialized)
            {
                throw new InvalidOperationException(INITIALIZED_ERROR);
            }
            
            // Send byte through simulated electrical path
            byte result = EntryRotor.ProcessByte(value,encode);
            
            // Turn Rotors as needed
            for (int i = 0; i < m_Turns.Count; i++)
            {
                if (m_Turns[i].StarterRotor)
                {
                    m_Turns[i].TurnRotor();
                }

                if (m_Turns[i].FullyCycled)
                {
                    if (m_Turns[i].NextTurn != null)
                    {
                        m_Turns[i].NextTurn.TurnRotor();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Resets its components back to their start state.
        /// </summary>
        public override void Reset()
		{
			for(int i = 0; i < m_Rotors.Count; i++)
            {
                m_Rotors[i].Reset();
            }

            for (int i = 0; i < m_Turns.Count; i++)
            {
                m_Turns[i].Reset();
            } 
        }

        /// <summary>
        /// Clears all settings for this controller
        /// </summary>
        public override void Clear()
		{
            m_Turns.Clear();
            m_Rotors.Clear();
            Reflector = null;
            EntryRotor = null;
            m_Initialized = false;
		}

        #endregion
    }
}
