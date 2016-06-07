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
    /// Abstract base class defining common operations.
    /// </summary>
    /// <seealso cref="IRotor" />
    [Serializable]
    public abstract class AbstractRotor : IRotor
	{
        #region Class Variables

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected int m_LstPosition = LAST_POSITION;            // Last position before incrementation to determine if rotor has cycled.
        protected Dictionary<byte, byte> m_Translation;        // Rotor translation from input to output byte value.
        protected const int ELEMENT_SIZE = 256;                 // Number of elements to cover each byte value 0 - 255.
        protected bool m_SettingsInUse = false;                 // Guard from changing settings while in use. Force user to use reset.
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private IRotor m_NextRotor = null;                      // Reference to next rotor.
        private int m_StartPosition = 0;                        // Rotor start position.
        private int m_Incrementation = 1;                       // Rotor incrementation amount 
        private RotorTurnDirection m_TurnDirection = RotorTurnDirection.CW;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected const int START_POSITION = 0;
        protected const int INCREMENTATION = 1;
        protected const int LAST_POSITION = -1;

        protected const string SUBSTITUTIONSETERR = "Can not change end points while in use. You must Reset first.";
        protected const string ARRAYSIZENOT256 = "Array size must be 256";
        protected const string DUPLICATE_SIDE_A_ENDPOINTS = "Duplicate Side-A end points.";
        protected const string DUPLICATE_SIDE_B_ENDPOINTS = "Duplicate Side-B end points.";
        protected const string STARTPOINT_VALUE = "Start position must be between (0-255)";
        protected const string INCREMENTATION_VALUE = "Incrementation value must be between (1-255)";
        protected const string SUBSTITUTIONSET_NULL = "SubstitutionSet is null";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRotor"/> class.
        /// </summary>
        internal AbstractRotor()
		{
            m_Translation = new Dictionary<byte, byte>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear all settings
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Encode/Decode byte
        /// </summary>
        /// <param name="value">The value.</param>
        ///  /// <param name="encode">Bool mode of operation "true" is encode and "false" is decode</param>
        /// <returns>
        /// Return encode/decode byte
        /// </returns>
        public abstract byte ProcessByte(byte value, bool encode);

        /// <summary>
        /// Resets back to its start state. 
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Turn a rotor
        /// </summary>
        public abstract void Turn();

        /// <summary>
        /// Populates the Translation points.
        /// </summary>
        /// <param name="values">The values.</param>
        protected void PopulateTransulationPoints(EndPointPair<byte, byte>[] values)
        {
            ValidateTransulationPoints(values);

            for (int i = 0; i < values.Length; i++)
            {
                m_Translation.Add(values[i].SideA, values[i].SideB);
            }
        }

        /// <summary>
        /// Validates the Translation points.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <exception cref="System.InvalidOperationException">Can not change end points while in use. You must Reset first.</exception>
        /// <exception cref="System.ArgumentException">
        /// Value array size must be 256
        /// or
        /// Duplicate Side-A end points.
        /// or
        /// Duplicate Side-A end points.
        /// </exception>
        protected void ValidateTransulationPoints(EndPointPair<byte, byte>[] values)
        {
            if(values == null)
            {
                throw new NullReferenceException("SubsitiutionSet");
            }

            if (m_SettingsInUse)
            {
                throw new InvalidOperationException(SUBSTITUTIONSETERR);
            }

            if (values.Length != ELEMENT_SIZE)
            {
                throw new ArgumentException(ARRAYSIZENOT256);
            }

            byte valueA = 0;
            byte valueB = 0;
            for (int i = 0; i < values.Length; i++)
            {
                valueA = values[i].SideA;
                valueB = values[i].SideB;
                for (int j = i + 1; j < values.Length; j++)
                {
                    if (valueA == values[j].SideA)
                    {
                        throw new ArgumentException(DUPLICATE_SIDE_A_ENDPOINTS);
                    }
                    if (valueB == values[j].SideB)
                    {
                        throw new ArgumentException(DUPLICATE_SIDE_B_ENDPOINTS);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this instance has cycled.
        /// </summary>
        protected virtual void HasCycled()
        {
            if (m_LstPosition != -1 && Position == StartPosition)
            {
                Cycled = true;
                return;
            }
            else
            {
                Cycled = false;
                return;
            }
  
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current position of the rotor.
        /// </summary>
        /// <value>
        /// Current rotor position.
        /// </value>
        public int Position
		{
            get; internal set;
		}

        /// <summary>
        /// Gets or sets the next rotor.
        /// </summary>
        /// <value>
        /// The next rotor.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Can not change end points while in use. You must Reset first.</exception>
        public IRotor NextRotor
		{
            get
            {
                return m_NextRotor;
            }
            set
            {
                if (m_SettingsInUse)
                {
                    throw new InvalidOperationException(SUBSTITUTIONSETERR);
                }

                m_NextRotor = value;
            }
		}

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>
        /// Integer values (0 - 255) for position for rotor to start.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Can not change end points while in use. You must Reset first.</exception>
        public int StartPosition
		{
            get
            {
                return m_StartPosition;
            }
            set
            {
                if (m_SettingsInUse)
                {
                    throw new InvalidOperationException(SUBSTITUTIONSETERR);
                }
                else if(value > 255 || value < 0)
                {
                    throw new InvalidOperationException(STARTPOINT_VALUE);
                }
                else
                {
                    m_StartPosition = value;
                    Position = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the substitution set.
        /// </summary>
        /// <value>
        /// Array of EndPointPair&lt;byte, byte&gt;
        /// </value>
        public virtual EndPointPair<byte, byte>[] SubstitutionSet
        {
            get
            {
                if (m_Translation.Count == 0)
                {
                    return null;
                }

                EndPointPair<byte, byte>[] Set = new EndPointPair<byte, byte>[ELEMENT_SIZE];
                for (int i = 0; i < m_Translation.Count; i++)
                {
                    Set[i] = new EndPointPair<byte, byte>((byte)i, m_Translation[(byte)i]);
                }
                return Set;
            }
            set
            {
                ValidateTransulationPoints(value);
                m_Translation.Clear();
                PopulateTransulationPoints(value);
            }
        }

        /// <summary>
        /// Gets or sets the turn direction.
        /// </summary>
        /// <value>
        /// RotorTurnDirection enum value.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Can not change end points while in use. You must Reset first.</exception>
        public RotorTurnDirection TurnDirection
		{
            get
            {
                return m_TurnDirection;
            }
            set
            {
                if (m_SettingsInUse)
                {
                    throw new InvalidOperationException(SUBSTITUTIONSETERR);
                }

                m_TurnDirection = value;
            }
		}

        /// <summary>
        /// Gets a value indicating whether this <see cref="IRotor" /> has cycled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cycled; otherwise, <c>false</c>.
        /// </value>
        public bool Cycled
		{
            get; internal set;
		}

        /// <summary>
        /// Gets or Sets the incrementation of the rotor.
        /// </summary>
        /// <value>
        /// The incrementation.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Can not change end points while in use. You must Reset first.</exception>
        public virtual int Incrementation
		{
            get
            {
                return m_Incrementation;
            }
            set
            {
                if (m_SettingsInUse)
                {
                    throw new InvalidOperationException(SUBSTITUTIONSETERR);
                }
                if(value < 1 || value > 255)
                {
                    throw new InvalidOperationException(INCREMENTATION_VALUE);
                }
                m_Incrementation = value;
            }
        }

        #endregion
    }
}
