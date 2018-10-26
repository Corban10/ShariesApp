using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace ShariesApp
{
    public class ShariesAzureDatabase
    {
        MobileServiceClient _client;
        IMobileServiceTable<UserData> userDataTable;

        public ShariesAzureDatabase()
        {
            _client = new MobileServiceClient("https://Sharies.azurewebsites.net");
            this.userDataTable = _client.GetTable<UserData>();
        }

        public UserData GetUserDataAsync(string Id)
        {
            try
            {
                var userData = Task.Run(async () => {
                    return await userDataTable.LookupAsync(Id);
                }).Result;
                return userData;
            }
            catch (Exception e)
            {
                return new UserData();
            }
        }
        // does same thing as GetUserDataAsync but left it in as an example of a query method
        public UserData QueryUserDataById(string Id) 
        {
            try
            {
                var userData = Task.Run(async () => {
                    return await userDataTable.Where(item => item.id == Id).ToListAsync();
                }).Result.First();
                return userData;
            }
            catch (Exception e)
            {
                return new UserData();
            }
        }
        public async Task<List<UserData>> QueryUserDataByIdAsync(string Id)
        {
            return await userDataTable.Where(item => item.id == Id).ToListAsync();
        }
        public async void InsertUserDataAsync(UserData user)
        {
            await userDataTable.InsertAsync(user);
        }
        public async void UpdateUserDataAsync(UserData user)
        {
            await userDataTable.UpdateAsync(user);
        }
        public async void DeleteUserDataAsync(UserData user)
        {
            await userDataTable.DeleteAsync(user);
        }
    }
}
