using System;
using System.Collections.Generic;

namespace nacs_tracker
{
    class Program
    {
        static List<Character> charList;
        static int nameLength;
        static int healthLength;
        static Character AddCharacter(int id, string name, char position, int hp, int maxHp) {
            Character newChar = new Character();
            newChar.Id = id;
            newChar.Name = name;
            if(name.Length > nameLength) {nameLength = name.Length;}
            newChar.Position = position;
            newChar.Hp = hp;
            newChar.MaxHp = maxHp;
            if(maxHp > healthLength) {healthLength = maxHp;}
            return newChar;
        }
        static Character AddCharacter(int id, string name, int hp, int maxHp) {
            Character newChar = new Character();
            newChar.Id = id;
            newChar.Name = name;
            if(name.Length > nameLength) {nameLength = name.Length;}
            newChar.Hp = hp;
            newChar.MaxHp = maxHp;
            if(maxHp > healthLength) {healthLength = maxHp;}
            return newChar;
        }
        static string PrintDivider() {
            string divider = "+--+--------------------+---+--------------------+---+------+---+";
            return divider;
        }
        static string PrintCharacter(Character printChar) {
            string output = "|";
            int numChars;
            int numSpaces;

            // Print ID
            if(printChar.Id < 10) {
                output = output + " " + printChar.Id + "|";
            } else {
                output = output + printChar.Id + "|";
            }

            // Print name
            numSpaces = 20;
            numChars = printChar.Name.Length;
            if(numChars <= 20) {
            output = output + printChar.Name;
        } else {
            numChars = 20;
            output = output + printChar.Name.Substring(0, 20);
        }
            for(int i = numChars; i < numSpaces; i++) {
                output = output + " ";
            }
            output = output + "|";

            // Print position
            output = output + " " + printChar.Position + " |";

            // Print HP
            int hpCounter;
                for(hpCounter = 1; hpCounter <= printChar.Hp; hpCounter++) {
                    output = output + Character.fullHeart;
                }
                for(/*Keeping the value from last time*/; hpCounter <= printChar.MaxHp; hpCounter++) {
                    output = output + Character.emptyHeart;
                }
                for(/*Keeping the value from last time*/; hpCounter <= 20; hpCounter++) {
                    output = output + " ";
                }
                output = output + "|";
            }

            // Print charge
            if(printChar.Charge < 10) {
                output = output + "  " + printChar.Charge + "|";
            } else {
                output = output + " " + printChar.Charge + "|";
            }

            // Print action
            numSpaces = 6;
            switch (printChar.CharAction)
            {
                case Action.Heal:
                    numChars = 4;
                    output = output + "Heal";
                    break;
                case Action.Wait:
                    numChars = 4;
                    output = output + "Wait";
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
            }
            for(int i = numChars; i < numSpaces; i++) {
                output = output + " ";
            }
            output = output + "|";

            // Print target
            if(printChar.Target < 10) {
                output = output + " " + printChar.Target + "|";
            } else {
                output = output + printChar.Target + "|";
            }

            return output;

        }
        static void PrintTracker() {
            Console.WriteLine(PrintDivider());
            foreach (Character item in charList)
            {
                // TODO:Print character and divider
            }
        }
        static void Main(string[] args)
        {
            nameLength = 0;
            healthLength = 0;
            Console.WriteLine("Hello World!");
        }
    }
}
