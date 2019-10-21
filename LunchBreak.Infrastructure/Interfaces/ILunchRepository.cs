using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface ILunchRepository
    {
        Task<List<Lunch>> GetLunches(bool isAdmin = false);
        Task<Lunch> GetLunch(string lunchId);
        Task<bool> AddLunch(Lunch newLunch);
        Task<bool> RemoveLunch(string lunchId);
        Task<bool> UpdateLunch(string lunchId, Lunch lunch);
    }
}