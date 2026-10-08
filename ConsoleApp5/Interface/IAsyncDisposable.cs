using System.Threading.Tasks;

namespace ConsoleApp5.Interface
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }
}