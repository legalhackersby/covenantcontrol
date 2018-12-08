using src.Data;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public interface IUploadDocumentService
    {
        Task<string> CreateDocument(UploadFileRequest file);
    }
}
