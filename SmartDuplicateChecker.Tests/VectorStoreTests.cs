using SmartDuplicateChecker.API.Services;
using SmartDuplicateChecker.API.DTOs.Responses;
using Xunit;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

public class VectorStoreTests
{
    private const string TestPath = "test_vectors.json";

    [Fact]
    public async Task AddAndLoadVector_WorksCorrectly()
    {
        var store = new JsonVectorStore(TestPath);
        var item = new VectorItem { DocumentId = 1, Title = "Test", Vector = new[] { 0.1f, 0.2f } };

        await store.AddVectorAsync(item);
        var list = await store.LoadAllVectorsAsync();

        Assert.Contains(list, v => v.Title == "Test" && v.Vector.Length == 2);
        File.Delete(TestPath);
    }
}
