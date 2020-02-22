using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    

    public class FileUploadReq
    {
        public Files[] FileUpload { get; set; }
    }

    public class Files
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string base64 { get; set; }
        public File file { get; set; }
    }

    public class File
    {
    }

    public class FileUploadResp
    {
        public FilesResp[] Files { get; set; }
    }
    public class FilesResp {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }


}