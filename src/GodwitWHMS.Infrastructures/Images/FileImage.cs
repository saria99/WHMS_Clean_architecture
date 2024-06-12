#nullable disable

namespace GodwitWHMS.Infrastructures.Images
{
    public class FileImage
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] ImageData { get; set; }
    }
}
