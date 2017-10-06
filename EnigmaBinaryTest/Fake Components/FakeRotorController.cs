using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{
    [ExcludeFromCoverageAttribute]
    public class FakeRotorController : IRotorController
    {

        protected Collection<IRotor> m_Rotors;
        protected Collection<ITurn> m_Turns; 

        public FakeRotorController()
        {
            m_Rotors = new Collection<IRotor>();
            m_Turns = new Collection<ITurn>();
            Reflector = null;
            EntryRotor = null;
        }

        public IRotor EntryRotor
        {
            get; set;
        }

        public IRotor Reflector
        {
            get; set;
        }

        public Collection<IRotor> Rotors
        {
            get
            {
                return m_Rotors;
            }
            set
            {
                m_Rotors = value;
            }
        }

        public Collection<ITurn> Turns
        {
            get
            {
                return m_Turns;
            }
            set
            {
                m_Turns = value;
            }
        }

        public void Clear()
        {
            m_Turns.Clear();
            m_Rotors.Clear();
            EntryRotor = null;
            Reflector = null;
        }

        private bool m_SkipInit = false;
        public bool FakeInitializer
        {
            get { return m_SkipInit; }
            set { m_SkipInit = value; }
        }

        public void Initialize()
        {
            if(m_SkipInit)
            {
                return;
            }

            if (m_Rotors.Count != m_Turns.Count)
            {
                throw new InvalidOperationException("Rotor and Turn Collection count do not match.");
            }

            if (Reflector == null)
            {
                throw new NullReferenceException("Reflector");
            }

            if (EntryRotor == null)
            {
                throw new NullReferenceException("EntryRotor");
            }

            for (int i = 0; i < m_Turns.Count - 1; i++)
            {
                m_Turns[i].NextTurn = m_Turns[i + 1];
                m_Rotors[i].NextRotor = m_Rotors[i + 1];
            }
        }

        private bool m_FakeProcessByte = false;
        public bool FakeProcessByte
        {
            get { return m_FakeProcessByte; }
            set { m_FakeProcessByte = value; }
        }

        public byte ProcessByte(byte value, bool encode = true)
        {
            // NOTE: fake not checking boundaries of 0 or 255.
            if(FakeProcessByte)
            {
                if(encode)
                {
                    return value += 1;
                }
                else
                {
                    return value -= 1;
                }
            }
            // Sequence of events Turn rotors process byte value through EntryRotor, Rotors, Reflector.

            // Turn Rotors as needed
            for (int i = 0; i < m_Turns.Count - 1; i++)
            {
                if (m_Turns[i].FullyCycled)
                {
                    m_Turns[i].NextTurn.TurnRotor();
                }
            }

            // Send byte through simulated electrical path
            byte result = EntryRotor.ProcessByte(value);

            for (int i = 0; i < m_Rotors.Count; i++)
            {
                result = m_Rotors[i].ProcessByte(result);
            }

            result = Reflector.ProcessByte(result);

            for (int i = 0; i < m_Rotors.Count; i++)
            {
                result = m_Rotors[i].ProcessByte(result);
            }

            return EntryRotor.ProcessByte(result);
        }

        public void Reset()
        {
            // reset each rotor back to starting position
            for (int i = 0; i < m_Rotors.Count; i++)
            {
                m_Rotors[i].Reset();
            }
        }
    }
}
