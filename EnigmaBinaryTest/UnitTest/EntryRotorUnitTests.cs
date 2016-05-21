using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class EntryRotorUnitTests
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();

        // Provide the same instance of the rotor object to preform basic operation tests.
        private EntryRotor m_Rotor = new EntryRotor();
        private EndPointPair<byte, byte>[] m_SubitutionSet = new EndPointPair<byte, byte>[256];
        private byte result = (byte)0;
        private byte toEncode = (byte)32;
        private const string SUBSTITUTIONSETERR = "Can not change end points while in use. You must Reset first.";
        private const string ARRAYSIZENOT256 = "Array size must be 256";
        private const string DUPLICATE_SIDE_A_ENDPOINTS = "Duplicate Side-A end points.";
        private const string DUPLICATE_SIDE_B_ENDPOINTS = "Duplicate Side-B end points.";
        private const string STARTPOINT_VALUE = "Start position must be between (0-255)";
        private const string TURN_EXCEPTION = "EntryRotor does not turn.";
        private const string INCREMENTATION_VALUE = "Incrementation value must be between (1-255)";
        private const string SUBSTITUTIONSET_NULL = "SubstitutionSet is null";

        private EndPointPair<byte, byte>[] PopulatedSubitutionSet()
        {
            return m_SubitutionSet = rand.GenerateEntryRotor();
        }


        #region Initialization of EntryRotor Tests

        [Test]
        [Category("EntryRotor")]
        public void Next_Rotor_Default_Value()
        {
            Assert.AreEqual(null, m_Rotor.NextRotor);
        }

        [Test]
        [Category("EntryRotor")]
        public void SubstitutionSet_Default_Value()
        {
            Assert.AreEqual(null, m_Rotor.SubstitutionSet);
        }

        #endregion

        #region Basic operation Tests

        [Test]
        [Category("EntryRotor")]
        public void SubstitutionSet_Constructor_Mapping()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            List<EndPointPair<byte, byte>> items = rotor.SubstitutionSet.ToList();

            for (int k = 0; k < items.Count; k++)
            {
                Assert.AreEqual(m_SubitutionSet[k].SideA, items[k].SideA);
                Assert.AreEqual(m_SubitutionSet[k].SideB, items[k].SideB);
            }
        }

        [Test]
        [Category("EntryRotor")]
        public void SubstitutionSet_Property_Mapping()
        {
            EntryRotor rotor = new EntryRotor();
            rotor.SubstitutionSet = PopulatedSubitutionSet();
            List<EndPointPair<byte, byte>> items = rotor.SubstitutionSet.ToList();

            for (int k = 0; k < items.Count; k++)
            {
                Assert.AreEqual(m_SubitutionSet[k].SideA, items[k].SideA);
                Assert.AreEqual(m_SubitutionSet[k].SideB, items[k].SideB);
            }
        }

        [Test]
        [Category("EntryRotor")]
        public void Clear()
        {
            
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            rotor.ProcessByte((byte)0);

            rotor.Clear();

            Assert.AreEqual(null, rotor.SubstitutionSet);
        }

        [Test]
        [Category("EntryRotor")]
        public void Clear_Then_Add_SubstitutionSet()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            rotor.ProcessByte((byte)0);

            var ex = Assert.Catch<Exception>(() => rotor.SubstitutionSet = PopulatedSubitutionSet());
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);

            rotor.Clear();

            Assert.DoesNotThrow(() => rotor.SubstitutionSet = PopulatedSubitutionSet());
        }

        #endregion

        #region Operational Tests    

        [Test]
        [Category("EntryRotor")]
        public void ProcessByte_Encoding_Decoding()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());

            for (int pos = 0; pos < 270; pos++)
            {
                // encode
                result = rotor.ProcessByte(toEncode);
                Assert.AreEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);

                // decode
                result = rotor.ProcessByte(result, false);
                Assert.AreEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);
            }

        }

        [Test]
        [Category("EntryRotor")]
        public void ProcessByte_NextTurn_Call()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            FakeRotor fr = new FakeRotor(PopulatedSubitutionSet());
            fr.NextRotorNull = false;
            fr.FakeProcessByte = true;
            rotor.NextRotor = fr;

            rotor.ProcessByte((byte)42);

            Assert.AreEqual(true, ((FakeRotor)rotor.NextRotor).FakeNextRotorCalled);
        }

        [Test]
        [Category("EntryRotor")]
        public void Reset_Change_Settings()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            rotor.ProcessByte((byte)42);

            rotor.Reset();

            Assert.DoesNotThrow(() => rotor.SubstitutionSet = PopulatedSubitutionSet());
            
        }

        #endregion

        #region Exceptions Tests

        #region Exception SubstitutionSet Test

        [Test]
        [Category("EntryRotor")]
        public void Change_SubstitutionSet_Exception()
        {
            EntryRotor rotor = new EntryRotor(PopulatedSubitutionSet());
            rotor.ProcessByte(toEncode);
            var ex = Assert.Catch<Exception>(() => rotor.SubstitutionSet = m_SubitutionSet);
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);
        }

        [Test]
        [Category("EntryRotor")]
        public void SubstitutionSet_Size_Exception()
        {
            PopulatedSubitutionSet();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>(m_SubitutionSet);
            items.RemoveAt(20);
            m_SubitutionSet = items.ToArray();

            EntryRotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new EntryRotor(m_SubitutionSet));
            StringAssert.Contains(ARRAYSIZENOT256, ex.Message);
        }

        [Test]
        [Category("EntryRotor")]
        public void Duplicat_Side_A_Exception()
        {
            PopulatedSubitutionSet();
            m_SubitutionSet[20].SideA = m_SubitutionSet[33].SideA;

            EntryRotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new EntryRotor(m_SubitutionSet));
            StringAssert.Contains(DUPLICATE_SIDE_A_ENDPOINTS, ex.Message);

        }

        [Test]
        [Category("EntryRotor")]
        public void Duplicat_Side_B_Exception()
        {
            PopulatedSubitutionSet();
            m_SubitutionSet[20].SideB = m_SubitutionSet[33].SideB;

            EntryRotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new EntryRotor(m_SubitutionSet));
            StringAssert.Contains(DUPLICATE_SIDE_B_ENDPOINTS, ex.Message);

        }

        #endregion

        #endregion
    }
}
