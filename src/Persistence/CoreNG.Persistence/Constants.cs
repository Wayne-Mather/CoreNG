namespace CoreNG.Persistence.SqlServer
{
    public static class Constants
    {
        public static class ColumnTypes
        {
            public static string AccountName = "varchar(50)";
            public static string Balance = "numeric(12,2)";
            public static string Comments = "varchar(1024)";
            public static string CategoryName = "varchar(50)";
            public static string DateTime = "datetime";
            public static string ScheduledName = "varchar(50)";
        }
    }
}