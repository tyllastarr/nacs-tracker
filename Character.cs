using System;

namespace nacs_tracker
{
    class Character
    {
        private int id;
        private string name;
        private char position;
        private int hp;
        private int maxHp;
        private int charge;
        private Action charAction;
        private int target;

        public const char emptyHeart = '♡';
        public const char fullHeart = '♥';

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public char Position
        {
            get { return position; }
            set { position = value; }
        }
        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        public int MaxHp {
            get {return maxHp;}
            set {maxHp = value;}
        }
        public int Charge
        {
            get { return charge; }
            set { charge = value; }
        }
        public Action CharAction
        {
            get { return charAction; }
            set { charAction = value; }
        }
        public int Target
        {
            get { return target; }
            set { target = value; }
        }
    }
}
