using System.Collections.Generic;
using System.Linq;
using System;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace lealta_mobile
{
    public class ContractRepo
    {
        SQLiteAsyncConnection database;
        public ContractRepo(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteAsyncConnection(databasePath);
            database.CreateTableAsync<Contract>();
        }
        //public IEnumerable<Contract> GetItems()
        //{
        //    return (from i in database.Table<Contract>() select i).ToList();
        //}
        public async Task<bool> CheckNumber(string num)
        {
            var table = database.Table<Contract>();
            var filtered = table.Where(c => c.ContractNumber == num);
            var acc = await filtered.FirstOrDefaultAsync();
            return acc != null ? true : false;
        }
        public async Task<Contract> GetItem(int id)
        {
            return await database.GetAsync<Contract>(id);
        }
        public async Task<Contract> GetItemByIdPas(string id, string pas)
        {
            var check = await database.Table<Contract>().Where(c => c.ContractId == id && c.Password == pas).ToListAsync();
            return CheckAndGet(check);
        }
        public async Task<Contract> GetItemByNumberPas(string number, string pas)
        {
            var check = await database.Table<Contract>().Where(c => c.ContractNumber == number && c.Password == pas).ToListAsync();
            return CheckAndGet(check);
        }
        private Contract CheckAndGet(List<Contract> entries)
        {
            int cCount = entries.Count();
            if (cCount == 1)
                return entries.First();
            else if (cCount == 0)
                throw new Exception("Не верный логин или пароль.");
            else
                throw new Exception("Конфликт существующих аккаунтов. Обратитесь в службу поддержки.");
        }
        //public int DeleteItem(int id)
        //{
        //    return database.Delete<Contract>(id);
        //}
        public async Task<int> SaveItem(Contract item)
        {
            if (item.Id != 0)
            {
                await database.UpdateAsync(item);
                return item.Id;
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }
    }
}