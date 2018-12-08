namespace src.Data
{
    public class UploadFileRequest
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
    }
}
