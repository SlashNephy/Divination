using System;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.XivApi
{
    public interface IXivApiClient : IDisposable
    {
        public Task<XivApiResponse> GetAsync(string content, uint id, bool ignoreCache = false);
        public Task<XivApiResponse> GetCharacterAsync(string name, string world, bool ignoreCache = false);
    }
}
