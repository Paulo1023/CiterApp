using CITER.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CITER.Data
{
    public class QuestionnairesRepository
    {

        public readonly SQLiteAsyncConnection _database;

        public QuestionnairesRepository()
        {
            _database = new SQLiteAsyncConnection(TopicsRepository.dbPath);
            _database.CreateTableAsync<Quiz>();
        }
        public Task<List<Quiz>> ListQuestionnaires()
        {
            return _database.Table<Quiz>().ToListAsync();
        }
        public Task<int> SaveQuestionaires(Quiz q)
        {
            return _database.InsertAsync(q);
        }
        public Task<int> DelQuestionnaires()
        {
            return _database.DeleteAllAsync<Quiz>();
        }
        public Task<int> Update(string ans, int qID)
        {
            return _database.ExecuteAsync("UPDATE tbl_questionnaires SET userAnswer = ? WHERE questionnaireID = ?;", ans, qID);
            
        }
        public Task<List<Quiz>> Answered(int id)
        {
            return _database.Table<Quiz>().Where(value => value.questionID.Equals(id)).ToListAsync();
        }
    }
}
