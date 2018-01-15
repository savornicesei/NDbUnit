/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using System.IO;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenAddingScriptsFromAnInvalidPath
    {
        [Test]
        public void Can_Bubble_Underlying_Exception_Up_To_Caller()
        {
            Core.ScriptManager manager = new Core.ScriptManager(new FileSystemService());
            Assert.Throws<DirectoryNotFoundException>(() => manager.AddWithWildcard("somefolderthatdoesntexist", "*.sql"));
        }
    }
}