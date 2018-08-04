using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jsa.DomainModel;
using Jsa.WinIrsService.Properties;

namespace Jsa.WinIrsService
{
    public class PropertiesTableAdpater : IDomainList<Property>
    {
        private readonly IDatabase _database;

        public PropertiesTableAdpater(string dbPath)
        {
            _database = new IrsDatabase(dbPath);
        }

        #region IDomainList<Property> Members

        public IList<Property> GetData()
        {
            ITable propTable = new PropertiesTable();
            DataTable dataTable = _database.GetTable(propTable);
            return CreateProprty(dataTable);
        }

        #endregion

        private static IList<Property> CreateProprty(DataTable table)
        {
            if (table == null) throw new ArgumentNullException();
            if (table.Rows.Count == 0) throw new InvalidOperationException(Resources.NoPropertiesMsg);
            return (from DataRow row in table.Rows select TrasnformToProperty(row)).ToList();
        }

        private static Property TrasnformToProperty(DataRow row)
        {
            return new Property
                       {
                           PropertyNo = row.Field<string>("Property"),
                           Description = row.Field<string>("Name"),
                           Location = row.Field<string>("Location"),
                           Type = row.Field<string>("Type")
                       };
        }
    }
}