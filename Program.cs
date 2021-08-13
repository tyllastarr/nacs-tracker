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
        static SqlCommand command1;
        static SqlCommand command2;
        static SqlDataReader dataReader1;
        static SqlDataReader dataReader2;
        static string sql1 = null;
        static string sql2 = null;
        static int power;
        static int targetHp;
        static int targetDef;
        static int targetChg;
        static string targetActionStr;
        static int targetActionInt;
        static int count;
        const char emptyHealthBox = '\u2591';
        const char fullHealthBox = '\u2588';


        static void AddCharacter(string name, char position, int hp, int maxHp)
        {
            sql1 = $"INSERT INTO Characters(Name, Position, Hp, MaxHp) VALUES('{name}', '{position}', {hp}, {maxHp})";
            try
            {
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, int hp, int maxHp)
        {
            sql1 = $"INSERT INTO Characters(Name, Hp, MaxHp) VALUES('{name}', {hp}, {maxHp})";
            try
            {
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(5)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"SELECT * FROM Characters WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetHp = Convert.ToInt32(dataReader1.GetValue(3)) - power;
                    if (targetHp < 0)
                    {
                        targetHp = 0;
                    }
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters WHERE id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(5)) + 1;
                    targetDef = Convert.ToInt32(dataReader1.GetValue(6)) + power;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Defense = {targetDef}, Charge = 0 WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(5)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"SELECT * FROM Characters WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetHp = Convert.ToInt32(dataReader1.GetValue(3)) + power;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Hp = {targetHp} WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetActionStr = Convert.ToString(dataReader1.GetValue(10));
                    targetChg = Convert.ToInt32(dataReader1.GetValue(5));
                }
                dataReader1.Close();
                command1.Dispose();

                if (targetActionStr != "Attack" && targetActionStr != "Defend" && targetActionStr != "Heal") // Other actions cannot be boosted
                {
                    conn1.Close();
                    return;
                }

                sql1 = $"SELECT * FROM Characters WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = 1 + Convert.ToInt32(dataReader1.GetValue(5)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                targetChg += power;

                sql1 = $"UPDATE Characters SET Charge = {targetChg} WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Charge = 0 WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetActionStr = Convert.ToString(dataReader1.GetValue(10));
                    power = Convert.ToInt32(dataReader1.GetValue(5));
                }
                dataReader1.Close();
                command1.Dispose();

                if (targetActionStr != "Dead") // Can't revive someone who isn't dead
                {
                    conn1.Close();
                    return;
                }

                if (power <= 0)
                {
                    sql1 = $"SELECT * FROM Actions WHERE Action = 'Cooldown'";
                    command1 = new SqlCommand(sql1, conn1);
                    dataReader1 = command1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader1.GetValue(0));
                    }
                    dataReader1.Close();
                    command1.Dispose();

                    sql1 = $"UPDATE Characters SET Action = {targetActionInt} WHERE Id = {origin}";
                    command1 = new SqlCommand(sql1, conn1);
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    sql1 = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command1 = new SqlCommand(sql1, conn1);
                    dataReader1 = command1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader1.GetValue(0));
                    }
                    dataReader1.Close();
                    command1.Dispose();

                    sql1 = $"UPDATE Characters SET Hp = 1, Action = {targetActionInt} WHERE Id = {target}";
                    command1 = new SqlCommand(sql1, conn1);
                    command1.ExecuteNonQuery();
                    command1.Dispose();
                }
                else
                {
                    sql1 = $"SELECT * FROM Actions WHERE Action = 'None'";
                    command1 = new SqlCommand(sql1, conn1);
                    dataReader1 = command1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        targetActionInt = Convert.ToInt32(dataReader1.GetValue(0));
                    }
                    dataReader1.Close();
                    command1.Dispose();

                    sql1 = $"UPDATE Characters SET Hp = {power}, Action = {targetActionInt} WHERE Id = {target}";
                    command1 = new SqlCommand(sql1, conn1);
                    command1.ExecuteNonQuery();
                    command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(5)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Charge = {power} WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(5)) + amount + 1;
                    targetHp = Convert.ToInt32(dataReader1.GetValue(3)) - amount;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Charge = {power}, Hp = {targetHp} WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql1 = $"SELECT * FROM Actions WHERE Action = 'Dead'";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetActionInt = Convert.ToInt32(dataReader1.GetValue(0));
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"UPDATE Characters SET Action = {targetActionInt}, Hp = 0 WHERE Hp <= 0";
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
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
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Boost'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if(Convert.ToInt32(dataReader2.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Boost(Convert.ToInt32(dataReader2.GetValue(0)), Convert.ToInt32(dataReader2.GetValue(8)));
                    }
                }
                command2.Dispose();

                // Revive
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Revive'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Revive(Convert.ToInt32(dataReader2.GetValue(0)), Convert.ToInt32(dataReader2.GetValue(8)));
                    }
                }
                command2.Dispose();

                // Heal
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Heal'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Heal(Convert.ToInt32(dataReader2.GetValue(0)), Convert.ToInt32(dataReader2.GetValue(8)));
                    }
                }
                command2.Dispose();

                // Defend
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Defend'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Defend(Convert.ToInt32(dataReader2.GetValue(0)));
                }
                command2.Dispose();

                // Attack
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Attack'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(8)) != 0) // 0 refers to no target
                    {
                        Attack(Convert.ToInt32(dataReader2.GetValue(0)), Convert.ToInt32(dataReader2.GetValue(8)));
                    }
                }
                command2.Dispose();

                // Charge
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Charge'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Charge(Convert.ToInt32(dataReader2.GetValue(0)));
                }
                command2.Dispose();

                // Overcharge
                sql2 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Overcharge'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Console.Write($"{Convert.ToString(dataReader2.GetValue(1))} is overcharging.  How many HP are used?  ");
                    int overcharge = Convert.ToInt32(Console.ReadLine());
                    Overcharge(Convert.ToInt32(dataReader2.GetValue(0)), overcharge);
                }
                command2.Dispose();

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
            Console.WriteLine("| ID |        Name        |Pos|       Health       |Chg|  Action  |Tar |");
            Console.WriteLine(PrintDivider());

            try
            {
                string output = "|";
                int numChars;
                int numSpaces;

                sql1 = $"SELECT * FROM Characters JOIN Actions ON Characters.Action = Actions.Id";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    output = "|";
                    // Print ID
                    if (Convert.ToInt32(dataReader1.GetValue(0)) < 10)
                    {
                        output = $"{output}   {Convert.ToInt32(dataReader1.GetValue(0))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(0)) < 100)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader1.GetValue(0))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(0)) < 1000)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader1.GetValue(0))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader1.GetValue(0))}|";
                    }

                    // Print name
                    numSpaces = 20;
                    numChars = Convert.ToString(dataReader1.GetValue(1)).Length;
                    if (numChars <= 20)
                    {
                        output = $"{output}{Convert.ToString(dataReader1.GetValue(1))}";
                    }
                    else
                    {
                        numChars = 20;
                        output = $"{output}{Convert.ToString(dataReader1.GetValue(1)).Substring(0, 20)}";
                    }
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print position
                    output = $"{output} {Convert.ToString(dataReader1.GetValue(2))} |";

                    // Print HP
                    int hpCounter;
                    for (hpCounter = 1; hpCounter <= Convert.ToInt32(dataReader1.GetValue(3)); hpCounter++)
                    {
                        output = $"{output}{fullHealthBox}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= Convert.ToInt32(dataReader1.GetValue(4)); hpCounter++)
                    {
                        output = $"{output}{emptyHealthBox}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= 20; hpCounter++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";


                    // Print charge
                    if (Convert.ToInt32(dataReader1.GetValue(5)) < 10)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader1.GetValue(5))}|";
                    }
                    else
                    {
                        output = $"{output} {Convert.ToInt32(dataReader1.GetValue(5))}|";
                    }

                    // Print action
                    numSpaces = 10;
                    numChars = Convert.ToString(dataReader1.GetValue(10)).Length;
                    output = $"{output}{Convert.ToString(dataReader1.GetValue(10))}";
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print target
                    if(dataReader1.IsDBNull(8))
                    {
                        output = $"{output}    |";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(8)) < 10)
                    {
                        output = $"{output}   {Convert.ToInt32(dataReader1.GetValue(8))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(8)) < 100)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader1.GetValue(8))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(8)) < 1000)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader1.GetValue(8))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader1.GetValue(8))}|";
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
        static void SetTarget(int origin, int target)
        {
            try
            {
                sql1 = $"UPDATE Characters SET Target = {target} WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                command1.ExecuteNonQuery();
                command1.Dispose();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {
            PrintTracker();
        }
    }
}
