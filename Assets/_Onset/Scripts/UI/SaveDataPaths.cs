using UnityEngine;

public static class SaveDataPaths
{
    public static void SetHighScore(AudioClipData audioClipData, int score) => PlayerPrefs.SetInt(GetFileNameWithoutExtension(audioClipData), score);
    public static int GetHighScore(AudioClipData audioClipData) => PlayerPrefs.GetInt(GetFileNameWithoutExtension(audioClipData), 0);
    public static string GetFileName(AudioClipData audioClipData) => $"{audioClipData.Author}_{audioClipData.SongTitle}{audioClipData.Extension}";
    public static string GetFileNameWithoutExtension(AudioClipData audioClipData) => $"{audioClipData.Author}_{audioClipData.SongTitle}";

    public const string SAVE_FOLDER_MAP_NAME = "SONG_MAPS";
    public static string SongMapsCampaignAbsoluteFolderPath => Application.dataPath + $"/{SAVE_FOLDER_MAP_NAME}";
    public static string AudioClipCampaignAbsoluteFolderPath => Application.streamingAssetsPath + $"/Songs/";
    public static string GetAbsolutePathAudioClipCampaign(AudioClipData audioClipData) => Application.streamingAssetsPath + $"/Songs/" + GetFileName(audioClipData);
    public static string GetAbsolutePathSongCreditsCampaign(AudioClipData audioClipData) => SongMapsCampaignAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_credit.json";
    public static string GetAbsolutePathSongMapCampaign(AudioClipData audioClipData) => SongMapsCampaignAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_map.json";

    public static string SongMapsImportedAbsoluteFolderPath => Application.persistentDataPath + $"/{SAVE_FOLDER_MAP_NAME}";
    public static string GetAbsolutePathSongMapImported(AudioClipData audioClipData) => SongMapsImportedAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_map.json";
    public static string GetAbsolutePathSongCreditsImported(AudioClipData audioClipData) => SongMapsImportedAbsoluteFolderPath + $"/{audioClipData.Author}_{audioClipData.SongTitle}_credit.json";
}
