using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class RoomService
    {
        private readonly IMongoCollection<Room> _roomCollection;

        public RoomService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _roomCollection = mongoDatabase.GetCollection<Room>(
                maSoiDataBaseSetting.Value.RoomCollectionName);
        }

        public async Task<List<Room>> GetList() =>
            await _roomCollection.Find(x => x.Status == true && x.Sl > 0).ToListAsync();

        public async Task<Room> GetRoomNull() =>
            await _roomCollection.Find(x => x.Sl == 0).FirstOrDefaultAsync();

        public async Task<Room?> GetAsync1(string id) =>
            await _roomCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Room?> CheckPass(string id, string pass) =>
            await _roomCollection.Find(x => x.Id == id && x.Pass == pass).FirstOrDefaultAsync();

        public async Task<Room?> GetAsync(string Number) =>
            await _roomCollection.Find(x => x.Number == Number).FirstOrDefaultAsync();

        public async Task CreateRoom(Room newRoom) =>
            await _roomCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(Room updatedRoom) =>
            await _roomCollection.ReplaceOneAsync(x => x.Id == updatedRoom.Id, updatedRoom);

        public async Task RemoveAsync(string id) =>
            await _roomCollection.DeleteOneAsync(x => x.Id == id);
    }
}
