using CalisApi.Models;
using CalisApi.Models.DTOs;

namespace CalisApi.Database.Interfaces
{
    public interface IRutineRepository
    {
        Task<IEnumerable<RutinesDto>> GetAllRutines();
        Task<RutineDto> GetRutineById(int id);
        Task Create(Rutine rutine);
    }
}
