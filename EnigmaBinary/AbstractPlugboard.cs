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
    /// <seealso cref="IPlugboard" />
    [Serializable]
    public abstract class AbstractPlugboard : IPlugboard
    {
        #region  Variables

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected Dictionary<byte, byte> m_Transulation;        // Rotor translation from input to output byte value.
        protected bool m_SettingsInUse = false;

        protected const string TRANSPOSITIONERR = "Can not change end points while in use. You must Reset first.";
        protected const string DUPLICATE_SIDE_A_ENDPOINTS = "Duplicate Side-A end points.";
        protected const string DUPLICATE_SIDE_B_ENDPOINTS = "Duplicate Side-B end points.";
        protected const string TRANSPOSITION_SIZE = "Translation points too small to provide a strong encryption.";
        protected const int MINIMUM_TRANSPOSITION_SIZE = 240;
        protected const string TRANSPOSITIONSET_NULL = "TranspositionSet is null";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPlugboard"/> class.
        /// </summary>
        internal AbstractPlugboard()
		{
            m_Transulation = new Dictionary<byte, byte>();
		}

        #endregion

        #region Methods

        /// <summary>
        /// Transposes the specified value.
        /// </summary>
        /// <param name="value">byte value to be transposed</param>
        /// <returns>
        /// Return a transposed byte iff plugboard has a transpose mapping, otherwise; return the given byte
        /// </returns>
        public abstract byte Transpose(byte value);

        /// <summary>
        /// Clears all settings
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Resets back to its start state. 
        /// </summary>
        public void Reset()
        {
            m_SettingsInUse = false;
        }

        /// <summary>
        /// Populates the translation points.
        /// </summary>
        /// <param name="values">Array of EndPointPair&lt;byte, byte&gt;</param>
        protected void PopulateTransulationPoints(EndPointPair<byte, byte>[] values)
        {

            for (int i = 0; i < values.Length; i++)
            {
                m_Transulation.Add(values[i].SideA, values[i].SideB);
            }
        }

        /// <summary>
        /// Validates the Translation points.
        /// </summary>
        /// <param name="values">An array of EndPointPair&lt;byte, byte&gt;</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        protected void ValidateTranslationPoints(EndPointPair<byte, byte>[] values)
        {
            if(values.Length < MINIMUM_TRANSPOSITION_SIZE)
            {
                throw new ArgumentException(TRANSPOSITION_SIZE);
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the transposition set.
        /// </summary>
        /// <value>
        /// The transposition set.
        /// </value>
        public EndPointPair<byte,byte>[] TranspositionSet
		{
            get
            {
                if (m_Transulation.Count == 0)
                {
                    return null;
                }

                EndPointPair<byte, byte>[] Set = new EndPointPair<byte, byte>[m_Transulation.Count];
                for (int i = 0; i < m_Transulation.Count; i++)
                {
                    if (m_Transulation.ContainsKey((byte)i))
                    {
                        Set[i] = new EndPointPair<byte, byte>((byte)i, m_Transulation[(byte)i]);
                    }
                }
                return Set;
            }
            set
            {
                if(m_SettingsInUse)
                {
                    throw new InvalidOperationException(TRANSPOSITIONERR);
                }

                ValidateTranslationPoints(value);
                m_Transulation.Clear();
                PopulateTransulationPoints(value);
            }
        }

        #endregion
    }
}
