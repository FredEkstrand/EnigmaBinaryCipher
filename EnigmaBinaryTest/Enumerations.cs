
using System;

namespace EnigmaBinaryTest
{
    /// <summary>
    /// Enigma Binary Parameters configuration enumeration.
    /// </summary>
    [Flags]
    public enum ConfigurationTypes
    {
        /// <summary>
        /// Default unconfigured state.
        /// </summary>
        NONE = 0x0,
        /// <summary>
        /// Cipher Controller instance object only.
        /// </summary>
        CIPHERCONTROLLER = 0x1,

        /// <summary>
        /// Cipher Controller instance fully populated.
        /// </summary>
        CIPHERCONTROLLER_FULL = 0x2,

        /// <summary>
        /// Rotor Controller instance object only.
        /// </summary>
        ROTORCONTROLLER = 0x4,

        /// <summary>
        /// Rotor Controller instance fully populated.
        /// </summary>
        ROTORCONTROLLER_FULL = 0x8,

        /// <summary>
        /// Rotor instance object.
        /// </summary>
        ROTOR_INSTANCE = 0x10,

        /// <summary>
        /// Rotor parameters only.
        /// </summary>
        ROTOR_PARAMETERS = 0x20,

        /// <summary>
        /// Entry Rotor instance object.
        /// </summary>
        ENTRYROTOR_INSTANCE = 0x40,

        /// <summary>
        /// Entry Rotor parameters only.
        /// </summary>
        ENTRYROTOR_PARAMETERS = 0x80,

        /// <summary>
        /// Reflector instance object.
        /// </summary>
        REFLECTOR_INSTANCE = 0x100,

        /// <summary>
        /// Reflector parameters only.
        /// </summary>
        REFLECTOR_PARAMETERS = 0x200,

        /// <summary>
        /// Turn instance object.
        /// </summary>
        TURN_INSTANCE = 0x400,

        /// <summary>
        /// Plugboard instance object.
        /// </summary>
        PLUGBOARD_INSTANCE = 0x800,

        /// <summary>
        /// Plugboard Parameters.
        /// </summary>
        PLUGBOARD_PARAMETERS = 0x1000
    }
}