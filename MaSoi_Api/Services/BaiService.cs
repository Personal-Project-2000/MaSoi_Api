using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class BaiService
    {
        private readonly IMongoCollection<Bai> _baiCollection;

        public BaiService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _baiCollection = mongoDatabase.GetCollection<Bai>(
                maSoiDataBaseSetting.Value.BaiCollectionName);
        }

        public async Task<List<Bai>> GetAsync() =>
            await _baiCollection.Find(_ => true).ToListAsync();

        public async Task<Bai?> GetAsync(string id) =>
            await _baiCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Bai newBai) =>
            await _baiCollection.InsertOneAsync(newBai);

        public async Task UpdateAsync(string id, Bai updateBai) =>
            await _baiCollection.ReplaceOneAsync(x => x.Id == id, updateBai);

        public async Task RemoveAsync(string id) =>
            await _baiCollection.DeleteOneAsync(x => x.Id == id);
    }
}
