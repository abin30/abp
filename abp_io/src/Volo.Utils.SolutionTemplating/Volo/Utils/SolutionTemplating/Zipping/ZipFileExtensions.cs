using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
 using Volo.Utils.SolutionTemplating.Files;

namespace Volo.Utils.SolutionTemplating.Zipping
{
    public static class ZipFileExtensions
    {
        public static byte[] GetBytes(this ZipArchiveEntry zipFile)
        {
            using (var ms = new MemoryStream())
            {
                using (var stream = zipFile.Open())
                {
                      stream.CopyTo(ms);
                      return  ms.ToArray();
                }
            }
        }

        public static bool IsDirectory(this ZipArchiveEntry ZipFile)
        {
            return ZipFile.FullName.EndsWith("/");
        }
        public static FileEntryList ToFileEntryList(this ZipArchive zipFile, string rootFolder = null)
        {
            var zipEntries = zipFile.Entries.ToList();

            if (rootFolder != null)
            {
                zipEntries = zipFile.Entries.Where(entry => 
                entry.FullName.StartsWith(rootFolder)).ToList();
            }

            var fileEntries = new List<FileEntry>();

            foreach (var zipEntry in zipEntries)
            {


                var fileName = zipEntry.FullName;

                if (rootFolder != null)
                {
                     fileName = fileName.RemovePreFix(rootFolder);
                }

                if (fileName.IsNullOrEmpty())
                {
                    continue;
                }
            fileEntries.Add(new FileEntry(fileName, zipEntry.GetBytes(), zipEntry.IsDirectory())); 
            }

            return new FileEntryList(fileEntries);
        }
    }
}
