using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UISongRemover : MonoBehaviour
{
    [SerializeField]
    private UISong uiSong;

    [SerializeField]
    private Button trashButton;

    [SerializeField]
    private Button cancelButton, confirmButton;

    [SerializeField]
    private GameObject deletePopUp;

    private void Start()
    {
        trashButton.onClick.AddListener(ShowDeletePopUp);
        confirmButton.onClick.AddListener(DeleteSongMapAndCredits);
        cancelButton.onClick.AddListener(HideDeletePopUp);
    }
    private void OnDestroy()
    {
        trashButton.onClick.RemoveListener(ShowDeletePopUp);
        confirmButton.onClick.RemoveListener(DeleteSongMapAndCredits);
        cancelButton.onClick.RemoveListener(HideDeletePopUp);
    }

    public void ShowDeletePopUp()
    {
        deletePopUp.transform.SetParent(FindFirstObjectByType<UIMenu>().transform);
        deletePopUp.SetActive(true);
    }

    private void HideDeletePopUp() => deletePopUp.SetActive(false);
    public void DeleteSongMapAndCredits()
    {
        string filePathMap = uiSong.AudioClipData.IsCampaign ?
               SaveDataPaths.GetAbsolutePathSongMapCampaign(uiSong.AudioClipData) :
               SaveDataPaths.GetAbsolutePathSongMapImported(uiSong.AudioClipData);

        string filePathCredits = uiSong.AudioClipData.IsCampaign ?
            SaveDataPaths.GetAbsolutePathSongCreditsCampaign(uiSong.AudioClipData) :
            SaveDataPaths.GetAbsolutePathSongCreditsImported(uiSong.AudioClipData);


        File.Delete(filePathMap);
        File.Delete(filePathCredits);

        HideDeletePopUp();
        uiSong.gameObject.SetActive(false);
    }

}
