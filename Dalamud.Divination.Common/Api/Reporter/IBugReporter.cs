using System;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Reporter
{
    public interface IBugReporter : IDisposable
    {
        public Task SendAsync(string message);
    }
}
