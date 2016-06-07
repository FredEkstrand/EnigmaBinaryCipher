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
    /// Defines Rotor operations.
    /// </summary>
    /// <seealso cref="AbstractRotor" />
    [Serializable]
    public class Rotor : AbstractRotor
	{
        #region Class Variables

        private int m_BthLstPosition = 0;               // Both last position before CCW move.
        private bool m_ResetCCWTurn = false;            // Reset rotor position back to last position before CCW turn operation.
        private bool m_CCWTurn = false;                 // Rotor moving in CCW turn operation.

        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryRotor"/> class.
        /// </summary>
        public Rotor():base()
		{
			
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryRotor"/> class.
        /// </summary>
        /// <param name="values">Array of EndPointPair&lt;byte, byte&gt;</param>
        public Rotor(EndPointPair<byte,byte>[] values):this()
		{
            PopulateTransulationPoints(values);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the byte.
        /// </summary>
        /// <param name="value">Encoded/decoded byte value</param>
        /// <param name="encode">Bool mode of operation "true" is encode and "false" is decode</param>
        /// <returns>Returns encoded or decoded byte value</returns>
        public override byte ProcessByte(byte value, bool encode = true)
		{
            if(m_Translation.Count == 0)
            {
                throw new InvalidOperationException(SUBSTITUTIONSET_NULL);
            }

            m_SettingsInUse = true;
            byte result = (byte)0;

            if (encode)
            {
                if (NextRotor != null)
                {
                    result = NextRotor.ProcessByte(m_Translation[(byte)(((int)value + Position) % ELEMENT_SIZE)],encode);
                    return m_Translation[(byte)(((int)result + Position) % ELEMENT_SIZE)];
                }
                else
                {
                    return m_Translation[(byte)(((int)value + Position) % ELEMENT_SIZE)];
                }
            }
            else
            {
                if (NextRotor != null)
                {
                    result = NextRotor.ProcessByte((byte)(((ELEMENT_SIZE + (int)m_Translation[value]) - Position) % ELEMENT_SIZE), encode);
                    return (byte)(((ELEMENT_SIZE + (int)m_Translation[result]) - Position) % ELEMENT_SIZE);
                }
                else
                {
                    return (byte)(((ELEMENT_SIZE + (int)m_Translation[value]) - Position) % ELEMENT_SIZE);
                }
            }
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
        /// Clears all rotor settings 
        /// </summary>
        public override void Clear()
		{
            m_SettingsInUse = false;
            m_Translation = new Dictionary<byte, byte>();
            StartPosition = START_POSITION;
            Incrementation = INCREMENTATION;
            m_LstPosition = LAST_POSITION;
            NextRotor = null;
            TurnDirection = RotorTurnDirection.CW;           
            Position = StartPosition;       
            Cycled = false;
        }

        /// <summary>
        /// Turns rotor
        /// </summary>
        public override void Turn()
        {
            m_SettingsInUse = true;
            m_LstPosition = Position;
            if(TurnDirection == RotorTurnDirection.CW)
            {
                Position = (Position + Incrementation) % ELEMENT_SIZE;
                HasCycled();
            }
            else if(TurnDirection == RotorTurnDirection.CCW)
            {
                Position = (ELEMENT_SIZE + (((-ELEMENT_SIZE + Position) - Incrementation) % ELEMENT_SIZE)) % ELEMENT_SIZE;
                HasCycled();
            }
            else
            { // BOTH just mean working in a screwy manner using both directions.
                /* In this default case it will go CW and on every 4th turn move CCW - 10 position.
                   Then back to its previous CW position on the next turn move. */
                if(m_CCWTurn)
                {
                    Position = m_BthLstPosition;
                    m_CCWTurn = false;
                    m_ResetCCWTurn = true;
                }

                if(m_ResetCCWTurn == false)
                {
                    Position = (Position + Incrementation) % ELEMENT_SIZE;
                    HasCycled();

                    if (!m_FirstIteration && Position % 4 == 0)
                    {
                        m_BthLstPosition = Position;
                        Position = (ELEMENT_SIZE + (((-ELEMENT_SIZE + Position) - 10) % ELEMENT_SIZE)) % ELEMENT_SIZE;
                        m_CCWTurn = true;
                    }
                }
                m_FirstIteration = false;
                m_ResetCCWTurn = false;  
            }
        }

        private bool m_FirstIteration = true;
        #endregion
    }
}
