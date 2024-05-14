using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    public class Table
    {
        private readonly int tableID;
        private readonly string tableName;

        public Table(int id, string name)
        {
            tableID = id;
            tableName = name;
        }

        public string GetTable()
        {
            return tableName;
        }

        public int GetID()
        {
            return tableID;
        }
    }
}
