using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.SaveLoad
{
    public class NickNameInputFieldSaveData : MonoBehaviour
    {
        public InputField inputField;

        void Start() => inputField.text = GameManager.NickName;
    }
}