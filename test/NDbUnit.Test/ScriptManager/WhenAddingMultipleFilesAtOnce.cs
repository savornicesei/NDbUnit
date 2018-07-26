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
using System.Linq;

namespace NDbUnit.Test.ScriptManager
{
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class WhenAddingMultipleFilesAtOnce
    {
        [Test]
        public void Can_Add_In_Order_Sorted_By_Name()
        {
            const string FIRSTFILE = "file1.sql";
            const string SECONDFILE = "file2.sql";
            const string THIRDFILE = "file3.sql";

            var mocks = new MockRepository();

            var fileService = mocks.Stub<IFileSystemService>();
            SetupResult.For(fileService.GetFilesInSpecificDirectory(".", "*.*")).Return(new FileInfo[] { new FileInfo(SECONDFILE), new FileInfo(THIRDFILE), new FileInfo(FIRSTFILE) });

            mocks.ReplayAll();

            var manager = new Core.ScriptManager(fileService);

            manager.AddWithWildcard(".", "*.*");

            Assert.AreEqual(FIRSTFILE, manager.Scripts.ElementAt(0).Name);
            Assert.AreEqual(SECONDFILE, manager.Scripts.ElementAt(1).Name);
            Assert.AreEqual(THIRDFILE, manager.Scripts.ElementAt(2).Name);
        }
    }
}