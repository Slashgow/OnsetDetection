using UnityEngine;

public static class SaveSongFolderURL
{
    public const string SAVE_FOLDER_NAME = "SONG_MAPS";
    public static string SaveSongAbsoluteFolderURL => Application.dataPath + $"/{SAVE_FOLDER_NAME}"; 
}
