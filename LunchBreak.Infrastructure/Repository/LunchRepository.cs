using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<List<Lunch>> GetLunches(bool isAdmin = false)
        {
            if (isAdmin)
            {
                return await _lunches.Find(lunch => true).ToListAsync();
            }
            else
            {
                return await _lunches.Find(lunch => lunch.Approved == true).ToListAsync();
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
    }
}