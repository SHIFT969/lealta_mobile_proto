using System;

using lealta_mobile.Droid;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteResolver_Droid))]
namespace lealta_mobile.Droid
{
    class SQLiteResolver_Droid : ISQLite
    {
        public SQLiteResolver_Droid() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}