using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EricUIFramework
{

    public abstract class BaseView : MonoBehaviour
    {
        public virtual void OnInit(){}
        public virtual void OnOpen(){}
        public virtual void OnShow(){}
        public virtual void OnHide(){}
        public virtual void OnClose(){}
        
    }
}

