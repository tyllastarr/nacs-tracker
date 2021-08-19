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


        static void AddCharacter(string name, char position, int hp)
        {
            sql1 = $"INSERT INTO Characters(Name, Position, Hp, MaxHp) VALUES('{name}', '{position}', {hp}, {hp})";
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
        static void AddCharacter(string name, int hp)
        {
            sql1 = $"INSERT INTO Characters(Name, Hp, MaxHp) VALUES('{name}', {hp}, {hp})";
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
        static void AddCharacter(string name, char position, int hp, int action)
        {
            sql1 = $"INSERT INTO Characters(Name, Position, Hp, MaxHp, Action) VALUES('{name}', '{position}', {hp}, {hp}, {action})";
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
        static void AddCharacter(string name, int hp, int action)
        {
            sql1 = $"INSERT INTO Characters(Name, Hp, MaxHp, Action) VALUES('{name}', {hp}, {hp}, {action})";
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
                sql1 = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(0)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"SELECT Hp FROM Characters WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetHp = Convert.ToInt32(dataReader1.GetValue(0)) - power;
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
                sql1 = $"SELECT Charge, Defense FROM Characters WHERE id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(0)) + 1;
                    targetDef = Convert.ToInt32(dataReader1.GetValue(1)) + power;
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
                sql1 = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(0)) + 1;
                }
                dataReader1.Close();
                command1.Dispose();

                sql1 = $"SELECT Hp FROM Characters WHERE Id = {target}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetHp = Convert.ToInt32(dataReader1.GetValue(0)) + power;
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
                sql1 = $"SELECT Actions.Action, Characters.Charge FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetActionStr = Convert.ToString(dataReader1.GetValue(0));
                    targetChg = Convert.ToInt32(dataReader1.GetValue(1));
                }
                dataReader1.Close();
                command1.Dispose();

                if (targetActionStr != "Attack" && targetActionStr != "Defend" && targetActionStr != "Heal") // Other actions cannot be boosted
                {
                    conn1.Close();
                    return;
                }

                sql1 = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = 1 + Convert.ToInt32(dataReader1.GetValue(0)) + 1;
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
                sql1 = $"SELECT Actions.Action, Characters.Charge FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Characters.Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    targetActionStr = Convert.ToString(dataReader1.GetValue(0));
                    power = Convert.ToInt32(dataReader1.GetValue(1));
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
                    sql1 = $"SELECT Id FROM Actions WHERE Action = 'Cooldown'";
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

                    sql1 = $"SELECT Id FROM Actions WHERE Action = 'None'";
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
                    sql1 = $"SELECT Id FROM Actions WHERE Action = 'None'";
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
                sql1 = $"SELECT Charge FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(0)) + 1;
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
                sql1 = $"SELECT Charge, Hp FROM Characters WHERE Id = {origin}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    power = Convert.ToInt32(dataReader1.GetValue(0)) + amount + 1;
                    targetHp = Convert.ToInt32(dataReader1.GetValue(1)) - amount;
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
        static void DamageTarget(int target, int damageAmount)
        {
            try
            {
                sql1 = $"SELECT Hp FROM Characters WHERE Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    if (Convert.ToInt32(dataReader1.GetValue(0)) - damageAmount <= 0)
                    {
                        sql2 = $"UPDATE Characters SET Hp = 0, Action = 11 WHERE Id = {target}"; // 11 is the code for dead in the action field
                        conn2.Open();
                        command2 = new SqlCommand(sql2, conn2);
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                        conn2.Close();
                    }
                    else
                    {
                        int finalHp = Convert.ToInt32(dataReader1.GetValue(0)) - damageAmount;
                        sql2 = $"UPDATE Characters SET Hp = {finalHp} WHERE Id = {target}";
                        conn2.Open();
                        command2 = new SqlCommand(sql2, conn2);
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                        conn2.Close();
                    }
                }
                dataReader1.Close();
                command1.Dispose();
                conn1.Close();
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
                sql1 = $"UPDATE Characters SET Action = 10 WHERE Id = {target}"; // 10 is the code for CCd
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
        static void UnCCTarget(int target)
        {
            try
            {
                bool ccd = false; // TRUE if character is currently CCd, FALSE otherwise.  Default to FALSE.
                sql1 = $"SELECT Action FROM Characters WHERE Id = {target}";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    if (Convert.ToInt32(dataReader1.GetValue(0)) != 10)
                    {
                        ccd = false;
                    }
                    else
                    {
                        ccd = true;
                    }
                }
                dataReader1.Close();
                command1.Dispose();
                conn1.Close();

                if (!ccd) // If not CCd then don't do anything
                {
                    return;
                }

                sql1 = $"UPDATE Characters SET Action = 10 WHERE Id = {target}";
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
        static void CatchDead()
        {
            try
            {
                sql1 = $"SELECT Id FROM Actions WHERE Action = 'Dead'";
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
                sql2 = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Boost'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Boost(Convert.ToInt32(dataReader2.GetValue(1)), Convert.ToInt32(dataReader2.GetValue(0)));
                    }
                }
                command2.Dispose();

                // Revive
                sql2 = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Revive'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Revive(Convert.ToInt32(dataReader2.GetValue(1)), Convert.ToInt32(dataReader2.GetValue(0)));
                    }
                }
                command2.Dispose();

                // Heal
                sql2 = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Heal'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Heal(Convert.ToInt32(dataReader2.GetValue(1)), Convert.ToInt32(dataReader2.GetValue(0)));
                    }
                }
                command2.Dispose();

                // Defend
                sql2 = $"SELECT Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Defend'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Defend(Convert.ToInt32(dataReader2.GetValue(0)));
                }
                command2.Dispose();

                // Attack
                sql2 = $"SELECT Characters.Target, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Attack'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    // Check for no target
                    if (Convert.ToInt32(dataReader2.GetValue(0)) != 0) // 0 refers to no target
                    {
                        Attack(Convert.ToInt32(dataReader2.GetValue(1)), Convert.ToInt32(dataReader2.GetValue(0)));
                    }
                }
                command2.Dispose();

                // Charge
                sql2 = $"SELECT Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Charge'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Charge(Convert.ToInt32(dataReader2.GetValue(0)));
                }
                command2.Dispose();

                // Overcharge
                sql2 = $"SELECT Characters.Name, Characters.Id FROM Characters JOIN Actions ON Characters.Action = Actions.Id WHERE Actions.Action = 'Overcharge'";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Console.Write($"{Convert.ToString(dataReader2.GetValue(0))} is overcharging.  How many HP are used?  ");
                    int overcharge = Convert.ToInt32(Console.ReadLine());
                    Overcharge(Convert.ToInt32(dataReader2.GetValue(1)), overcharge);
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

                sql1 = $"SELECT Characters.Id, Characters.Name, Characters.Position, Characters.Hp, Characters.MaxHp, Characters.Charge, Characters.Target, Actions.Action FROM Characters JOIN Actions ON Characters.Action = Actions.Id";
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
                    numChars = Convert.ToString(dataReader1.GetValue(7)).Length;
                    output = $"{output}{Convert.ToString(dataReader1.GetValue(7))}";
                    for (int i = numChars; i < numSpaces; i++)
                    {
                        output = $"{output} ";
                    }
                    output = $"{output}|";

                    // Print target
                    if (dataReader1.IsDBNull(8))
                    {
                        output = $"{output}    |";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(6)) < 10)
                    {
                        output = $"{output}   {Convert.ToInt32(dataReader1.GetValue(6))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(6)) < 100)
                    {
                        output = $"{output}  {Convert.ToInt32(dataReader1.GetValue(6))}|";
                    }
                    else if (Convert.ToInt32(dataReader1.GetValue(6)) < 1000)
                    {
                        output = $"{output} {Convert.ToInt32(dataReader1.GetValue(6))}|";
                    }
                    else
                    {
                        output = $"{output}{Convert.ToInt32(dataReader1.GetValue(6))}|";
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
        static void SetTarget()
        {
            Console.WriteLine();
            Console.Write("Type the ID number of the character you want to set the target of: ");
            int origin = Convert.ToInt32(Console.ReadLine());
            Console.Write("Type the ID number of the target: ");
            int target = Convert.ToInt32(Console.ReadLine());
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
        static int AskForActions(string characterName)
        {
            int actionNumber;
            List<int> actionIds = new List<int>();

            try
            {
                Console.WriteLine();
                Console.WriteLine("CHOOSE AN ACTION");
                Console.WriteLine("━━━━━━━━━━━━━━━━");
                sql1 = "SELECT Id, Action FROM Actions WHERE IsPlayerAction = 1";
                conn1.Open();
                command1 = new SqlCommand(sql1, conn1);
                dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    actionIds.Add(Convert.ToInt32(dataReader1.GetValue(0)));
                    Console.WriteLine($"{Convert.ToInt32(dataReader1.GetValue(0))}) {Convert.ToString(dataReader1.GetValue(1))}");
                }
                dataReader1.Close();
                command1.Dispose();
                conn1.Close();
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
                Console.WriteLine("━━━━━━━━━━━━━━━━━━");
                sql2 = "SELECT Id, Name FROM Characters WHERE Action < 11";
                conn2.Open();
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    playerIds.Add(Convert.ToInt32(dataReader2.GetValue(0)));
                    Console.WriteLine($"{Convert.ToInt32(dataReader2.GetValue(0))}) {Convert.ToString(dataReader2.GetValue(1))}");
                }
                dataReader2.Close();
                command2.Dispose();

                Console.Write("Type the number of the character you want to perform an action: ");
                characterNumber = Convert.ToInt32(Console.ReadLine());
                while (!playerIds.Contains(characterNumber))
                {
                    Console.WriteLine("Error: Not a valid character.");
                    Console.Write("Type the number of the character you want to perform an action: ");
                    characterNumber = Convert.ToInt32(Console.ReadLine());
                }

                sql2 = $"SELECT Name FROM Characters WHERE Id = {characterNumber}";
                command2 = new SqlCommand(sql2, conn2);
                dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    characterName = Convert.ToString(dataReader2.GetValue(0));
                }
                dataReader2.Close();
                command2.Dispose();

                actionNumber = AskForActions(characterName);

                sql2 = $"UPDATE Characters SET Action = {actionNumber} WHERE Id = {characterNumber}";
                command2 = new SqlCommand(sql2, conn2);
                command2.ExecuteNonQuery();
                command2.Dispose();
                conn2.Close();
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
            Console.WriteLine("NPC ACTIONS");
            Console.WriteLine("━━━━━━━━━━━");
            Console.WriteLine("C) CC target");
            Console.WriteLine("D) Damage target");
            Console.WriteLine("N) Add NPC");
            Console.WriteLine("P) Add PC");
            Console.WriteLine("R) Revert to player actions");
            Console.WriteLine("Q) Quit tracker");
            Console.WriteLine();
            Console.Write("Press the letter key for your choice: ");
            choice = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            switch (choice) {
                case 'C':
                    Console.Write("Please enter the ID number of the character that you want to CC: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    CCTarget(target);
                    return TrackerStatus.NPC;
                case 'D':
                    Console.Write("Please enter the ID number of the character that you want to damage: ");
                    target = Convert.ToInt32(Console.ReadLine());
                    Console.Write("How much damage do you want to do? ");
                    damage = Convert.ToInt32(Console.ReadLine());
                    DamageTarget(target, damage);
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

            int target;
            char choice;
            Console.WriteLine("PC ACTIONS");
            Console.WriteLine("━━━━━━━━━━");
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

            switch(choice)
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
            PrintTracker();
        }
    }
}
