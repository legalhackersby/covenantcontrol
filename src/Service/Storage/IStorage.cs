using System.Threading.Tasks;

namespace src.Service.Upload
{
    public interface IStorage
    {

          Task<string> ReadAsync(string id, string fileName);
          Task<string> SaveAsync(string id, byte[] content, string fileName);

          Task EnsureDirectory(string name);
    }
}
