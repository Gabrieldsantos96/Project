namespace Project.Shared.Consts;

public class FileUploadConsts
{
    /// <summary>
    /// 20 MB
    /// </summary>
    public const long MaxNonVideoFileSize = 20 * 1024 * 1024;

    public const string MaxNonVideoFileSizeString = "20MB";

    public const long MaxVideoFileSize = 350 * 1024 * 1024;

    public const string MaxVideoFileSizeString = "350MB";

    // Lista de extensões permitidas
    public static readonly HashSet<string> AllowedExtensionsForProcedureEvents = new(StringComparer.OrdinalIgnoreCase)
    {
        // Fotos
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp",

        // Vídeos
        ".mp4", ".webm",

        // PDFs
        ".pdf",
    };


    public class MimeTypeConsts
    {
        public const string Pdf = "application/pdf";
        public const string Json = "application/json";
        public const string Xml = "application/xml";
        public const string Html = "text/html";
        public const string Css = "text/css";
        public const string PlainText = "text/plain";
        public const string Jpeg = "image/jpeg";
        public const string Png = "image/png";
        public const string Gif = "image/gif";
        public const string Bmp = "image/bmp";
        public const string Webp = "image/webp";
        public const string Mp3 = "audio/mpeg";
        public const string Mp4 = "video/mp4";
        public const string Ogg = "application/ogg";
        public const string OctetStream = "application/octet-stream";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        public const string MultipartFormData = "multipart/form-data";
        public const string Zip = "application/zip";
        public const string Rar = "application/x-rar-compressed";
        public const string Csv = "text/csv";
        public const string Tiff = "image/tiff";
        public const string Svg = "image/svg+xml";
        public const string Avi = "video/x-msvideo";
        public const string Wav = "audio/wav";
        public const string WebM = "video/webm";
        public const string Doc = "application/msword";
        public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string Xls = "application/vnd.ms-excel";
        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Ppt = "application/vnd.ms-powerpoint";
        public const string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        public const string Msg = "application/vnd.ms-outlook";
        public const string OneNote = "application/onenote";
        public const string Visio = "application/vnd.visio";
        public const string Publisher = "application/x-mspublisher";
    }
}