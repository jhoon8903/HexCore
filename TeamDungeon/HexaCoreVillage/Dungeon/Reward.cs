
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Dungeon;

public class Reward : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.REWARD;

    public override void Start()
    {
        Clear();
        SetCursorPosition(85,20);
        WriteLine("여긴 리워드다!!");
        SetCursorPosition(85, 22);
        Write($"디버그 결과는 : {Data.BattleSuccess} 입니다!");
        ConsoleSizeUtility.RedrawBorder();
    }

    public override void Update()
    {
    }

    // Battle 성공시 Data Class의 "bool BattleSuccess" 로  bool 값 넣어두도록 하겠습니다,
    // 해당 bool 값으로 성공 실패 정하시면 될 것 같아요 
    // Battle에서 끝나면 Start() 호출해서 실행하도록 하겠습니다.
}