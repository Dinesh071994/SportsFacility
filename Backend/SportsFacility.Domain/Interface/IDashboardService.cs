using System.Threading.Tasks;

namespace SportsFacility.Domain.Interface
{
    public interface IDashboardService
    {
        Task<object> GetDashboardDataAsync();
    }
}
