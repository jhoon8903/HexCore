using NetCoreAudio.Interfaces;

namespace HexaCoreVillage.Utility
{
    public class AudioPlayer
    {
        public enum PlayOption { Play, Pause, Resume, Stop, Change}
        private static readonly IPlayer _audioPlayer = new NetCoreAudio.Player();
        
        /// <summary>
        ///  오디오 플레이어 메서드
        /// </summary>
        /// <param name="filePath">Audio File Path</param>
        /// <param name="playOption">Play : 재생, Pause : 일시정지, Resume : 재실행, Stop : 중지, Change : 파일 바꾸기</param>
        public static void AudioController(string filePath, PlayOption playOption)
        {
            switch (playOption)
            {
                case PlayOption.Play:
                    _audioPlayer.Play(filePath);
                    break;
                case PlayOption.Pause:
                    _audioPlayer.Pause();
                    break;
                case PlayOption.Resume:
                    _audioPlayer.Resume();
                    break;
                case PlayOption.Stop:
                    _audioPlayer.Stop();
                    break;
                case PlayOption.Change:
                    _audioPlayer.Stop();
                    _audioPlayer.Play(filePath);
                    break;
            }
        }
    }
}