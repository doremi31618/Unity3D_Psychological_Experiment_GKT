using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitlePage : Page
{
    
    public float fadeOutTime = 0.5f;
    public AnimationCurve curve;
    private void Start() {
        InitPage();
    }

    public override void InitPage(){
        this.gameObject.SetActive(true);
        foreach (var item in GetComponentsInChildren<RectTransform>(true))
        {

            string[] obj_NameAnalize = item.gameObject.name.Split('_');
            string _last_Name_Tag = obj_NameAnalize[obj_NameAnalize.Length-1];

            if (item == this.transform  || 
                item.gameObject.name == "Template" ||
                _last_Name_Tag == "dontInit") continue;

            Fade(item.gameObject);
        }
        Invoke("EndPage", fadeOutTime);
    }

    void Fade(GameObject obj){

        
        if(obj.GetComponent<Image>()){
            obj.GetComponent<Image>().DOFade(0,fadeOutTime).SetEase(curve);
        }
        else if( obj.GetComponent<Text>()){
            obj.GetComponent<Text>().DOFade(0,fadeOutTime).SetEase(curve);
        }
    }

    public override void EndPage(){
        EndPageEvent();
        // foreach (var item in GetComponentsInChildren<RectTransform>(true))
        // {

        //     string[] obj_NameAnalize = item.gameObject.name.Split('_');
        //     string _last_Name_Tag = obj_NameAnalize[obj_NameAnalize.Length-1];

        //     if (item == this.transform  || 
        //         item.gameObject.name == "Template" ||
        //         _last_Name_Tag == "dontInit") continue;

        //     Fade(0,item.gameObject);
        // }
        
        
    }

    void EndPageEvent()
    {
        this.gameObject.SetActive(false);
        // endingEvent.Invoke();
    }
    


}
