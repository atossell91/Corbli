using MySqlConnector;
using System.Text;

namespace Corbli.Services
{
    public class UserDbService
    {
        private readonly MySqlDataSource _dbSource;

        public UserDbService(MySqlDataSource dbSource)
        {
            _dbSource = dbSource;
        }

        public bool QueryPasswordHash(string username, ref string hash)
        {
            bool result = false;
            var dbConnection = _dbSource.OpenConnection();

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT password ");
            queryBuilder.Append("FROM USERINFO ");
            queryBuilder.Append("WHERE Username='");
            queryBuilder.Append(username);
            queryBuilder.Append("';");

            var command = dbConnection.CreateCommand();
            command.CommandText = queryBuilder.ToString();
            var reader = command.ExecuteReader();

            if (reader != null && reader.Read())
            {
                hash = reader.GetString(0);
                result = true;
                reader.Close();
            }

            dbConnection.Close();
            return result;
        }

        public bool AddNewUser(string username, string hashedPassword)
        {
            var connection = _dbSource.OpenConnection();

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO USERINFO (Username, Password) ");
            queryBuilder.Append($"VALUES ('{username}', '{hashedPassword}');");

            var command = connection.CreateCommand();
            command.CommandText = queryBuilder.ToString();

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            return rowsAffected > 0;
        }
    }
}
