using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPage : Page
{
    public GameObject PreviewPrefabs;
    public GameObject Content;

    public List<GameObject> TrialPreviewList;

    private void Awake()
    {
        if (Content == null)
            Content = transform.Find("Content").gameObject;
    }
    public void InitPage(string previewPath, DataManager dataManager)
    {
        base.InitPage();
        int trialNumber = dataManager.setting.trialNumber;
        if (TrialPreviewList.Count != 0)
        {
            for ( int i=0; i<trialNumber; i++)
                Destroy(TrialPreviewList[i]);
            TrialPreviewList = new List<GameObject>();
        }
        for (int i = 0; i < trialNumber; i++)
        {
            GameObject preview = Instantiate(PreviewPrefabs);

            string name = "Trial " + (i + 1);
            preview.transform.SetParent(Content.transform);
            preview.name = name;

            PreviewFormat previewContent = preview.GetComponent<PreviewFormat>();
            previewContent.Initialize(name, dataManager.resouce.getTrialImage(i));

            TrialPreviewList.Add(preview);
        }


    }
    bool RemoveFromList(GameObject obj){
        return obj != null;
    }

}
