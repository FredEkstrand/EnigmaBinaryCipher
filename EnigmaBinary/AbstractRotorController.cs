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
    /// Abstract base class for operation of EntryRotor, Rotors, Reflector, and Turns.
    /// </summary>
    /// <seealso cref="IRotorController" />
    [Serializable]
    public abstract class AbstractRotorController : IRotorController
	{
        #region Variables

        /// <summary>
        /// The rotor collection
        /// </summary>
        protected readonly ObservableCollection<IRotor> m_Rotors;
        /// <summary>
        /// The turn collection
        /// </summary>
        protected readonly ObservableCollection<ITurn> m_Turns;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected bool m_Initialized;
        protected const string INITIALIZED_ERROR = "RotorController not initialized";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRotorController"/> class.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        internal AbstractRotorController()
		{
            m_Rotors = new ObservableCollection<IRotor>();
            m_Turns = new ObservableCollection<ITurn>();
            Reflector = null;
            EntryRotor = null;
            m_Initialized = false;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Encoding/Decoding of byte.
        /// </summary>
        /// <param name="value">Byte to be encoded/decoded</param>
        /// <param name="encode">Bool <c>true</c> to encode byte or <c>false</c> to decode byte.</param>
        /// <returns>
        /// Return encoded/decoded byte
        /// </returns>
        public abstract byte ProcessByte(byte value, bool encode = true);

        /// <summary>
        /// Resets its components back to their start state.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Clears all settings for this controller
        /// </summary>
        public abstract void Clear();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the entry rotor.
        /// </summary>
        /// <value>
        /// The entry rotor.
        /// </value>
        public IRotor EntryRotor
		{
            get; set;
		}

        /// <summary>
        /// Gets the collection of rotors
        /// </summary>
        /// <value>
        /// The rotors.
        /// </value>
        public Collection<IRotor> Rotors
		{
			get
			{
                return m_Rotors;
			}
		}

        /// <summary>
        /// Gets or sets the reflector.
        /// </summary>
        /// <value>
        /// The reflector.
        /// </value>
        public IRotor Reflector
		{
            get; set;
		}

        /// <summary>
        /// Gets or sets the turns.
        /// </summary>
        /// <value>
        /// The turns.
        /// </value>
        public Collection<ITurn> Turns
		{
			get
			{
                return m_Turns;
			}
		}

        #endregion
    }
}
