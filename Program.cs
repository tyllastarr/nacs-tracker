using System;
using System.Collections.Generic;

namespace nacs_tracker
{
    class Program
    {
        static List<Character> charList;
        static int length;
        static Character AddCharacter(int id, string name, char position, int hp) {
            Character newChar = new Character();
            newChar.Id = id;
            newChar.Name = name;
            if(name.Length > length) {length = name.Length;}
            newChar.Position = position;
            newChar.Hp = hp;
            return newChar;
        }
        static Character AddCharacter(int id, string name, int hp) {
            Character newChar = new Character();
            newChar.Id = id;
            newChar.Name = name;
            newChar.Hp = hp;
            return newChar;
        }
        static string PrintDivider() {
            string divider = "+--+";
            for(int i = 0; i < length; i++) {divider = divider + "-";}
            divider = divider + "+---+--+---+------+---+";
            return divider;
        }
        static string PrintCharacter(Character printChar) {
            string output = "+";
            int numChars;
            int numSpaces;

            // Print ID
            if(printChar.Id < 10) {
                output = output + " " + printChar.Id + "+";
            } else {
                output = output + printChar.Id + "+";
            }

            // Print name
            numSpaces = length;
            numChars = printChar.Name.Length;
            output = output + printChar.Name;
            for(int i = numChars; i < numSpaces; i++) {
                output = output + " ";
            }
            output = output + "+";

            // Print position
            output = output + " " + printChar.Position + " +";

            // Print HP
            if(printChar.Hp < 10) {
                output = output + " " + printChar.Hp + "+";
            } else {
                output = output + printChar.Hp + "+";
            }

            // Print charge
            if(printChar.Charge < 10) {
                output = output + "  " + printChar.Charge + "+";
            } else {
                output = output + " " + printChar.Charge + "+";
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
            output = output + "+";

            // Print target
            if(printChar.Target < 10) {
                output = output + " " + printChar.Target + "+";
            } else {
                output = output + printChar.Target + "+";
            }

            return output;

        }
        static void PrintTracker() {
            Console.WriteLine(PrintDivider());
            foreach (Character item in charList)
            {
                // Print character and divider
            }
        }
        static void Main(string[] args)
        {
            length = 0;
            Console.WriteLine("Hello World!");
        }
    }
}
