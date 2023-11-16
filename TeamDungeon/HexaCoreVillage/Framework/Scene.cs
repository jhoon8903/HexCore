
namespace HexaCoreVillage.Framework;


/// <summary>
/// # Scene에 베이스가 되는 추상 클래스
/// </summary>
public abstract class Scene
{
    public abstract SCENE_NAME SceneName { get; }

    public abstract void Start();
    public abstract void Update();
}

// 추 후 Utility 같은 정적 클래스로 뺄 예정
public enum SCENE_NAME
{
    TITLE,
    LOGIN,
    LOBBY,
    STATUS,
    STORE,
    BATTLE,
    REWARD
}