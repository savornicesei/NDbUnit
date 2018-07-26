/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;

namespace NDbUnit.Test.ScriptManager
{
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class WhenClearingTheScripts
    {
        [Test]
        public void All_Scripts_Are_Removed()
        {
            const string FIRSTFILE = "file1.sql";
            const string SECONDFILE = "file2.sql";

            var mocks = new MockRepository();

            var fileService = mocks.Stub<IFileSystemService>();
            SetupResult.For(fileService.GetSpecificFile(FIRSTFILE)).Return(new FileInfo(FIRSTFILE));
            SetupResult.For(fileService.GetSpecificFile(SECONDFILE)).Return(new FileInfo(SECONDFILE));

            mocks.ReplayAll();

            var manager = new Core.ScriptManager(fileService);

            manager.AddSingle(FIRSTFILE);
            manager.AddSingle(SECONDFILE);

            Assert.IsNotEmpty(manager.Scripts);

            manager.ClearAll();

            Assert.IsEmpty(manager.Scripts);

        }
    }
}

