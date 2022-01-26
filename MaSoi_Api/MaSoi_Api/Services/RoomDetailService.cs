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

        public List<RoomDetail> GetAllPlayer(string roomId) =>
            _roomDetailCollection.Find(x => x.RoomId == roomId).ToList();

        public async Task<RoomDetail> GetPlayerHeader(string roomId) =>
            await _roomDetailCollection.Find(x => x.RoomId == roomId).FirstOrDefaultAsync();

        //Kiểm tra tài khoản đã vô phòng nào chưa
        public async Task<RoomDetail> CheckUser(string Tk) =>
            await _roomDetailCollection.Find(x => x.Tk == Tk).FirstOrDefaultAsync();

        public async Task<RoomDetail> GetPlayer(string Tk, string roomId) =>
            await _roomDetailCollection.Find(x => x.Tk == Tk && x.RoomId == roomId).FirstOrDefaultAsync();

        public async Task InsertPlayer(RoomDetail newRoomDetail) =>
            await _roomDetailCollection.InsertOneAsync(newRoomDetail);

        public async Task UpdateAsync(RoomDetail updatedRoomDetail) =>
            await _roomDetailCollection.ReplaceOneAsync(x => x.Id == updatedRoomDetail.Id, updatedRoomDetail);

        public async Task RemoveAsync(string RoomId, string Tk) =>
            await _roomDetailCollection.DeleteOneAsync(x => x.RoomId == RoomId && x.Tk == Tk);
    }
}
