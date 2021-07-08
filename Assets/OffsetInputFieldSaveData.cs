using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.SaveLoad
{
    public class OffsetInputFieldSaveData : MonoBehaviour
    {
        public InputField inputField;

        void Start() => inputField.text = ((int)(GameManager.InputOffset * 1000)).ToString();
    }
}