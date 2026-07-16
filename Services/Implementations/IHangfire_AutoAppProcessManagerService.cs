using System.Threading;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IHangfire_AutoAppProcessManagerService
    {
        Task ExecuteElapsedAction();
    }
}
