using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class StoryService
    {
        private readonly IMongoCollection<Story> _storyCollection;

        public StoryService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _storyCollection = mongoDatabase.GetCollection<Story>(
                maSoiDataBaseSetting.Value.UserCollectionName);
        }

        public async Task<List<Story>> GetAsync() =>
            await _storyCollection.Find(_ => true).ToListAsync();

        public async Task<Story?> GetAsync(string id) =>
            await _storyCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Story newStory) =>
            await _storyCollection.InsertOneAsync(newStory);

        public async Task UpdateAsync(string id, Story updatedStory) =>
            await _storyCollection.ReplaceOneAsync(x => x.Id == id, updatedStory);

        public async Task RemoveAsync(string id) =>
            await _storyCollection.DeleteOneAsync(x => x.Id == id);
    }
}
