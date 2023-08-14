using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Update
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection sqlite_conn;
            Console.WriteLine("Connect to database...");
            sqlite_conn = CreateConnection();
            Console.WriteLine(sqlite_conn.State.ToString());
            string command;
            command = "CREATE TABLE _ReportDefinitions (" +
                "ReportDefinitionID INTEGER, " +
                "ReportName TEXT, " +
                "ReportTypeID INTEGER DEFAULT 0, " +
                "ReportDestID  INTEGER DEFAULT 0, " +
                "DestinationInfo TEXT, " +
                "Arhive INTEGER DEFAULT 0, " +
                "Header2 INTEGER DEFAULT 0, " +
                "WhereClause TEXT, " +
                "TimeFormatID INTEGER DEFAULT 0, " +
                "TimePeriodInfo TEXT, " +
                "SampleTimeFormatID INTEGER DEFAULT 0, " +
                "SampleTimePeriodInfo TEXT, " +
                "AdditionalParams TEXT, " +
                "Unit INTEGER DEFAULT 0, " +
                "OffSet TEXT DEFAULT '00:00:00', " +
                "Start TEXT, " +
                "End TEXT, " +
                "NextEvent TEXT, " +
                "Enable INTEGER DEFAULT 0, " +
                "PRIMARY KEY(ReportDefinitionID));";
            Table(sqlite_conn, command);

            command = "INSERT INTO _ReportDefinitions SELECT * FROM ReportDefinitions";
            Table(sqlite_conn, command);

            command = "DROP TABLE ReportDefinitions";
            Table(sqlite_conn, command);

            command = "CREATE TABLE ReportDefinitions (" +
                "ReportDefinitionID INTEGER, " +
                "ReportName TEXT, " +
                "ReportTypeID INTEGER DEFAULT 0, " +
                "ReportDestID  INTEGER DEFAULT 0, " +
                "DestinationInfo TEXT, " +
                "Arhive INTEGER DEFAULT 0, " +
                "Header2 TEXT, " +
                "WhereClause TEXT, " +
                "TimeFormatID INTEGER DEFAULT 0, " +
                "TimePeriodInfo TEXT, " +
                "SampleTimeFormatID INTEGER DEFAULT 0, " +
                "SampleTimePeriodInfo TEXT, " +
                "AdditionalParams TEXT, " +
                "Unit INTEGER DEFAULT 0, " +
                "OffSet TEXT DEFAULT '00:00:00', " +
                "Start TEXT, " +
                "End TEXT, " +
                "NextEvent TEXT, " +
                "Enable INTEGER DEFAULT 0, " +
                "PRIMARY KEY(ReportDefinitionID));";
            Table(sqlite_conn, command);

            command = "ALTER TABLE ReportDefinitions RENAME COLUMN WhereClause TO LastUsed";
            Table(sqlite_conn, command);
            
            command = "INSERT INTO ReportDefinitions SELECT * FROM _ReportDefinitions";
            Table(sqlite_conn, command);

            command = "DROP TABLE _ReportDefinitions";
            Table(sqlite_conn, command);


            Modify(sqlite_conn);
          

            command = "ALTER TABLE Historians RENAME COLUMN UserName TO UnitNet";
            Table(sqlite_conn, command);
                       

            ModifyHist(sqlite_conn);
            sqlite_conn.Close();

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source=Rpt.db;Version=3;New=true;Compress=true;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return sqlite_conn;
        }
        static void Table(SQLiteConnection conn, string commandText)
        {
            Console.WriteLine(commandText);
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = commandText;
            sqlite_cmd.ExecuteNonQuery();
            Console.WriteLine("OK!");
        }
        static void Modify(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "select * from ReportDefinitions";
            Dictionary<int, string> dict = new Dictionary<int, string>();
            Dictionary<int, string> dict1 = new Dictionary<int, string>();
            using (SQLiteDataReader rdr = sqlite_cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    List<string> list = new List<string>();
                    int id =rdr.GetInt32(0);
                    int h = int.Parse(rdr.GetString(6));
                    if (IsBitSet(h, 2)) list.Add("1");
                    if (IsBitSet(h, 1)) list.Add("2");
                    if (IsBitSet(h, 0)) list.Add("3");
                    string join = String.Join(",", list.ToArray());
                    dict.Add(id, join);
                }
            }
            foreach (var item in dict)
            {
                string command = "UPDATE ReportDefinitions SET Header2='" + item.Value + "' WHERE ReportDefinitionID=" + item.Key;
                Table(conn, command);
            }
        }
        static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        static void ModifyHist(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "select * from Historians";
            Dictionary<int, string> dict = new Dictionary<int, string>();
            using (SQLiteDataReader rdr = sqlite_cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    List<string> list = new List<string>();
                    int id = rdr.GetInt32(2);
                    //int h = int.Parse(rdr.GetString(3));
                    if (id==2) dict.Add(id, "UNIT0@SCL");
                    if (id == 3) dict.Add(id, "UNIT@SMS");
                    if (id == 4) dict.Add(id, "UNIT4@BLK4");
                    if (id == 5) dict.Add(id, "UNIT5@BLK5");
                    if (id == 6) dict.Add(id, "UNIT6@NET0");
                    if (id == 7) dict.Add(id, "UNIT7@BLK7");
                    if (id == 8) dict.Add(id, "UNIT8@BLK8");
                    if (id == 9) dict.Add(id, "UNIT9@BLK9");
                }
            }
            foreach (var item in dict)
            {
                string command = "UPDATE Historians SET UnitNet='" + item.Value + "' WHERE Unit=" + item.Key;
                Table(conn, command);
            }
        }
    }
}
