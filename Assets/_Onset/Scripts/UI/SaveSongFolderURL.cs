using UnityEngine;

public static class SaveSongFolderURL
{
    public static string GetFileName(string author, string title) => $"{author}_{title}";
    public const string SAVE_FOLDER_NAME = "SONG_MAPS";
    public static string SaveSongAbsoluteFolderURL => Application.dataPath + $"/{SAVE_FOLDER_NAME}";

    public static string GetAbsoluteURLSongCredits(AudioClipData audioClipData) => SaveSongAbsoluteFolderURL + $"/{audioClipData.Author}_{audioClipData.SongTitle}_credit.json";
    public static string GetAbsoluteURLSongMap(AudioClipData audioClipData) => SaveSongAbsoluteFolderURL + $"/{audioClipData.Author}_{audioClipData.SongTitle}_map.json";
}
