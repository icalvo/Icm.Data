using System.Collections.Generic;
using System.Data;

namespace Icm.Data.DataTableFormatters
{
    internal abstract class BaseDataTableFormatter : IDataTableFormatter
    {
        string IDataTableFormatter.GetStringRepresentation(DataTable dataTable)
        {
            return string.Join("", GetStringRepresentation(dataTable));
        }

        protected abstract IEnumerable<string> GetStringRepresentation(DataTable dataTable);

    }
}