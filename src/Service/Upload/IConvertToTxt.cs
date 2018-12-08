
using System.Threading.Tasks;

namespace src.Service
{
    public interface IConvertToTxt
    {
         Task<string> Convert(string path);
    }
}
