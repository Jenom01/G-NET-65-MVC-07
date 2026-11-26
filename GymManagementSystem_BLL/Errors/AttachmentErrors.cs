using GymManagementSystem_BLL.Attachments;
using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class AttachmentErrors
    {
        public static readonly Error NotUploaded =
            new("Attachment.NotUploaded", "Please choose an image", ErrorType.Validation);

        public static readonly Error UnsupportedType =
            new("Attachment.UnsupportedType", "Only JPG and PNG images are allowed", ErrorType.Validation);

        public static readonly Error TooBig =
            new("Attachment.TooBig",
                $"The image must be {AttachmentRules.MaxFileSizeBytes / (1024 * 1024)} MB or smaller",
                ErrorType.Validation);

        public static readonly Error NotAnImage =
            new("Attachment.NotAnImage", "The file is not a valid image", ErrorType.Validation);

        public static readonly Error DimensionsTooLarge =
            new("Attachment.DimensionsTooLarge",
                $"The image is too large in pixels (max {AttachmentRules.MaxImageDimension} x {AttachmentRules.MaxImageDimension})",
                ErrorType.Validation);

        public static readonly Error FileNotFound =
            new("Attachment.FileNotFound", "File not found", ErrorType.NotFound);
    }
}
