using PracticeAssignment.Models;

namespace PracticeAssignment.Repositories;

public interface IAssetRepository
{
    Task<IEnumerable<Asset>> GetAllAssetsAsync();
    Task<Asset?> GetAssetByIdAsync(int id);
    Task<int> InsertAssetAsync(Asset asset);
    Task<bool> UpdateAssetAsync(int id, Asset asset);
    Task<bool> DeleteAssetAsync(int id);
}
