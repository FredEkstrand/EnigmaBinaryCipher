using System;
using System.Collections.Generic;
using System.Text;

using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{
    [ExcludeFromCoverageAttribute]
    public class FakeEntryRotor : IRotor
    { 
        private Dictionary<byte, byte> m_Translation = new Dictionary<byte, byte>(256);

        public FakeEntryRotor(List<EndPointPair<byte, byte>> items)
        {
            DefaultSittings();
            PopulateTranslatuon(items.ToArray());
        }

        public FakeEntryRotor(EndPointPair<byte, byte>[] items)
        {
            DefaultSittings();
            PopulateTranslatuon(items);
        }

        private void PopulateTranslatuon(EndPointPair<byte, byte>[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                m_Translation.Add(items[i].SideA, items[i].SideB);
            }
        }

        private void DefaultSittings()
        {
            this.Position = this.StartPosition = 0;
            this.Cycled = false;
            this.Incrementation = 1;
            this.TurnDirection = RotorTurnDirection.CW;
        }

        public bool Cycled
        {
            get; set;
        }

        public int Incrementation
        {
            get; set;
        }

        public IRotor NextRotor
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
                for (int i = 0; i < m_Translation.Count; i++)
                {
                    items[i] = new EndPointPair<byte, byte>((byte)i, m_Translation[(byte)i]);
                }
                return items;
            }

            set
            {
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

        public bool ByteProccessedSimple
        { get; set; }

        public byte ProcessByte(byte value, bool encode = true)
        {
            if(ByteProccessedSimple)
            {
                return ProcessedByte(value);
            }
            return m_Translation[value];
        }

        private byte ProcessedByte(byte value)
        {
            return (byte)(((int)value + 1) % 256);
        }

        public void Reset()
        {
            DefaultSittings();
        }

        public void Turn()
        {
            // do nothing
        }
    }
}
