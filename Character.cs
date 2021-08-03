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
        private int defense;
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
        public int Defense {
            get {return defense;}
            set {defense = value;}
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
        public void Damage(int amount) {
            if(amount <= defense) { // Defense blocks the entire attack
                defense -= amount;
                return;
            }
                amount -= defense;
                defense = 0;

            if(amount >= hp) { // Attack kills target
                hp = 0;
 //               charAction = Action.Dead;
                return;
            }

            hp -= amount;
            return;
        }
        public void Damage() {
            Damage(1); // Assume one damage
            return;
        }
        public void Heal(int amount) {
            if(hp + amount >= maxHp) { // Heals to maxS
                hp = maxHp;
            } else {
                hp += amount;
            }
            return;
        }
        public void Heal() {
            Heal(1); // Assume one damage
            return;
        }
    }
}
