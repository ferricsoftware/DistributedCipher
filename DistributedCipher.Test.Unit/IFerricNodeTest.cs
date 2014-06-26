using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IFerricNodeTest
    {
        protected IFerricNode childFerricNode;
        protected IFerricNode ferricNodeUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestInitialize()]
        public virtual void TestInitialize()
        {
            this.childFerricNode = MockRepository.GenerateMock<IFerricNode>();
        }

        [TestCleanup()]
        public virtual void TestCleanup()
        {
            this.childFerricNode = null;
            this.ferricNodeUnderTest = null;
        }

        [TestMethod]
        public void Decrypt_Test_Calls_Decrypt_With_Byte_Array_On_Child_FerricNode()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            this.childFerricNode.Expect(fn => fn.Decrypt(Arg<byte []>.Is.Anything)).Return(expected);

            //Act
            this.ferricNodeUnderTest.Decrypt(expected);

            //Assert
            this.childFerricNode.VerifyAllExpectations();
        }

        [TestMethod]
        public void Decrypt_Test_Calls_Decrypt_With_String_On_Child_FerricNode()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });
            string injected = "xyz";

            this.childFerricNode.Expect(fn => fn.Decrypt(Arg<byte[]>.Is.Anything)).Return(expected);

            //Act
            this.ferricNodeUnderTest.Decrypt(injected);

            //Assert
            this.childFerricNode.VerifyAllExpectations();
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Encrypt_With_Byte_Array_On_Child_FerricNode()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });

            this.childFerricNode.Expect(fn => fn.Encrypt(Arg<byte[]>.Is.Anything)).Return(expected);

            //Act
            this.ferricNodeUnderTest.Encrypt(expected);

            //Assert
            this.childFerricNode.VerifyAllExpectations();
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Encrypt_With_String_On_Child_FerricNode()
        {
            //Arrange
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', ' '
            });
            string injected = "Stu";

            this.childFerricNode.Expect(fn => fn.Encrypt(Arg<byte[]>.Is.Anything)).Return(expected);

            //Act
            this.ferricNodeUnderTest.Encrypt(injected);

            //Assert
            this.childFerricNode.VerifyAllExpectations();
        }
    }
}
