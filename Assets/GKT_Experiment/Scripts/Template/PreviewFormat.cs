using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewFormat : MonoBehaviour
{
    public Text title;
    public Image top_left;
    public Image top_right;
    public Image bottom_left;
    public Image bottom_right;

    public void Initialize(string _title, Sprite[] imgs){
        title.text = _title;
        top_left.sprite = imgs[0];
        top_right.sprite = imgs[1];
        bottom_right.sprite = imgs[2];
        bottom_left.sprite = imgs[3];
        for(int i=0; i<imgs.Length; i++){
            if (imgs[i]==null)
                Debug.LogError("[PreviewFormat]" + _title+ "-"+ (i+1) + " image not found " );
        }
    }

}
