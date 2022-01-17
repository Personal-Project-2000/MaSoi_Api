using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class RoomDetailService
    {
        private readonly IMongoCollection<RoomDetail> _roomDetailCollection;

        public RoomDetailService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _roomDetailCollection = mongoDatabase.GetCollection<RoomDetail>(
                maSoiDataBaseSetting.Value.RoomDetailCollectionName);
        }

        public async Task<List<RoomDetail>> GetAsync() =>
            await _roomDetailCollection.Find(_ => true).ToListAsync();

        public async Task<RoomDetail?> GetAsync(string Tk, string roomId) =>
            await _roomDetailCollection.Find(x => x.Tk == Tk && x.RoomId == roomId).FirstOrDefaultAsync();

        public async Task CreateAsync(RoomDetail newRoomDetail) =>
            await _roomDetailCollection.InsertOneAsync(newRoomDetail);

        public async Task UpdateAsync(string id, RoomDetail updatedRoomDetail) =>
            await _roomDetailCollection.ReplaceOneAsync(x => x.Id == id, updatedRoomDetail);

        public async Task RemoveAsync(string id) =>
            await _roomDetailCollection.DeleteOneAsync(x => x.Id == id);
    }
}
