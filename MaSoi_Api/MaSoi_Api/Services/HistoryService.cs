using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class HistoryService
    {
        private readonly IMongoCollection<History> _historyCollection;

        public HistoryService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _historyCollection = mongoDatabase.GetCollection<History>(
                maSoiDataBaseSetting.Value.HistoryCollectionName);
        }

        public async Task<List<History>> GetAsync() =>
            await _historyCollection.Find(_ => true).ToListAsync();

        public async Task<History?> GetAsync(string id) =>
            await _historyCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(History newHistory) =>
            await _historyCollection.InsertOneAsync(newHistory);

        public async Task UpdateAsync(string id, History updatedHistory) =>
            await _historyCollection.ReplaceOneAsync(x => x.Id == id, updatedHistory);

        public async Task RemoveAsync(string id) =>
            await _historyCollection.DeleteOneAsync(x => x.Id == id);
    }
}
