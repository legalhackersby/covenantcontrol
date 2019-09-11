using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using src.Hubs;
using src.Models;
using src.Repository;
using src.Service.Document;

namespace src.Service.iSwarm
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly WebCrawlerRepository repository = new WebCrawlerRepository();

        private readonly IChapterMongoRepository chapterMongoRepository;

        private readonly IChangesSearchResultMongoRepository changesSearchResultMongoRepository;

        private readonly ITextParserService textParserService;

        private readonly ICovenantsWebRepository covenantsWebRepository;

        private string[] paragraphSeparators = new[] { "\n", "." };

        private readonly IHubContext<NotifyHub> hubContext;

        public WebCrawlerService(IChapterMongoRepository chapterMongoRepository, IChangesSearchResultMongoRepository changesSearchResultMongoRepository, ITextParserService textParserService, ICovenantsWebRepository covenantsWebRepository, IHubContext<NotifyHub> hubContext)
        {
            this.chapterMongoRepository = chapterMongoRepository;
            this.changesSearchResultMongoRepository = changesSearchResultMongoRepository;
            this.textParserService = textParserService;
            this.covenantsWebRepository = covenantsWebRepository;
            this.hubContext = hubContext;
        }

        public void HandleData()
        {
            this.hubContext.Clients.All.SendAsync("sendToAll", "some text").Wait();
            var contentList = repository.GetData();

            var addedContent = new List<Tuple<string, string>>();
            foreach (var jsonString in contentList)
            {
                
                var json = JArray.Parse(jsonString);

                List<ChapterEntity> returnedList = new List<ChapterEntity>();

                foreach (var item in json)
                {
                    var entity = new ChapterEntity();
                    entity.Body = (string)item["body"];
                    entity.PageTitle = (string)item["title"];
                    var user = item["user"];
                    var fullName = user["fullname"];
                    entity.ChapterTitle = (string)fullName.FirstOrDefault();
                    entity.Source = (string)item["url"];
                    entity.CreatedTime = DateTime.Now;
                    returnedList.Add(entity);
                }

                 /*var toSampleData = json.Where(x =>
                     (string) x["title"] == "Liquidity Adequacy Requirements (LAR): Chapter 6 – Intraday Liquidity Monitoring Tools");

                 using (var fs = File.OpenWrite(@"D:\sampleData.txt"))
                 {
                     using (var sw = new StreamWriter(fs))
                     {
                         foreach (var item in toSampleData)
                         {
                             sw.WriteLine(item.ToString()+ ",");
                         }
                     }
                 }*/

                List<ChapterEntity> existingList = this.chapterMongoRepository.GetAll().OrderByDescending(x => x.CreatedTime).Distinct(((first, second) => first.ChapterTitle == second.ChapterTitle)).ToList();

                List<ChapterEntity> entitiesToInsert = returnedList.Where(model => existingList.All(x => x.ChapterTitle != model.ChapterTitle)).ToList();

                addedContent.AddRange(entitiesToInsert.Select(x => new Tuple<string, string>(x.PageTitle, x.ChapterTitle)));

                foreach (var item in returnedList)
                {
                    var existingItem = existingList.FirstOrDefault(model => model.ChapterTitle == item.ChapterTitle && model.PageTitle == item.PageTitle);

                    var addedInCurrentSession =
                        addedContent.Any(x => x.Item1 == item.PageTitle && x.Item2 == item.ChapterTitle);

                    if (existingItem != null && existingItem.Body != item.Body && !addedInCurrentSession)
                    {
                        var newEntity = new ChapterEntity()
                        {
                            Body = item.Body,
                            ChapterTitle = item.ChapterTitle,
                            CreatedTime = DateTime.Now,
                            Id = ObjectId.GenerateNewId(),
                            PageTitle = item.PageTitle,
                            Source = item.Source
                        };

                        this.FindChanges(newEntity, existingItem);
                        entitiesToInsert.Add(newEntity);
                        addedContent.Add(new Tuple<string, string>(newEntity.PageTitle, newEntity.ChapterTitle));
                    }
                }

                if (entitiesToInsert.Count > 0)
                {
                    this.chapterMongoRepository.InsertMany(entitiesToInsert);
                    this.FindCovenants(entitiesToInsert);
                }
            }
        }

        public string GetLiquidityAdequacyRequirementsPage()
        {
            var result = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll().Where(x =>
                x.PageTitle ==
                    "Liquidity Adequacy Requirements (LAR): Chapter 1 – Overview").OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }).ToList();

            foreach (var chapter in chapters)
            {
                string modifiedText = string.Empty;
                string text = chapter.Body;
                var chapterChanges =
                    this.changesSearchResultMongoRepository.Find(x => x.ChapterTitle == chapter.ChapterTitle);


                var head = 0;
                foreach (var change in chapterChanges)
                {
                    var stringBuilder = new StringBuilder(modifiedText);

                    stringBuilder.Append(chapter.Body.Substring(head, change.StartIndex - head));
                    stringBuilder.Append("<mark id=\"" + change.Id + "\" class=\"highlight\">");
                    stringBuilder.Append(change.ChangeValue);
                    stringBuilder.Append("</mark>");
                    head = change.EndIndex;
                    modifiedText = stringBuilder.ToString();
                    text = chapter.Body.Substring(head, chapter.Body.Length - head);
                }

                result.AppendLine(modifiedText + text + "<br>");

            }
            return result.ToString().Replace(Environment.NewLine, "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace(".", ".<br>");
        }

        public string GetLiquidityAdequacyRequirementsPageWithCovenants()
        {
            var result = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll().Where(x =>
                x.PageTitle ==
                    "Liquidity Adequacy Requirements (LAR): Chapter 1 – Overview").OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }).ToList();

            foreach (var chapter in chapters)
            {
                string modifiedText = string.Empty;
                string text = chapter.Body;
                var chapterCovenants =
                    this.covenantsWebRepository.Find(x => x.ChapterId == chapter.Id);

                var head = 0;
                foreach (var change in chapterCovenants)
                {
                    var stringBuilder = new StringBuilder(modifiedText);

                    stringBuilder.Append(chapter.Body.Substring(head, change.StartIndex - head));
                    stringBuilder.Append("<mark id=\"" + change.Id + "\" class=\"highlight\">");
                    stringBuilder.Append(change.CovenantValue);
                    stringBuilder.Append("</mark>");
                    head = change.EndIndex;
                    modifiedText = stringBuilder.ToString();
                    text = chapter.Body.Substring(head, chapter.Body.Length - head);
                }

                result.AppendLine(modifiedText + text + "<br>");

            }
            return result.ToString().Replace(Environment.NewLine, "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace(".", ".<br>");
        }

        public List<CovenantWebSearchResult> GetCovenants(string title)
        {
            return this.covenantsWebRepository.Find(x => x.PageTitle == title).ToList();
        }

        private void FindChanges(ChapterEntity newVersion, ChapterEntity oldVersion)
        {
            if (newVersion.Body.Contains("\n"))
            {
                this.paragraphSeparators = new[] { "\n" };
            }
            else
            {
                this.paragraphSeparators = new[] { "\n", "." };
            }
            var allTextParagraphs = newVersion.Body.Split(this.paragraphSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
            var changesList = new List<ChangesSearchEntity>();
            if (allTextParagraphs.Any())
            {
                foreach (var paragraph in allTextParagraphs)
                {
                    if (!oldVersion.Body.Contains(paragraph))
                    {
                        var index = newVersion.Body.IndexOf(paragraph, StringComparison.Ordinal);

                        if (index > -1)
                        {
                            changesList.Add(new ChangesSearchEntity
                            {
                                ChangeValue = paragraph,
                                ChapterTitle = newVersion.ChapterTitle,
                                StartIndex = index,
                                EndIndex = index + paragraph.Length,
                                PageTitle = newVersion.PageTitle
                            });
                        }
                    }
                }
            }

            if (changesList.Count > 0)
            {
                this.changesSearchResultMongoRepository.DeleteMany(x => x.ChapterTitle == newVersion.ChapterTitle && x.PageTitle == newVersion.PageTitle);
                this.changesSearchResultMongoRepository.InsertMany(changesList);
            }
        }

        public List<string> GetPageTitles()
        {
            var result = this.chapterMongoRepository.GetAll().Select(x => x.PageTitle);
            return result.ToList();
        }

        public List<string> GetChapterTitles(string pageTitle)
        {
            var result = this.chapterMongoRepository.GetAll().Where(x => x.PageTitle == pageTitle).Select(x => x.ChapterTitle);
            return result.ToList();
        }

        public ChapterEntity GetChapter(string chapterTitle)
        {
            var result = this.chapterMongoRepository.GetAll().Where(x => x.ChapterTitle == chapterTitle)
                .OrderBy(x => x.CreatedTime).FirstOrDefault();
            return result;
        }

        public string GetPage(string pageTitle)
        {
            var result = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll().Where(x =>
                x.PageTitle == pageTitle).OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }).ToList();

            foreach (var chapter in chapters)
            {
                string modifiedText = "<h2>" + chapter.ChapterTitle.Replace("(changed)", String.Empty) + "</h2><br><br>";
                string text = chapter.Body;
                var chapterChanges =
                    this.changesSearchResultMongoRepository.Find(x => x.ChapterTitle == chapter.ChapterTitle && x.PageTitle == chapter.PageTitle).OrderBy(x => x.StartIndex);


                var head = 0;
                foreach (var change in chapterChanges)
                {
                    if (change.StartIndex - head >= 0)
                    {
                        var stringBuilder = new StringBuilder(modifiedText);


                        stringBuilder.Append(chapter.Body.Substring(head, change.StartIndex - head));
                        stringBuilder.Append("<mark id=\"" + change.Id + "\" class=\"highlight\">");
                        stringBuilder.Append(change.ChangeValue);
                        stringBuilder.Append("</mark>");
                        head = change.EndIndex;
                        modifiedText = stringBuilder.ToString();
                        text = chapter.Body.Substring(head, chapter.Body.Length - head);
                    }
                }

                result.AppendLine(modifiedText + text + "<br>");

            }

            if (pageTitle != "Liquidity Adequacy Requirements (LAR): Chapter 6 – Intraday Liquidity Monitoring Tools (changed)")
            {
                result.Replace(".", ".<br>");
            }

            return result.ToString().Replace(Environment.NewLine, "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
        }

        public string GetPageWithCovenants(string pageTitle)
        {
            var result = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll().Where(x =>
                x.PageTitle == pageTitle).OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }).ToList();

            foreach (var chapter in chapters)
            {
                string modifiedText = "<h2>" + chapter.ChapterTitle.Replace("(changed)", String.Empty) + "</h2><br><br>";
                string text = chapter.Body;
                var chapterCovenants =
                    this.covenantsWebRepository.Find(x => x.ChapterId == chapter.Id);

                var head = 0;
                foreach (var change in chapterCovenants)
                {
                    if (change.StartIndex - head >= 0)
                    {
                        var stringBuilder = new StringBuilder(modifiedText);

                        stringBuilder.Append(chapter.Body.Substring(head, change.StartIndex - head));
                        stringBuilder.Append("<mark id=\"" + change.Id + "\" class=\"highlight\">");
                        stringBuilder.Append(change.CovenantValue);
                        stringBuilder.Append("</mark>");
                        head = change.EndIndex;
                        modifiedText = stringBuilder.ToString();
                        text = chapter.Body.Substring(head, chapter.Body.Length - head);
                    }
                }

                result.AppendLine(modifiedText + text + "<br>");

            }

            if (pageTitle != "Liquidity Adequacy Requirements (LAR): Chapter 6 – Intraday Liquidity Monitoring Tools (changed)")
            {
                result.Replace(".", ".<br>");
            }
            return result.ToString().Replace(Environment.NewLine, "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
        }

        private void FindCovenants(List<ChapterEntity> chapters)
        {
            foreach (var chapterEntity in chapters)
            {


                var covenants = GetValidCovenantsWeb(textParserService.GetCovenantWebResults(chapterEntity.Body));
                foreach (var covenant in covenants)
                {
                    covenant.Id = ObjectId.GenerateNewId();
                    covenant.ChapterId = chapterEntity.Id;
                    covenant.State = CovenantState.New;
                    covenant.PageTitle = chapterEntity.PageTitle;
                }
                
                if (covenants.Count > 0)
                {
                    this.covenantsWebRepository.InsertMany(covenants);
                }
                textParserService.GetCovenantResults("");
            }
        }

        private static List<CovenantWebSearchResult> GetValidCovenantsWeb(List<CovenantWebSearchResult> covenants)
        {
            // ISSUE: not optimal at all, fix on search engine side
            var ncovs = covenants
                .Where(x => x.StartIndex < x.EndIndex)//BUG: will be ensured by tests
                .OrderBy(x => x.StartIndex)
                .ToList();

            var dict = new List<CovenantWebSearchResult>();

            foreach (var covenant in ncovs)
            {
                dict.Add(covenant);// BUG: we just use one covenant, but should all
            }

            var result = new List<CovenantWebSearchResult>();
            ncovs = dict;

            foreach (var n in ncovs)
            {
                if (!result.Any(n.IntersectNotFully)) result.Add(n);
            }

            return result;
        }
    }
}
