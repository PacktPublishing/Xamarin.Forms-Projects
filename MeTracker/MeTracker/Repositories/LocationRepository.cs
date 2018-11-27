using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MeTracker.Models;
using SQLite;

namespace MeTracker.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private SQLiteAsyncConnection connection;

        private async Task CreateConnection()
        {
            if (connection != null)
            {
                return;
            }

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Locations.db");

            connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<Location>();
        }

        public async Task Save(Location location)
        {
            await CreateConnection();
            await connection.InsertAsync(location);
        }

        public async Task<List<Location>> GetAll()
        {
            await CreateConnection();

            var locations = await connection.Table<Location>().ToListAsync();

            return locations;
        }
    }
}
