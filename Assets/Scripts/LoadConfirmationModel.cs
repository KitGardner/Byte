using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadConfirmationModel : MonoBehaviour
{
    public GameObject DataContainer;
    public Button YesBtn;
    public Button NoBtn;

    public void Initialize(GameObject saveFileDisplayObject, Guid saveFileId, TitleScreenManager titleScreenManager)
    {
        saveFileDisplayObject.transform.parent = DataContainer.transform;
        YesBtn.onClick.AddListener(() => titleScreenManager.LoadGame(saveFileId, this.gameObject));
        NoBtn.onClick.AddListener(() => titleScreenManager.ShowSavesList());
    }
}
