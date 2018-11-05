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
        IMobileServiceTable<NotificationData> transactionDataTable;

        public ShariesAzureDatabase()
        {
            _client = new MobileServiceClient("https://Sharies.azurewebsites.net");
            this.userDataTable = _client.GetTable<UserData>();
            this.systemDataTable = _client.GetTable<SystemData>();
            this.blockedAccountsTable = _client.GetTable<BlockedAccounts>();
            this.userCreditTable = _client.GetTable<UserCredit>();
            this.transactionDataTable = _client.GetTable<NotificationData>();
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
        // does same thing as GetUserDataAsync but left it in as an example of a query method
        public UserData QueryUserDataById(int Id) 
        {
            try
            {
                var userData = Task.Run(async () => {
                    return await userDataTable.Where(item => item.accountNumber == Id).ToListAsync();
                }).Result.First();
                return userData;
            }
            catch
            {
                return new UserData();
            }
        }
        public async Task<List<UserData>> QueryUserDataByIdAsync(int Id)
        {
            return await userDataTable.Where(item => item.accountNumber == Id).ToListAsync();
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
        #endregion

        #region SystemDataQueries
        public SystemData GetSystemData(string Id)
        {
            var systemData = Task.Run(async () => {
                return await systemDataTable.LookupAsync(Id);
            }).Result;
            return systemData;
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
        public List<UserCredit> QueryCreditDataByAccountNumber(int aNum)
        {
            var userCredit = Task.Run(async () =>
            {
                return await userCreditTable.Where(item => item.accountNumber == aNum).ToListAsync();
            }).Result;
            return userCredit;
        }
        // insert
        public async void InsertCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.InsertAsync(uCredit);
        }
        // update
        public async void UpdateCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.UpdateAsync(uCredit);
        }
        // delete
        public async void DeleteCreditDataAsync(UserCredit uCredit)
        {
            await userCreditTable.DeleteAsync(uCredit);
        }
        #endregion

        #region TransactionDataQueries
        // query
        public List<NotificationData> QueryTransactionDataBySource(int aNum)
        {
            var transaction = Task.Run(async () =>
            {
                return await transactionDataTable.Where(item => item.transactionSource == aNum).ToListAsync();
            }).Result;
            return transaction;
        }
        public List<NotificationData> QueryTransactionDataByDestination(int aNum)
        {
            var transaction = Task.Run(async () =>
            {
                return await transactionDataTable.Where(item => item.transactionDestination == aNum).ToListAsync();
            }).Result;
            return transaction;
        }
        // insert
        public async void InsertTransactionDataAsync(NotificationData transaction)
        {
            await transactionDataTable.InsertAsync(transaction);
        }
        // update
        public async void UpdateTransactionDataAsync(NotificationData transaction)
        {
            await transactionDataTable.UpdateAsync(transaction);
        }
        #endregion
    }
}
