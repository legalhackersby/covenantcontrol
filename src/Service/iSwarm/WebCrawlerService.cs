using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using src.Models;
using src.Repository;

namespace src.Service.iSwarm
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly WebCrawlerRepository repository = new WebCrawlerRepository();

        private readonly ChapterMongoRepository chapterMongoRepository;

        public WebCrawlerService(ChapterMongoRepository chapterMongoRepository)
        {
            this.chapterMongoRepository = chapterMongoRepository;
        }

        public void HandleData()
        {
            var content = repository.GetData();

            var json = JArray.Parse(content);

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
            
            List<ChapterEntity> existingList = this.chapterMongoRepository.GetAll().ToList();

            List<ChapterEntity> entitiesToInsert = returnedList.Where(model => existingList.All(x => x.ChapterTitle != model.ChapterTitle)).ToList();

            foreach (var item in returnedList)
            {
                var existingItem = existingList.FirstOrDefault(model => model.ChapterTitle == item.ChapterTitle);

                if (existingItem != null && existingItem.Body != item.Body)
                {
                    entitiesToInsert.Add(new ChapterEntity()
                    {
                        Body = item.Body,
                        ChapterTitle = item.ChapterTitle,
                        CreatedTime = DateTime.Now,
                        Id = ObjectId.GenerateNewId(),
                        PageTitle = item.PageTitle,
                        Source = item.Source
                    });
                }
            }

            if (entitiesToInsert.Count > 0)
            {
                this.chapterMongoRepository.InsertMany(entitiesToInsert);
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
