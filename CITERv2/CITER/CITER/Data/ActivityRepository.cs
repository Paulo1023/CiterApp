using CITER.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CITER.Data
{
    public class ActivityRepository
    {
        public readonly SQLiteAsyncConnection _database;
        public ActivityRepository()
        {
            _database = new SQLiteAsyncConnection(TopicsRepository.dbPath);
            _database.CreateTableAsync<Activity>();
        }
        public Task<List<Activity>> List()
        {
            return _database.Table<Activity>().OrderByDescending(value => value.Date).ToListAsync();
        }
        public Task<int> Save(Activity a)
        {
            return _database.InsertAsync(a);
        }
        public Task<int> Delete(int actID)
        {
            return _database.ExecuteAsync("DELETE FROM tbl_activities WHERE activityID = ?;", actID);
        }
    }
}
