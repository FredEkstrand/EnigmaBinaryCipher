using System;
using System.Collections.Generic;
using System.Text;

using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    public class FakeRotor : IRotor
    {
        private Dictionary<byte, byte> m_Transulation = new Dictionary<byte, byte>(256);
        private bool start = true;
        private const int ELEMENT_SIZE = 256;
        

        public FakeRotor(List<EndPointPair<byte,byte>> items)
        {
            DefaultSittings();
            PopulateTranslatuon(items.ToArray());
        }

        public FakeRotor(EndPointPair<byte, byte>[] items)
        {
            DefaultSittings();
            PopulateTranslatuon(items);
        }

        private void PopulateTranslatuon(EndPointPair<byte, byte>[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                m_Transulation.Add(items[i].SideA, items[i].SideB);
            }
        }

        private void DefaultSittings()
        {
            start = true;
            this.Position = this.StartPosition = 0;
            this.Cycled = false;
            this.Incrementation = 1;
            this.TurnDirection = RotorTurnDirection.CW;
            this.NextRotorNull = false;
            this.FakeProcessByte = false;
            this.FakeNextRotorCalled = false;
        }

        private bool m_Cycled;
        public bool Cycled
        {
            get
            {
                if(FakeCycled)
                {
                    m_Cycled = true;
                }
                return m_Cycled;       
            }

            set
            {
                m_Cycled = value;
            }
        }

        public bool FakeCycled
        { get; set; }

        public int Incrementation
        {
            get; set;
        }

        public IRotor NextRotor
        {
            get; set;
        }

        public bool NextRotorNull
        {
            get; set;
        }

        public int Position
        {
            get; set;
        }

        public int StartPosition
        {
            get; set;
        }

        public EndPointPair<byte, byte>[] SubstitutionSet
        {
            get
            {
                EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
                for (int i = 0; i < m_Transulation.Count; i++)
                {
                    items[i] = new EndPointPair<byte, byte>((byte)i, m_Transulation[(byte)i]);
                }
                return items;
            }

            set
            {
                m_Transulation.Clear();
                PopulateTranslatuon(value);
            }
        }

        public RotorTurnDirection TurnDirection
        {
            get; set;
        }

        public void Clear()
        {
            Reset();
        }

        public bool FakeProcessByte
        { get; set; }

        public bool FakeNextRotorCalled
        { get; set; }

        public byte ProcessByte(byte value, bool encode = true)
        {
            if(FakeProcessByte)
            {
                FakeNextRotorCalled = true;
                return value += 1;   
            }

            byte result = (byte)0;

            if (encode)
            {
                if (NextRotor != null)
                {
                    FakeNextRotorCalled = true;
                    result = NextRotor.ProcessByte(m_Transulation[(byte)(((int)value + Position) % ELEMENT_SIZE)]);
                    return m_Transulation[(byte)(((int)result + Position) % ELEMENT_SIZE)];
                }
                else
                {
                    FakeNextRotorCalled = false;
                    return m_Transulation[(byte)(((int)value + Position) % ELEMENT_SIZE)];
                }
            }
            else
            {
                if (NextRotor != null)
                {
                    FakeNextRotorCalled = true;
                    result = NextRotor.ProcessByte((byte)(((ELEMENT_SIZE + (int)m_Transulation[value]) - Position) % ELEMENT_SIZE), false);
                    return (byte)(((ELEMENT_SIZE + (int)m_Transulation[result]) - Position) % ELEMENT_SIZE);
                }
                else
                {
                    FakeNextRotorCalled = false;
                    return (byte)(((ELEMENT_SIZE + (int)m_Transulation[value]) - Position) % ELEMENT_SIZE);
                }
            }
        }

        public void Reset()
        {

            Position = StartPosition;
            Cycled = false;

        }

        public void Turn()
        {

            Position = (Position + 1) % 256;

            if(!start)
            {
                if(Position == StartPosition)
                {
                    Cycled = true;
                }
                else
                {
                    Cycled = false;
                }
            }

            start = false;

        }
    }
}
