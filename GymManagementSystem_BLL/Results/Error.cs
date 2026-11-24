namespace GymManagementSystem_BLL.Results
{
    public enum ErrorType
    {
        NotFound,
        Conflict,
        Validation,
        Failure
    }

    public sealed record Error(string Code, string Message, ErrorType Type, string? Field = null);
}
