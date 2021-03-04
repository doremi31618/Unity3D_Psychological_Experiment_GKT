using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public virtual void InitPage(){
        if (!gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }
    public virtual void InitPage(int trialIndex){}
    public virtual void InitPage(DataManager dataManager){
        this.gameObject.SetActive(true);
    }
    
    public virtual void UpdatePage(DataManager dataManager){

    }

    public virtual void EndPage(){
        this.gameObject.SetActive(false);
    }
}
