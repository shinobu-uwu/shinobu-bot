using System.Threading.Tasks;

namespace ShinobuBot.Core.Interfaces
{
    public interface IInitializableService
    {
        Task InitializeAsync();
    }
}