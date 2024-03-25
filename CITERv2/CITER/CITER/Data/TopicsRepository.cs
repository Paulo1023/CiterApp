using CITER.Models;
using Plugin.CloudFirestore;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CITER.Data
{
    public class TopicsRepository
    {
        public readonly SQLiteAsyncConnection _database;

        public static string dbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "citerdb.db");
        public TopicsRepository()
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Downloads>();
        }
        public Task<int> SaveTopic(Downloads topic)
        {
            return _database.InsertAsync(topic);
        }
        public Task<int> CountTopic()
        {
            return _database.Table<Downloads>().CountAsync();
        }
        public Task<List<Downloads>> ListTopics()
        {
            return _database.Table<Downloads>().ToListAsync();
        }
        public Task<int> DeleteTopic(string title)
        {
            return _database.ExecuteAsync("DELETE FROM tbl_downloads WHERE title = ?;", title);
        }
        //        public Task<List<Topics>> Search(string searchTerm)
        //        {
        //            return _database.Table<Topics>().Where(value => value.Title.ToLower().Contains(searchTerm)).ToListAsync();
        //        }
        //        //public Task<List<Topics>> Filter(string c)
        //        //{
        //        //    return _database.Table<Topics>().Where(value => value.CertProvider.ToLower().Contains(c)).ToListAsync();
        //        //}
        //        //public Task<List<Topics>> GetTitle(int id)
        //        //{
        //        //    return _database.Table<Topics>().Where(value => value.TopicID.Equals(id)).ToListAsync();
        //        //}
        //        //public Task<List<Topics>> DisplayBookmarkTopic(int n)
        //        //{
        //        //    return _database.Table<Topics>().Where(value => value.IsMarked.Equals(n)).ToListAsync();
        //        //}
        //        public Task<int> UpdateTopic(Topics topic)
        //        {
        //            return _database.UpdateAsync(topic);
        //        }



        //        //firebase database
        //        public async Task<IEnumerable<Topics>> ListFbTopics()
        //        {
        //            IEnumerable<Topics> Topics;
        //            var Query = await CrossCloudFirestore.Current
        //                                             .Instance
        //                                             .CollectionGroup("Topics")
        //                                             .GetAsync();
        //            Topics = Query.ToObjects<Topics>();
        //            return Topics;
        //        }


    }
}
