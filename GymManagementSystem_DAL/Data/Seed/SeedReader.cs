using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymManagementSystem_DAL.Data.Seed
{
    internal static class SeedReader
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }   // "Boxing" -> Speciality.Boxing
        };

        public static async Task<List<T>> ReadAsync<T>(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "Files", fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"Seed file '{fileName}' was not found. Check that it is set to 'Copy to Output Directory'.", path);

            await using var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream, Options) ?? [];
        }
    }

    // Collects every problem in a seed file, then fails once with the full list
    internal sealed class SeedProblems(string fileName)
    {
        private readonly List<string> _items = [];

        public void Add(string message) => _items.Add(message);

        public void ThrowIfAny()
        {
            if (_items.Count == 0) return;
            throw new InvalidOperationException(
                $"Seed data in '{fileName}' is invalid:{Environment.NewLine}- "
                + string.Join(Environment.NewLine + "- ", _items));
        }
    }
}
