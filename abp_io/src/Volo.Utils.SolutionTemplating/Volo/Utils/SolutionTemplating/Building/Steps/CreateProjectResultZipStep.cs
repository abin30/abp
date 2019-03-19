using System.IO;
using  System.IO.Compression;
using Volo.Utils.SolutionTemplating.Files;
using Volo.Utils.SolutionTemplating.Zipping;

namespace Volo.Utils.SolutionTemplating.Building.Steps
{
    public class CreateProjectResultZipStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Result.ZipContent = CreateZipFileFromEntries(context.Files);
        }

        private static byte[] CreateZipFileFromEntries(FileEntryList entries)
        {
            using (var stream = new MemoryStream())
            {
                using (var resultZipFile = new ZipArchive(stream,ZipArchiveMode.Create))
                {
                    entries.CopyToZipFile(resultZipFile);
                   
                }
                return stream.ToArray();
            }
        }
    }
}
