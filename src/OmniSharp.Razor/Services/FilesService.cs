using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using OmniSharp.Mef;
using OmniSharp.Models;

namespace OmniSharp.Razor.Services
{
    [OmniSharpHandler(OmnisharpEndpoints.Open, RazorLanguage.Razor)]
    [OmniSharpHandler(OmnisharpEndpoints.Close, RazorLanguage.Razor)]
    public class FilesService : RequestHandler<FileOpenRequest, FileOpenResponse>, RequestHandler<FileCloseRequest, FileCloseResponse>
    {
        private readonly OmnisharpWorkspace _workspace;

        [ImportingConstructor]
        public FilesService(OmnisharpWorkspace workspace)
        {
            _workspace = workspace;
        }

        Task<FileOpenResponse> RequestHandler<FileOpenRequest, FileOpenResponse>.Handle(FileOpenRequest request)
        {
            var documents = _workspace.GetDocuments(request.FileName);
            foreach (var document in documents)
            {
                _workspace.OpenDocument(document.Id, false);
            }
            return Task.FromResult(new FileOpenResponse());
        }

        Task<FileCloseResponse> RequestHandler<FileCloseRequest, FileCloseResponse>.Handle(FileCloseRequest request)
        {
            var documents = _workspace.GetDocuments(request.FileName);
            foreach (var document in documents)
            {
                _workspace.CloseDocument(document.Id);
            }
            return Task.FromResult(new FileCloseResponse());
        }
    }
}