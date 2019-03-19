using System;
using System.IO;
using System.IO.Compression;
using Volo.Abp.Domain.Entities;

namespace Volo.Utils.SolutionTemplating.Files
{
    public static class FileEntryListExtensions
    {
        public static void CopyToZipFile(this FileEntryList fileEntryList, ZipArchive zipFile)
        {
            foreach (var entry in fileEntryList)
            {
                try
                {
                    var fileEntry = zipFile.CreateEntry(entry.Name);
                    using (var stream = fileEntry.Open())
                    {
                        stream.Write(entry.Bytes, 0, entry.Bytes.Length);
                        stream.Flush();
                    }

                    //WatchItem item = WatchItemList.List[fileEntry.FullName];
                    //item.OutSize = fileEntry.Length;
                    //item.OutZipSize = fileEntry.CompressedLength;

                    //using (var bw = new BinaryWriter(fileEntry.Open()))
                    //{
                    //    bw.Write(entry.Bytes);

                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}