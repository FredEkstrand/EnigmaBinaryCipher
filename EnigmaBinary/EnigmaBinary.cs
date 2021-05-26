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
    /// Enigma Binary
    /// </summary>
    /// <seealso cref="IStreamCipher" />
    [Serializable]
    public class EnigmaBinary : IStreamCipher
    {
        #region Variables

        private EnigmaManager m_EnigmaManager;
        private bool m_Initialized;
        private bool m_Encrypt;
 

        /// <summary>
        /// Guard from changing settings while in use. Force user to use reset. 
        /// </summary>
        protected bool m_SettingsInUse = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaBinary"/> class.
        /// </summary>
        public EnigmaBinary()
        {
            m_Initialized = false;
            m_Encrypt = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the cipher.
        /// </summary>
        /// <param name="forEncryption">Initialize for encryption if true, for decryption if false.</param>
        /// <param name="parameters">The key or other data required by the cipher.</param>
        public void Init(bool forEncryption = true, ICipherParameters parameters = null)
        {
            m_Encrypt = forEncryption;
            m_EnigmaManager = new EnigmaManager(parameters);
            m_EnigmaManager.Initialize();
            m_Initialized = true;
        }

        /// <summary>
        /// encrypt/decrypt a single byte returning the result.
        /// </summary>
        /// <param name="input">the byte to be processed.</param>
        /// <returns>
        /// the result of processing the input byte.
        /// </returns>
        public byte ReturnByte(byte input)
        {
            return m_EnigmaManager.ProcessByte(input, m_Encrypt);
        }

        /// <summary>
        /// Process a block of bytes from <c>input</c> putting the result into <c>output</c>.
        /// </summary>
        /// <param name="input">The input byte array.</param>
        /// <param name="inOff">The offset into <c>input</c> where the data to be processed starts.</param>
        /// <param name="length">The number of bytes to be processed.</param>
        /// <param name="output">The output buffer the processed bytes go into.</param>
        /// <param name="outOff">The offset into <c>output</c> the processed data starts at.</param>
        /// <exception cref="System.InvalidOperationException">You must call Init(...) before processing blocks</exception>
        /// <exception cref="DataLengthException">
        /// Input buffer too short.
        /// or
        /// Output buffer too short.
        /// </exception>
        public void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            if(!m_Initialized)
            {
                throw new InvalidOperationException("You must call Init(...) before processing blocks");
            }

            m_SettingsInUse = true;
            if (inOff + length > input.Length)
            {
                throw new DataLengthException("Input buffer too short.");
            }

            if(outOff + length > output.Length)
            {
                throw new DataLengthException("Output buffer too short.");
            }

            for(int i = 0 + inOff; i < input.Length; i++)
            {
                output[outOff + i] = m_EnigmaManager.ProcessByte(input[inOff + i], m_Encrypt);
            }
        }

        /// <summary>
        /// Reset the cipher to the same state as it was after the last init (if there was one).
        /// </summary>
        public void Reset()
        {
            m_EnigmaManager.Reset();
            m_SettingsInUse = false;

        }

        /// <summary>
        /// Returns the current configuration of the EnigmaBinary
        /// </summary>
        /// <returns>Return EnigmaBinaryParameters</returns>
        public EnigmaBinaryParameters ReturnConfiguration()
        {
            return m_EnigmaManager.ReturnConfiguration();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the algorithm.
        /// </summary>
        /// <value>
        /// The name of the algorithm.
        /// </value>
        public string AlgorithmName
        {
            get
            {
                return "EnigmaBinary";
            }
        }

        /// <summary>
        /// Indicates whether this cipher can handle partial blocks.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is partial block okay; otherwise, <c>false</c>.
        /// </value>
        public bool IsPartialBlockOkay
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The EnigmaBinary Version
        /// </value>
        public string Version
        {
            get
            {
                return typeof(EnigmaBinary).Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the entry rotor.
        /// </summary>
        /// <value>
        /// Type of EntryRotor
        /// </value>
        public IRotor EntryRotor
        {
            get
            {
                if(m_EnigmaManager == null)
                {
                    return null;
                }

                return m_EnigmaManager.CipherController.RotorController.EntryRotor;
            }
            set
            {
                if (value.SubstitutionSet == null || value.SubstitutionSet.Length != 256)
                {
                    throw new ArgumentException("EntryRotor SubstitutionSet.");
                }

                m_EnigmaManager.CipherController.RotorController.EntryRotor = value;
            }
        }

        /// <summary>
        /// Gets the rotor collection.
        /// </summary>
        /// <value>
        /// Collection of Rotors
        /// </value>
        public Collection<IRotor> Rotors
        {
            get
            {
                if(m_EnigmaManager == null)
                {
                    return null;
                }

                return m_EnigmaManager.CipherController.RotorController.Rotors;

            }
        }

        /// <summary>
        /// Gets or sets the reflector.
        /// </summary>
        /// <value>
        /// Type of IReflector
        /// </value>
        public IRotor Reflector
        {
            get
            {
                if (m_EnigmaManager == null)
                {
                    return null;
                }

                return m_EnigmaManager.CipherController.RotorController.Reflector;

            }
            set
            {
                if (value.SubstitutionSet == null || value.SubstitutionSet.Length != 256)
                {
                    throw new ArgumentException("Reflector SubstitutionSet.");
                }

                m_EnigmaManager.CipherController.RotorController.Reflector = value;
            }
        }

        /// <summary>
        /// Gets or sets the plugboard.
        /// </summary>
        /// <value>
        /// Type of IPlugboard
        /// </value>
        public IPlugboard Plugboard
        {
            get
            {
                if (m_EnigmaManager == null)
                {
                    return null;
                }

                return m_EnigmaManager.CipherController.Plugboard;
            }
            set
            {
                if (value.TranspositionSet == null)
                {
                    throw new ArgumentException("Plugboard TranspositionSet.");
                }

                m_EnigmaManager.CipherController.Plugboard = value;
            }
        }

        /// <summary>
        /// Gets the turn collection.
        /// </summary>
        /// <value>
        /// Collection of Turns
        /// </value>
        public Collection<ITurn> Turns
        {
            get
            {
                if (m_EnigmaManager == null)
                {
                    return null;
                }
                return m_EnigmaManager.CipherController.RotorController.Turns;
            }
        }

        /// <summary>
        /// Gets or sets the rotor controller.
        /// </summary>
        /// <value>
        /// Type of IRotorController
        /// </value>
        public IRotorController RotorController
        {
            get
            {
                if (m_EnigmaManager == null)
                {
                    return null;
                }
                return m_EnigmaManager.CipherController.RotorController;
            }
            set
            {
                if(m_SettingsInUse)
                {
                    throw new ArgumentException("RotorController in use. You must Reset before changing objects.");
                }
                m_EnigmaManager.CipherController.RotorController = value;
            }
        }

        /// <summary>
        /// Gets or sets the cipher controller.
        /// </summary>
        /// <value>
        /// The cipher controller.
        /// </value>
        public ICipherController CipherController
        {
            get
            {
                if(m_EnigmaManager == null)
                {
                    return null;
                }
                return m_EnigmaManager.CipherController;
            }
            set
            {
                if(m_SettingsInUse)
                {
                    throw new ArgumentException("CipherController in use. You must Reset before changing objects.");
                }
                m_EnigmaManager.CipherController = value;
            }
        }

        #endregion
    }
}
