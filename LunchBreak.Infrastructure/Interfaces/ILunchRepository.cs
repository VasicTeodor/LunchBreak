using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface ILunchRepository
    {
        Task<PaginationData<Lunch>> GetLunches(string search, bool isAdmin = false, PaginationData<Lunch> paginationData = null);
        Task<List<Lunch>> GetLunchesAdmin();
        Task<Lunch> GetLunch(string lunchId);
        Task<bool> AddLunch(Lunch newLunch);
        Task<bool> RemoveLunch(string lunchId);
        Task<bool> UpdateLunch(string lunchId, Lunch lunch);
    }
}