
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PupilLabs{
    public delegate void InitTextureHandler(object sender, PupilLabsEventArgs e);
    public class PupilLabsEventArgs
    {
        public enum EventType{
            InitEyeTexture,
            Connect_success,
            Connect_failed,
        }
        public EventType eventType;
        public string info;
        public int eyeIndex;
    }
}