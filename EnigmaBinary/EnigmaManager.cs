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
    /// Responsible for managing: operation, initialization, and configuring: EntryRotor, Rotors, Reflector, Turn, Plugboard, CipherController and RotorController.
    /// </summary>
    [Serializable]
    public class EnigmaManager
	{
        #region Variables

        private bool m_SettingsInUse = false;               // Guard from changing settings while in use. Force user to use reset. 
        private RandomEndPointsGenerator m_REPG;       
        private ICipherController m_CipherController;
        private EnigmaBinaryParameters m_Param;
        private bool m_LastEncryptState = true;             // default encrypting

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaManager"/> class.
        /// </summary>
        public EnigmaManager()
		{
            m_SettingsInUse = false;
            CipherController = null;
            m_REPG = new RandomEndPointsGenerator();
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaManager"/> class.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <exception cref="System.NullReferenceException">CipherParameters</exception>
        /// <exception cref="System.ArgumentException">CipherParameters not of type EnigmaBinaryParameters</exception>
        public EnigmaManager(ICipherParameters param):this()
		{
            if (param == null)
            {
                return;
            }

            m_Param = param as EnigmaBinaryParameters;

            if (m_Param == null)
            {
                throw new ArgumentException("CipherParameters not of type EnigmaBinaryParameters");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
		{
            if (m_Param != null)
            {
                EnigmaBinaryConfiguration ebc = new EnigmaBinaryConfiguration(this, m_Param);
            }
            else
            { // no parameters given so create a default setup.
                // move to configuration as default configuration setup
                this.CipherController = new CipherController();
                this.CipherController.RotorController = new RotorController();
                RandomEndPointsGenerator rand = new RandomEndPointsGenerator();

                for (int i = 0; i < 3; i++)
                {
                    Rotor r = new Rotor(rand.GenerateRotor());
                    r.StartPosition = i;
                    r.Incrementation = 1;
                    r.TurnDirection = RotorTurnDirection.CW;
                    this.CipherController.RotorController.Rotors.Add(r);

                    Turn t = new Turn();
                    this.CipherController.RotorController.Turns.Add(t);
                }

                this.CipherController.Plugboard = new Plugboard(rand.GeneratePlugboard());
                this.CipherController.RotorController.EntryRotor = new EntryRotor(rand.GenerateEntryRotor());
                this.CipherController.RotorController.Reflector = new Reflector(rand.GenerateReflector());
            }

            CipherController.Reset();
            CipherController.Initialize();
        }

        /// <summary>
        /// Processes the byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encrypted"><c>bool</c> <c>true</c> to encrypt or <c>false</c> to decrypt</param>
        /// <returns>Return encrypted byte</returns>
        public byte ProcessByte(byte value, bool encrypted = true)
		{
            if(CipherController == null)
            {
                throw new NullReferenceException("CipherController");
            }

            if(m_LastEncryptState != encrypted)
            {
                this.Reset();
                m_LastEncryptState = encrypted;
            }

            m_SettingsInUse = true;
            if (encrypted)
            {
                return CipherController.ProcessByte(value);
            }
            else
            {
                return CipherController.ProcessByte(value, false);
            }
		}

        /// <summary>
        /// Resets back to its start state.
        /// </summary>
        public void Reset()
        {
            CipherController.Reset();
            m_SettingsInUse = false;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            CipherController.Clear();
            CipherController = null;
            m_SettingsInUse = false;
        }

        /// <summary>
        /// Returns the enigma configuration.
        /// </summary>
        /// <returns>Returns EnigmabinaryParameters with current configurations.</returns>
        public EnigmaBinaryParameters ReturnConfiguration()
        {
            EnigmaBinaryConfiguration ebc = new EnigmaBinaryConfiguration();
            return ebc.ReturnConfiguration(this);
        }

        /// <summary>
        /// Initializes a collection of rotors.
        /// </summary>
        private void InitRotors()
		{
            if (CipherController.RotorController.Rotors.Count == 0)
            {
                if (m_Param != null)
                {
                    if (m_Param.Turns != null)
                    {
                        if (m_Param.Turns.Count > 0)
                        {
                            InitRotors(m_Param.Turns.Count);
                        }
                        else
                        {
                            InitRotors(3);
                        }
                    }
                    else
                    {
                        InitRotors(3);
                    }
                }
                else
                {
                    InitRotors(3);
                }            
            }            
		}

        /// <summary>
        /// Initializes a collection of rotors.
        /// </summary>
        /// <param name="count">Number of rotors to be created.</param>
        private void InitRotors(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CipherController.RotorController.Rotors.Add(new Rotor(m_REPG.GenerateRotor()));
            }
        }

        /// <summary>
        /// Initializes the entry rotor.
        /// </summary>
        private void InitEntryRotor()
		{
            if (CipherController.RotorController.EntryRotor == null)
            {
                CipherController.RotorController.EntryRotor = new EntryRotor(m_REPG.GenerateEntryRotor());
            }
        }

        /// <summary>
        /// Initializes the reflector.
        /// </summary>
        private void InitReflector()
        {
            if (CipherController.RotorController.Reflector == null)
            {
                CipherController.RotorController.Reflector = new Reflector(m_REPG.GenerateReflector());
            }
        }

        /// <summary>
        /// Initializes the turn.
        /// </summary>
        private void InitTurn()
        {
            if(CipherController.RotorController.Turns.Count == 0)
            {
                if(CipherController.RotorController.Rotors.Count > 0)
                {
                    for(int i = 0; i < CipherController.RotorController.Rotors.Count; i++)
                    {
                        CipherController.RotorController.Turns.Add(new Turn());
                    }
                }
                else
                {
                    for (int i = 0; i < CipherController.RotorController.Rotors.Count; i++)
                    {
                        CipherController.RotorController.Turns.Add(new Turn());
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the plugboard.
        /// </summary>
        private void InitPlugboard()
        {
            if(CipherController.Plugboard == null)
            {
                CipherController.Plugboard = new Plugboard(m_REPG.GeneratePlugboard());
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cipher controller.
        /// </summary>
        /// <value>
        /// The cipher controller.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Can not change Cipher Controller while in use. You must call Clear.</exception>
        public ICipherController CipherController
		{
            get
            {
                return m_CipherController;
            }
            set
            {
                if(m_SettingsInUse)
                {
                    throw new InvalidOperationException("Can not change Cipher Controller while in use. You must call Reset.");
                }
                m_CipherController = value;
            }
		}

        #endregion
    }
}
