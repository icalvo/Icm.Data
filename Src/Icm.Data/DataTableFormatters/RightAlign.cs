namespace Icm.Data.DataTableFormatters
{
    public class RightAlign : IFormatConfiguration
    {
        public RightAlign(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; set; }
    }
}