using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class PlayerService
    {
        private readonly IMongoCollection<Player> _playerCollection;

        public PlayerService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _playerCollection = mongoDatabase.GetCollection<Player>(
                maSoiDataBaseSetting.Value.PlayerCollectionName);
        }

        //public async Task<List<Player>> GetAsync() =>
        //    await _playerCollection.Find(_ => true).ToListAsync();

        public List<Player> GetAllHistory(string Tk) =>
            _playerCollection.Find(x => x.Tk == Tk).ToList();

        public  Player GetPlayer(string Tk) =>
             _playerCollection.Find(x => x.Tk == Tk).FirstOrDefault();

        public List<Player> GetAllPlayer(string HistoryId) =>
           _playerCollection.Find(x => x.HistoryId == HistoryId).ToList();

        public int PlayerCount(string HistoryId) =>
            _playerCollection.Find(x => x.HistoryId == HistoryId).ToList().Count();

        public bool Win(string HistoryId, string Tk) =>
            _playerCollection.Find(x => x.HistoryId == HistoryId & x.Tk == Tk).FirstOrDefault().Win;

        public async Task<Player> GetAsync(string Tk, string historyId) =>
            await _playerCollection.Find(x => x.Tk == Tk && x.HistoryId == historyId).FirstOrDefaultAsync();

        public async Task CreateAsync(Player newPlayer) =>
            await _playerCollection.InsertOneAsync(newPlayer);

        public async Task UpdateAsync(Player updatedPlayer) =>
            await _playerCollection.ReplaceOneAsync(x => x.Id == updatedPlayer.Id, updatedPlayer);

        public async Task RemoveAsync(string id) =>
            await _playerCollection.DeleteOneAsync(x => x.Id == id);
    }
}
