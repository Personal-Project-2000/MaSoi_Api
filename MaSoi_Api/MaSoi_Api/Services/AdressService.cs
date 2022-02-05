using MaSoi_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Services
{
    public class AdressService
    {
        private readonly IMongoCollection<AdressRealTime> _adressCollection;

        public AdressService(
            IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            var mongoClient = new MongoClient(
                maSoiDataBaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                maSoiDataBaseSetting.Value.DatabaseName);

            _adressCollection = mongoDatabase.GetCollection<AdressRealTime>(
                maSoiDataBaseSetting.Value.AdressCollectionName);
        }

        public async Task<AdressRealTime> GetAsync() =>
            await _adressCollection.Find(_ => true).FirstOrDefaultAsync();
    }
}
