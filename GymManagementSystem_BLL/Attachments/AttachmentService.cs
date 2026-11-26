using GymManagementSystem_BLL.Errors;
using GymManagementSystem_BLL.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SkiaSharp;

namespace GymManagementSystem_BLL.Attachments
{
    public class AttachmentService(IHostEnvironment env) : IAttachmentService
    {
        private readonly string _root = Path.GetFullPath(Path.Combine(env.ContentRootPath, "Attachments"));

        public async Task<Result<string>> SaveAsync(IFormFile file, string folder, CancellationToken ct = default)
        {
            var (error, extension) = Validate(file);
            if (error is not null) return error;

            var directory = ResolvePath(folder)
                ?? throw new ArgumentException("Invalid attachment folder.", nameof(folder));

            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directory, fileName);

            try
            {
                await using var stream = new FileStream(
                    fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await file.CopyToAsync(stream, ct);
            }
            catch
            {
                File.Delete(fullPath);   // so we don't leave a half-written file behind
                throw;
            }

            return $"{folder}/{fileName}";
        }

        public Task<Result<AttachmentFile>> GetAsync(string storageKey, CancellationToken ct = default)
        {
            var path = ResolvePath(storageKey);

            if (path is null)
                return Task.FromResult<Result<AttachmentFile>>(AttachmentErrors.FileNotFound);

            try
            {
                var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
                return Task.FromResult<Result<AttachmentFile>>(new AttachmentFile(stream, ContentTypeFor(path)));
            }
            catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                return Task.FromResult<Result<AttachmentFile>>(AttachmentErrors.FileNotFound);
            }
        }

        public Task<Result> DeleteAsync(string storageKey, CancellationToken ct = default)
        {
            var path = ResolvePath(storageKey);

            if (path is null || !File.Exists(path))
                return Task.FromResult<Result>(AttachmentErrors.FileNotFound);

            File.Delete(path);
            return Task.FromResult<Result>(new Ok());
        }

        // Extension -> size -> real content. Returns the extension to store (from the detected format).
        private static (Error? Error, string Extension) Validate(IFormFile? file)
        {
            if (file is null || file.Length == 0)
                return (AttachmentErrors.NotUploaded, "");

            if (!AttachmentRules.AllowedExtensions.Contains(Path.GetExtension(file.FileName)))
                return (AttachmentErrors.UnsupportedType, "");

            if (file.Length > AttachmentRules.MaxFileSizeBytes)
                return (AttachmentErrors.TooBig, "");

            using var stream = file.OpenReadStream();
            using var codec = SKCodec.Create(stream);   // reads the header only; null if not a recognised image

            if (codec is null)
                return (AttachmentErrors.NotAnImage, "");

            var extension = codec.EncodedFormat switch
            {
                SKEncodedImageFormat.Jpeg => ".jpg",
                SKEncodedImageFormat.Png => ".png",
                _ => null
            };

            if (extension is null)
                return (AttachmentErrors.UnsupportedType, "");

            if (codec.Info.Width > AttachmentRules.MaxImageDimension ||
                codec.Info.Height > AttachmentRules.MaxImageDimension)
                return (AttachmentErrors.DimensionsTooLarge, "");

            return (null, extension);
        }

        // Combine with the root, but only accept paths that stay inside it
        private string? ResolvePath(string relativePath)
        {
            var full = Path.GetFullPath(Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar)));

            return full.StartsWith(_root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                ? full
                : null;
        }

        // Only JPG and PNG are ever stored (see Validate)
        private static string ContentTypeFor(string path)
            => string.Equals(Path.GetExtension(path), ".png", StringComparison.OrdinalIgnoreCase)
                ? "image/png"
                : "image/jpeg";
    }
}
