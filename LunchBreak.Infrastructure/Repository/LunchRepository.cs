using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.DatabaseSettings;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace LunchBreak.Infrastructure.Repository
{
    public class LunchRepository : ILunchRepository
    {
        private readonly IMongoCollection<Lunch> _lunches;
        private readonly string _collectionName = "Lunches";

        public LunchRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _lunches = database.GetCollection<Lunch>(_collectionName);
        }
        public async Task<PaginationData<Lunch>> GetLunches(string search, bool isAdmin = false, PaginationData<Lunch> paginationData = null)
        {
            var pagination = paginationData ?? new PaginationData<Lunch>() { PageSize = 5, PageNumber = 1 };

            var searchString = search ?? "";
            if (isAdmin)
            {
                return new PaginationData<Lunch>
                {
                    Items = await _lunches.Find(lunch => lunch.Name.ToLower().Contains(searchString.ToLower()))
                                .Skip((pagination.PageNumber - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync(),
                    NumberOfItems = Convert.ToInt32(await _lunches.CountDocumentsAsync(doc => true)),
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };
                //return await _lunches.Find(lunch => lunch.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }
            else
            {
                return new PaginationData<Lunch>
                {
                    Items = await _lunches.Find(lunch => lunch.Approved == true && lunch.Name.ToLower().Contains(searchString.ToLower()))
                                        .Skip((pagination.PageNumber - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync(),
                    PageSize = pagination.PageSize,
                    PageNumber = pagination.PageNumber,
                    NumberOfItems = Convert.ToInt32(await _lunches.CountDocumentsAsync(doc => doc.Approved == true))
                };
                //return await _lunches.Find(lunch => lunch.Approved == true && lunch.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }
        }
        public async Task<Lunch> GetLunch(string lunchId) => await _lunches.Find(lunch => lunch.Id.Equals(lunchId)).FirstOrDefaultAsync();

        public async Task<bool> AddLunch(Lunch newLunch)
        {
            try
            {
                newLunch.Id = Guid.NewGuid().ToString();
                await _lunches.InsertOneAsync(newLunch);
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while adding new lunch: {e.Message}", "ERROR");
                return false;
            }
        }

        public async Task<bool> RemoveLunch(string lunchId) => (await _lunches.DeleteOneAsync(lunch => lunch.Id.Equals(lunchId))).DeletedCount > 0;

        public async Task<bool> UpdateLunch(string lunchId, Lunch lunch)
        {
            lunch.Id = lunchId;

            try
            {
                var result = await _lunches.ReplaceOneAsync(lun => lun.Id.Equals(lunchId), lunch);
                return result.ModifiedCount > 0;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while saving changes: {e.Message}", "ERROR");
                return false;
            }
        }

        public async Task<List<Lunch>> GetLunchesAdmin()
        {
            return await _lunches.Find(lunch => true).ToListAsync();
        }
    }
}