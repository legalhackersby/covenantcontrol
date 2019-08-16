
using System.Threading.Tasks;

namespace src.Service.Upload
{
    public interface IConvertToTxt
    {
        Task<string> ExtractTextAsync(string inputFilePath);
    }
}
