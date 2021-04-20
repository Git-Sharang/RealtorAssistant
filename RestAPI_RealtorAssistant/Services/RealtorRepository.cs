using Microsoft.EntityFrameworkCore;
using RestAPI_RealtorAssistant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Services
{
    public class RealtorRepository : IRealtorRepository
    {
        private RealtorDBContext _context;
        public RealtorRepository(RealtorDBContext context)
        {
            _context = context;
        }

        //Check if the Realtor Exists
        public async Task<bool> RealtorExists(int realtorId)
        {
            return await _context.Realtor.AnyAsync<Realtors>(r => r.Id == realtorId);
        }

        //Check if the Realtor Username Exists
        public async Task<bool> RealtorUsernameExists(string realtorUsername)
        {
            return await _context.Realtor.AnyAsync<Realtors>(r => r.Username == realtorUsername);
        }

        //Get every Realtor
        public async Task<IEnumerable<Realtors>> GetRealtors()
        {
            var result = _context.Realtor.OrderBy(r => r.Firstname);
            return await result.ToListAsync();
        }

        //Get Realtor with Assets
        public async Task<Realtors> GetRealtorById(int realtorId, bool includeAsset)
        {
            IQueryable<Realtors> result;

            if (includeAsset)
            {
                result = _context.Realtor.Include(r => r.Asset)
                    .Where(r => r.Id == realtorId);
            }
            else 
            { 
                result = _context.Realtor.Where(r => r.Id == realtorId); 
            }
            return await result.FirstOrDefaultAsync();
        }

        //Get Realtor with Assets
        public async Task<Realtors> GetRealtorByUsername(string realtorUsername, bool includeAsset)
        {
            IQueryable<Realtors> result;

            if (includeAsset)
            {
                result = _context.Realtor.Include(r => r.Asset)
                    .Where(r => r.Username == realtorUsername);
            }
            else
            {
                result = _context.Realtor.Where(r => r.Username == realtorUsername);
            }
            return await result.FirstOrDefaultAsync();
        }

        //Adding a Realtor
        public async Task AddRealtor(Realtors realtor)
        {
            await _context.Realtor.AddAsync(realtor);
        }

        //Deleting a Realtor
        public void DeleteRealtor(Realtors realtors)
        {
            _context.Realtor.Remove(realtors);
        }

        //Getting the Assets of a Realtor
        public async Task<IEnumerable<Asset>> GetAssetsForRealtor(int realtorId)
        {
            IQueryable<Asset> result = _context.Asset.Where(a => a.RealtorId == realtorId);
            return await result.ToListAsync();
        }

        //Getting the particular Asset by Id belonging to a Realtor
        public async Task<Asset> GetAssetForRealtorById(int realtorId, int assetId)
        {
            IQueryable<Asset> result = _context.Asset.Where(a => a.RealtorId == realtorId && a.Id == assetId);
            return await result.FirstOrDefaultAsync();
        }


        //Getting the particular type of Assets belonging to a Realtor
        public async Task<IEnumerable<Asset>> GetAssetsForRealtorByType(int realtorId, string assetType)
        {
            IQueryable<Asset> result = _context.Asset.Where(a => a.RealtorId == realtorId && a.Type == assetType);
            return await result.ToListAsync();
        }

        //Getting the particular Assest Id of a Realtor
        public async Task<IEnumerable<Asset>> GetAssetsForRealtorByOwnership(int realtorId, bool isOwnedByRealtor)
        {
            IQueryable<Asset> result = _context.Asset.Where(a => a.RealtorId == realtorId && a.isOwned == isOwnedByRealtor);
            return await result.ToListAsync();
        }

        //Getting the assets that are sold by the realtor
        public async Task<IEnumerable<Asset>> GetSoldAssetsOfRealtor(int realtorId, bool isSoldByRealtor)
        {
            IQueryable<Asset> result = _context.Asset.Where(a => a.RealtorId == realtorId && a.isSold == isSoldByRealtor);
            return await result.ToListAsync();
        }

        //Adding an Asset for a Realtor
        public async Task AddAssetForRealtor(int realtorId, Asset assetOfRealtor)
        {
            var realtor = await GetRealtorById(realtorId, false);
            realtor.Asset.Add(assetOfRealtor);
        }
        
        //Deleting an Asset for a Realtor
        public void DeleteAsset(Asset asset)
        {
            _context.Asset.Remove(asset);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
