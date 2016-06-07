using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using NUnit.Common;
using Ekstrand.Encryption.Ciphers;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class EnigmaBinaryIntegrationTests
    {
        private string m_Message = string.Empty;
        private byte[] m_ToEncryptMsg;
        private EnigmaBinaryParameters ebp;



        private EndPointPair<byte, byte>[] FakeEndPoints()
        {
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
            for (int i = 0; i < 256; i++)
            {
                items[i] = new EndPointPair<byte, byte>((byte)i, (byte)i);
            }
            return items;
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }

        public void ConfigurationType(ConfigurationTypes type, int count = 3)
        {
            ebp = new EnigmaBinaryParameters();

            if (type.Equals(ConfigurationTypes.NONE))
            {
                return;
            }

            if (type.HasFlag(ConfigurationTypes.CIPHERCONTROLLER_FULL))
            {
                ebp.CipherController = new CipherController();
                ebp.CipherController.RotorController = new RotorController();
                initializeRemandingComponents(count);
            }

            if (type.HasFlag(ConfigurationTypes.CIPHERCONTROLLER))
            {
                ebp.CipherController = new CipherController();
            }

            if (type.HasFlag(ConfigurationTypes.PLUGBOARD_INSTANCE))
            {
                ebp.Plugboard = new Plugboard(FakeEndPoints());
            }

            if (type.HasFlag(ConfigurationTypes.PLUGBOARD_PARAMETERS))
            {
                ebp.PlugboardParameters.TranspositionSet = FakeEndPoints();
            }

            if (type.HasFlag(ConfigurationTypes.ROTORCONTROLLER_FULL))
            {
                ebp.RotorController = new RotorController();
                ebp.RotorController.EntryRotor = new EntryRotor(FakeEndPoints());
                ebp.Reflector = new Reflector(FakeEndPoints());
                for (int i = 0; i < count; i++)
                {
                    ebp.Rotors.Add(new Rotor(FakeEndPoints()));
                    ebp.Turns.Add(new Turn());
                }
            }

            if (type.HasFlag(ConfigurationTypes.ROTORCONTROLLER))
            {
                ebp.RotorController = new RotorController();
            }

            if (type.HasFlag(ConfigurationTypes.ROTOR_INSTANCE))
            {
                for (int i = 0; i < count; i++)
                {
                    ebp.Rotors.Add(new Rotor(FakeEndPoints()));
                }
            }

            if (type.HasFlag(ConfigurationTypes.ROTOR_PARAMETERS))
            {
                for (int i = 0; i < count; i++)
                {
                    RotorParameters r = new RotorParameters();
                    r.StartPosition = i;
                    r.Incrementation = 1;
                    r.TurnDirection = RotorTurnDirection.CW;
                    r.SubstitutionSet = FakeEndPoints();
                    ebp.RotorParameters.Add(r);
                }
            }

            if (type.HasFlag(ConfigurationTypes.ENTRYROTOR_INSTANCE))
            {
                ebp.EntryRotor = new EntryRotor(FakeEndPoints());
            }

            if (type.HasFlag(ConfigurationTypes.ENTRYROTOR_PARAMETERS))
            {
                ebp.EntryRotorParameters.SubstitutionSet = FakeEndPoints();
            }

            if (type.HasFlag(ConfigurationTypes.REFLECTOR_INSTANCE))
            {
                ebp.Reflector = new Reflector(FakeEndPoints());
            }

            if (type.HasFlag(ConfigurationTypes.REFLECTOR_PARAMETERS))
            {
                ebp.ReflectorParameters.SubstitutionSet = FakeEndPoints();
            }

            if (type.HasFlag(ConfigurationTypes.TURN_INSTANCE))
            {
                for (int i = 0; i < count; i++)
                {
                    ebp.Turns.Add(new Turn());
                }
            }
        }

        private void initializeRemandingComponents(int count)
        {

            if (ebp.CipherController == null)
            {
                ebp.CipherController = new CipherController();
            }

            if (ebp.CipherController.Plugboard == null)
            {
                ebp.CipherController.Plugboard = new Plugboard(FakeEndPoints());
            }

            if (ebp.CipherController.RotorController == null)
            {
                ebp.CipherController.RotorController = new RotorController();
            }

            if (ebp.CipherController.RotorController.EntryRotor == null)
            {
                ebp.CipherController.RotorController.EntryRotor = new EntryRotor(FakeEndPoints());
            }

            if (ebp.CipherController.RotorController.Reflector == null)
            {
                ebp.CipherController.RotorController.Reflector = new Reflector(FakeEndPoints());
            }

            if (ebp.CipherController.RotorController.Rotors.Count == 0)
            {
                if (ebp.CipherController.RotorController.Turns.Count > 0)
                {
                    for (int i = 0; i < ebp.CipherController.RotorController.Turns.Count; i++)
                    {
                        Rotor r = new Rotor(FakeEndPoints());
                        r.Incrementation = 1;
                        r.StartPosition = i;
                        r.TurnDirection = RotorTurnDirection.CW;
                        ebp.CipherController.RotorController.Rotors.Add(r);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        Rotor r = new Rotor(FakeEndPoints());
                        r.Incrementation = 1;
                        r.StartPosition = i;
                        r.TurnDirection = RotorTurnDirection.CW;
                        ebp.CipherController.RotorController.Rotors.Add(r);
                    }
                }
            }

            if (ebp.CipherController.RotorController.Turns.Count == 0)
            {
                if (ebp.CipherController.RotorController.Rotors.Count > 0)
                {
                    for (int i = 0; i < ebp.CipherController.RotorController.Rotors.Count; i++)
                    {
                        Turn t = new Turn();
                        ebp.CipherController.RotorController.Turns.Add(t);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        Turn t = new Turn();
                        ebp.CipherController.RotorController.Turns.Add(t);
                    }
                }
            }
        }

        #region Encryption and Decryption of a file
        /*
         * This set of tests focus on encoding/decoding of 1MB, 2MB and 20MB files; along,
         *  with rotor rotation in CW, CCW, BOTH. (all three rotors in same direction)
         * The 20MB file would allow each rotor to cycled at least one for encoding/decoding.
         * Each test in this region uses two instance of EnigmaBinary one to encode and the other
         *  to decode with passing configuration file to the decode instance.
         * Validation is comparing each byte from the source to the decoded copy.
         */
        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_1MB_File_CW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T1MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));

            
            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false,cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_1MB_File_CCW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T1MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for(int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_1MB_File_BOTH()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T1MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_2MB_File_BOTH()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_2MB_File_CCW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_2MB_File_CW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_20MB_File_CW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T20MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_20MB_File_CCW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T20MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_20MB_File_BOTH()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T1MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        #endregion

        #region Encryption and Decryption of an image file
        /*
         * This set of tests focus on encoding/decoding of an image files; along,
         *  with rotor rotation in CW, CCW, BOTH. (all three rotors in same direction)
         * Each test in this region uses two instance of EnigmaBinary one to encode and the other
         *  to decode with passing configuration file to the decode instance.
         * Validation is comparing each byte from the source to the decoded copy.
         */
        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Image_File_CW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.matrixlarge_2;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Image_File_CCW()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.matrixlarge_2;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }
            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Image_File_BOTH()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.matrixlarge_2;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init();
            for (int i = 0; i < eb1.CipherController.RotorController.Rotors.Count; i++)
            {
                eb1.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }
            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        #endregion

        #region Predefined Configuration file

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_CW_Full()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.PLUGBOARD_INSTANCE | ConfigurationTypes.ENTRYROTOR_INSTANCE | ConfigurationTypes.REFLECTOR_INSTANCE | ConfigurationTypes.ROTOR_INSTANCE | ConfigurationTypes.TURN_INSTANCE);
            
            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true,ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_CCW_Full()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.PLUGBOARD_INSTANCE | ConfigurationTypes.ENTRYROTOR_INSTANCE | ConfigurationTypes.REFLECTOR_INSTANCE | ConfigurationTypes.ROTOR_INSTANCE | ConfigurationTypes.TURN_INSTANCE);

            for (int i = 0; i < ebp.Rotors.Count; i++)
            {
                ebp.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);
            
            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_BOTH_Full()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.PLUGBOARD_INSTANCE | ConfigurationTypes.ENTRYROTOR_INSTANCE | ConfigurationTypes.REFLECTOR_INSTANCE | ConfigurationTypes.ROTOR_INSTANCE | ConfigurationTypes.TURN_INSTANCE);

            for (int i = 0; i < ebp.Rotors.Count; i++)
            {
                ebp.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_CipherController_Full()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.CIPHERCONTROLLER_FULL);

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_RotorController_Full()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.ROTORCONTROLLER_FULL);

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_Rotors_Reflector_Plugboard()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.ROTORCONTROLLER | ConfigurationTypes.REFLECTOR_INSTANCE | ConfigurationTypes.PLUGBOARD_INSTANCE);

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        [Test, Category("EnigmaBinary Integration")]
        public void Encode_Decode_Cfg_Params_Only()
        {
            var testFile = EnigmaBinaryTest.Properties.Resources.T2MB;
            m_ToEncryptMsg = testFile;
            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            ConfigurationType(ConfigurationTypes.PLUGBOARD_PARAMETERS | ConfigurationTypes.ENTRYROTOR_PARAMETERS | ConfigurationTypes.REFLECTOR_PARAMETERS | ConfigurationTypes.ROTOR_PARAMETERS);

            EnigmaBinary eb1 = new EnigmaBinary();
            eb1.Init(true, ebp);

            EnigmaBinaryParameters cfg = eb1.ReturnConfiguration();
            eb1.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, encrypted, 0);

            Assert.AreEqual(false, ByteArrayCompare(m_ToEncryptMsg, encrypted));


            EnigmaBinary eb2 = new EnigmaBinary();
            eb2.Init(false, cfg);
            eb2.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);

            Assert.AreEqual(true, ByteArrayCompare(m_ToEncryptMsg, decrypted));
        }

        #endregion

        [Test, Category("EnigmaBinary Integration")]
        public void Serialization_Entire_Instance_Fully_Populated()
        {
            /*
             * This is not so much a test than just seeing how many bytes
             *  a serialized enigma binary fully populated take.
             */
            byte[] result;
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, eb);
                result = stream.ToArray();
            }

            Console.WriteLine("Fully Populated File size: " + result.Length + " bytes");
        }
    }
}
