using System.IO;
using System.Threading.Tasks;

namespace src.Service.Upload
{
    public class Storage : IStorage
    {
        private const string StorageDirectory = ".storage";

        public Task EnsureDirectory(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }

            return Task.CompletedTask;
        }

        public async Task<string> ReadAsync(string id, string fileName)
        {
            EnsureDirectory(StorageDirectory);
           var target = Path.Combine(StorageDirectory, id, fileName);
           return await File.ReadAllTextAsync(target);
        }

        public async Task<string> SaveAsync(string id, byte[] content, string fileName)
        {
             EnsureDirectory(StorageDirectory);
            var target = Path.Combine(StorageDirectory, id);
            this.EnsureDirectory(target);
            var path = Path.Combine(target, fileName);
            await File.WriteAllBytesAsync(path, content);
            return path;
        }
    }
}
