using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartDuplicateChecker.API.Services
{
    public interface IVectorStore
    {
        Task<List<VectorItem>> LoadAllVectorsAsync();
        Task AddVectorAsync(VectorItem item);

    }
}
