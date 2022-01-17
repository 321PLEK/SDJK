using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.PlayMode
{
    public class BarPosEffect : MonoBehaviour
    {
        public bool A = false;
        public bool S = false;
        public bool D = false;
        public bool J = false;
        public bool K = false;
        public bool L = false;

        public Vector3 defaultPos;

        void Update()
        {
            if (A)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.ABarPos) + defaultPos;
            else if (S)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.SBarPos) + defaultPos;
            else if (D)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.DBarPos) + defaultPos;
            else if (J)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.JBarPos) + defaultPos;
            else if (K)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.KBarPos) + defaultPos;
            else if (L)
                transform.localPosition = JVector3.JVector3ToVector3(PlayerManager.effect.LBarPos) + defaultPos;
        }
    }
}