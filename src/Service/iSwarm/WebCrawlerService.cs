using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using src.Models;
using src.Repository;

namespace src.Service.iSwarm
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly WebCrawlerRepository repository = new WebCrawlerRepository();

        private readonly IChapterMongoRepository chapterMongoRepository;

        private readonly IChangesSearchResultMongoRepository changesSearchResultMongoRepository;

        private readonly char[] paragraphSeparators = new[] { '\n', '.' };

        public WebCrawlerService(IChapterMongoRepository chapterMongoRepository, IChangesSearchResultMongoRepository changesSearchResultMongoRepository)
        {
            this.chapterMongoRepository = chapterMongoRepository;
            this.changesSearchResultMongoRepository = changesSearchResultMongoRepository;
        }

        public void HandleData()
        {
            Debugger.Launch();
            var contentList = repository.GetData();

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

                List<ChapterEntity> existingList = this.chapterMongoRepository.GetAll().OrderByDescending(x => x.CreatedTime).Distinct(((first, second) => first.ChapterTitle == second.ChapterTitle )).ToList();

                List<ChapterEntity> entitiesToInsert = returnedList.Where(model => existingList.All(x => x.ChapterTitle != model.ChapterTitle)).ToList();

                foreach (var item in returnedList)
                {
                    var existingItem = existingList.FirstOrDefault(model => model.ChapterTitle == item.ChapterTitle);

                    if (existingItem != null && existingItem.Body != item.Body)
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
                    }
                }

                if (entitiesToInsert.Count > 0)
                {
                    this.chapterMongoRepository.InsertMany(entitiesToInsert);
                }
            }
        }

        public string GetLiquidityAdequacyRequirementsPage()
        {
            var result = new StringBuilder();
            var chapters = this.chapterMongoRepository.GetAll()/*.Where(x =>
                x.PageTitle ==
                    "Liquidity Adequacy Requirements (LAR): Chapter 6 – Intraday Liquidity Monitoring Tools")*/.OrderBy(x => x.ChapterTitle).ThenByDescending(x => x.CreatedTime).Distinct(
                (first, second) => { return first.ChapterTitle == second.ChapterTitle; }) .ToList();

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

        private void FindChanges(ChapterEntity newVersion, ChapterEntity oldVersion)
        {
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
                                EndIndex = index + paragraph.Length
                            });
                        }
                    }
                }
            }

            if (changesList.Count > 0)
            {
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
    }
}
