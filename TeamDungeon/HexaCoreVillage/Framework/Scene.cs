
namespace HexaCoreVillage.Framework;


/// <summary>
/// # Scene에 베이스가 되는 추상 클래스
/// </summary>
public abstract class Scene
{
    protected bool _isRun;
    public abstract SCENE_NAME SceneName { get; }

    // Initalize Method
    public abstract void Start();
    public abstract void Update();
    public abstract void Stop();

    // Start,Update Logic
    protected void StartCommon()
    {
        _isRun = true;
    }
    protected void UpdateCommon()
    {
        if (!_isRun) return;
    }
    protected void StopCommon()
    {
        _isRun = false;
    }

    // Getter
    public bool GetIsRun() { return _isRun; }
}

// 추 후 Utility 같은 정적 클래스로 뺄 예정
public enum SCENE_NAME
{
    LOGIN,
    LOBBY,
    STATUS,
    STORE,
    BATTLE,
    REWARD
}