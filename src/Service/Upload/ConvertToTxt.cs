using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace src.Service.Upload
{
    public class ConvertToTxt : IConvertToTxt
    {
        // TODO: uses env variables for program, check of uno path, 
        private const string DefaultPathToLiberOffice = @"C:/Program Files/LibreOffice/program/python.exe";
        private const string DefaultPathToUnoconv = "src/Converter/unoconv/unoconv";
        public async Task<string> Convert(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var unoconv = DefaultPathToUnoconv;
            if (File.Exists(unoconv))
            {
                var p = new ProcessStartInfo(DefaultPathToLiberOffice);
                p.Arguments = "-f txt " + path;
                var converter = Process.Start(p);
                await Task.Run(()=> converter.WaitForExit());
                return path;
            }
            else
            {
                     return Process.GetCurrentProcess().StartInfo.WorkingDirectory;
            }
        }
    }
}
