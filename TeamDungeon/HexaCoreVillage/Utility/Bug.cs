using HexaCoreVillage.Dungeon;
using System.Text.Json.Serialization;

namespace HexaCoreVillage.Utility
{
    public class Bug
    {
        // 버그 이름
        [JsonPropertyName("bugName")] public string? BugName { get; set; }

        // 버그 설명
        [JsonPropertyName("bugDesc")] public string? BugDesc { get; set; }
        
        // 버그 타입 (플레이어 주는 피해에 추가 영향을 줌)
        [JsonPropertyName("bugSolution")]public Debugging.SolutionTypes SolutionType { get; set; }
        
        // 버그 난이도 (Defence)
        [JsonPropertyName("bugDifficulty")] public int BugDifficulty{ get; set; }
        
        //  버그 수정률  (HP)
        // 버그의 해결도는 0 > 100% 로 표현
        public int BugProgress { get; set; }
        
        // 버그의 복잡도 (Damage) 
        [JsonPropertyName("bugComplexity")] public int BugComplexity { get; set; }

    }
}