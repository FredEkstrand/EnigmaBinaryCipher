using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class EnigmaBinaryUnitTest
    {
        private RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
        private string m_Message = string.Empty;
        private byte[] m_ToEncryptMsg;
        private EnigmaBinaryParameters m_Param;

        public EnigmaBinaryUnitTest()
        {
            Init();
        }

        public void Init()
        {
            //m_Message = "Hello there every body!";

            m_Message =
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

            m_ToEncryptMsg = GetBytes(m_Message);

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

        #region Initialization EnigmaBinary Tests

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_CipherController()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(null, eb.CipherController);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_Plugboard()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(null, eb.Plugboard);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_EntryRotor()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(null, eb.EntryRotor);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_Reflector()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(null, eb.Reflector);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_RotorController()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(null, eb.RotorController);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_Version()
        {
            EnigmaBinary eb = new EnigmaBinary();

            StringAssert.Contains(typeof(EnigmaBinary).Assembly.GetName().Version.ToString(), eb.Version);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_IsPartialBlockOkay()
        {
            EnigmaBinary eb = new EnigmaBinary();

            Assert.AreEqual(false, eb.IsPartialBlockOkay);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Initialization_Algorithm_Name()
        {
            EnigmaBinary eb = new EnigmaBinary();

            StringAssert.Contains("EnigmaBinary", eb.AlgorithmName);
        }

        #endregion

        #region Basic Operations Tests

        [Test, Category("EnigmaBinary Unit")]
        public void Reset()
        {
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();
            byte[] items = new byte[1];
            items[0] = (byte)42;
            byte[] output = new byte[1];

            eb.ProcessBytes(items, 0, items.Length,  output, 0);
            eb.Reset();

            Assert.DoesNotThrow(() => eb.CipherController.RotorController.Rotors[0].SubstitutionSet = rand.GenerateRotor());
        }


        [Test, Category("EnigmaBinary Unit")]
        public void ReturnConfiguration()
        {
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            EnigmaBinaryParameters param = eb.ReturnConfiguration();

            Assert.AreNotEqual(null, param.Plugboard);
            Assert.AreNotEqual(null, param.EntryRotor);
            Assert.AreNotEqual(null, param.Reflector);
            Assert.AreNotEqual(0, param.Rotors.Count);
            Assert.AreNotEqual(0, param.Turns.Count);
        }

        #endregion

        #region Operational Tests

        [Test, Category("EnigmaBinary Unit")]
        public void ReturnByte_Encode_decode()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length];
            byte[] Decoded = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            Output[0] = eb.ReturnByte(m_ToEncryptMsg[0]);
            string encodedString = GetString(Output);

            StringAssert.AreNotEqualIgnoringCase(m_Message[0].ToString(), encodedString[0].ToString());
            m_Param = eb.ReturnConfiguration();

            // reset
            eb = null;
            eb = new EnigmaBinary();
            eb.Init(false, m_Param);

            Decoded[0] = eb.ReturnByte(Output[0]);
            encodedString = GetString(Decoded);

            StringAssert.AreEqualIgnoringCase(m_Message[0].ToString(), encodedString[0].ToString());
        }

        [Test, Category("EnigmaBinary Unit")]
        public void ProcessBytes_Encode_Decode()
        {
            // Sense encoding and decoding is done in the same method it also
            // utilize cipher parameters to decode a new instance; thus, testing 
            // three separate functions in one method call.

            byte[] Output = new byte[m_ToEncryptMsg.Length];
            byte[] Input = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            eb.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, Output, 0);
            string encodedString = GetString(Output);

            StringAssert.AreNotEqualIgnoringCase(m_Message, encodedString);
            m_Param = eb.ReturnConfiguration();

            // reset
            eb = null;
            eb = new EnigmaBinary();
            eb.Init(false,m_Param);

            eb.ProcessBytes(Output, 0, m_ToEncryptMsg.Length, Input, 0);
            encodedString = GetString(Input);

            StringAssert.AreEqualIgnoringCase(m_Message, encodedString);
        }

        #endregion

        #region Exceptions Tests

        [Test, Category("EnigmaBinary Unit")]
        public void ProcessBlock_Without_Init()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length];
            EnigmaBinary eb = new EnigmaBinary();

            var ex = Assert.Catch<Exception>(() => eb.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, Output, 0));
            StringAssert.Contains("You must call Init(...) before processing blocks", ex.Message);
            
        }

        [Test, Category("EnigmaBinary Unit")]
        public void ProcessBlock_Input_Buffer_Error()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length];
            byte[] Decoded = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            var ex = Assert.Catch<Exception>(() => eb.ProcessBytes(m_ToEncryptMsg, 44, m_ToEncryptMsg.Length, Output, 0));
            StringAssert.Contains("Input buffer too short.", ex.Message);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void ProcessBlock_Output_Buffer_Error()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length-44];
            byte[] Decoded = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            var ex = Assert.Catch<Exception>(() => eb.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, Output, 0));
            StringAssert.Contains("Output buffer too short.", ex.Message);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void EntryRotor_SubstitutionSet()
        {
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            var ex = Assert.Catch<Exception>(() => eb.EntryRotor = new EntryRotor());
            StringAssert.Contains("EntryRotor SubstitutionSet", ex.Message);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Reflector_SubstitutionSet()
        {
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            var ex = Assert.Catch<Exception>(() => eb.Reflector = new Reflector());
            StringAssert.Contains("Reflector SubstitutionSet", ex.Message);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void Plugboard_TranspositionSet()
        {
            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            var ex = Assert.Catch<Exception>(() => eb.Plugboard = new Plugboard());
            StringAssert.Contains("Plugboard TranspositionSet", ex.Message);
        }

        [Test, Category("EnigmaBinary Unit")]
        public void RotorController_Inuse_Error()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length];
            byte[] Decoded = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            eb.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, Output, 0);

            var ex = Assert.Catch<Exception>(() => eb.RotorController = new RotorController());
            StringAssert.Contains("RotorController in use. You must Reset before changing objects.", ex.Message); 
        }

        [Test, Category("EnigmaBinary Unit")]
        public void CipherController_Inuse_Error()
        {
            byte[] Output = new byte[m_ToEncryptMsg.Length];
            byte[] Decoded = new byte[m_ToEncryptMsg.Length];

            EnigmaBinary eb = new EnigmaBinary();
            eb.Init();

            eb.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, Output, 0);

            var ex = Assert.Catch<Exception>(() => eb.CipherController = new CipherController());
            StringAssert.Contains("CipherController in use. You must Reset before changing objects.", ex.Message);
        }

        #endregion

    }
}
