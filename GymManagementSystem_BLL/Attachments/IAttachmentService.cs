using GymManagementSystem_BLL.Results;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem_BLL.Attachments
{
    public interface IAttachmentService
    {
        Task<Result<string>> SaveAsync(IFormFile file, string folder, CancellationToken ct = default);
        Task<Result<AttachmentFile>> GetAsync(string storageKey, CancellationToken ct = default);
        Task<Result> DeleteAsync(string storageKey, CancellationToken ct = default);
    }
}
