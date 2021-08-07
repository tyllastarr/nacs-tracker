using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace nacs_tracker
{
    class Program
    {
        static int nameLength;
        static int healthLength;
        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=P:\\PROJECTS\\NACS-TRACKER\\DATABASE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static SqlConnection conn1 = new SqlConnection(connectionString);
        static SqlConnection conn2 = new SqlConnection(connectionString);
        static SqlCommand command;
        static SqlDataReader dataReader;
        static string sql = null;
        static int power;
        static int targetHp;
        static int targetDef;
        static int targetChg;
        static string targetActionStr;
        static int targetActionInt;
        static int count;
        const char emptyHeart = '♡';
        const char fullHeart = '♥';


        static void AddCharacter(string name, char position, int hp, int maxHp)
        {
            sql = $"INSERT INTO Characters(Name, Position, Hp, MaxHp) VALUES('{name}', '{position}', {hp}, {maxHp})";
            try
            {
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, int hp, int maxHp)
        {
            sql = $"INSERT INTO Characters(Name, Hp, MaxHp) VALUES('{name}', {hp}, {maxHp})";
            try
            {
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static string PrintDivider()
        {
            string divider = "+----+--------------------+---+--------------------+---+----------+----+";
            return divider;
        }
        static void Attack(int origin, int target)
        {

            try
            {
                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT * FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) - power;
                    if (targetHp < 0)
                    {
                        targetHp = 0;
                    }
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                    targetDef = Convert.ToInt32(dataReader.GetValue(6)) + power;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Defense = {targetDef}, Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT * FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) + power;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(10));
                    targetChg = Convert.ToInt32(dataReader.GetValue(5));
                }
                dataReader.Close();
                command.Dispose();

                if (targetActionStr != "Attack" && targetActionStr != "Defend" && targetActionStr != "Heal") // Other actions cannot be boosted
                {
                    conn1.Close();
                    return;
                }

                sql = $"SELECT * FROM Characters WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = 1 + Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                targetChg += power;

                sql = $"UPDATE Characters SET Charge = {targetChg} WHERE Id = {target}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(10));
                    power = Convert.ToInt32(dataReader.GetValue(5));
                }
                dataReader.Close();
                command.Dispose();

                if (targetActionStr != "Dead") // Can't revive someone who isn't dead
                {
                    conn1.Close();
                    return;
                }

                if (power <= 0)
                {
                    sql = $"SELECT * FROM Actions WHERE Action = 'Cooldown'";
                    command = new SqlCommand(sql, conn1);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Action = {targetActionInt} WHERE Id = {origin}";
                    command = new SqlCommand(sql, conn1);
                    command.ExecuteNonQuery();
                    command.Dispose();

                    sql = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command = new SqlCommand(sql, conn1);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Hp = 1, Action = {targetActionInt} WHERE Id = {target}";
                    command = new SqlCommand(sql, conn1);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    sql = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command = new SqlCommand(sql, conn1);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                    }
                    dataReader.Close();
                    command.Dispose();

                    sql = $"UPDATE Characters SET Hp = {power}, Action = {targetActionInt} WHERE Id = {target}";
                    command = new SqlCommand(sql, conn1);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = {power} WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(5)) + amount + 1;
                    targetHp = Convert.ToInt32(dataReader.GetValue(3)) - amount;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Charge = {power}, Hp = {targetHp} WHERE Id = {origin}";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
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
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionInt = Convert.ToInt32(dataReader.GetValue(0));
                }
                dataReader.Close();
                command.Dispose();

                sql = $"UPDATE Characters SET Action = {targetActionInt}, Hp = 0 WHERE Hp <= 0";
                command = new SqlCommand(sql, conn1);
                command.ExecuteNonQuery();
                command.Dispose();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void PlayerActions()
        {
            try
            {
                conn2.Open();

                // Boost
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Boost'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if(Convert.ToInt32(dataReader.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Boost(Convert.ToInt32(dataReader.GetValue(0)), Convert.ToInt32(dataReader.GetValue(8)));
                    }
                }
                command.Dispose();

                // Revive
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Revive'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Revive(Convert.ToInt32(dataReader.GetValue(0)), Convert.ToInt32(dataReader.GetValue(8)));
                    }
                }
                command.Dispose();

                // Heal
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Heal'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Heal(Convert.ToInt32(dataReader.GetValue(0)), Convert.ToInt32(dataReader.GetValue(8)));
                    }
                }
                command.Dispose();

                // Defend
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Defend'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Defend(Convert.ToInt32(dataReader.GetValue(0)));
                }
                command.Dispose();

                // Attack
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Attack'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Attack(Convert.ToInt32(dataReader.GetValue(0)), Convert.ToInt32(dataReader.GetValue(8)));
                    }
                }
                command.Dispose();

                // Charge
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Charge'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Charge(Convert.ToInt32(dataReader.GetValue(0)));
                }
                command.Dispose();

                // Overcharge
                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Overcharge'";
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.Write($"{Convert.ToString(dataReader.GetValue(1))} is overcharging.  How many HP are used?  ");
                    int overcharge = Convert.ToInt32(Console.ReadLine());
                    Overcharge(Convert.ToInt32(dataReader.GetValue(0)), overcharge);
                }
                command.Dispose();

                conn2.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void PrintTracker()
        {
            Console.WriteLine(PrintDivider());

            try
            {
                string output = "|";
                int numChars;
                int numSpaces;

                sql = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id";
                conn1.Open();
                command = new SqlCommand(sql, conn1);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Print ID
                    if (Convert.ToInt32(dataReader.GetValue(0)) < 10)
                    {
                        output = $"{output}   {Convert.ToInt32(dataReader.GetValue(0))}|";
                    }
                    else if (Convert.ToInt32(dataReader.GetValue(0)) < 100)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader.GetValue(0))}|";
                    }
                    else if (Convert.ToInt32(dataReader.GetValue(0)) < 1000)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader.GetValue(0))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader.GetValue(0))}|";
                    }

                    // Print name
                    numSpaces = 20;
                    numChars = Convert.ToString(dataReader.GetValue(1)).Length;
                    if (numChars <= 20)
                    {
                        output = $"{output}{Convert.ToString(dataReader.GetValue(1))}";
                    }
                    else
                    {
                        numChars = 20;
                        output = $"{output}{Convert.ToString(dataReader.GetValue(1)).Substring(0, 20)}";
                    }
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print position
                    output = $"{output} {Convert.ToString(dataReader.GetValue(2))} |";

                    // Print HP
                    int hpCounter;
                    for (hpCounter = 1; hpCounter <= Convert.ToInt32(dataReader.GetValue(3)); hpCounter++)
                    {
                        output = $"{output}{fullHeart}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= Convert.ToInt32(dataReader.GetValue(4)); hpCounter++)
                    {
                        output = $"{output}{emptyHeart}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= 20; hpCounter++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";


                    // Print charge
                    if (Convert.ToInt32(dataReader.GetValue(5)) < 10)
                    {
                        output = $"output  {Convert.ToInt32(dataReader.GetValue(5))}|";
                    }
                    else
                    {
                        output = $"output {Convert.ToInt32(dataReader.GetValue(5))}|";
                    }

                    // Print action
                    numSpaces = 10;
                    numChars = Convert.ToString(dataReader.GetValue(10)).Length;
                    output = $"{output}{Convert.ToString(dataReader.GetValue(10))}";
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print target
                    if (Convert.ToInt32(dataReader.GetValue(8)) < 10)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader.GetValue(8))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader.GetValue(8))}|";
                    }

                    Console.WriteLine(output);
                    Console.WriteLine(PrintDivider());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {
            // This method currently used for debugging
        }
    }
}
