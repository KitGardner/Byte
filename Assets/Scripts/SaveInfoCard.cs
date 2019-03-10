using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveInfoCard : MonoBehaviour
{
    private string _screenShotFilePath;
    public Guid SaveFileId;
    public RawImage SaveFileScreenShot;
    public Text LevelText;
    public Text DifficultyText;
    public Text PlayTimeText;
    public Text CreatedLocalTimeText;

    [SerializeField]
    private Button ParentButton;

    public void Initialize(string screenShotFilePath, Guid saveFileId, string levelName, string difficulty, DateTime playTime, DateTime CreatedLocaltime)
    {
        _screenShotFilePath = screenShotFilePath;
        SaveFileId = saveFileId;
        LevelText.text = levelName;
        DifficultyText.text = difficulty;
        PlayTimeText.text = playTime.ToString();
        CreatedLocalTimeText.text = CreatedLocaltime.ToString();
        ParentButton.onClick.AddListener(() => TitleScreenManager.SaveFileSelected(saveFileId));
        Debug.Log(ParentButton.onClick);
    }
}
