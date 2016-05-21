using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    public class FakeTurn : ITurn
    {
#pragma warning disable CS0414 // In use but compiler insists it is not.
        private bool m_SettingsInUse;
#pragma warning restore CS0414

        public FakeTurn()
        {
            NextTurn = null;
            Rotor = null;
            NextTurnNull = true;
            RotorNull = false;
            StarterRotor = false;
            FakeFullyCyceled = false;
            NoRotor = false;
            m_SettingsInUse = false;
            FakeTurnRotor = false;
            TurnRotorCalled = false;
        }

        public bool FullyCycled
        {
            get
            {
                if(FakeFullyCyceled)
                {
                    return true;
                }

                if(NoRotor)
                {
                    return FakeFullyCyceled;
                }

                if (Rotor == null)
                {
                    throw new NullReferenceException("Rotor");
                }

                return Rotor.Cycled;
            }
        }

        public bool FakeFullyCyceled
        { get; set; }

        public bool NoRotor
        { get; set; }

        public ITurn NextTurn
        {
            get; set;
        }

        public bool NextTurnNull
        {
            get; set;
        }

        public IRotor Rotor
        {
            get; set;
        }

        public bool RotorNull
        {
            get; set;
        }

        public bool StarterRotor
        {
            get; set;
        }

        public void Clear()
        {
            m_SettingsInUse = false;
            Rotor = null;
            NextTurn = null;
        }

        public void Reset()
        {
            m_SettingsInUse = false;
        }

        public bool FakeTurnRotor
        { get; set; }

        public bool TurnRotorCalled
        { get; set; }

        public void TurnRotor()
        {
            if(FakeTurnRotor)
            {
                TurnRotorCalled = true;
                return;
            }

            if (Rotor == null && RotorNull == true)
            {
                throw new ArgumentNullException("Rotor");
            }

            if (FullyCycled)
            {
                m_SettingsInUse = true;
                Rotor.Turn();
                if (NextTurn != null)
                {
                    NextTurn.TurnRotor();
                }
            }
        }
    }
}
