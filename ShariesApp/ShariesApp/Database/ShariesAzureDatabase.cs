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
        #region Tables
        MobileServiceClient _client;
        IMobileServiceTable<UserData> userDataTable;
        IMobileServiceTable<SystemData> systemDataTable;
        IMobileServiceTable<BlockedAccounts> blockedAccountsTable;
        IMobileServiceTable<UserCredit> userCreditTable;
        IMobileServiceTable<RequestData> requestDataTable;

        public ShariesAzureDatabase()
        {
            _client = new MobileServiceClient("https://Sharies.azurewebsites.net");
            this.userDataTable = _client.GetTable<UserData>();
            this.systemDataTable = _client.GetTable<SystemData>();
            this.blockedAccountsTable = _client.GetTable<BlockedAccounts>();
            this.userCreditTable = _client.GetTable<UserCredit>();
            this.requestDataTable = _client.GetTable<RequestData>();
        }
        #endregion

        #region UserDataQueries
        public UserData GetUserData(string Id)
        {
            try
            {
                var userData = Task.Run(async () => {
                    return await userDataTable.LookupAsync(Id);
                }).Result;
                return userData;
            }
            catch
            {
                return new UserData();
            }
        }
        // query
        public UserData QueryUserDataByAccountNumber(int aNum) 
        {
            try
            {
                var userData = Task.Run(async () => {
                    return await userDataTable.Where(item => item.accountNumber == aNum).ToListAsync();
                }).Result.First();
                return userData;
            }
            catch
            {
                return new UserData();
            }
        }
        public async Task<List<UserData>> QueryUserDataByAccountNumberAsync(int aNum)
        {
            return await userDataTable.Where(item => item.accountNumber == aNum).ToListAsync();
        }
        // insert
        public void InsertUserData(UserData user)
        {
            Task.Run(async () => {
                await userDataTable.InsertAsync(user);
            });
        }
        public async void InsertUserDataAsync(UserData user)
        {
            await userDataTable.InsertAsync(user);
        }
        // update
        public void UpdateUserData(UserData user)
        {
            Task.Run(async () => {
                await userDataTable.UpdateAsync(user);
            });
        }
        public async void UpdateUserDataAsync(UserData user)
        {
            await userDataTable.UpdateAsync(user);
        }
        // delete
        public void DeleteUserData(UserData user)
        {
            Task.Run(async () => {
                await userDataTable.DeleteAsync(user);
            });
        }
        public async void DeleteUserDataAsync(UserData user)
        {
            await userDataTable.DeleteAsync(user);
        }
        #endregion

        #region SystemDataQueries
        public SystemData GetSystemData(string Id)
        {
            var systemData = Task.Run(async () => {
                return await systemDataTable.LookupAsync(Id);
            }).Result;
            return systemData;
        }
        public async Task<SystemData> GetSystemDataAsync(string Id)
        {
            return await systemDataTable.LookupAsync(Id);
        }
        public async void UpdateSystemDataAsync(SystemData systemData)
        {
            await systemDataTable.UpdateAsync(systemData);
        }
        #endregion

        #region BlockedAccountsQueries
        public List<BlockedAccounts> QueryBlockedAccountsByBlocker(int blocker)
        {
            var userData = Task.Run(async () =>
            {
                return await blockedAccountsTable.Where(item => item.blocker == blocker).ToListAsync();
            }).Result;
            return userData;
        }
        public async void InsertBlockedAccountsAsync(BlockedAccounts bAccounts)
        {
            await blockedAccountsTable.InsertAsync(bAccounts);
        }
        public async void DeleteBlockedAccountsAsync(BlockedAccounts bAccounts)
        {
            await blockedAccountsTable.DeleteAsync(bAccounts);
        }
        #endregion

        #region CreditDataQueries
        // query
        public UserCredit QueryCreditDataByAccountNumber(int aNum)
        {
            try
            {
                var userCredit = Task.Run(async () =>
                {
                    return await userCreditTable.Where(item => item.accountNumber == aNum).ToListAsync();
                }).Result.First();
                return userCredit;
            }
            catch
            {
                return new UserCredit();
            }
        }
        // insert
        public void InsertCreditData(UserCredit uCredit)
        {
            Task.Run(async () => {
                await userCreditTable.InsertAsync(uCredit);
            });
        }
        public async void InsertCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.InsertAsync(uCredit);
        }
        // update
        public void UpdateCreditData(UserCredit uCredit)
        {
            Task.Run(async () => {
                await userCreditTable.UpdateAsync(uCredit);
            });
        }
        public async void UpdateCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.UpdateAsync(uCredit);
        }
        // delete
        public void DeleteCreditData(UserCredit uCredit)
        {
            Task.Run(async () => {
                await userCreditTable.DeleteAsync(uCredit);
            });
        }
        public async void DeleteCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.DeleteAsync(uCredit);
        }
        #endregion

        #region RequestDataQueries
        // query
        public List<RequestData> QueryRequestDataBySource(int aNum)
        {
            var request = Task.Run(async () =>
            {
                return await requestDataTable.Where(item => item.requestSource == aNum).ToListAsync();
            }).Result;
            return request;
        }
        public List<RequestData> QueryRequestDataByDestination(int aNum)
        {
            var request = Task.Run(async () =>
            {
                return await requestDataTable.Where(item => item.requestDestination == aNum).ToListAsync();
            }).Result;
            return request;
        }
        // insert
        public async void InsertRequestDataAsync(RequestData data)
        {
            await requestDataTable.InsertAsync(data);
        }
        // update
        public async void UpdateRequestDataAsync(RequestData data)
        {
            await requestDataTable.UpdateAsync(data);
        }
        // delete
        public async void DeleteRequestDataAsync(RequestData data)
        {
            await requestDataTable.DeleteAsync(data);
        }
        #endregion
    }
}
