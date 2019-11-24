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
using WebPush;

namespace src.Service.iSwarm
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly WebCrawlerRepository repository = new WebCrawlerRepository();

        private readonly IChapterMongoRepository chapterMongoRepository;

        private readonly IChangesSearchResultMongoRepository changesSearchResultMongoRepository;

        private readonly ITextParserService textParserService;

        private readonly ICovenantsWebRepository covenantsWebRepository;

        private readonly IJsonContentChangesSearchResultMongoRepository jsonContentChangesSearchResultMongoRepository;

        private string[] paragraphSeparators = new[] { "\n", "." };

        private readonly IHubContext<NotifyHub> hubContext;

        public WebCrawlerService(IChapterMongoRepository chapterMongoRepository, IChangesSearchResultMongoRepository changesSearchResultMongoRepository, ITextParserService textParserService, ICovenantsWebRepository covenantsWebRepository, IJsonContentChangesSearchResultMongoRepository jsonContentChangesSearchResultMongoRepository)
        {
            this.chapterMongoRepository = chapterMongoRepository;
            this.changesSearchResultMongoRepository = changesSearchResultMongoRepository;
            this.textParserService = textParserService;
            this.covenantsWebRepository = covenantsWebRepository;
            this.jsonContentChangesSearchResultMongoRepository = jsonContentChangesSearchResultMongoRepository;
        }

        public void HandleData()
        {
            var sadadsd = new WebPushClient();
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
                    entity.JsonContent = this.ParseJsonContent(item["content_json"]);
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
                            Source = item.Source,
                            JsonContent = item.JsonContent
                        };

                        this.FindChanges(newEntity, existingItem);
                        this.FindJsonContentChanges(newEntity, existingItem);
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

        private Paragraph ParseJsonContent(JToken jsonParagraph)
        {
            var result = new Paragraph();
            result.Id = ObjectId.GenerateNewId();
            result.Text = (string)jsonParagraph["paragraphText"] ?? string.Empty;
            result.HeaderLevel = (string)jsonParagraph["headerLevel"] ?? string.Empty;
            result.Type = (string)jsonParagraph["type"] ?? string.Empty;
            var list = new List<Paragraph>();
            if (jsonParagraph["subParagraphs"] != null)
            {
                foreach (var jsonSubParagraph in jsonParagraph["subParagraphs"])
                {
                    list.Add(this.ParseJsonContent(jsonSubParagraph));
                }
            }

            result.SubParagraphs = list;
            return result;
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

        private void FindJsonContentChanges(ChapterEntity newVersion, ChapterEntity oldVersion)
        {
            var changesList = new List<JsonContentChangesSearchEntity>();
            this.FindJsonContentChangesInternal(changesList, newVersion.JsonContent, oldVersion.JsonContent, newVersion.ChapterTitle, newVersion.PageTitle);

            if (changesList.Count > 0)
            {
                this.jsonContentChangesSearchResultMongoRepository.DeleteMany(x => x.ChapterTitle == newVersion.ChapterTitle && x.PageTitle == newVersion.PageTitle);
                this.jsonContentChangesSearchResultMongoRepository.InsertMany(changesList);
            }
        }

        private void FindJsonContentChangesInternal(List<JsonContentChangesSearchEntity> resultList, Paragraph newParagraph, Paragraph oldParagraph, string chapterTitle, string pageTitle)
        {
            if (newParagraph.Text != null && oldParagraph.Text != null && newParagraph.Text != oldParagraph.Text)
            {
                var changes = new JsonContentChangesSearchEntity();
                changes.NewParagraphId = newParagraph.Id;
                changes.OldParagraphId = oldParagraph.Id;
                changes.ChapterTitle = chapterTitle;
                changes.PageTitle = pageTitle;
                resultList.Add(changes);
            }

            var newSubParagraphs = newParagraph.SubParagraphs.ToList();
            var oldSubParagraphs = oldParagraph.SubParagraphs.ToList();

            if (newSubParagraphs.Count > 0 && oldSubParagraphs.Count > 0)
            {
                if (newSubParagraphs.Count == oldSubParagraphs.Count)
                {
                    for (int i = 0; i < newSubParagraphs.Count; i++)
                    {
                        this.FindJsonContentChangesInternal(resultList, newSubParagraphs[i], oldSubParagraphs[i], chapterTitle, pageTitle);
                    }
                }
                else
                {
                    foreach (var subParagraph in newSubParagraphs)
                    {
                        this.MarkAllParagraphsAsChanged(resultList, subParagraph, chapterTitle, pageTitle);
                    }
                }
               
            }
        }

        private void MarkAllParagraphsAsChanged(List<JsonContentChangesSearchEntity> resultList, Paragraph newParagraph, string chapterTitle, string pageTitle)
        {
            var changes = new JsonContentChangesSearchEntity();
            changes.NewParagraphId = newParagraph.Id;
            changes.OldParagraphId = newParagraph.Id;
            changes.ChapterTitle = chapterTitle;
            changes.PageTitle = pageTitle;
            resultList.Add(changes);

            var newSubParagraphs = newParagraph.SubParagraphs.ToList();
            if (newSubParagraphs.Count > 0)
            {
                for (int i = 0; i < newSubParagraphs.Count; i++)
                {
                    this.MarkAllParagraphsAsChanged(resultList, newSubParagraphs[i], chapterTitle, pageTitle);
                }
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

        public string GetPageForJsonContent(string pageTitle)
        {
            var stringBuilder = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll().Where(x =>
                x.PageTitle == pageTitle).OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }).ToList();

            foreach (var chapter in chapters)
            {
               
                var chapterChanges =
                    this.jsonContentChangesSearchResultMongoRepository.Find(x => x.ChapterTitle == chapter.ChapterTitle && x.PageTitle == chapter.PageTitle).ToList();
                
                this.ParseParagraphsToString(stringBuilder, chapter.JsonContent, chapterChanges);
            }

           /* if (pageTitle != "Liquidity Adequacy Requirements (LAR): Chapter 6 – Intraday Liquidity Monitoring Tools (changed)")
            {
                result.Replace(".", ".<br>");
            }*/

            return stringBuilder.ToString();
        }

        private void ParseParagraphsToString(StringBuilder result, Paragraph paragraph, List<JsonContentChangesSearchEntity> changes)
        {
            switch (paragraph.Type)
            {
                case "HEADER":
                    var currentParagraphChanges = changes.FirstOrDefault(x => x.NewParagraphId == paragraph.Id);
                    if (currentParagraphChanges != null)
                    {
                        result.AppendFormat("<h{0}><mark id=\"{2}\" class=\"highlight\">{1}</mark></h{0}>", paragraph.HeaderLevel, paragraph.Text, currentParagraphChanges.Id);
                    }
                    else
                    {
                        result.AppendFormat("<h{0}>{1}</h{0}>", paragraph.HeaderLevel, paragraph.Text);
                    }

                    foreach (var subParagraph in paragraph.SubParagraphs)
                    {
                        this.ParseParagraphsToString(result, subParagraph, changes);
                    }
                    break;
                case "LIST":
                    result.AppendFormat("<ol>");
                    foreach (var subParagraph in paragraph.SubParagraphs)
                    {
                        this.ParseParagraphsToString(result, subParagraph, changes);
                    }
                    result.AppendFormat("</ol>");
                    break;
                case "LIST_ITEM":
                    currentParagraphChanges = changes.FirstOrDefault(x => x.NewParagraphId == paragraph.Id);
                    if (currentParagraphChanges != null)
                    {
                        result.AppendFormat("<li><mark id=\"{1}\" class=\"highlight\">{0}</mark></li>", paragraph.Text, currentParagraphChanges.Id);
                    }
                    else
                    {
                        result.AppendFormat("<li>{0}</li>", paragraph.Text);
                    }

                    foreach (var subParagraph in paragraph.SubParagraphs)
                    {
                        this.ParseParagraphsToString(result, subParagraph, changes);
                    }
                    break;
            }
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
