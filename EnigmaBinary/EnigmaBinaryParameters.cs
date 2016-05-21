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
    /// Provide configuration for the Encryption.Ciphers
    /// </summary>
    /// <seealso cref="ICipherParameters" />
    [Serializable]
    public class EnigmaBinaryParameters : ICipherParameters
    {
        private readonly Collection<ITurn> m_Turns;
        private readonly Collection<IRotorParameters> m_RotorParams;
        private readonly Collection<IRotor> m_Rotors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaBinaryParameters"/> class.
        /// </summary>
        public EnigmaBinaryParameters()
        {
            m_Turns = new Collection<ITurn>();         
            m_Rotors = new Collection<IRotor>();
            m_RotorParams = new Collection<IRotorParameters>();
            EntryRotorParameters = new EntryRotorParameters();
            ReflectorParameters = new ReflectorParameters();
            PlugboardParameters = new PlugboardParameters();
            RotorController = null;
            CipherController = null;            
            EntryRotor = null;
            Reflector = null;
            Plugboard = null;
        }

        /// <summary>
        /// Gets or sets the entry rotor parameters.
        /// </summary>
        /// <value>
        /// EntryRotorParameters
        /// </value>
        public IEntryRotorParameters EntryRotorParameters
        { get; set; }

        /// <summary>
        /// Gets or sets the EntryRotor
        /// </summary>
        /// <value>
        /// The entry rotor.
        /// </value>
        public IRotor EntryRotor
        { get; set; }

        /// <summary>
        /// Gets the rotor parameters collection.
        /// </summary>
        /// <value>
        /// The rotor parameters collection.
        /// </value>
        public Collection<IRotorParameters> RotorParameters
        { get { return m_RotorParams; } }

        /// <summary>
        /// Gets the rotor collection
        /// </summary>
        /// <value>
        /// Rotor collection.
        /// </value>
        public Collection<IRotor> Rotors
        { get { return m_Rotors; } }

        /// <summary>
        /// Gets or sets the reflector parameters.
        /// </summary>
        /// <value>
        /// The reflector parameters.
        /// </value>
        public IReflectorParameters ReflectorParameters
        { get; set; }

        /// <summary>
        /// Gets or sets the reflector.
        /// </summary>
        /// <value>
        /// The reflector.
        /// </value>
        public IRotor Reflector
        { get; set; }

        /// <summary>
        /// Gets the turns collection.
        /// </summary>
        /// <value>
        /// The turns collection.
        /// </value>
        public Collection<ITurn> Turns
        { get { return m_Turns; } }

        /// <summary>
        /// Gets or sets the plugboard.
        /// </summary>
        /// <value>
        /// The plugboard.
        /// </value>
        public IPlugboard Plugboard
        { get; set; }

        /// <summary>
        /// Gets or sets the plugboard parameters.
        /// </summary>
        /// <value>
        /// The plugboard parameters.
        /// </value>
        public PlugboardParameters PlugboardParameters
        { get; set; }

        /// <summary>
        /// Gets or sets the rotor controller.
        /// </summary>
        /// <value>
        /// The rotor controller.
        /// </value>
        public IRotorController RotorController
        { get; set; }

        /// <summary>
        /// Gets or sets the cipher controller.
        /// </summary>
        /// <value>
        /// The cipher controller.
        /// </value>
        public ICipherController CipherController
        { get; set; }

    }
}
