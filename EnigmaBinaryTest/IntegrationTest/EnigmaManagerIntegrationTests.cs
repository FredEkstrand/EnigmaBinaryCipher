using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class EnigmaManagerIntegrationTests
    {
        private string m_Message = string.Empty;
        private byte[] m_ToEncryptMsg;

        public EnigmaManagerIntegrationTests()
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

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Fully_Configured_CW()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for(int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Fully_Configured_CCW()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();
            for (int i = 0; i < 3; i++)
            {
                em.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.CCW;
            }
            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Fully_Configured_BOTH()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            // set each rotor turn direction to both enum value
            for (int i = 0; i < 3; i++)
            {
                em.CipherController.RotorController.Rotors[i].TurnDirection = RotorTurnDirection.BOTH;
            }

            byte[] encrypted = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encrypted[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string stringEncrypted = GetString(encrypted);
            StringAssert.AreNotEqualIgnoringCase(m_Message, stringEncrypted);

            em.Reset();
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encrypted[i], false);
            }

            stringEncrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, stringEncrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Fully_Configured_Internal_Reset()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];
            string encrypted = string.Empty;
            string strDecrypted = string.Empty;

            // first cycle of encrypted-decrypted
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            // second cycle of encryption-decryption
            encryptedMsg = new byte[m_ToEncryptMsg.Length];
            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            encrypted = string.Empty;
            encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Custom_Configured_Rotors()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            em.CipherController.RotorController.Rotors.Clear();
            for(int i = 0; i < 3; i++)
            {
                em.CipherController.RotorController.Rotors.Add(new Rotor(FakeEndPoints()));
            }
            // reinitialized rotor controller reset turn rotor references.
            //em.CipherController.RotorController.Initializer();

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Custom_Configured_EntryRotor()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            em.CipherController.RotorController.EntryRotor = new EntryRotor(FakeEndPoints());

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Custom_Configured_Reflector()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            em.CipherController.RotorController.Reflector = new Reflector(FakeEndPoints());

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Custom_Configured_Turn()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();

            em.CipherController.RotorController.Turns[0].Rotor = em.CipherController.RotorController.Rotors[2];
            em.CipherController.RotorController.Turns[0].StarterRotor = false;

            em.CipherController.RotorController.Turns[1].Rotor = em.CipherController.RotorController.Rotors[0];
            em.CipherController.RotorController.Turns[1].StarterRotor = true;

            em.CipherController.RotorController.Turns[2].Rotor = em.CipherController.RotorController.Rotors[1];
            em.CipherController.RotorController.Turns[2].StarterRotor = false;

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }

        [Test, Category("EnigmaManager Integration")]
        public void ProcessBytes_Custom_Configured_Plugboard()
        {
            EnigmaManager em = new EnigmaManager();
            em.Initialize();
            em.CipherController.Plugboard = new Plugboard(FakeEndPoints());

            byte[] encryptedMsg = new byte[m_ToEncryptMsg.Length];
            byte[] decrypted = new byte[m_ToEncryptMsg.Length];

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                encryptedMsg[i] = em.ProcessByte(m_ToEncryptMsg[i]);
            }

            string encrypted = GetString(encryptedMsg);
            StringAssert.AreNotEqualIgnoringCase(m_Message, encrypted);

            for (int i = 0; i < m_ToEncryptMsg.Length; i++)
            {
                decrypted[i] = em.ProcessByte(encryptedMsg[i], false);
            }

            string strDecrypted = GetString(decrypted);
            StringAssert.AreEqualIgnoringCase(m_Message, strDecrypted);
        }
    }
}
