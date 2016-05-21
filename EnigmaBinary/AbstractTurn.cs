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
    /// Abstract base class defining common operation.
    /// </summary>
    /// <seealso cref="ITurn" />
    [Serializable]
    public abstract class AbstractTurn : ITurn
	{
        #region Variables

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected bool m_SettingsInUse = false;                 // Guard from changing settings while in use. Force user to use reset.
        private IRotor m_Rotor = null;
        private ITurn m_NextTurn = null;
        protected const string NEXTTURN_INUSE = "Can not change Turn while in use. You must Reset first.";
        protected const string ROTOR_INUSE    = "Can not change Rotor while in use. You must Reset first.";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTurn"/> class.
        /// </summary>
        public AbstractTurn()
		{
            Rotor = null;
            NextTurn = null;
            StarterRotor = false;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Turns the rotor.
        /// </summary>
        public abstract void TurnRotor();

        /// <summary>
        /// Clears all settings
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            m_SettingsInUse = false;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rotor.
        /// </summary>
        /// <value>
        /// Rotor object
        /// </value>
        /// <exception cref="System.InvalidOperationException">Rotor</exception>
        public IRotor Rotor
		{
            get
            {
                return m_Rotor;
            }

            set
            {
                if(m_SettingsInUse)
                {
                    throw new InvalidOperationException(ROTOR_INUSE);
                }
                m_Rotor = value;
            }
		}

        /// <summary>
        /// Gets or sets the next turn.
        /// </summary>
        /// <value>
        /// Next Turn object
        /// </value>
        /// <exception cref="System.InvalidOperationException">Rotor</exception>
        public ITurn NextTurn
		{
            get
            {
                return m_NextTurn;
            }

            set
            {
                if (m_SettingsInUse)
                {
                    throw new InvalidOperationException(NEXTTURN_INUSE);
                }
                m_NextTurn = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [fully cycled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [fully cycled]; otherwise, <c>false</c>.
        /// </value>
        public bool FullyCycled
		{
			get
			{
				if(Rotor == null)
                {
                    return false;
                }
                return Rotor.Cycled;
            }
		}

        /// <summary>
        /// Gets a value indicating whether Rotor is starter
        /// </summary>
        /// <value>
        ///   <c>true</c> if starter rotor otherwise, <c>false</c>.
        /// </value>
        public bool StarterRotor
        {
            get;
            set;
        }

        #endregion
    }
}
