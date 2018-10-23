using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ShariesApp
{
    public class ShariesDataBase
    {
        readonly SQLiteAsyncConnection database;

        public ShariesDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<UserData>().Wait();
        }
        // Get methods
        public Task<List<UserData>> GetUserDataTableAsync() // get UserData table as list
        {
            return database.Table<UserData>().ToListAsync();
        }
        public Task<UserData> GetUserDataItemAsync(int id) // get one row from UserData table with query
        {
            return database.Table<UserData>().Where(i => i.AccountNumber == id).FirstOrDefaultAsync();
        }
        public Task<UserData> GetUserDataFromPK(int id) // find row with PK
        {
            return database.FindAsync<UserData>(id);
        }
        // Save methods
        public Task<int> SaveUserData(UserData item)
        {
            return database.InsertAsync(item);
        }
        // Delete methods
        public Task<int> DeleteUserData(UserData item)
        {
            return database.DeleteAsync(item);
        }

        /*
        public Task<List<UserData>> GetItemsNotDoneAsync() // dunno
        {
            return database.QueryAsync<UserData>("SELECT * FROM [UserData] WHERE [Done] = 0");
        }
         */
    }
}
