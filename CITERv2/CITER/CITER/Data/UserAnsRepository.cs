using CITER.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CITER.Data
{
    public class UserAnsRepository
    {
        public readonly SQLiteAsyncConnection _database;

        public UserAnsRepository()
        {
            _database = new SQLiteAsyncConnection(TopicsRepository.dbPath);
            _database.CreateTableAsync<Exam>();
        }

        public Task<List<Exam>> List()
        {
            return _database.Table<Exam>().ToListAsync();
        }
        public Task<int> Save(Exam e)
        {
            return _database.InsertAsync(e);
        }
        public Task<List<Exam>> CheckActID(int id)
        {
            return _database.Table<Exam>().Where(value => value.ActivityID.Equals(id)).ToListAsync();
        }
        public Task<List<Exam>> CheckCorrect(int id)
        {
            return _database.Table<Exam>().Where(value => value.ActivityID.Equals(id) && value.UserAnswer == value.CorrectAnswer).ToListAsync();
        }
        public Task<int> examCount(int id)
        {
            return _database.Table<Exam>().Where(value => value.ActivityID.Equals(id)).CountAsync();
        }
        public Task<int> Delete(int actID)
        {
            return _database.ExecuteAsync("DELETE FROM tbl_questions WHERE activityID = ?;", actID);
        }
    }
}
