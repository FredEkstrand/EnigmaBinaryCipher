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
    public class RotorTestSet
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
       
        // Provide the same instance of the rotor object to preform basic operation tests.
        private  Rotor m_Rotor = new Rotor(rand.GenerateRotor());
        private EndPointPair<byte, byte>[] m_SubitutionSet = new EndPointPair<byte, byte>[256];
        private  byte result = (byte)0;
        private  byte toEncode = (byte)32;

        private const string SUBSTITUTIONSETERR = "Can not change end points while in use. You must Reset first.";
        private const string ARRAYSIZENOT256 = "Array size must be 256";
        private const string DUPLICATE_SIDE_A_ENDPOINTS = "Duplicate Side-A end points.";
        private const string DUPLICATE_SIDE_B_ENDPOINTS = "Duplicate Side-B end points.";
        private const string STARTPOINT_VALUE = "Start position must be between (0-255)";
        private const string INCREMENTATION_VALUE = "Incrementation value must be between (1-255)";
        private const string SUBSTITUTIONSET_NULL = "SubstitutionSet is null";

        private EndPointPair<byte, byte>[] PopulateSubitutionSet()
        {
            return m_SubitutionSet = rand.GenerateRotor();
        }

        #region Initialization of Rotor Tests

        [Test]
        [Category("Rotor")]
        public void Init_Rotor_With_EndPoints()
        {
            bool success = true;
            EndPointPair<byte, byte>[] items;
            items = rand.GenerateRotor();
            Rotor rotor = new Rotor(items);

            EndPointPair<byte, byte>[] itemRotor;
            itemRotor = rotor.SubstitutionSet;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].SideA != itemRotor[i].SideA)
                {
                    success = false;
                    System.Console.WriteLine("i: " + i + " A: " + items[i].SideA + " B: " + itemRotor[i].SideA);
                }

                if (items[i].SideB != itemRotor[i].SideB)
                {
                    success = false;
                    System.Console.WriteLine("i: " + i + " A: " + items[i].SideB + " B: " + itemRotor[i].SideB);
                }
            }

            Assert.AreEqual(true, success);
        }

        [Test]
        [Category("Rotor")]
        public void Init_Rotor_With_No_EndPoints()
        {
            Rotor rotor = new Rotor();

            Assert.AreEqual(null, rotor.SubstitutionSet);
        }

        [Test]
        [Category("Rotor")]
        public void StartPosition_Defalult_Value()
        {

            Assert.AreEqual(0, m_Rotor.StartPosition);
        }

        [Test]
        [Category("Rotor")]
        public void Position_Default_Value()
        {
            Assert.AreEqual(0, m_Rotor.Position);
        }

        [Test]
        [Category("Rotor")]
        public void Incrementation_Default_Value()
        {
            Assert.AreEqual(1, m_Rotor.Incrementation);
        }

        [Test]
        [Category("Rotor")]
        public void Cycled_Default_Value()
        {
            Assert.AreEqual(false, m_Rotor.Cycled);
        }

        [Test]
        [Category("Rotor")]
        public void TurnDirection_Default_Value()
        {
            Assert.AreEqual(RotorTurnDirection.CW, m_Rotor.TurnDirection);
        }

        [Test]
        [Category("Rotor")]
        public void Next_Rotor_Default_Value()
        {
            Assert.AreEqual(null, m_Rotor.NextRotor);
        }

        #endregion

        #region Basic operation Tests

        [Test]
        [Category("Rotor")]
        public void SubstitutionSet_Mapping()
        {
            Rotor rotor = new Rotor();
            rotor.SubstitutionSet = PopulateSubitutionSet();
            List<EndPointPair<byte, byte>> items = rotor.SubstitutionSet.ToList();

            for(int k = 0; k < items.Count; k++)
            {
                Assert.AreEqual(m_SubitutionSet[k].SideA, items[k].SideA);
                Assert.AreEqual(m_SubitutionSet[k].SideB, items[k].SideB);
            }
        }

        #region Incrementation tests

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_One_CW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            for (int i = 1; i < 270; i++)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Two_CW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 2;
            for (int i = 2; i < 270; i +=2)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Three_CW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 3;
            for (int i = 3; i < 270; i+=3)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        // CCW
        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_One_CCW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.TurnDirection = RotorTurnDirection.CCW;
            for (int i = 511; i > 0; i--)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Two_CCW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 2;
            rotor.TurnDirection = RotorTurnDirection.CCW;

            for (int i = 510; i > 0; i -= 2)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Three_CCW()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 3;
            rotor.TurnDirection = RotorTurnDirection.CCW;

            for (int i = 509; i > 0; i -= 3)
            {
                rotor.Turn();
                Assert.AreEqual((i % 256), rotor.Position);
            }
        }

        // both

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_One_BOTH()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.TurnDirection = RotorTurnDirection.BOTH;
            int lstPosition = -1;
            bool ccwTurn = false;
            bool ccwTurnReset = false;
            for (int i = 0; i < 270; i++)
            {
      
                if (ccwTurn)
                {
                    i = lstPosition;
                    ccwTurn = false;
                    ccwTurnReset = true;
                }

                if (!ccwTurnReset)
                {
                    if (rotor.Position != 0 && i % 4 == 0)
                    {
                        lstPosition = i;
                        i = (256 + (((-256 + i) - 10) % 256)) % 256;
                        ccwTurn = true;
                    }
                }

                Assert.AreEqual((i % 256), rotor.Position);
                rotor.Turn();
                ccwTurnReset = false;
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Two_BOTH()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 2;
            rotor.TurnDirection = RotorTurnDirection.BOTH;
            int lstPosition = -1;
            bool ccwTurn = false;
            bool ccwTurnReset = false;
            for (int i = 0; i < 270; i += 2)
            {

                if (ccwTurn)
                {
                    i = lstPosition;
                    ccwTurn = false;
                    ccwTurnReset = true;
                }

                if (!ccwTurnReset)
                {
                    if (rotor.Position != 0 && i % 4 == 0)
                    {
                        lstPosition = i;
                        i = (256 + (((-256 + i) - 10) % 256)) % 256;
                        ccwTurn = true;
                    }
                }

                Assert.AreEqual((i % 256), rotor.Position);
                rotor.Turn();
                ccwTurnReset = false;
            }
        }

        [Test]
        [Category("Rotor")]
        public void Posistion_Incrementation_By_Three_BOTH()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            rotor.Incrementation = 3;
            rotor.TurnDirection = RotorTurnDirection.BOTH;
            int lstPosition = -1;
            bool ccwTurn = false;
            bool ccwTurnReset = false;
            for (int i = 0; i < 270; i += 3)
            {

                if (ccwTurn)
                {
                    i = lstPosition;
                    ccwTurn = false;
                    ccwTurnReset = true;
                }

                if (!ccwTurnReset)
                {
                    if (rotor.Position != 0 && i % 4 == 0)
                    {
                        lstPosition = i;
                        i = (256 + (((-256 + i) - 10) % 256)) % 256;
                        ccwTurn = true;
                    }
                }

                Assert.AreEqual((i % 256), rotor.Position);
                rotor.Turn();
                ccwTurnReset = false;
            }
        }

        #endregion

        [Test]
        [Category("Rotor")]
        public void Reset()
        {
            Rotor rotor = new Rotor(rand.GenerateRotor());
            for (int i = 0; i < 270; i++) // cycled at least once
            {
                rotor.Turn(); // sets internal in-use bool flag
            }

            rotor.Reset();
            Assert.AreEqual(false, rotor.Cycled);
            Assert.AreEqual(0, rotor.Position);
            Assert.AreEqual(1, rotor.Incrementation);
        }

        [Test]
        [Category("Rotor")]
        public void Clear()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.StartPosition = 212;
            rotor.TurnDirection = RotorTurnDirection.CCW;

            for (int i = 0; i < 5; i++) // cycled at least once
            {
                rotor.Turn();
            }

            rotor.Clear();

            Assert.AreEqual(0, rotor.StartPosition, "Start Position");
            Assert.AreEqual(0, rotor.Position, "Position");
            Assert.AreEqual(RotorTurnDirection.CW, rotor.TurnDirection, "Rotor Turn Direction");
            Assert.AreEqual(null, rotor.SubstitutionSet, "SubstitutionSet Null");
        }


        #endregion

        #region Operational Tests  
          
        [Test]
        [Category("Rotor")]
        public void Cycle_Check()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.Position = 255; // using friend assembly
            rotor.Turn();

            Assert.AreEqual(true, rotor.Cycled);        
        }

        [Test]
        [Category("Rotor")]
        public void Cycle_Check_Custom_StartPosition()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.StartPosition = 42;
            rotor.Position = 41; // using friend assembly
            rotor.Turn();

            Assert.AreEqual(true, rotor.Cycled);
        }

        [Test]
        [Category("Rotor")]
        public void Turn()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            // default position is 0
            rotor.Turn();

            Assert.AreEqual(1, rotor.Position);
        }

        [Test]
        [Category("Rotor")]
        public void NextRotor()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            fr.NextRotorNull = false;
            fr.FakeProcessByte = true;
            rotor.NextRotor = fr;

            byte result = rotor.ProcessByte((byte)42);
            Assert.AreNotEqual((byte)42, result);         
        }

        [Test]
        [Category("Rotor")]
        public void ProcessByte_Encoding_Decoding_CW_Cycled()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());

            for(int pos = 0; pos < 270; pos++)
            {
                // encode
                result = rotor.ProcessByte(toEncode);
                Assert.AreNotEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);

                // decode
                result = rotor.ProcessByte(result, false);
                Assert.AreEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);      
            }

        }

        [Test]
        [Category("Rotor")]
        public void ProcessByte_Encoding_Decoding_CCW_Cycled()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.TurnDirection = RotorTurnDirection.CCW;

            for (int pos = 0; pos < 270; pos++)
            {
                // encode
                result = rotor.ProcessByte(toEncode);
                Assert.AreNotEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);

                // decode
                result = rotor.ProcessByte(result, false);
                Assert.AreEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);
            }

        }

        [Test]
        [Category("Rotor")]
        public void ProcessByte_Encoding_Decoding_BOTH_Cycled()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.TurnDirection = RotorTurnDirection.BOTH;

            for (int pos = 0; pos < 270; pos++)
            {
                // encode
                result = rotor.ProcessByte(toEncode);
                Assert.AreNotEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);

                // decode
                result = rotor.ProcessByte(result, false);
                Assert.AreEqual(toEncode, result, "Position " + rotor.Position + " to encode: " + toEncode + " result: " + result);
            }

        }

        [Test]
        [Category("Rotor")]
        public void Reset_Change()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.ProcessByte((byte)42);

            rotor.Reset();

            // reset should allow the following changes to be made.
            Assert.DoesNotThrow(() => rotor.SubstitutionSet = PopulateSubitutionSet(),"Substitution Set change error");
            Assert.DoesNotThrow(() => rotor.TurnDirection = RotorTurnDirection.BOTH, "Turn Direction change error");
            Assert.DoesNotThrow(() => rotor.Incrementation = 5, "Incrementation change error");
            Assert.DoesNotThrow(() => rotor.StartPosition = 44, "Start Position change error");
        }

        #endregion

        #region Exceptions Tests

        [Test]
        [Category("Rotor")]
        public void Change_SubstitutionSet_Exception()
        {
            Rotor rotor = new Rotor(PopulateSubitutionSet());
            rotor.Turn();
            var ex = Assert.Catch<Exception>(() => rotor.SubstitutionSet = m_SubitutionSet);
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);         
        }

        [Test]
        [Category("Rotor")]
        public void SubstitutionSet_Size_Exception()
        {
            PopulateSubitutionSet();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>(m_SubitutionSet);
            items.RemoveAt(20);
            m_SubitutionSet = items.ToArray();

            Rotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new Rotor(m_SubitutionSet));
            StringAssert.Contains(ARRAYSIZENOT256, ex.Message);
        }

        [Test]
        [Category("Rotor")]
        public void Duplicat_Side_A_Exception()
        {
            PopulateSubitutionSet();
            m_SubitutionSet[20].SideA = m_SubitutionSet[33].SideA;

            Rotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new Rotor(m_SubitutionSet));
            StringAssert.Contains(DUPLICATE_SIDE_A_ENDPOINTS, ex.Message);

        }

        [Test]
        [Category("Rotor")]
        public void Duplicat_Side_B_Exception()
        {
            PopulateSubitutionSet();
            m_SubitutionSet[20].SideB = m_SubitutionSet[33].SideB;

            Rotor rotor;
            var ex = Assert.Catch<Exception>(() => rotor = new Rotor(m_SubitutionSet));
            StringAssert.Contains(DUPLICATE_SIDE_B_ENDPOINTS, ex.Message);

        }

        [Test]
        [Category("Rotor")]
        public void Change_StartPosition()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            rotor.Turn();
            var ex = Assert.Catch<Exception>(() => rotor.StartPosition = 3);
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);    
        }

        [Test]
        [Category("Rotor")]
        public void StartPosition_Negative_Value()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            var ex = Assert.Catch<Exception>(() => rotor.StartPosition = -5);
            StringAssert.Contains(STARTPOINT_VALUE, ex.Message);
        }

        [Test]
        [Category("Rotor")]
        public void StartPosition_Greatherthan_255()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            var ex = Assert.Catch<Exception>(() => rotor.StartPosition = 256);
            StringAssert.Contains(STARTPOINT_VALUE, ex.Message);
        }

        [Test]
        [Category("Rotor")]
        public void Change_TurnDirection_Exception()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            rotor.Turn();
            // note rotor turn direction enum value doesn't matter in this test.
            var ex = Assert.Catch<Exception>(() => rotor.TurnDirection = RotorTurnDirection.CW);
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);
            
        }

        [Test]
        [Category("Rotor")]
        public void Change_Incrementation()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            rotor.Turn();
            var ex = Assert.Catch<Exception>(() => rotor.Incrementation = 5);
            StringAssert.Contains(SUBSTITUTIONSETERR, ex.Message);  
        }

        [Test]
        [Category("Rotor")]
        public void Incrementation_Past_Min_Value()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            var ex = Assert.Catch<Exception>(() => rotor.Incrementation = -1);
            StringAssert.Contains(INCREMENTATION_VALUE, ex.Message);
        }

        [Test]
        [Category("Rotor")]
        public void Incrementation_Past_Max_Value()
        {
            PopulateSubitutionSet();
            Rotor rotor = new Rotor(m_SubitutionSet.ToArray());
            var ex = Assert.Catch<Exception>(() => rotor.Incrementation = 256);
            StringAssert.Contains(INCREMENTATION_VALUE, ex.Message);
        }

        [Test, Category("Rotor")]
        public void ProcessByte_SubitutionSet_Null()
        {
            Rotor rotor = new Rotor();
            byte item;
            var ex = Assert.Catch<Exception>(() => item = rotor.ProcessByte((byte)42));
            StringAssert.Contains(SUBSTITUTIONSET_NULL, ex.Message);
        }

        #endregion
    }
}  
