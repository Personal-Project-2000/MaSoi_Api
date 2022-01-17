using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class BaiRoomService
    {
        private readonly IMongoCollection<BaiRoom> _baiRoomCollection;

        public BaiRoomService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _baiRoomCollection = mongoDatabase.GetCollection<BaiRoom>(
                maSoiDataBaseSetting.Value.BaiRoomCollectionName);
        }

        public async Task<List<BaiRoom>> GetAsync() =>
            await _baiRoomCollection.Find(_ => true).ToListAsync();

        public async Task<BaiRoom?> GetAsync(string RoomId, string BaiId) =>
            await _baiRoomCollection.Find(x => x.RoomId == RoomId && x.BaiId == BaiId).FirstOrDefaultAsync();

        public async Task CreateAsync(BaiRoom newBaiRoom) =>
            await _baiRoomCollection.InsertOneAsync(newBaiRoom);

        public async Task UpdateAsync(string id, BaiRoom updatedBaiRoom) =>
            await _baiRoomCollection.ReplaceOneAsync(x => x.Id == id, updatedBaiRoom);

        public async Task RemoveAsync(string id) =>
            await _baiRoomCollection.DeleteOneAsync(x => x.Id == id);
    }
}
