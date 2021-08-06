using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace nacs_tracker
{
    class Program
    {
        static List<Character> charList;
        static int nameLength;
        static int healthLength;
        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=P:\\PROJECTS\\NACS-TRACKER\\DATABASE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static SqlConnection conn = new SqlConnection(connectionString);
        static SqlCommand command;
        static SqlDataReader dataReader;
        static string sql = null;
        static int power;
        static int targetHp;
        static int targetDef;
        static int targetChg;
        static string targetActionStr;
        static int targetActionInt;

        static void AddCharacter(string name, char position, int hp, int maxHp)
        {
            sql = $"INSERT INTO Characters(Name, Position, Hp, MaxHp) VALUES('{name}', '{position}', {hp}, {maxHp})";
            try
            {
                conn.Open();
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, int hp, int maxHp)
        {
            sql = $"INSERT INTO Characters(Name, Hp, MaxHp) VALUES('{name}', {hp}, {maxHp})";
            try
            {
                conn.Open();
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static string PrintDivider()
        {
            string divider = "+--+--------------------+---+--------------------+---+------+---+";
            return divider;
        }
        static string PrintCharacter(Character printChar)
        {
            string output = "|";
            int numChars;
            int numSpaces;

            // Print ID
            if (printChar.Id < 10)
            {
                output = output + " " + printChar.Id + "|";
            }
            else
            {
                output = output + printChar.Id + "|";
            }

            // Print name
            numSpaces = 20;
            numChars = printChar.Name.Length;
            if (numChars <= 20)
            {
                output = output + printChar.Name;
            }
            else
            {
                numChars = 20;
                output = output + printChar.Name.Substring(0, 20);
            }
            for (int i = numChars; i < numSpaces; i++)
            {
                output = output + " ";
            }
            output = output + "|";

            // Print position
            output = output + " " + printChar.Position + " |";

            // Print HP
            int hpCounter;
            for (hpCounter = 1; hpCounter <= printChar.Hp; hpCounter++)
            {
                output = output + Character.fullHeart;
            }
            for (/*Keeping the value from last time*/; hpCounter <= printChar.MaxHp; hpCounter++)
            {
                output = output + Character.emptyHeart;
            }
            for (/*Keeping the value from last time*/; hpCounter <= 20; hpCounter++)
            {
                output = output + " ";
            }
            output = output + "|";


            // Print charge
            if (printChar.Charge < 10)
            {
                output = output + "  " + printChar.Charge + "|";
            }
            else
            {
                output = output + " " + printChar.Charge + "|";
            }

            // Print action
            numSpaces = 6;
 /*           switch (printChar.CharAction)
            {
                case Action.Heal:
                    numChars = 4;
                    output = output + "Heal";
                    break;
                case Action.Boost:
                    numChars = 5;
                    output = output + "Boost";
                    break;
                case Action.Attack:
                    numChars = 6;
                    output = output + "Attack";
                    break;
                case Action.Defend:
                    numChars = 6;
                    output = output + "Defend";
                    break;
                case Action.Revive:
                    numChars = 6;
                    output = output + "Revive";
                    break;
                case Action.Charge:
                    numChars = 6;
                    output = output + "Charge";
                    break;
                case Action.None:
                    numChars = 4;
                    output = output + "None";
                    break;
                case Action.Dead:
                    numChars = 4;
                    output = output + "Dead";
                    break;
            } */
            for (int i = numChars; i < numSpaces; i++)
            {
                output = output + " ";
            }
            output = output + "|";

            // Print target
            if (printChar.Target < 10)
            {
                output = output + " " + printChar.Target + "|";
            }
            else
            {
                output = output + printChar.Target + "|";
            }

            return output;

        }
        static void Attack(int origin, int target)
        {

            try
            {
                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT * FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) - power;
                    if(targetHp < 0)
                    {
                        targetHp = 0;
                    }
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        static void Defend(int origin)
        {
            try
            {
                sql = $"SELECT * FROM Characters WHERE id = {origin}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                    targetDef = Convert.ToInt32(dataReader.GetValue(6)) + power;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Defense = {targetDef}, Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Heal(int origin, int target)
        {
            try
            {
                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT * FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) + power;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Boost(int origin, int target)
        {
            try
            {
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(10));
                    targetChg = Convert.ToInt32(dataReader.GetValue(5));
                }
                dataReader.Close();
                command.Dispose();

                if (targetActionStr != "Attack" && targetActionStr != "Defend" && targetActionStr != "Heal") // Other actions cannot be boosted
                {
                    conn.Close();
                    return;
                }

                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = 1 + Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                targetChg += power;

                sql = $"UPDATE Characters SET Charge = {targetChg} WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Revive(int origin, int target)
        {
            try
            {
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(10));
                    power = Convert.ToInt32(dataReader.GetValue(5));
                }
                dataReader.Close();
                command.Dispose();

                if(targetActionStr != "Dead") // Can't revive someone who isn't dead
                {
                    conn.Close();
                    return;
                }

                if (power <= 0) {
                    sql = $"SELECT * FROM Actions WHERE Action = 'Cooldown'";
                    command = new SqlCommand(sql, conn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Action = {targetActionInt} WHERE Id = {origin}";
                    command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                    command.Dispose();

                    sql = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command = new SqlCommand(sql, conn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Hp = 1, Action = {targetActionInt} WHERE Id = {target}";
                    command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                    command.Dispose();
                } else
                {
                    sql = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command = new SqlCommand(sql, conn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Hp = {power}, Action = {targetActionInt} WHERE Id = {target}";
                    command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Charge(int origin)
        {
            try
            {
                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = {power} WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Overcharge(int origin, int amount)
        {
/*            origin.Charge += amount;
            origin.Damage(amount);*/
            try
            {
                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + amount + 1;
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) - amount;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = {power}, Hp = {targetHp} WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void CatchDead()
        {
            try
            {
                sql = $"SELECT * FROM Actions WHERE Action = 'Dead'";
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Action = {targetActionInt}, Hp = 0 WHERE Hp <= 0";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void PrintTracker()
        {
            Console.WriteLine(PrintDivider());
            foreach (Character item in charList)
            {
                // TODO: Print character and divider
            }
        }
/*        static void ReadSql(string query)
        {
            sql = query;
            try
            {
                conn.Open();
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                Console.WriteLine(System.Environment.CurrentDirectory);
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetValue(0));
                    Console.WriteLine(dataReader.GetValue(1));
                }
                dataReader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection failed");
            }
        } */
        static void Main(string[] args)
        {
            // This method currently used for debugging
        }
    }
}
