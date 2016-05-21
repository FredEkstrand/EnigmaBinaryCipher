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
    public class PlugboardTests
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
        private EndPointPair<byte, byte>[] m_TranspositionSet = new EndPointPair<byte, byte>[256];
        private Plugboard m_Plugboard = new Plugboard();

        private const string TRANSPOSITIONERR = "Can not change end points while in use. You must Reset first.";
        private const string DUPLICATE_SIDE_A_ENDPOINTS = "Duplicate Side-A end points.";
        private const string DUPLICATE_SIDE_B_ENDPOINTS = "Duplicate Side-B end points.";
        private const string TRANSPOSITION_SIZE = "Translation points too small to provide a strong encryption.";
        private const string TRANSPOSITIONSET_NULL = "TranspositionSet is null";

        private EndPointPair<byte, byte>[] PopulateTranspositionSet(RandomEndPointsGenerator.TranspositionLevel level = RandomEndPointsGenerator.TranspositionLevel.HIGH)
        {
            return m_TranspositionSet = rand.GeneratePlugboard(level);
        }


        #region Initialization of plugboard Tests

        [Test]
        [Category("Plugboard")]
        public void Plugboard_With_Out_EndPoints()
        {
            Plugboard pb = new Plugboard();

            Assert.AreEqual(null, pb.TranspositionSet);
        }

        [Test]
        [Category("Plugboard")]
        public void Plugboard_With_EndPoints()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet());

            bool success = true;
            EndPointPair<byte, byte>[] items;
            items = pb.TranspositionSet;

            // validate through mapping check
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].SideA != items[i].SideA)
                {
                    success = false;
                    System.Console.WriteLine("i: " + i + " A: " + items[i].SideA + " B: " + items[i].SideA);
                }

                if (items[i].SideB != items[i].SideB)
                {
                    success = false;
                    System.Console.WriteLine("i: " + i + " A: " + items[i].SideB + " B: " + items[i].SideB);
                }
            }

            Assert.AreEqual(true, success);
        }

        [Test]
        [Category("Plugboard")]
        public void Plugboard_TranspositionSet_Default()
        {
            Plugboard pb = new Plugboard();

            Assert.AreEqual(null, pb.TranspositionSet);
        }

        #endregion

        #region Basic operation Test

        [Test]
        [Category("Plugboard")]
        public void Clear()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet());
            pb.Clear();

            Assert.AreEqual(null, pb.TranspositionSet);
        }

        [Test]
        [Category("Plugboard")]
        public void Reset()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet());
            pb.Transpose((byte)42);

            pb.Reset();

            Assert.DoesNotThrow(() => pb.TranspositionSet = PopulateTranspositionSet());

        }

        [Test]
        [Category("Plugboard")]
        public void Change_SubstitutionSet_After_Clear()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet());
            pb.Transpose((byte)2);
            pb.Clear();
            Assert.DoesNotThrow(() => pb.TranspositionSet = m_TranspositionSet);
        }

        #endregion

        #region Operational Tests 

        [Test]
        [Category("Plugboard")]
        public void Encod_Decode_Low_Test()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet(RandomEndPointsGenerator.TranspositionLevel.LOW));
            byte result;
            byte toTranspos;

            for (int i = 0; i < 256; i++)
            {
                toTranspos = (byte)i;
                result = pb.Transpose(toTranspos);

                if (result != toTranspos)
                {
                    Assert.AreNotEqual(result, toTranspos);

                    result = pb.Transpose(result);
                    Assert.AreEqual(toTranspos, result);
                }
            }
        }

        [Test]
        [Category("Plugboard")]
        public void Encod_Decode_Medium_Test()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet(RandomEndPointsGenerator.TranspositionLevel.MEDIUM));
            byte result;
            byte toTranspos;

            for (int i = 0; i < 256; i++)
            {
                toTranspos = (byte)i;
                result = pb.Transpose(toTranspos);

                if (result != toTranspos)
                {
                    Assert.AreNotEqual(result, toTranspos);

                    result = pb.Transpose(result);
                    Assert.AreEqual(toTranspos, result);
                }
            }
        }

        [Test]
        [Category("Plugboard")]
        public void Encod_Decode_High_Test()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet(RandomEndPointsGenerator.TranspositionLevel.HIGH));
            byte result;
            byte toTranspos;

            for (int i = 0; i < 256; i++)
            {
                toTranspos = (byte)i;
                result = pb.Transpose(toTranspos);

                if (result != toTranspos)
                {
                    Assert.AreNotEqual(result, toTranspos);

                    result = pb.Transpose(result);
                    Assert.AreEqual(toTranspos, result);
                }
            }
        }

        #endregion

        #region Throwing errors

        [Test]
        [Category("Plugboard")]
        public void TranspositionSet_Change_In_Use()
        {
            Plugboard pb = new Plugboard(PopulateTranspositionSet());
            pb.Transpose((byte)2);
            var ex = Assert.Catch<Exception>(() => pb.TranspositionSet = m_TranspositionSet);
            StringAssert.Contains(TRANSPOSITIONERR, ex.Message);
        }

        [Test]
        [Category("Plugboard")]
        public void TranspositionSet_Size_LOW_Error()
        {
            PopulateTranspositionSet();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>(m_TranspositionSet);
            for (int i = 0; i < 17; i++)
            {
                items.RemoveAt(20);
            }
            m_TranspositionSet = items.ToArray();

            Plugboard pb;
            var ex = Assert.Catch<Exception>(() => pb = new Plugboard(m_TranspositionSet));
            StringAssert.Contains(TRANSPOSITION_SIZE, ex.Message);
        }

        [Test]
        [Category("Plugboard")]
        public void Duplicat_Side_A_Exception()
        {
            PopulateTranspositionSet();
            m_TranspositionSet[20].SideA = m_TranspositionSet[33].SideA;

            Plugboard pb;
            var ex = Assert.Catch<Exception>(() => pb = new Plugboard(m_TranspositionSet));
            StringAssert.Contains(DUPLICATE_SIDE_A_ENDPOINTS, ex.Message);

        }

        [Test]
        [Category("Plugboard")]
        public void Duplicat_Side_B_Exception()
        {
            PopulateTranspositionSet();
            m_TranspositionSet[20].SideB = m_TranspositionSet[33].SideB;

            Plugboard pb;
            var ex = Assert.Catch<Exception>(() => pb = new Plugboard(m_TranspositionSet));
            StringAssert.Contains(DUPLICATE_SIDE_B_ENDPOINTS, ex.Message);

        }
        
        [Test]
        [Category("Plugboard")]
        public void Missing_TranspositionSet()
        {
            Plugboard pb = new Plugboard();
            byte items;

            var ex = Assert.Catch<Exception>(() => items = pb.Transpose((byte)42));
            StringAssert.Contains(TRANSPOSITIONSET_NULL, ex.Message);
        }
        #endregion
    }
}
