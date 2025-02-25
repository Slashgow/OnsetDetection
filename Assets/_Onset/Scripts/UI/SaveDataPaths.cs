using UnityEngine;

public static class SaveDataPaths
{
    public static void SetHighScore(AudioClipData audioClipData, int score) => PlayerPrefs.SetInt(GetFileNameWithoutExtension(audioClipData), score);
    public static int GetHighScore(AudioClipData audioClipData) => PlayerPrefs.GetInt(GetFileNameWithoutExtension(audioClipData), 0);  
    public static string GetFileName(AudioClipData audioClipData) => $"{audioClipData.Author}_{audioClipData.SongTitle}{audioClipData.Extension}";
    public static string GetFileNameWithoutExtension(AudioClipData audioClipData) => $"{audioClipData.Author}_{audioClipData.SongTitle}";
    public const string SAVE_FOLDER_NAME = "SONG_MAPS";
    public static string SaveSongAbsoluteFolderPath => Application.dataPath + $"/{SAVE_FOLDER_NAME}";
    public static string AbsolutePathAudioClipDirectory => Application.streamingAssetsPath + $"/Songs/";
    public static string GetAbsolutePathAudioClip(AudioClipData audioClipData) => Application.streamingAssetsPath + $"/Songs/" + GetFileName(audioClipData) ;
    public static string GetAbsolutePathSongCredits(AudioClipData audioClipData) => SaveSongAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_credit.json";
    public static string GetAbsolutePathSongMap(AudioClipData audioClipData) => SaveSongAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_map.json";
}
