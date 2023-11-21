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
            Gold = 1000;
            switch(_Job){
                case Job.Unity:                 //무난
                    TypingSpeed = 10;
                    C = 10;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 100;
                    CurrentHp = 100;
                    Mental = 100;
                    CurrentMental = 100;
                    break;

                case Job.Unreal:                //공격력 높고 방어력 낮음
                    TypingSpeed = 12;
                    C = 8;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 100;
                    CurrentHp = 100;
                    Mental = 100;
                    CurrentMental = 100;
                    break;

                case Job.AI:                    //공방 높고 체력이 낮음
                    TypingSpeed = 11;
                    C = 11;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 80;
                    CurrentHp = 80;
                    Mental = 100;
                    CurrentMental = 100;
                    break;

                case Job.PM:                    //멘탈 좋음 공방 낮음
                    TypingSpeed = 9;
                    C = 9;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 100;
                    CurrentHp = 100;
                    Mental = 130;
                    CurrentMental = 130;
                    break;

                case Job.QA:                    //체력 좋고 공방 낮음
                    TypingSpeed = 9;
                    C = 9;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 130;
                    CurrentHp = 130;
                    Mental = 100;
                    CurrentMental = 100;
                    break;

                case Job.Neo:                   //히든이니까 사기
                    TypingSpeed = 20;
                    C = 20;
                    BonusDmg = 10;
                    BonusDef = 10;
                    HP = 200;
                    CurrentHp = 200;
                    Mental = 200;
                    CurrentMental = 200;
                    break;
            }
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
