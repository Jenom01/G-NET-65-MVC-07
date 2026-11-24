namespace GymManagementSystem_BLL.Results
{
    public sealed record class Ok;

    public union Result(Ok, Error);

    public union Result<T>(T, Error);
}
