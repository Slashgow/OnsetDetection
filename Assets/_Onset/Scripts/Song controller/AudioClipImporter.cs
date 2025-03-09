using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using System.IO;
using System;

public class AudioClipImporter : MonoSingleton<AudioClipImporter>
{
    public AudioClip AudioClip { get; private set; }
    public string Extension { get; private set; }

    public event Action OnEndImportAudioClip;
    public event Action OnFailImportAudioClip;
    public string CurrentFilePath { get; private set; }

    public void OpenFileBrowser(bool showImportSongMenu)
    {
        var bp = new BrowserProperties();
        bp.filter = "Music Files (*.mp3, *.wav, *.ogg) | *.mp3; *.wav; *.ogg";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadAudioClip(path));
        });

        if(showImportSongMenu )
            UIMenuController.Instance.ShowOnly(MenuType.IMPORT_SONG);
    }

    public IEnumerator LoadAudioClip(string path)
    {
        Extension = Path.GetExtension(path);

        AudioType audioType = AudioType.UNKNOWN;
        if (Extension == ".mp3")
            audioType = AudioType.MPEG;
        else if(Extension == ".wav")
            audioType = AudioType.WAV;
        else if(Extension == ".ogg")
            audioType = AudioType.OGGVORBIS;

        Debug.Log(path);

        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                OnFailImportAudioClip?.Invoke();
                Debug.Log(uwr.error);
            }
            else
            {
                AudioClip = DownloadHandlerAudioClip.GetContent(uwr);
                CurrentFilePath = path;
                OnEndImportAudioClip?.Invoke();
            }
        }
    }

    public void CopyImportedFile(string currentFilePath, string copyDirectoryPath, string fileName)
    {
        try
        {
            // Will not overwrite if the destination file already exists.
            File.Copy(currentFilePath, Path.Combine(copyDirectoryPath, fileName ));
        }
    
        // Catch exception if the file was already copied.
        catch (IOException copyError)
        {
            Console.WriteLine(copyError.Message);
        }
    }
}
