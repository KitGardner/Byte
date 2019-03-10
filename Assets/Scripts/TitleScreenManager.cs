using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{

    public GameObject SaveFilesDisplay;
    public GameObject SaveFilesContainer;
    public GameObject SaveFileInfoObject;
    public GameObject MainPanel;
    public GameObject LoadConfirmationModel;

    public List<GameObject> saveFilesDisplayed = new List<GameObject>();

    private bool saveListPopulated = false;

    // Start is called before the first frame update
    void Start()
    {
        SaveFilesDisplay.SetActive(false);
    }

    public void ShowSavesList()
    {
        if (!saveListPopulated)
        {
            SaveFilesDisplay.SetActive(true);
            //if(saveFilesDisplayed.Count == 0)
            //{
            //    //call database and handle creating from new set
            //}
            //else
            //{
            //    //reload UI elements with cached savefiles to save time and memory
            //}
            //TODO add code to handle calling back end database and populate with save info cards

            for (int i = 0; i < 10; i++)
            {
                var newSaveInfo = Instantiate(SaveFileInfoObject, SaveFilesContainer.transform);
                newSaveInfo.GetComponent<SaveInfoCard>().Initialize("/screenShots/testImage.png", Guid.NewGuid(), "Duvall Estate Exterior", "Hard", DateTime.UtcNow, DateTime.Now);
                saveFilesDisplayed.Add(newSaveInfo);
            }
            saveListPopulated = true;
        }
        else
        {
            HideSavesList();
        }
    }

    public void HideSavesList()
    {
        SaveFilesDisplay.SetActive(false);
        foreach (var saveFileObject in saveFilesDisplayed)
        {
            Destroy(saveFileObject);
        }
        saveFilesDisplayed.Clear();
        saveListPopulated = false;
    }

    public static void SaveFileSelected(Guid SaveFileId)
    {
        Debug.Log("I was called and passed in " + SaveFileId.ToString());
    }

    public void SpawnConfirmationModel(Guid saveFileId)
    {
        var newConfirmModel = Instantiate(LoadConfirmationModel, MainPanel.transform);
    }

    public void LoadGame(Guid SaveFileId, GameObject confirmationModel)
    {
        Destroy(confirmationModel);
        //Async calls to backend to get all save data while handling the loading of the level in the save data.
    }
}
