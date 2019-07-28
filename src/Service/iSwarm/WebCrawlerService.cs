using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using src.Models;
using src.Repository;

namespace src.Service.iSwarm
{
    public class WebCrawlerService
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

            List<string> result = new List<string>();

            result.Add(content);

            List<ChapterEntity> returnedList = new List<ChapterEntity>();
            List<ChapterEntity> existingList = new List<ChapterEntity>();

            List<ChapterEntity> entitiesToInsert = returnedList.Where(model => existingList.All(x => x.ChapterTitle != model.ChapterTitle)).ToList();

            foreach (var item in returnedList)
            {
                var existingItem = existingList.FirstOrDefault(model => model.ChapterTitle == item.ChapterTitle);

                if (existingItem != null && existingItem.ChapterTitle != item.ChapterTitle)
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
