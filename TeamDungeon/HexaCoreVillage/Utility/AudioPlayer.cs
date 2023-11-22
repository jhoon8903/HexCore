using NetCoreAudio.Interfaces;

namespace HexaCoreVillage.Utility
{
    public class AudioPlayer
    {
        public enum PlayOption { Play, Pause, Resume, Stop, Change, LoopStart, LoopStop, SetVolume }
        private static readonly IPlayer _audioPlayer = new NetCoreAudio.Player();
        private static bool _loop = false;
        private static string? _currentFilePath;


        /// <summary>
        ///  오디오 플레이어 메서드
        /// </summary>
        /// <param name="filePath">Audio File Path</param>
        /// <param name="playOption">Play : 재생, Pause : 일시정지, Resume : 재실행, Stop : 중지, Change : 파일 바꾸기, LoopStart : 반복 실행, LoopStop : 반복실행취소</param>
        public static void AudioController(string? filePath, PlayOption playOption)
        {
            _currentFilePath = filePath;

            switch (playOption)
            {
                case PlayOption.Play:
                    PlayAudio();
                    return;
                case PlayOption.Pause:
                    _audioPlayer.Pause();
                    return;
                case PlayOption.Resume:
                    _audioPlayer.Resume();
                    return;
                case PlayOption.Stop:
                    StopAudio();
                    return;
                case PlayOption.Change:
                    ChangeAudio(filePath);
                    return;
                case PlayOption.LoopStart:
                    _loop = true;
                    PlayAudio();
                    return;
                case PlayOption.LoopStop:
                    _loop = false;
                    StopAudio();
                    return;
            }
        }

        public static void AudioVolume(byte volume)
        {
            _audioPlayer.SetVolume(volume);
        }
        
        private static void PlayAudio()
        {
            _audioPlayer.PlaybackFinished += OnPlayFinished;
            _audioPlayer.Play(_currentFilePath);
        }

        private static void StopAudio()
        {
            _loop = false;
            _audioPlayer.Stop();
            _audioPlayer.PlaybackFinished -= OnPlayFinished;
        }

        private static void ChangeAudio(string? filePath)
        {
            _audioPlayer.Stop();
            _audioPlayer.Play(filePath);
        }

        private static void OnPlayFinished(object sender, EventArgs eventArgs)
        {
            if (_loop)
            {
                PlayAudio();
            }
        }

        public static void SetupApplicationExitHandling()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            // Stop the audio when the application is exiting
            StopAudio();
        }
    }
}