using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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
        public async void InsertUserDataAsync(UserData user)
        {
            await userDataTable.InsertAsync(user);
        }
    }
}
