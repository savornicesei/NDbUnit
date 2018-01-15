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
    public class FileSystemService : IFileSystemService
    {
        public IEnumerable<FileInfo> GetFilesInCurrentDirectory(string fileSpec)
        {
            return GetFilesInSpecificDirectory(".", fileSpec);
        }

        public IEnumerable<FileInfo> GetFilesInSpecificDirectory(string pathSpec, string fileSpec)
        {
            DirectoryInfo dir = new DirectoryInfo(pathSpec);
            return dir.GetFiles(fileSpec);
        }

        public FileInfo GetSpecificFile(string fileSpec)
        {
            return new FileInfo(fileSpec);
        }

    }
}
