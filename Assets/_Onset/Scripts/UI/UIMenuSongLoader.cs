using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class UIMenuSongLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject content;

    [SerializeField]
    private UISongToggleGroupController toggleGroupController;

    [SerializeField]
    private GameObject UISongGroupPrefab;

    private List<string> fileNames =new List<string>();

    private void Start()
    {
        fileNames = GetAllSavedSongsFileNames();
        LoadUI();
    }

    public List<string> GetAllSavedSongsFileNames()
    {       
        //Debug.Log(Application.dataPath);
        //Debug.Log(SaveSongFolderURL.SaveSongAbsoluteFolderURL);

        List<string> filesPaths = Directory.GetFiles(SaveSongFolderURL.SaveSongAbsoluteFolderURL, "*.txt").ToList();
        filesPaths.ForEach(filePath => fileNames.Add(Path.GetFileNameWithoutExtension(filePath)));
        return fileNames;
    }

    public void LoadUI()
    {
        foreach(string filePath in fileNames)
        {
            GameObject uiSongGroupGameObjectInstance = Instantiate(UISongGroupPrefab, content.transform);
            UISong uiSong = uiSongGroupGameObjectInstance.GetComponent<UISong>();
            uiSong.UpdateSongTitle(filePath);

            toggleGroupController.Toggles.Add(uiSong.Toggle);
        }

        toggleGroupController.RegisterValueChanged();
    }

}
