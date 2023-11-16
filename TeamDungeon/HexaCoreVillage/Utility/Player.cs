namespace HexaCoreVillage.Utility
{
    public class Player
    {
       public  string ID {  get; set; }
        public string NickName { get; set; }
        public Job Job {  get; set; }
        public int Level { get; set; }
       public  int Exp {  get; set; }
       public int TypingSpeed {  get; set; }    //DMG
       public  int C {  get; set; }             //DEF
       public  int BonusDmg { get; set; }
       public  int BonusDef {  get; set; }
       public  int HP {  get; set; }
       public  int CurrentHp {  get; set; }
       public int Mental {  get; set; }
       public int CurrentMental {  get; set; }
       public  int Gold {  get; set; }
       public  List<InventoryItem> Inventory = new List<InventoryItem>();

        public Player()
        {
            ID = "enos";
            NickName = "주니어개발자";
            Job = Job.AI;
            Level = 1;
            Exp = 0;
            TypingSpeed = 10;
            C = 10;
            BonusDmg = 10;
            BonusDef = 10;
            HP = 100;
            CurrentHp = 100;
            Mental = 100;
            CurrentMental = 100;
            Gold = 1000;
        }

        public Player(string _ID, string _NickName, Job _Job)
        {
            ID = _ID;
            NickName = _NickName;
            Job = _Job;
            Level = 1;
            Exp = 0;
            TypingSpeed = 10;
            C = 10;
            BonusDmg = 10;
            BonusDef = 10;
            HP = 100;
            CurrentHp = 100;
            Mental = 100;
            CurrentMental = 100;
            Gold = 1000;
        }
    }   

   public enum Job
    {
        Unity,
        Unreal,
        AI,
        PM,
        QA,
        Neo
    }
}
