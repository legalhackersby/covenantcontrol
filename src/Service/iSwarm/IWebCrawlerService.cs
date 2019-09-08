using System.Collections.Generic;
using src.Models;

namespace src.Service.iSwarm
{
    public interface IWebCrawlerService
    {
        void HandleData();

        List<string> GetPageTitles();

        List<string> GetChapterTitles(string pageTitle);

        ChapterEntity GetChapter(string chapterTitle);

        string GetLiquidityAdequacyRequirementsPage();

        string GetLiquidityAdequacyRequirementsPageWithCovenants();

        List<CovenantWebSearchResult> GetCovenants();

        string GetPage(string pageTitle);

        string GetPageWithCovenants(string pageTitle);
    }
}
