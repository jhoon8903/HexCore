using System.Text.Json.Serialization;

namespace HexaCoreVillage.Utility
{
    public class Bug
    {
        // 버그 이름
        [JsonPropertyName("bugName")] public string? BugName { get; set; }
        // 버그 치명도
        public enum BugTypes { Critical, Major, Minor, Trivial, Enhancement }
        
        // 버그 치명도
        [JsonPropertyName("bugType")] public BugTypes BugType { get; set; }
        
        // 버그 난이도 (Defence)
        [JsonPropertyName("bugDifficulty")] public int BugDifficulty{ get; set; }
        
        //  버그 수정률  (HP)
        // 버그의 해결도는 0 > 100% 로 표현
        public int BugProgress { get; set; }
        
        // 버그의 복잡도 (Damage) 
        [JsonPropertyName("bugComplexity")] public int BugComplexity { get; set; }

    }
}