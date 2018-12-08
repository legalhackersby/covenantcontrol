using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace src.Service.Upload
{
    /// <summary>
    /// The txt converter.
    /// </summary>
    /// <seealso cref="src.Service.Upload.IConvertToTxt" />
    public class ConvertToTxt : IConvertToTxt
    {
        // "C:/Program Files/LibreOffice/program/python.exe" src/Converter/unoconv/unoconv --verbose --doctype=document --format=text  --output="data/.data/2_аренда_хакатон.doc.txt"  "data/2_аренда_хакатон.doc"
        // --verbose --doctype=document --format=text  --output="data/.data/2_аренда_хакатон.doc.txt"  "data/2_аренда_хакатон.doc"

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
        public ConvertToTxt() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertToTxt"/> class.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        public ConvertToTxt(string rootFolder)
        {
            this.CustomUnoconvPath = Path.Combine(rootFolder, DefaultPathToUnoconv);
        }

        /// <summary>
        /// Converts the asynchronous.
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<string> ConvertAsync(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException(inputFilePath);
            }

            string arguments = $@"{CustomUnoconvPath ?? DefaultPathToUnoconv} {ConverterArgs.VerboseParamArg} {ConverterArgs.DotTypeDocumentParamArg} {ConverterArgs.FormatTextParamArg} ""{inputFilePath}""";

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

                await this.StartProcessAsync(converter);

                return $"{Path.GetFileNameWithoutExtension(inputFilePath)}.{"txt"}";
            }
        }

        /// <summary>
        /// Runs the process asynchronous.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Could not start process: " + process</exception>
        private Task StartProcessAsync(Process process)
        {
            return Task.Run(() =>
            {
                var errorOutputStringBuilder = new StringBuilder();
                var outputStringBuilder = new StringBuilder();
                DataReceivedEventHandler errorRecievedEventHandler = (sender, args) => errorOutputStringBuilder.Append(args.Data);
                DataReceivedEventHandler outputStringBuilderHandler = (sender, args) => outputStringBuilder.Append(args.Data);

                process.ErrorDataReceived += errorRecievedEventHandler;
                process.OutputDataReceived += outputStringBuilderHandler;

                if (process.Start())
                {
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    process.ErrorDataReceived -= errorRecievedEventHandler;
                    process.OutputDataReceived -= outputStringBuilderHandler;

                    // Handle unoconv errors and outputs
                    var errorOutput = errorOutputStringBuilder.ToString();
                    var output = outputStringBuilder.ToString();
                    var exceptionInfo = !string.IsNullOrEmpty(output) ? $"Output: {output}" : string.Empty;
                    exceptionInfo += !string.IsNullOrEmpty(errorOutput) ? $"Error Output: {errorOutput}" : string.Empty;

                    if (process.ExitCode != 0)
                    {
                        throw new IOException(exceptionInfo);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Could not start process: {process}");
                }
            });
        }
    }
}
