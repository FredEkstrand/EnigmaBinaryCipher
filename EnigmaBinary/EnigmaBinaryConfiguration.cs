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
using System.Linq;
using System.Text;

namespace Ekstrand.Encryption.Ciphers
{
    /// <summary>
    /// Enigma Binary Configuration class is responsible for:
    /// Reading configuration file and configure Enigma Manager, and
    /// Create a configuration file. 
    /// </summary>
    internal class EnigmaBinaryConfiguration
    {
        EnigmaManager m_EnigmaManager;
        EnigmaBinaryParameters m_Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaBinaryConfiguration"/> class.
        /// </summary>
        public EnigmaBinaryConfiguration()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaBinaryConfiguration"/> class.
        /// </summary>
        /// <param name="em">EnigmaManager</param>
        /// <param name="param">EnigmaBinaryParameters</param>
        public EnigmaBinaryConfiguration(EnigmaManager em, EnigmaBinaryParameters param):this()
        {
            m_EnigmaManager = em;
            m_Parameters = param;
            ProcessParameters(); 
        }

        /// <summary>
        /// Returns an EnigmaBinaryParameters of the current Enigma Binary configuration.
        /// </summary>
        /// <param name="em">EnigmaManager em</param>
        /// <returns>Return EnigmaBinaryParameters of the current configuration.</returns>
        public EnigmaBinaryParameters ReturnConfiguration(EnigmaManager em)
        {
            EnigmaBinaryParameters ebp = new EnigmaBinaryParameters();
           
            ebp.Rotors.Clear();
            for (int i = 0; i < em.CipherController.RotorController.Rotors.Count; i++)
            {
                ebp.Rotors.Add(em.CipherController.RotorController.Rotors[i]);
            }

            ebp.Turns.Clear();
            for (int i = 0; i < em.CipherController.RotorController.Turns.Count; i++)
            {
                ebp.Turns.Add(em.CipherController.RotorController.Turns[i]);
            }

            ebp.Plugboard = em.CipherController.Plugboard;
            ebp.EntryRotor = em.CipherController.RotorController.EntryRotor;
            ebp.Reflector = em.CipherController.RotorController.Reflector;

            return ebp;
        }

        /// <summary>
        /// Processes the given EnigmaBinaryParameters.
        /// </summary>
        private void ProcessParameters()
        {
            RandomEndPointsGenerator rand = new RandomEndPointsGenerator();

            if (m_Parameters.CipherController != null)
            {
                m_EnigmaManager.CipherController = m_Parameters.CipherController;
                if (FullyPopulated())
                {
                    return;
                }
            }
            else
            {
                m_EnigmaManager.CipherController = new CipherController();
            }

            if (m_Parameters.RotorController != null)
            {
                m_EnigmaManager.CipherController.RotorController = m_Parameters.RotorController;
                if (FullyPopulated())
                {
                    return;
                }
            }
            else
            {
                m_EnigmaManager.CipherController.RotorController = new RotorController();
            }

            if (m_Parameters.PlugboardParameters.TranspositionSet != null && m_EnigmaManager.CipherController.Plugboard == null)
            {
                m_EnigmaManager.CipherController.Plugboard = new Plugboard(m_Parameters.PlugboardParameters.TranspositionSet);
            }

            if (m_Parameters.Plugboard != null && m_EnigmaManager.CipherController.Plugboard == null)
            {
                m_EnigmaManager.CipherController.Plugboard = m_Parameters.Plugboard;
            }
            else
            {
                m_EnigmaManager.CipherController.Plugboard = new Plugboard(rand.GeneratePlugboard());
            }

            if (m_Parameters.EntryRotorParameters.SubstitutionSet != null && m_EnigmaManager.CipherController.RotorController.EntryRotor == null)
            {
                m_EnigmaManager.CipherController.RotorController.EntryRotor = new EntryRotor(m_Parameters.EntryRotorParameters.SubstitutionSet);
            }

            if (m_Parameters.EntryRotor != null && m_EnigmaManager.CipherController.RotorController.EntryRotor == null)
            {
                m_Parameters.EntryRotor.Reset(); // copied instance includes its state
                m_EnigmaManager.CipherController.RotorController.EntryRotor = m_Parameters.EntryRotor;
            }
            else
            {
                m_EnigmaManager.CipherController.RotorController.EntryRotor = new EntryRotor(rand.GenerateEntryRotor());
            }

            if (m_Parameters.ReflectorParameters.SubstitutionSet != null && m_EnigmaManager.CipherController.RotorController.Reflector == null)
            {
                m_EnigmaManager.CipherController.RotorController.Reflector = new Reflector(m_Parameters.ReflectorParameters.SubstitutionSet);
            }

            if (m_Parameters.Reflector != null && m_EnigmaManager.CipherController.RotorController.Reflector == null)
            {
                m_Parameters.Reflector.Reset(); // copied instance includes its state
                m_EnigmaManager.CipherController.RotorController.Reflector = m_Parameters.Reflector;
            }
            else
            {
                m_EnigmaManager.CipherController.RotorController.Reflector = new Reflector(rand.GenerateReflector());
            }
            /*
             * Note: if cipher controller or rotor controller parameters is fully populated and 
             *  rotors and turns parameters are also populated then 
             *  cipher/rotor controller rotors/turns would be overridden with the parameter rotor and/or turn 
             */
            if (m_Parameters.RotorParameters.Count > 0 && m_EnigmaManager.CipherController.RotorController.Rotors.Count == 0)
            {
                for (int i = 0; i < m_Parameters.RotorParameters.Count; i++)
                {
                    Rotor r = new Rotor(m_Parameters.RotorParameters[i].SubstitutionSet);
                    r.StartPosition = m_Parameters.RotorParameters[i].StartPosition;
                    r.Incrementation = m_Parameters.RotorParameters[i].Incrementation;
                    r.TurnDirection = m_Parameters.RotorParameters[i].TurnDirection;

                    m_EnigmaManager.CipherController.RotorController.Rotors.Add(r);

                }
            }
            else if (m_Parameters.Rotors.Count > 0 && m_EnigmaManager.CipherController.RotorController.Rotors.Count == 0)
            {
                for (int i = 0; i < m_Parameters.Rotors.Count; i++)
                {
                    m_EnigmaManager.CipherController.RotorController.Rotors.Add(m_Parameters.Rotors[i]);
                    m_EnigmaManager.CipherController.RotorController.Rotors[i].Reset(); // copied instance includes its state
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    m_EnigmaManager.CipherController.RotorController.Rotors.Add(new Rotor(rand.GenerateRotor()));
                }
            }

            if (m_Parameters.Turns.Count > 0 && m_EnigmaManager.CipherController.RotorController.Turns.Count == 0)
            {
                for (int i = 0; i < m_Parameters.Turns.Count; i++)
                {
                    m_Parameters.Turns[i].Reset(); // copied instance includes its state
                    m_EnigmaManager.CipherController.RotorController.Turns.Add(m_Parameters.Turns[i]);          
                }

                if (m_EnigmaManager.CipherController.RotorController.Turns.Count > m_EnigmaManager.CipherController.RotorController.Rotors.Count)
                {
                    for (; m_EnigmaManager.CipherController.RotorController.Rotors.Count < m_EnigmaManager.CipherController.RotorController.Turns.Count;)
                    {
                        m_EnigmaManager.CipherController.RotorController.Rotors.Add(new Rotor(rand.GenerateRotor()));
                    }
                }

                if (m_EnigmaManager.CipherController.RotorController.Rotors.Count > m_EnigmaManager.CipherController.RotorController.Turns.Count)
                {
                    for (; m_EnigmaManager.CipherController.RotorController.Turns.Count < m_EnigmaManager.CipherController.RotorController.Rotors.Count;)
                    {
                        m_EnigmaManager.CipherController.RotorController.Turns.Add(new Turn());
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_EnigmaManager.CipherController.RotorController.Rotors.Count; i++)
                {
                    m_EnigmaManager.CipherController.RotorController.Turns.Add(new Turn());
                }
            }
        }

        /// <summary>
        /// Checks if the CipherController and RotorController are fully populated.
        /// </summary>
        /// <returns>Returns <c>ture</c> if fully populated otherwise <c>false</c></returns>
        private bool FullyPopulated()
        {
            if (m_EnigmaManager.CipherController.Plugboard != null)
            {
                if (m_EnigmaManager.CipherController.RotorController != null)
                {
                    if (m_EnigmaManager.CipherController.RotorController.EntryRotor != null)
                    {
                        if (m_EnigmaManager.CipherController.RotorController.Reflector != null)
                        {
                            if (m_EnigmaManager.CipherController.RotorController.Rotors.Count > 0)
                            {
                                if (m_EnigmaManager.CipherController.RotorController.Turns.Count > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
