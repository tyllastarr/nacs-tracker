using System;
using System.Collections.Generic;

namespace nacs_tracker
{
    class Program
    {
        static List<Character> charList;
        static Character AddCharacter(int id, string name, char position, int hp) {
            Character newChar = new Character();
            newChar.Id = id;
            newChar.Name = name;
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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
