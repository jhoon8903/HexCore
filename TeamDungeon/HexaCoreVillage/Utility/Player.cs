namespace HexaCoreVillage.Utility
{
    internal class Player
    {
       public  string ID {  get; set; }
        public string NickName { get; set; }
        public Job Job {  get; set; }
        public int Level { get; set; }
       public  int Exp {  get; set; }
       public int TypingSpeed {  get; set; }
       public  int C {  get; set; }
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
            Mental = 10;
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
