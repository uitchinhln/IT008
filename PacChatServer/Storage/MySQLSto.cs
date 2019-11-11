﻿using MySql.Data.MySqlClient;
using PacChatServer.Command;
using PacChatServer.Entities;
using PacChatServer.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Storage
{
    public class MySQLSto
    {
        static MySQLSto instance;

        //Number of attempt to reconnect before displaying an error and stopping the server.
        private static int RETRY_LIMIT = 10;
        private static int RETRY_COUNT = 0;


        MySqlConnection dbConn;


        private MySQLSto()
        {
            OpenConnection();
            CreateDefault();
        }

        public User GetUser(string email)
        {
            User user = null;
            email = email.Trim().ToLower();

            MySqlDataReader reader = null;
            MySqlCommand query = null;

            try
            {
                query = new MySqlCommand(Query.GET_USER_INFO_BYMAIL, dbConn);
                query.Parameters.AddWithValue("email", email);

                reader = query.ExecuteReader();
                if (reader.Read())
                {
                    user = new User(reader.GetInt32("id"));
                    user.Email = reader.GetString("email");
                    user.PassHashed = reader.GetString("passhash");
                    user.FirstName = reader.GetString("firstname");
                    user.LastName = reader.GetString("lastname");
                    user.DoB = reader.GetDateTime("dob");
                    user.Gender = (Gender) reader.GetInt16("gender");
                }
            }
            catch (MySqlException e)
            {
                PacChatServer.GetServer().Logger.Error(e);
                if (RETRY_COUNT >= RETRY_LIMIT)
                {
                    PacChatServer.GetCommandManager().ExecuteCommand(ConsoleSender.Instance, DefaultCommands.STOP);
                    return null;
                }
                RETRY_COUNT++;
                OpenConnection();
                user = GetUser(email);
            } finally
            {
                Cleanup(reader);
            }
            RETRY_COUNT = 0;
            return user;
        }

        public User GetUser(int id)
        {
            User user = null;

            MySqlDataReader reader = null;
            MySqlCommand query = null;

            try
            {
                query = new MySqlCommand(Query.GET_USER_INFO_BYID, dbConn);
                query.Parameters.AddWithValue("id", id);

                reader = query.ExecuteReader();  
                if (reader.Read())
                {
                    user = new User(reader.GetInt32("id"));
                    user.Email = reader.GetString("email");
                    user.PassHashed = reader.GetString("passhash");
                    user.FirstName = reader.GetString("firstname");
                    user.LastName = reader.GetString("lastname");
                    user.DoB = reader.GetDateTime("dob");
                    user.Gender = (Gender) reader.GetInt16("gender");
                }
            }
            catch (MySqlException e)
            {
                PacChatServer.GetServer().Logger.Error(e);
                if (RETRY_COUNT >= RETRY_LIMIT)
                {
                    PacChatServer.GetCommandManager().ExecuteCommand(ConsoleSender.Instance, DefaultCommands.STOP);
                    return null;
                }
                RETRY_COUNT++;
                OpenConnection();
                user = GetUser(id);
            } finally
            {
                Cleanup(reader);
            }
            RETRY_COUNT = 0;
            return user;
        }

        public User AddNewUser(User user)
        {
            User check = GetUser(user.Email);
            if (check != null) return check;

            MySqlDataReader reader = null;
            MySqlCommand query = null;

            try
            {
                query = new MySqlCommand(Query.ADD_NEW_USER, dbConn);
                query.Parameters.AddWithValue("email", user.Email);
                query.Parameters.AddWithValue("passhash", user.PassHashed);
                query.Parameters.AddWithValue("firstname", user.FirstName);
                query.Parameters.AddWithValue("lastname", user.LastName);
                query.Parameters.AddWithValue("dob", user.DoB);
                query.Parameters.AddWithValue("gender", (int) user.Gender);

                query.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                PacChatServer.GetServer().Logger.Error(e);
                if (RETRY_COUNT >= RETRY_LIMIT)
                {
                    PacChatServer.GetCommandManager().ExecuteCommand(ConsoleSender.Instance, DefaultCommands.STOP);
                    return null;
                }
                RETRY_COUNT++;
                OpenConnection();
                check = AddNewUser(user);
            }
            finally
            {
                Cleanup(reader);
            }
            RETRY_COUNT = 0;
            check = GetUser(user.Email);
            return check;
        }

        private void CreateDefault()
        {
            MySqlDataReader reader = null;
            MySqlCommand query = null;

            try
            {
                query = new MySqlCommand(Query.CREATE_TBL_USER, dbConn);
                query.ExecuteNonQuery();


            } catch (MySqlException e)
            {
                PacChatServer.GetServer().Logger.Error(e);
                if (RETRY_COUNT >= RETRY_LIMIT)
                {
                    PacChatServer.GetCommandManager().ExecuteCommand(ConsoleSender.Instance, DefaultCommands.STOP);
                    return;
                }
                RETRY_COUNT++;
                OpenConnection();
                CreateDefault();
            }
            finally
            {
                Cleanup(reader);
            }
            RETRY_COUNT = 0;
        }

        public void OpenConnection()
        {
            CloseConnection();

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = ServerSettings.MYSQL_HOST;
            builder.UserID = ServerSettings.MYSQL_USER;
            builder.Password = ServerSettings.MYSQL_PASSWORD;
            builder.Database = ServerSettings.MYSQL_DATABASE;
            builder.Port = ServerSettings.MYSQL_PORT;

            dbConn = new MySqlConnection(builder.ToString());
            dbConn.Open();
        }

        public void CloseConnection()
        {
            try
            {
                if (dbConn != null && dbConn.State != System.Data.ConnectionState.Closed)
                {
                    dbConn.Close();
                }
            } catch (Exception) { }
        }

        private void Cleanup(MySqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed) reader.Close();
        }


        private class Query
        {
            /*-----------------QUERY FOR THE FIRST RUN-----------------*/
            public static readonly string CREATE_TBL_USER = "CREATE TABLE IF NOT EXISTS `users` (`id` INT NOT NULL AUTO_INCREMENT, `email` VARCHAR(45) CHARACTER SET 'utf8' NOT NULL, `passhash` VARCHAR(45) NOT NULL, `firstname` VARCHAR(45) CHARACTER SET 'utf8' NOT NULL, `lastname` VARCHAR(45) CHARACTER SET 'utf8' NOT NULL, `dob` DATE NULL, `gender` TINYINT(10) NULL DEFAULT 0, PRIMARY KEY (`id`), UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE) ENGINE = InnoDB;";


            /*-----------------QUERY FOR USERs TABLE-------------------*/
            public static readonly string GET_USER_INFO_BYMAIL = "SELECT * FROM `users` WHERE email= ?email;";
            public static readonly string GET_USER_INFO_BYID = "SELECT * FROM `users` WHERE id= ?id;";
            public static readonly string GET_USER_RELA = "SELECT * FROM `users_rla` WHERE user1= ?user1 OR user2= ?user2;";
            public static readonly string ADD_NEW_USER = "INSERT INTO `users` (email, passhash, firstname, lastname, dob, gender) VALUES (?email, ?passhash, ?firstname, ?lastname, ?dob, ?gender);";


            /*-----------------QUERY FOR USER RELATIONSHIPs TABLE------*/



            /*-----------------QUERY FOR MESSAGEs TABLE----------------*/

        }

        public static MySQLSto Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MySQLSto();
                }
                return instance;
            }
        }
    }    
}
