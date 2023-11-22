
using HexaCoreVillage.Utility;
using NAudio.Wave;
using System.Globalization;

namespace HexaCoreVillage.Manager;

/// <summary>
/// # Resource 자원들을 관리하는 매니저 클래스
/// ## Utility -> Resources에 Key나 Path들을 관리한다.
/// ## .txt , .json과 같은 파일들을 로드하는 역할
/// </summary>
public class Manager_Resource
{
    #region Member Variables
    // JSON, TXT 파일 보관
    private Dictionary<ResourceKeys, string>? _textResources;
    // Sound 파일 경로 보관
    private Dictionary<ResourceKeys, string>? _soundResources;

    private bool _isComplete;

    #endregion


    #region Getter
    public bool IsComplete => _isComplete;
    #endregion


    #region Main Methods
    /// <summary>
    /// # 생성자 및 이니셜라이저 역할
    /// </summary>
    public Manager_Resource()
    {
        _textResources = new Dictionary<ResourceKeys, string>();
        _soundResources = new Dictionary<ResourceKeys, string>();

        _isComplete = false;
    }


    /// <summary>
    /// # 리소스를 모두 불러오는 메서드
    /// </summary>
    public void LoadAllResources()
    {
        string folderPath = GetResourceFolderPath();

        // 해당 폴더가 존재하지 않는다면?
        if (!Directory.Exists(folderPath))
            throw new Exception($"폴더 {folderPath}가 존재하지 않습니다.");
        else // 존재 한다면
        {
            var files = Directory.GetFiles(folderPath);

            foreach(string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileExtension = Path.GetExtension(filePath); // 확장자명

                if (IsSupportedFileExtension(fileExtension))
                    TextFileLoadFromResourceKey(filePath, fileName);
                else if (IsSupportedFileExtensionSound(fileExtension))
                    SoundFileLoadFromResourceKey(filePath, fileName);
            }
        }

        if(_textResources?.Count != 0 || _soundResources?.Count != 0)
            _isComplete = true;
    }

    public string GetTextResource(ResourceKeys key)
    {
        if (_textResources?.ContainsKey(key) == true)
            return _textResources[key];
        else
        {
            Console.WriteLine($"Text resources not found : {key.ToString()}");
            throw new Exception($"해당하는 리소스{key.ToString()}가 존재하지 않습니다");
        }
    }

    public string GetSoundResource(ResourceKeys key)
    {
        if (_soundResources?.ContainsKey(key) == true)
            return _soundResources[key];
        else
        {
            Console.WriteLine($"Sound resources not found : {key.ToString()}");
            throw new Exception($"해당하는 리소스{key.ToString()}가 존재하지 않습니다");
        }
    }

    // 제네릭 타입으로 반환받는 메서드
    public T GetResources<T>(ResourceKeys key)
    {
        if (typeof(T) == typeof(string) && _textResources != null)
        {
            if (_textResources.ContainsKey(key) == true)
                return (T)(object)_textResources[key];
        }
        else if (typeof(T) == typeof(AudioFileReader) && _soundResources != null)
        {
            if (_soundResources.ContainsKey(key) == true)
                return (T)(object)_soundResources[key];
        }
        
        throw new Exception($"해당하는 리소스{key.ToString()}가 존재하지 않습니다");
    }

    #endregion



    #region Helper Methods
    /// <summary>
    /// # 리소스 폴더의 위치를 반환하는 메서드
    /// </summary>
    private string GetResourceFolderPath()
    {
        // 현재 앱이 실행되는 위치에 디렉토리 즉 .Net폴더
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // 상위 폴더로 이동
        string teamProjectDirectory = Path.Combine(currentDirectory, "..", "..", "..");
        string resourcePath = Path.Combine(teamProjectDirectory, "Resources");

        return resourcePath;
    }

    private void TextFileLoadFromResourceKey(string filePath, string fileName)
    {
        foreach(ResourceKeys key in Enum.GetValues(typeof(ResourceKeys)))
        {
            string keyName = key.ToString();

            if(fileName.Equals(keyName, StringComparison.OrdinalIgnoreCase) && _textResources != null)
            {
                string fileContent = File.ReadAllText(filePath);
                _textResources[key] = fileContent;
                break;
            }
        }
    }

    private void SoundFileLoadFromResourceKey(string filePath, string fileName)
    {
        foreach (ResourceKeys key in Enum.GetValues(typeof(ResourceKeys)))
        {
            string keyName = key.ToString();

            if (fileName.Equals(keyName, StringComparison.OrdinalIgnoreCase) && _soundResources != null)
            {
                string fileContent = filePath;
                _soundResources[key] = fileContent;
                break;
            }
        }
    }

    /// <summary>
    /// # 파일이 json 또는 txt 확장자인지 확인하고 반환하는 메서드
    /// </summary>
    private bool IsSupportedFileExtension(string fileExtension)
    {
        return fileExtension.Equals(".json", StringComparison.OrdinalIgnoreCase) ||
            fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsSupportedFileExtensionSound(string fileExtension)
    {
        return fileExtension.Equals(".wav", StringComparison.OrdinalIgnoreCase);
    }
    #endregion
}