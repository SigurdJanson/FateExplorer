using System.Threading.Tasks;

namespace FateExplorer.Shared.ClientSideStorage;

public interface IClientSideStorage
{
    public Task SetValue(string key, string value, int? days = null);
    public Task<string> GetValue(string key, string def = "");
}
