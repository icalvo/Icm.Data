using System.Data;

namespace Icm.Data.DataTableFormatters
{
    internal interface IDataTableFormatter
    {
        string GetStringRepresentation(DataTable dataTable);
    }
}