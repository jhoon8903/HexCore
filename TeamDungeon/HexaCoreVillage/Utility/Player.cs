namespace HexaCoreVillage.Utility
{
    internal class Player
    {
        string ID {  get; set; }
        string NickName { get; set; }
        Job job {  get; set; }
        int Level { get; set; }
        int Exp {  get; set; }
        int TypingSpeed {  get; set; }
        int C {  get; set; }
        int BonusDmg { get; set; }
        int BounsDef {  get; set; }
        int HP {  get; set; }
        int CurrentHp {  get; set; }
        int Mental {  get; set; }
        int CurrentMental {  get; set; }
        int Gold {  get; set; }
        List<InvenItem> Inventory = new List<InvenItem>();

        public Player()
        {
            ID = "enos";
            NickName = "주니어개발자";
            job = Job.AI;
            Level = 1;
            Exp = 0;
            TypingSpeed = 10;
            C = 10;
            BonusDmg = 10;
            BounsDef = 10;
            HP = 100;
            CurrentHp = 100;
            Mental = 10;
            Gold = 1000;
        }
    }   

    enum Job
    {
        Unity,
        Unreal,
        AI,
        PM,
        QA,
        Neo
    }
}
