using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    public class CipherControllerUnitTest
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();

        private CipherController m_CipherController = new CipherController();

        private const string ROTORCONTROLLER_NULL = "RotorController";
        private const string PLUGBOARD_NULL = "Plugboard";
        private const string TRANSPOSITIONSET_NULL = "TranspositionSet is null";
        private const string ROTORCONTROLLER_INIT_ERROR = "RotorController";
        private const string NOTINITIALIZED = "Not Initialized";

        private EndPointPair<byte,byte>[] FakeEndPoints()
        {
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
            for(int i = 0; i < 256; i++)
            {
                items[i] = new EndPointPair<byte, byte>((byte)i, (byte)i);
            }
            return items;
        }

        #region Initialization of Cipher Controller Tests

        [Test, Category("CipherController Unit")]
        public void Plugboard_Default_Value()
        {
            Assert.AreEqual(null, m_CipherController.Plugboard);
        }

        [Test, Category("CipherController Unit")]
        public void RotorController_Default_Value()
        {
            Assert.AreEqual(null, m_CipherController.RotorController);
        }

        #endregion

        #region Basic Operation

        [Test, Category("CipherController Unit")]
        public void Reset()
        {
            CipherController cc = new CipherController();
            FakeRotorController frc = new FakeRotorController();
            cc.RotorController = frc;
            FakeRotor rotor = new FakeRotor(rand.GenerateRotor());
            rotor.StartPosition = 4;
            rotor.Position = 212;
            cc.RotorController.Rotors.Add(rotor);
            cc.Plugboard = new FakePlugboard(rand.GeneratePlugboard());

            cc.Reset();

            Assert.AreEqual(4, cc.RotorController.Rotors[0].Position);
        }

        [Test, Category("CipherController Unit")]
        public void Clear()
        {
            CipherController cc = new CipherController();
            FakeRotorController frc = new FakeRotorController();
            cc.RotorController = frc;
            FakeRotor rotor = new FakeRotor(rand.GenerateRotor());
            cc.RotorController.Rotors.Add(rotor);
            FakeEntryRotor fer = new FakeEntryRotor(rand.GenerateEntryRotor());
            FakeReflector frf = new FakeReflector(rand.GenerateReflector());
            cc.RotorController.Reflector = frf;
            cc.RotorController.EntryRotor = fer;
            FakePlugboard fpb = new FakePlugboard(rand.GeneratePlugboard());
            cc.Plugboard = fpb;

            cc.Clear();

            EndPointPair<byte, byte>[] items;
            var ex = Assert.Catch<Exception>(() => items = cc.Plugboard.TranspositionSet);
            StringAssert.Contains(TRANSPOSITIONSET_NULL, ex.Message);

            // rotor controller clear() call check from cipher controller
            Assert.AreEqual(0, cc.RotorController.Rotors.Count);
            Assert.AreEqual(null, cc.RotorController.EntryRotor);
            Assert.AreEqual(null, cc.RotorController.Reflector);
        }

        #endregion

        #region Operational Tests 

        [Test, Category("CipherController Unit")]
        public void ProcessByte_Encode_Decode()
        {
            CipherController cc = new CipherController();
            Plugboard pb = new Plugboard(FakeEndPoints());
            FakeRotorController rc = new FakeRotorController();
            rc.FakeProcessByte = true;
            rc.FakeInitializer = true;
            cc.RotorController = rc;
            cc.Plugboard = pb;
            cc.Initialize();

            byte item = (byte)42;

            byte result = cc.ProcessByte(item);

            Assert.AreEqual(43, (int)result);

            result = cc.ProcessByte(result,false);

            Assert.AreEqual(item, result);

        }

        #endregion

        #region Exceptions Tests

        [Test, Category("CipherController Unit")]
        public void Missing_Plugboard_Init_Call()
        {
            CipherController cc = new CipherController();
            FakeRotorController rc = new FakeRotorController();
            rc.FakeProcessByte = true;
            rc.FakeInitializer = true;
            cc.RotorController = rc;

            var ex = Assert.Catch<Exception>(() => cc.Initialize());
            StringAssert.Contains(PLUGBOARD_NULL, ex.Message);
        }

        [Test, Category("CipherController Unit")]
        public void Missing_RotorController_Init_Call()
        {
            CipherController cc = new CipherController();
            Plugboard pb = new Plugboard(rand.GeneratePlugboard());
            cc.Plugboard = pb;

            var ex = Assert.Catch<Exception>(() => cc.Initialize());
            StringAssert.Contains(ROTORCONTROLLER_NULL, ex.Message);
        }

        [Test, Category("CipherController Unit")]
        public void Not_Initialized_ProcessByte_Call()
        {
            CipherController cc = new CipherController();
            Plugboard pb = new Plugboard(FakeEndPoints());
            FakeRotorController rc = new FakeRotorController();
            rc.FakeProcessByte = true;
            rc.FakeInitializer = true;
            cc.RotorController = rc;
            cc.Plugboard = pb;

            byte item = (byte)42;

            var ex = Assert.Catch<Exception>(() => cc.ProcessByte(item));
            StringAssert.Contains(NOTINITIALIZED, ex.Message);
        }

        #endregion
    }
}
