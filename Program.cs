using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace nacs_tracker
{
    class Program
    {
        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=P:\\PROJECTS\\NACS-TRACKER\\DATABASE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static int power;
        static int targetHp;
        static int targetDef;
        static int targetChg;
        static string targetActionStr;
        static int targetActionInt;
        static TrackerStatus status;
        const char emptyHealthBox = '\u2591';
        const char fullHealthBox = '\u2588';

        static void AddCharacter(string name, char position, int hp)
        {
            string sql = $"INSERT INTO Characters(Name, Position, Hp, MaxHp) VALUES('{name}', '{position}', {hp}, {hp})";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, int hp)
        {
            string sql = $"INSERT INTO Characters(Name, Hp, MaxHp) VALUES('{name}', {hp}, {hp})";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, char position, int hp, int action)
        {
            string sql = $"INSERT INTO Characters(Name, Position, Hp, MaxHp, Action) VALUES('{name}', '{position}', {hp}, {hp}, {action})";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddCharacter(string name, int hp, int action)
        {
            string sql = $"INSERT INTO Characters(Name, Hp, MaxHp, Action) VALUES('{name}', {hp}, {hp}, {action})";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void AddNPC()
        {
            string name;
            int hp;
            char position;
            Console.WriteLine();
            Console.Write("Please enter the name of the NPC: ");
            name = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Please enter the NPC's position: ");
            position = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            Console.Write("Please enter the NPC's max HP: ");
            hp = Convert.ToInt32(Console.ReadLine()); // New characters always start with full HP
            AddCharacter(name, position, hp, 12); // 12 is code for an NPC
        }
        static void AddPC()
        {
            string name;
            int hp;
            Console.WriteLine();
            Console.Write("Please enter the name of the PC: ");
            name = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Please enter the PC's max HP: ");
            hp = Convert.ToInt32(Console.ReadLine()); // New characters always start with full HP
            AddCharacter(name, hp, 8); // 8 is code for np action
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
                string sql = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(0)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT Hp FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(0)) - power;
                    if (targetHp < 0)
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
                string sql = $"SELECT Charge, Defense FROM Characters WHERE id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(0)) + 1;
                    targetDef = Convert.ToInt32(dataReader.GetValue(1)) + power;
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
                string sql = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(0)) + 1;
                }
                dataReader.Close();
                command.Dispose();

                sql = $"SELECT Hp FROM Characters WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetHp = Convert.ToInt32(dataReader.GetValue(0)) + power;
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
                string sql = $"SELECT Actions.Action, Characters.Charge FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(0));
                    targetChg = Convert.ToInt32(dataReader.GetValue(1));
                }
                dataReader.Close();
                command.Dispose();

                if (targetActionStr != "Attack" && targetActionStr != "Defend" && targetActionStr != "Heal") // Other actions cannot be boosted
                {
                    return;
                }

                sql = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = 1 + Convert.ToInt32(dataReader.GetValue(0)) + 1;
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
                string sql = $"SELECT Actions.Action, Characters.Charge FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    targetActionStr = Convert.ToString(dataReader.GetValue(0));
                    power = Convert.ToInt32(dataReader.GetValue(1));
                }
                dataReader.Close();
                command.Dispose();

                if (targetActionStr != "Dead") // Can't revive someone who isn't dead
                {
                    return;
                }

                if (power <= 0)
                {
                    sql = $"SELECT Id FROM Actions WHERE Action = 'Cooldown'";
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

                    sql = $"SELECT Id FROM Actions WHERE Action = 'None'";
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
                }
                else
                {
                    sql = $"SELECT Id FROM Actions WHERE Action = 'None'";
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
                string sql = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(0)) + 1;
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
            try
            {
                string sql = $"SELECT Charge, Hp FROM Characters WHERE Id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    power = Convert.ToInt32(dataReader.GetValue(0)) + amount + 1;
                    targetHp = Convert.ToInt32(dataReader.GetValue(1)) - amount;
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
        static void DamageTarget(int target, int damageAmount)
        {
            try
            {
                string sql = $"SELECT Hp FROM Characters WHERE Id = {target}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    if (Convert.ToInt32(dataReader.GetValue(0)) - damageAmount <= 0)
                    {
                        string sql2 = $"UPDATE Characters SET Hp = 0, Action = 11 WHERE Id = {target}"; // 11 is the code for dead in the action field
                        SqlConnection conn2 = new SqlConnection(connectionString);
                        conn2.Open();
                        SqlCommand command2 = new SqlCommand(sql2, conn2);
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                        conn2.Close();
                    }
                    else
                    {
                        int finalHp = Convert.ToInt32(dataReader.GetValue(0)) - damageAmount;
                        string sql2 = $"UPDATE Characters SET Hp = {finalHp} WHERE Id = {target}";
                        SqlConnection conn2 = new SqlConnection(connectionString);
                        conn2.Open();
                        SqlCommand command2 = new SqlCommand(sql2, conn2);
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                        conn2.Close();
                    }
                }
                dataReader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void DamageTarget(int target)
        {
            DamageTarget(target, 1); // Use 1 as a default
        }
        static void HealTarget(int target, int healAmount)
        {
            DamageTarget(target, healAmount * -1); // Heals can be seen as negative damage
        }
        static void HealTarget(int target)
        {
            DamageTarget(target, -1); // Use 1 healing as a default, so -1 damage
        }
        static void CCTarget(int target)
        {
            try
            {
                string sql = $"UPDATE Characters SET Action = 10 WHERE Id = {target}"; // 10 is the code for CCd
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void UnCCTarget(int target)
        {
            try
            {
                bool ccd = false; // TRUE if character is currently CCd, FALSE otherwise.  Default to FALSE.
                string sql = $"SELECT Action FROM Characters WHERE Id = {target}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    int test = Convert.ToInt32(dataReader.GetValue(0));
                    if (Convert.ToInt32(dataReader.GetValue(0)) != 10)
                    {
                        ccd = false;
                    }
                    else
                    {
                        ccd = true;
                    }
                }
                dataReader.Close();
                command.Dispose();

                if (!ccd) // If not CCd then don't do anything
                {
                    return;
                }

                sql = $"UPDATE Characters SET Action = 8 WHERE Id = {target}";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
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
                string sql = $"SELECT Id FROM Actions WHERE Action = 'Dead'";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
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
        static void PlayerActions()
        {
            try
            {

                // Boost
                string sql = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Boost'";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Boost(Convert.ToInt32(dataReader.GetValue(1)), Convert.ToInt32(dataReader.GetValue(0)));
                    }
                }
                dataReader.Close();
                command.Dispose();

                // Revive
                sql = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Revive'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Revive(Convert.ToInt32(dataReader.GetValue(1)), Convert.ToInt32(dataReader.GetValue(0)));
                    }
                }
                dataReader.Close();
                command.Dispose();

                // Heal
                sql = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Heal'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Heal(Convert.ToInt32(dataReader.GetValue(1)), Convert.ToInt32(dataReader.GetValue(0)));
                    }
                }
                dataReader.Close();
                command.Dispose();

                // Defend
                sql = $"SELECT Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Defend'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Defend(Convert.ToInt32(dataReader.GetValue(0)));
                }
                dataReader.Close();
                command.Dispose();

                // Attack
                sql = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Attack'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Attack(Convert.ToInt32(dataReader.GetValue(1)), Convert.ToInt32(dataReader.GetValue(0)));
                    }
                }
                dataReader.Close();
                command.Dispose();

                // Charge
                sql = $"SELECT Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Charge'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Charge(Convert.ToInt32(dataReader.GetValue(0)));
                }
                dataReader.Close();
                command.Dispose();

                // Overcharge
                sql = $"SELECT Characters.Name, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Overcharge'";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.Write($"{Convert.ToString(dataReader.GetValue(0))} is overcharging.  How many HP are used?  ");
                    int overcharge = Convert.ToInt32(Console.ReadLine());
                    Overcharge(Convert.ToInt32(dataReader.GetValue(1)), overcharge);
                }
                dataReader.Close();
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
            Console.WriteLine("| ID |        Name        |Pos|       Health       |Chg|  Action  |Tar |");
            Console.WriteLine(PrintDivider());

            try
            {
                string output = "|";
                int numChars;
                int numSpaces;

                string sql = $"SELECT Characters.Id, Characters.Name, Characters.Position, Characters.Hp, Characters.MaxHp, Characters.Charge, Characters.Target, Actions.Action FROM Characters JOIN Actions ON Characters.Action = Actions.Id";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    output = "|";
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
                    if (Convert.ToString(dataReader.GetValue(2)).Equals(""))
                    {
                        output = $"{output}   |";
                    }
                    else
                    {
                        output = $"{output} {Convert.ToString(dataReader.GetValue(2))} |";
                    }

                    // Print HP
                    int hpCounter;
                    for (hpCounter = 1; hpCounter <= Convert.ToInt32(dataReader.GetValue(3)); hpCounter++)
                    {
                        output = $"{output}{fullHealthBox}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= Convert.ToInt32(dataReader.GetValue(4)); hpCounter++)
                    {
                        output = $"{output}{emptyHealthBox}";
                    }
                    for (/*Keeping the value from last time*/; hpCounter <= 20; hpCounter++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";


                    // Print charge
                    if (Convert.ToInt32(dataReader.GetValue(5)) < 10)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader.GetValue(5))}|";
                    }
                    else
                    {
                        output = $"{output} {Convert.ToInt32(dataReader.GetValue(5))}|";
                    }

                    // Print action
                    numSpaces = 10;
                    numChars = Convert.ToString(dataReader.GetValue(7)).Length;
                    output = $"{output}{Convert.ToString(dataReader.GetValue(7))}";
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print target
                    if (dataReader.IsDBNull(6))
                    {
                        output = $"{output}    |";
                    }
                    else if (Convert.ToInt32(dataReader.GetValue(6)) < 10)
                    {
                        output = $"{output}   {Convert.ToInt32(dataReader.GetValue(6))}|";
                    }
                    else if (Convert.ToInt32(dataReader.GetValue(6)) < 100)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader.GetValue(6))}|";
                    }
                    else if (Convert.ToInt32(dataReader.GetValue(6)) < 1000)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader.GetValue(6))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader.GetValue(6))}|";
                    }

                    Console.WriteLine(output);
                    Console.WriteLine(PrintDivider());

                }
                dataReader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void SetTarget()
        {
            Console.WriteLine();
            Console.Write("Type the ID number of the character you want to set the target of: ");
            int origin = Convert.ToInt32(Console.ReadLine());
            Console.Write("Type the ID number of the target: ");
            int target = Convert.ToInt32(Console.ReadLine());
            try
            {
                string sql = $"UPDATE Characters SET Target = {target} WHERE Id = {origin}";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static int AskForActions(string characterName)
        {
            int actionNumber;
            List<int> actionIds = new List<int>();

            try
            {
                Console.WriteLine();
                Console.WriteLine("CHOOSE AN ACTION");
                Console.WriteLine("────────────────");
                string sql = "SELECT Id, Action FROM Actions WHERE IsPlayerAction = 1";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    actionIds.Add(Convert.ToInt32(dataReader.GetValue(0)));
                    Console.WriteLine($"{Convert.ToInt32(dataReader.GetValue(0))}) {Convert.ToString(dataReader.GetValue(1))}");
                }
                dataReader.Close();
                command.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Write($"Type the number of the action you want {characterName} to perform: ");
            actionNumber = Convert.ToInt32(Console.ReadLine());
            while (!actionIds.Contains(actionNumber))
            {
                Console.WriteLine("Error: Not a valid action.");
                Console.Write($"Type the number of the action you want {characterName} to perform: ");
                actionNumber = Convert.ToInt32(Console.ReadLine());
            }
            return actionNumber;
        }
        static void ChangePlayerAction()
        {
            List<int> playerIds = new List<int>();
            int characterNumber;
            string characterName = "";
            int actionNumber;
            try
            {
                Console.WriteLine();
                Console.WriteLine("CHOOSE A CHARACTER");
                Console.WriteLine("──────────────────");
                string sql = "SELECT Id, Name FROM Characters WHERE Action < 11";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    playerIds.Add(Convert.ToInt32(dataReader.GetValue(0)));
                    Console.WriteLine($"{Convert.ToInt32(dataReader.GetValue(0))}) {Convert.ToString(dataReader.GetValue(1))}");
                }
                dataReader.Close();
                command.Dispose();

                Console.Write("Type the number of the character you want to perform an action: ");
                characterNumber = Convert.ToInt32(Console.ReadLine());
                while (!playerIds.Contains(characterNumber))
                {
                    Console.WriteLine("Error: Not a valid character.");
                    Console.Write("Type the number of the character you want to perform an action: ");
                    characterNumber = Convert.ToInt32(Console.ReadLine());
                }

                sql = $"SELECT Name FROM Characters WHERE Id = {characterNumber}";
                command = new SqlCommand(sql, conn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    characterName = Convert.ToString(dataReader.GetValue(0));
                }
                dataReader.Close();
                command.Dispose();

                actionNumber = AskForActions(characterName);

                sql = $"UPDATE Characters SET Action = {actionNumber} WHERE Id = {characterNumber}";
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
        static TrackerStatus NPCMenu()
        {
            int target;
            int damage;
            char choice;
            Console.WriteLine();
            Console.WriteLine("NPC ACTIONS");
            Console.WriteLine("───────────");
            Console.WriteLine("C) CC target");
            Console.WriteLine("U) Un-CC target");
            Console.WriteLine("D) Damage target");
            Console.WriteLine("H) Heal target");
            Console.WriteLine("N) Add NPC");
            Console.WriteLine("P) Add PC");
            Console.WriteLine("R) Revert to player actions");
            Console.WriteLine("Q) Quit tracker");
            Console.WriteLine();
            Console.Write("Press the letter key for your choice: ");
            choice = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            switch (choice)
            {
                case 'C':
                    Console.Write("Please enter the ID number of the character that you want to CC: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    CCTarget(target);
                    return TrackerStatus.NPC;
                case 'U':
                    Console.Write("Please enter the ID number of the character that you want to un-CC: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    UnCCTarget(target);
                    return TrackerStatus.NPC;
                case 'D':
                    Console.Write("Please enter the ID number of the character that you want to damage: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    Console.Write("How much damage do you want to do? ");
                    damage = Convert.ToInt32(Console.ReadLine());
                    DamageTarget(target, damage);
                    return TrackerStatus.NPC;
                case 'H':
                    Console.Write("Please enter the ID number of the character that you want to heal: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    Console.Write("How much damage do you want to heal? ");
                    damage = Convert.ToInt32(Console.ReadLine());
                    HealTarget(target, damage);
                    return TrackerStatus.NPC;
                case 'N':
                    AddNPC();
                    return TrackerStatus.NPC;
                case 'P':
                    AddPC();
                    return TrackerStatus.NPC;
                case 'R':
                    return TrackerStatus.PC;
                case 'Q':
                    return TrackerStatus.Quit;
                default:
                    Console.WriteLine("Error: unknown action.");
                    Console.WriteLine();
                    return NPCMenu(); // Run through the menu method again if input is invalid
            }
        }
        static TrackerStatus PCMenu() // TRUE will mean another PC, and FALSE will mean process turn
        {

            char choice;
            Console.WriteLine();
            Console.WriteLine("PC ACTIONS");
            Console.WriteLine("──────────");
            Console.WriteLine("A) Set action");
            Console.WriteLine("T) Set target");
            Console.WriteLine("P) Add PC");
            Console.WriteLine("N) Add NPC");
            Console.WriteLine("R) Run player actions");
            Console.WriteLine("Q) Quit tracker");
            Console.WriteLine();
            Console.Write("Press the letter key for your choice: ");
            choice = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            switch (choice)
            {
                case 'A':
                    ChangePlayerAction();
                    return TrackerStatus.PC;
                case 'T':
                    SetTarget();
                    return TrackerStatus.PC;
                case 'P':
                    AddPC();
                    return TrackerStatus.PC;
                case 'N':
                    AddNPC();
                    return TrackerStatus.PC;
                case 'R':
                    PlayerActions();
                    return TrackerStatus.NPC;
                case 'Q':
                    return TrackerStatus.Quit;
                default:
                    Console.WriteLine("Error: unknown action.");
                    Console.WriteLine();
                    return PCMenu(); // Run through the menu method again if input is invalid
            }
        }
        static void Main(string[] args)
        {
            status = TrackerStatus.PC;
            do
            {
                PrintTracker();
                switch (status)
                {
                    case TrackerStatus.PC:
                        status = PCMenu();
                        break;
                    case TrackerStatus.NPC:
                        status = NPCMenu();
                        break;
                }
            } while (status != TrackerStatus.Quit);
        }
    }
}
