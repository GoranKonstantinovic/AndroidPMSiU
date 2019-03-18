using Realms;
using System.IO;

namespace AndroidPMSiU.Models
{
    public class AttachmentModel : RealmObject
    {
        public string Data { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        ///// When accessed, reads all data from the picked file and returns it.
        //public byte[] DataArray { get; }

        ///// Filename of the picked file; doesn't contain any path.
        //public string FileName { get; }

        ///// Full file path of the picked file; note that on some platforms the
        ///// file path may not be a real, accessible path but may contain an
        ///// platform specific URI; may also be null.
        //public string FilePath { get; }

        ///// Returns a stream to the picked file; this is the most reliable way
        ///// to access the data of the picked file.
        ////public Stream GetStream();
    }
}