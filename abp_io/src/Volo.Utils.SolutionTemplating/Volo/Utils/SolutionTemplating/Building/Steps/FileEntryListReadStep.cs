using System.IO;
using Volo.Utils.SolutionTemplating.Files;
using System.IO.Compression;
using Volo.Utils.SolutionTemplating.Zipping;

namespace Volo.Utils.SolutionTemplating.Building.Steps
{
    public class FileEntryListReadStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            if (context.Template.RootPathInZipFile == null)
            {
                context.Files = GetEntriesFromZipFile(context.Template.FilePath);
                return;
            }

            var entryListCachePath = CreateCachePath(context);

            if (File.Exists(entryListCachePath))
            {
                context.Files = GetEntriesFromZipFile(entryListCachePath);
                return;
            }

            context.Files = GetEntriesFromZipFile(
                context.Template.FilePath,
                context.Template.RootPathInZipFile);

            SaveCachedEntries(entryListCachePath, context.Files);
        }

        private static FileEntryList GetEntriesFromZipFile(string filePath, string rootFolder = null)
        {
            using (var templateFileStream = File.OpenRead(filePath))
            {
       
                using (var archive = new ZipArchive(templateFileStream, ZipArchiveMode.Read))
                {
                   
                    return archive.ToFileEntryList(rootFolder);
                }
            }
        }

        private static void SaveCachedEntries(string filePath, FileEntryList entries)
        {
            using (var stream = new FileStream(filePath,FileMode.Create))
            {
                using (var resultZipFile = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    entries.CopyToZipFile(resultZipFile);
                }
            }
        }

        private string CreateCachePath(ProjectBuildContext context)
        {
            return context.Template.FilePath.Replace(".zip", "-filtered.zip");
        }
    }
}