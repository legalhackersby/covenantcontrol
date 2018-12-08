using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace src.Service.Upload
{
    public class ConvertToTxt : IConvertToTxt
    {

        // TODO: uses env variables for program, check of uno path, 
        private const string DefaultPathToLiberOffice = @"C:/Program Files/LibreOffice/program/python.exe";
        private const string DefaultPathToUnoconv = "Converter/unoconv/unoconv";

        /// <summary>
        /// Gets or sets the custom unoconv path.
        /// </summary>
        /// <value>
        /// The custom unoconv path.
        /// </value>
        public string CustomUnoconvPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertToTxt"/> class.
        /// </summary>
        public ConvertToTxt()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertToTxt"/> class.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        public ConvertToTxt(string rootFolder)
        {
            this.CustomUnoconvPath = rootFolder + DefaultPathToUnoconv;
        }

        // "C:/Program Files/LibreOffice/program/python.exe" src/Converter/unoconv/unoconv --verbose --doctype=document --format=text  --output="data/.data/2_аренда_хакатон.doc.txt"  "data/2_аренда_хакатон.doc"
        // --verbose --doctype=document --format=text  --output="data/.data/2_аренда_хакатон.doc.txt"  "data/2_аренда_хакатон.doc"

        public async Task<string> ConvertAsync(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException(inputFilePath);
            }

            string arguments = string.Format(@"{0} {1} {2} {3} ""{4}""",
                CustomUnoconvPath ?? DefaultPathToUnoconv,
                ConverterArgs.VerboseParamArg,
                ConverterArgs.DotTypeDocumentParamArg,
                ConverterArgs.FormatTextParamArg,
                inputFilePath);

            var startInfo = new ProcessStartInfo
            {
                FileName = DefaultPathToLiberOffice,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var converter = new Process())
            {
                converter.StartInfo = startInfo;

                await this.RunProcessAsync(converter);

                return string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(inputFilePath), "txt");
            }
        }

        /// <summary>
        /// Runs the process asynchronous.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Could not start process: " + process</exception>
        private Task RunProcessAsync(Process process)
        {
            return Task.Run(() =>
            {
                DataReceivedEventHandler errorRecievedEventHandler = 
                (sender, args) => Console.WriteLine(args.Data);
                DataReceivedEventHandler outputStringBuilderHandler = (sender, args) => Console.WriteLine(args.Data);

                process.Exited += (s, ea) => Console.WriteLine(process);

                if (process.Start())
                {
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    process.ErrorDataReceived -= errorRecievedEventHandler;
                    process.OutputDataReceived -= outputStringBuilderHandler;

                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start process: {0}", process));
                }
            });
        }
    }
}
