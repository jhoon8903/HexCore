
namespace HexaCoreVillage.Framework;

/// <summary>
/// # 싱글톤화를 위한 상속용 클래스
/// ## 사용 예제
/// ### public class Manager : Singleton<Manager>
/// ### 위와 같이 생성된 클래스는 싱글톤이 되며 Manager.Instance 사용 가능
/// </summary>
public class Singleton<T> where T : class, new()
{
    // 싱글톤화된 객체 인스턴스
    private static T? _instance;
    // 혹시 모를 비동기 작업을 위해 Lock 제약조건
    // 아예 무시하고 넘어가셔도 무방합니다.
    private static readonly object Lock = new object();

    #region Properties
    public static T Instance
    {
        get
        {
            lock(Lock)
            {
                if(_instance == null)
                    _instance = new T();

                return _instance;
            }
        }
    }
    #endregion

}
