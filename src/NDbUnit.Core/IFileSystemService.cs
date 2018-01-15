/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Collections.Generic;
using System.IO;


namespace NDbUnit.Core
{
    public interface IFileSystemService
    {
        IEnumerable<FileInfo> GetFilesInCurrentDirectory(string fileSpec);
        IEnumerable<FileInfo> GetFilesInSpecificDirectory(string pathSpec, string fileSpec);
        FileInfo GetSpecificFile(string fileSpec);
    }
}
