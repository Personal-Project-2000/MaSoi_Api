using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                maSoiDataBaseSetting.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<User> GetTk(string Tk, string Pass) =>
            await _userCollection.Find(x => x.Tk == Tk && x.Pass == Pass).FirstOrDefaultAsync();

        public User CheckTk(string Tk) =>
             _userCollection.Find(x => x.Tk == Tk).FirstOrDefault();

        public User GetPlayer(string Tk) =>
             _userCollection.Find(x => x.Tk == Tk).FirstOrDefault();

        public async Task CreateAsync(User newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(User updatedUser) =>
            await _userCollection.ReplaceOneAsync(x => x.Tk == updatedUser.Tk, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}
