using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IDistributedCipherTest
    {
        protected IList<ICipher> ciphers;
        protected IDistributedCipher distributedCipherUnderTest;
        protected IHeader header;
        protected Random random;

        [TestInitialize()]
        public virtual void TestInitialize()
        {
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(MockRepository.GenerateMock<ICipher>());
            this.ciphers.Add(MockRepository.GenerateMock<ICipher>());
            this.ciphers.Add(MockRepository.GenerateMock<ICipher>());
            this.header = MockRepository.GenerateMock<IHeader>();
            this.random = MockRepository.GenerateMock<Random>();
        }

        [TestCleanup()]
        public virtual void TestCleanup()
        {
            this.ciphers = null;
            this.distributedCipherUnderTest = null;
            this.header = null;
            this.random = null;
        }

        [TestMethod]
        public void Decrypt_Test_Calls_Decrypt_On_Each_Respective_Cipher()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), expected).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            
            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            this.distributedCipherUnderTest.Decrypt(expected);

            //Assert
            this.header.VerifyAllExpectations();
        }

        [TestMethod]
        public void Decrypt_Test_Calls_Decrypt_On_Header()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            
            this.header.Expect(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), expected).Dummy)).Return(0);
            
            //Act
            this.distributedCipherUnderTest.Decrypt(expected);

            //Assert
            this.header.VerifyAllExpectations();
        }

        [TestMethod]
        public void Decrypt_Test_Returns_Decrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'b', 'b', 'b'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'c', 'd', 'e'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), expected).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('b') };
            byte[] third = new byte[] { Convert.ToByte('b') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_Returns_Decrypted_String()
        {
            //Arrange
            string actual;
            string expected = "iii";
            string injected = "abc";

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), FerricHelper.ConvertToByteArray(injected)).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('i') };
            byte[] second = new byte[] { Convert.ToByte('i') };
            byte[] third = new byte[] { Convert.ToByte('i') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_With_EncryptedLength_Of_Two()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'b', 'a', 'y'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'd', 'e', 'k'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(2);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), FerricHelper.ConvertToByteArray(injected)).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('a') };
            byte[] third = new byte[] { Convert.ToByte('y') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[3]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Decrypt_Test_With_EncryptedLength_Of_Two_That_Doesnt_Remove_Correct_Number_Of_Bytes_Throws_Error()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'd', 'e'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(2);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), FerricHelper.ConvertToByteArray(injected)).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('a') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[3]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Throw(new NotImplementedException());

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
        }

        [TestMethod]
        public void Decrypt_Test_With_Header()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'x', 'g', 'b'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'd', 'e', 't'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[3]).Dummy)).Return(1);
            this.header.Stub(h => h.EncryptedLength).Return(1);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('x') };
            byte[] third = new byte[] { Convert.ToByte('g') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_With_Length_Of_Two()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'g', 'd', 'x', 'B',
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'd', 'e'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(2);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), FerricHelper.ConvertToByteArray(injected)).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('g') };
            byte[] second = new byte[] { Convert.ToByte('d'), Convert.ToByte('x') };
            byte[] third = new byte[] { Convert.ToByte('B') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Decrypt_Test_With_Length_Of_Two_That_Doesnt_Return_Correct_Number_Of_Bytes_Throws_Error()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'd', 'e'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(2);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), FerricHelper.ConvertToByteArray(injected)).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('g') };
            byte[] second = new byte[] { Convert.ToByte('d') };
            byte[] third = new byte[] { Convert.ToByte('B') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), first).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), second).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), third).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
        }

        [TestMethod]
        public void Decrypt_Test_With_Cipher_Overflow()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'b', 'b', 'b'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> {
                'c', 'd', 'e'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), expected).Dummy)).Return(0);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('b') };
            byte[] third = new byte[] { Convert.ToByte('b') };

            this.ciphers[0].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Encrypt_On_Each_Respective_Cipher()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' ',
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            this.distributedCipherUnderTest.Encrypt(expected);

            //Assert
            foreach (ICipher cipher in this.ciphers)
                cipher.VerifyAllExpectations();
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Encrypt_On_Header()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            this.header.Expect(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);

            //Act
            this.distributedCipherUnderTest.Encrypt(expected);

            //Assert
            this.header.VerifyAllExpectations();
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Next_On_Random()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);

            this.random.Expect(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            //Act
            this.distributedCipherUnderTest.Encrypt(expected);

            //Assert
            this.random.VerifyAllExpectations();
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'c', 'f', 'h'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e' 
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_String()
        {
            //Arrange
            string actual;
            string expected = "bdf";
            string injected = "abc";

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('b') };
            byte[] second = new byte[] { Convert.ToByte('d') };
            byte[] third = new byte[] { Convert.ToByte('f') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_Test_With_EncryptedLength_Of_Two()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'c', 'f', 'l', 'h'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e' 
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(2);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f'), Convert.ToByte('l') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Encrypt_Test_With_EncryptedLength_Of_Two_That_Doesnt_Return_Correct_Number_Of_Bytes_Throws_Error()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e' 
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(2);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), first).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), second).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), third).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_With_Header()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'z', 'Z', 'S', 's', 'c', 'f', 'h'
            });

            byte[] headerBytes = FerricHelper.ConvertToByteArray(new List<char> { 
                'z', 'Z', 'S', 's'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e' 
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(headerBytes);
            this.header.Stub(h => h.EncryptedLength).Return(4);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_Test_With_Length_Of_Two()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'c', 'f', 'h'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e', 'b'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(2);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[3]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Encrypt_Test_With_Length_Of_Two_That_Doesnt_Remove_Correct_Number_Of_Bytes_Throws_Error()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e', 'r'
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(2);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(0);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[3]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Throw(new NotImplementedException());

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_With_Cipher_Overflow()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'f', 'h', 'c'
            });

            byte[] injected = FerricHelper.ConvertToByteArray(new List<char> { 
                'c', 'd', 'e' 
            });

            this.ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[0].Stub(c => c.Length).Return(1);
            this.ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[1].Stub(c => c.Length).Return(1);
            this.ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            this.ciphers[2].Stub(c => c.Length).Return(1);
            this.header.Stub(h => h.Encrypt(Arg<int>.Is.Anything)).Return(new byte[0]);
            this.header.Stub(h => h.EncryptedLength).Return(0);
            this.random.Stub(r => r.Next(Arg<int>.Is.Anything)).Return(1);

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            this.ciphers[0].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(first);
            this.ciphers[1].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(second);
            this.ciphers[2].Expect(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(third);

            //Act
            actual = this.distributedCipherUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
