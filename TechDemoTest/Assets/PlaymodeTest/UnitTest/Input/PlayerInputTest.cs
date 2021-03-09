using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace PlaymodeTest.UnitTest.Input
{
    public class PlayerInputTest
    {
        private PlayerInput playerInput;
        [SetUp]
        public void PlayerInputSetup()
        {
            playerInput = new PlayerInput();
        }

        [Test]
        public void PlayerInputTestSimplePasses()
        {
            Assert.IsTrue(playerInput.Player.enabled == false);
            playerInput.Enable();
            Assert.IsTrue(playerInput.Player.enabled == true);
            Assert.IsTrue(playerInput.Contains(playerInput.Player.Click));
        }
    }
}