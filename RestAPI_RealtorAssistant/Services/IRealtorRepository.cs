using RestAPI_RealtorAssistant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Services
{
    public interface IRealtorRepository
    {
        //For the Realtor
        Task<bool> RealtorExists(int realtorId);
        Task<bool> RealtorUsernameExists(string realtorUsername);
        Task<IEnumerable<Realtors>> GetRealtors();
        Task<Realtors> GetRealtorById(int realtorId, bool includeAsset);
        Task<Realtors> GetRealtorByUsername(string realtorUsername, bool includeAsset);
        Task AddRealtor(Realtors realtor);
        void DeleteRealtor(Realtors realtors);


        //For the Assets
        Task<IEnumerable<Asset>> GetAssetsForRealtor(int realtorId);
        Task<Asset> GetAssetForRealtorById(int realtorId, int assetId);
        Task<IEnumerable<Asset>> GetAssetsForRealtorByType(int realtorId, string assetType);
        Task<IEnumerable<Asset>> GetAssetsForRealtorByOwnership(int realtorId, bool isOwnedByRealtor);
        Task<IEnumerable<Asset>> GetSoldAssetsOfRealtor(int realtorId, bool isSoldByRealtor);
        Task AddAssetForRealtor(int realtorId, Asset asset);
        void DeleteAsset(Asset asset);
        Task<bool> Save();
    }
}
