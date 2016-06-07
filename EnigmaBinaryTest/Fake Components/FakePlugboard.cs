using System;
using System.Collections.Generic;
using System.Text;

using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{
    public class FakePlugboard : IPlugboard
    { 
        private Dictionary<byte, byte> m_Translation = new Dictionary<byte, byte>(256);
        private const string TRANSPOSITIONSET_NULL = "TranspositionSet is null";
        private const string SETTINGS_ARE_IN_USE = "Can not change end points while in use. You must Reset first.";
        private bool m_SettingsInUse = false;

        public FakePlugboard(List<EndPointPair<byte, byte>> items)
        {
            DefaultSittings();
            PopulateTranslatuon(items.ToArray());
        }

        public FakePlugboard(EndPointPair<byte, byte>[] items)
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
            m_Translation.Clear();
        }

        public EndPointPair<byte, byte>[] TranspositionSet
        {
            get
            {
                if (m_Translation.Count == 0)
                {
                    throw new InvalidOperationException(TRANSPOSITIONSET_NULL);
                }

                EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
                for (int i = 0; i < m_Translation.Count; i++)
                {
                    items[i] = new EndPointPair<byte, byte>((byte)i, m_Translation[(byte)i]);
                }
                return items;
            }

            set
            {
                if(m_SettingsInUse)
                {
                    throw new ArgumentException(SETTINGS_ARE_IN_USE);
                }
                PopulateTranslatuon(value);
            }
        }

        public void Clear()
        {
            DefaultSittings();
            m_SettingsInUse = false;
        }

        public void Reset()
        {
            m_SettingsInUse = false;
        }

        public byte Transpose(byte value)
        {
            m_SettingsInUse = true;
            return m_Translation[value];
        }
    }
}
