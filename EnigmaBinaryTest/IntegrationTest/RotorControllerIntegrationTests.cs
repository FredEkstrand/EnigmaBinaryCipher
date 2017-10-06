using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class RotorControllerIntegrationTests
    {
        private string m_Msg = string.Empty;
        private byte[] m_ToEncryptMsg;
        private byte[] m_EncryptedMsg;

        private EndPointPair<byte, byte>[] m_SetSubitution = new EndPointPair<byte, byte>[256];

        private const string ROTOR_TURN_COUNT = "Rotor and Turn Collection count do not match.";
        private const string REFLECTOR_NULL = "Reflector";
        private const string ENTRYROTOR_NULL = "EntryRotor";
        private const string SUBSTITUTIONSET_NULL = "SubstitutionSet is null";

        #region Preparation

        public RotorControllerIntegrationTests()
        {
            Init();
        }

        public void Init()
        {
            //m_Msg = "Hello there every body!";

            m_Msg =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer a orci auctor, pellentesque dui tristique, sagittis arcu. Suspendisse et felis feugiat nisl accumsan" +
            "laoreet a a nunc. Fusce ullamcorper libero et aliquet vulputate. Proin luctus urna et velit rutrum ultricies. Etiam nunc lectus, dictum ut pellentesque gravida, varius" +
            "in enim. Vestibulum rhoncus maximus neque facilisis placerat. Maecenas malesuada libero a eros hendrerit malesuada. Nulla ut dignissim ante, ac dignissim felis. Duis" +
            "vitae nibh enim. Cras velit libero, condimentum sed pharetra vel, convallis vitae diam. Curabitur luctus venenatis tincidunt. Nunc laoreet augue nibh, at congue turpis" +
            "varius sed. Ut eu imperdiet leo. Donec vel justo tellus. Morbi et turpis nec tellus eleifend pretium.\n\n" +

            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque molestie quam eu metus suscipit, quis feugiat felis ultricies. Curabitur sit amet molestie augue. Cras" +
            "id fringilla odio. Cras et ante vestibulum, sollicitudin dolor non, maximus ex. Nunc a fringilla magna. Nunc ornare volutpat nisi, ac dignissim dolor mollis vel. Nunc" +
            "fermentum, enim id venenatis volutpat, nibh turpis volutpat enim, eget auctor dolor quam a mauris. Vivamus a bibendum mi, a interdum quam. Morbi sollicitudin dui eu" +
            "facilisis pellentesque. Mauris tristique purus eu lorem sagittis lacinia.\n\n" +

            "Suspendisse dignissim ipsum non purus tempus, a convallis ipsum mattis. Curabitur rhoncus, elit vel vulputate egestas, leo quam vestibulum turpis, pharetra sollicitudin" +
            "ante nunc eget ipsum. Quisque laoreet vitae enim quis ornare. Quisque in ipsum vel urna venenatis commodo auctor vel augue. Phasellus tincidunt iaculis lacus, eu" +
            "consequat nulla bibendum sit amet. Nullam pharetra a augue a euismod. Mauris gravida, ante vel suscipit vulputate, diam lectus fringilla felis, eget vulputate leo nunc" +
            "et dui. Nullam ut massa nisl. Nulla lobortis volutpat turpis, id auctor metus maximus non. Pellentesque congue eros ac ornare hendrerit. Nulla ornare urna ut tortor" +
            "cursus feugiat. Quisque viverra quam et eleifend porttitor. Vestibulum dapibus ligula ut diam lacinia, a consequat enim cursus.\n\n" +

            "Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris in interdum lectus. Ut laoreet viverra augue, non mattis purus commodo" +
            "ut. Nunc auctor nibh id lorem egestas, non convallis arcu dictum. Etiam interdum turpis justo, ac ultricies nulla tincidunt ac. Donec pulvinar sapien et nunc laoreet" +
            "commodo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Praesent ut posuere ex, feugiat venenatis orci. Vivamus ullamcorper" +
            "enim a lorem tempor consectetur. Cras pharetra tellus mauris, id auctor sem ultricies in. Mauris eu aliquam mi. Pellentesque habitant morbi tristique senectus et netus" +
            "et malesuada fames ac turpis egestas. Etiam feugiat massa cursus, convallis ante vel, vulputate dolor. Nulla facilisi. Fusce sodales diam id tincidunt interdum.\n\n" +

            "Vivamus viverra aliquet metus. Ut consequat commodo sem, id facilisis ipsum tempor sed. Nunc iaculis est vitae egestas scelerisque. Fusce non neque semper, consequat" +
            "magna at, viverra est. Nulla aliquet rutrum ligula, et semper quam feugiat id. Curabitur scelerisque metus posuere feugiat aliquet. Nullam quis leo odio. Pellentesque" +
            "a quam porttitor, eleifend lorem sit amet, egestas mauris. Nullam fringilla enim ligula, non viverra ligula imperdiet ac. Donec justo nisi, ornare quis libero sed," +
            "condimentum accumsan lacus. Cras sapien nisi, feugiat et leo et, eleifend placerat nulla. Aliquam nec lobortis sapien.\n\n" +

            "Donec iaculis in nisi non interdum. Phasellus posuere pretium mauris at porttitor. Praesent accumsan est sed leo consectetur, at sollicitudin diam eleifend. Praesent" +
            "eget libero feugiat, pulvinar nunc et, aliquam odio. Aliquam facilisis quam vitae dapibus bibendum. Duis sed est arcu. In hac habitasse platea dictumst. Curabitur ex" +
            "ipsum, cursus ut tristique nec, lacinia sit amet purus. Aliquam mauris ipsum, commodo vel ultricies nec, gravida eu elit. Maecenas rhoncus, erat eget aliquet" +
            "scelerisque, risus tellus efficitur arcu, vitae auctor dolor tortor non dolor. Phasellus ut ullamcorper nunc, in dictum nibh. Quisque et mauris semper nisi mattis" +
            "ornare. Sed aliquam interdum lorem non iaculis. Morbi ullamcorper dolor sit amet arcu elementum molestie. In quis nisl vulputate, auctor magna nec, posuere lacus. In" + "pretium est sit amet ornare tincidunt.\n\n" +

            "Mauris elementum porttitor lorem, ut malesuada lacus tincidunt at. Nam fermentum erat ut dapibus bibendum. Aenean hendrerit velit nec turpis bibendum cursus. Etiam" +
            "venenatis sed lacus in placerat. Mauris sit amet erat a justo sodales consectetur sed non urna. Pellentesque et gravida nunc. Interdum et malesuada fames ac ante ipsum" +
            "primis in faucibus. Aenean quis orci faucibus, tristique felis quis, ultricies massa. Donec in dictum metus, at vehicula mauris. Suspendisse finibus feugiat augue," +
            "ultrices tempus erat blandit quis. Nullam scelerisque rutrum lorem et viverra. Sed tincidunt justo id erat dapibus vestibulum. Mauris tincidunt facilisis dolor. Sed" +
            "luctus suscipit lacus nec auctor. Donec in lacinia mauris, egestas ullamcorper mi.\n\n" +

            "Nam pulvinar tincidunt sodales. Maecenas ullamcorper metus feugiat, porta felis sed, interdum tortor. Vivamus blandit odio elementum dapibus pharetra. Ut ullamcorper" +
            "cursus nunc, sed iaculis quam luctus id. Cras lacinia, leo eget dapibus ornare, mi nibh semper elit, pulvinar pretium eros eros eu ex. Nunc vehicula consectetur justo," +
            "sed lacinia ipsum eleifend at. Aliquam a justo odio. Ut cursus dolor justo, et venenatis nisl sagittis eget. Sed pretium elementum lorem, ac tincidunt sem viverra a." +
            "Fusce sed dolor id enim elementum commodo. Curabitur a neque a dui bibendum vestibulum vitae ut mi. Curabitur vel tincidunt nibh.\n\n" +

            "Vestibulum enim sem, mollis at condimentum eu, euismod id est. Morbi commodo pulvinar fringilla. Mauris dignissim nulla elit, non eleifend ligula sollicitudin non." +
            "Donec mattis luctus nulla. Mauris id libero tincidunt, sollicitudin arcu ac, luctus urna. Vivamus tellus ipsum, tempor in orci faucibus, lacinia tincidunt libero." +
            "Mauris nisi dui, laoreet sed bibendum vel, convallis id nulla. Donec luctus malesuada nisl, ut luctus eros faucibus ac. Nulla eget purus malesuada, volutpat urna ac," +
            "tristique nisi. Mauris purus tortor, sodales a laoreet ac, ultrices vitae mi. Maecenas placerat nisi et consequat fringilla. Mauris at laoreet quam. Mauris rutrum urna" +
            "sodales, pretium diam a, posuere magna. Pellentesque ut consectetur enim.\n\n" +

            "Fusce ac orci at libero finibus porttitor non at elit. Nulla tellus nulla, sollicitudin et risus non, consequat bibendum velit. Cras mollis est id accumsan tincidunt." +
            "Nunc cursus felis ex, non fringilla lorem imperdiet non. Nullam mattis ex nunc, sit amet luctus nibh ultricies eget. In tristique enim in tellus bibendum ultrices." +
            "Quisque ac sollicitudin nunc.\n\n";

            m_ToEncryptMsg = GetBytes(m_Msg);
            FakeEndPointPairs();
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

        private EndPointPair<byte, byte>[] FakeEndPointsReverse()
        {
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
            for (int i = 0; i < 256; i++)
            {
                items[i] = new EndPointPair<byte, byte>((byte)i, (byte)(255 - i));
            }
            return items;
        }

        // very simple assignment for all endpoints.
        private EndPointPair<byte, byte>[] FakeEndPointPairs()
        {
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
            for (int i = 0; i < 256; i++)
            {
                m_SetSubitution[i] = new EndPointPair<byte, byte>((byte)i, (byte)(255 - i));
                items[i] = new EndPointPair<byte, byte>((byte)i, (byte)(255 - i)); ;
            }

            return items;
        }

        #endregion

        #region Operational Tests with no errors

        [Test, Category("RotorController Integration")]
        public void Encryption_No_Errors()
        {

            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            rc.Initialize();
            m_EncryptedMsg = new byte[m_ToEncryptMsg.Length];
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                m_EncryptedMsg[i] = rc.ProcessByte(m_ToEncryptMsg[i]);
            }

            byte[] decodedMsg = new byte[m_ToEncryptMsg.Length];
            rc.Reset();

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decodedMsg[i] = rc.ProcessByte(m_EncryptedMsg[i], false);
            }

            string stringMsg = GetString(decodedMsg);

            StringAssert.AreEqualIgnoringCase(m_Msg, stringMsg);
        }

        [Test, Category("RotorController Integration")]
        public void Clear()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            rc.Initialize();
            m_EncryptedMsg = new byte[m_ToEncryptMsg.Length];
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                m_EncryptedMsg[i] = rc.ProcessByte(m_ToEncryptMsg[i]);
            }

            byte[] decodedMsg = new byte[m_ToEncryptMsg.Length];
            rc.Reset();

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decodedMsg[i] = rc.ProcessByte(m_EncryptedMsg[i], false);
            }

            rc.Clear();
            // clear operation clears the following properties 
            Assert.AreEqual(0, rc.Rotors.Count);
            Assert.AreEqual(0, rc.Turns.Count);
            Assert.AreEqual(null, rc.Reflector);
            Assert.AreEqual(null, rc.EntryRotor);
            
        }

        [Test, Category("RotorController Integration")]
        public void Reset()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            rc.Initialize();
            m_EncryptedMsg = new byte[m_ToEncryptMsg.Length];
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                m_EncryptedMsg[i] = rc.ProcessByte(m_ToEncryptMsg[i]);
            }

            rc.Reset();

            // reset would set each rotor position back to its start position
            Assert.AreEqual(rc.Rotors[0].StartPosition, rc.Rotors[0].Position);
            Assert.AreEqual(rc.Rotors[1].StartPosition, rc.Rotors[1].Position);
            Assert.AreEqual(rc.Rotors[2].StartPosition, rc.Rotors[2].Position);
            
        }

        #endregion

        #region Operational Tests with errors

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_Reflector()
        { 
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }

            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(REFLECTOR_NULL, ex.Message);

        }

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_EnteryRotor()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }

            rc.Reflector = new Reflector(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ENTRYROTOR_NULL, ex.Message);
        }

        [Test, Category("RotorController Integration")]
        public void Initializer_Mismatch_Rotor_Turn()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }

            rc.Rotors.RemoveAt(0);
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }
       

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_All_Rotors()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Turn t = new Turn();
                rc.Turns.Add(t);
            }
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_One_Rotor()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }
            rc.Rotors.RemoveAt(0);
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_All_Turns()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                rc.Rotors.Add(r);
  
            }
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }

        [Test, Category("RotorController Integration")]
        public void Initializer_Missing_One_Turn()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }
            rc.Turns.RemoveAt(0);
            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }

        [Test, Category("RotorController Integration")]
        public void Encryption_Missing_Rotor_EndPoints()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                Rotor r = new Rotor(m_SetSubitution);
                if (i == 1)
                {
                    r = new Rotor();
                }
                Turn t = new Turn();
                rc.Rotors.Add(r);
                rc.Turns.Add(t);
            }

            rc.Reflector = new Reflector(m_SetSubitution);
            rc.EntryRotor = new EntryRotor(m_SetSubitution);
            rc.Initialize();

            m_EncryptedMsg = new byte[m_ToEncryptMsg.Length];
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                var ex = Assert.Catch<Exception>(() => m_EncryptedMsg[i] = rc.ProcessByte(m_ToEncryptMsg[i]));
                StringAssert.Contains(SUBSTITUTIONSET_NULL, ex.Message);
            }
            
        }

        #endregion

        #region #!

        [Test, Category("RotorController Integration")]
        [Ignore("Confidence Test")]
        public void Two_Rotor_Encode_Decode()
        {
            /*
             * This test is to iterate 256 x 256 with two rotors ensuring each rotor 
             *  has cycled at least once during encryption and decryption.
             * The encryption and decryption is done using two different instances of
             *  RotorController. One rotor controller passes the configuration to the other.
             * This test currently focus on clockwise rotation.
             */
            RotorController rcEncrypt = new RotorController();
            RotorController rcDecrypt = new RotorController();

            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            string result;

            rcEncrypt.EntryRotor = new EntryRotor(FakeEndPointPairs());
            rcEncrypt.Reflector = new Reflector(FakeEndPointPairs());
            for (int i = 0; i < 2; i++)
            {
                rcEncrypt.Rotors.Add(new Rotor(FakeEndPointsReverse()));
                rcEncrypt.Turns.Add(new Turn());
            }
            rcEncrypt.Initialize();

            rcDecrypt.EntryRotor = new EntryRotor(FakeEndPointPairs());
            rcDecrypt.Reflector = new Reflector(FakeEndPointPairs());
            for (int i = 0; i < 2; i++)
            {
                rcDecrypt.Rotors.Add(new Rotor(FakeEndPointsReverse()));
                rcDecrypt.Turns.Add(new Turn());
            }
            rcDecrypt.Initialize();

            // encode
            for (int page = 0; page < 65536; page++)
            {
                for (int i = 0; i < m_ToEncryptMsg.Length; i++)
                {
                    encrypted[i] = rcEncrypt.ProcessByte(m_ToEncryptMsg[i]);
                }

                for (int i = 0; i < m_ToEncryptMsg.Length; i++)
                {
                    decrypted[i] = rcDecrypt.ProcessByte(encrypted[i],false);
                }

                result = GetString(decrypted);
                StringAssert.AreEqualIgnoringCase(m_Msg, result);

                result = string.Empty;
                encrypted = new byte[m_ToEncryptMsg.Length];
                decrypted = new byte[m_ToEncryptMsg.Length];
            }

        }

        #endregion
    }
}
