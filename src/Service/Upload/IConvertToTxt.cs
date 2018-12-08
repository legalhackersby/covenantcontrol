
using System.Threading.Tasks;

namespace src.Service.Upload
{
    public interface IConvertToTxt
    {
        Task<string> ConvertAsync(string inputFilePath);
    }
}
