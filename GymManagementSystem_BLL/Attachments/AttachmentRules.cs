namespace GymManagementSystem_BLL.Attachments
{
    public static class AttachmentRules
    {
        public const long MaxFileSizeBytes = 5 * 1024 * 1024;   // 5MB
        public const int MaxImageDimension = 4000;              // pixels, width and height

        public static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png" };
    }
}
