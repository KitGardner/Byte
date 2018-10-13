using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class SqliteExample : MonoBehaviour {

    private string dbPath;
    public Text displayText;

    // Use this for initialization
    void Start ()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDatabase.db";

        displayText.text = "Sql Database Test Text \r\n\r\n";
        CreateSchema();
        InsertScore("GG Meade", 3701);
        InsertScore("US Grant", 4242);
        InsertScore("GB McClellan", 107);
        GetHighScores(10);
    }

    public void CreateSchema()
    {
        Debug.Log(Application.persistentDataPath);
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score' ( " +
                                  "  'id' INTEGER PRIMARY KEY, " +
                                  "  'name' TEXT NOT NULL, " +
                                  "  'score' INTEGER NOT NULL" +
                                  ");";

                var result = cmd.ExecuteNonQuery();
                displayText.text += "create schema: " + result + "\r\n";
            }
        }
    }

    public void InsertScore(string highScoreName, int score)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO high_score (name, score) " +
                                  "VALUES (@Name, @Score);";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Name",
                    Value = highScoreName
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Score",
                    Value = score
                });

                var result = cmd.ExecuteNonQuery();
                displayText.text += "insert score: " + result + "\r\n";
            }
        }
    }

    public void GetHighScores(int limit)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM high_score ORDER BY score DESC LIMIT @Count;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Count",
                    Value = limit
                });

                displayText.text += "scores (begin)\r\n";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var highScoreName = reader.GetString(1);
                    var score = reader.GetInt32(2);
                    var text = string.Format("{0}: {1} [#{2}]", highScoreName, score, id);
                    displayText.text += text + "\r\n";
                }
                displayText.text += "scores (end)\r\n";
            }

            using (var command = conn.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM high_score";

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var itemCount = reader.GetInt32(0);
                    displayText.text += "There are " + itemCount + " items in the database.\r\n";
                }
            }

            using (var cmd1 = conn.CreateCommand())
            {
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "DELETE FROM high_score";

                var result = cmd1.ExecuteNonQuery();
            }
        }
    }

}
