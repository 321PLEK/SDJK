using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.PlayMode
{
    public class Visualizer : MonoBehaviour
    {
        public GameObject bar;
        private Image[] bars = new Image[256];
        public float[] samples = new float[256];

        void Awake()
        {
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i] = Instantiate(bar, transform).GetComponent<Image>();
                bars[i].rectTransform.anchoredPosition = new Vector2(i * 8, 0);
                bars[i].rectTransform.sizeDelta = new Vector2(5, 0);
                bars[i].color = JColor32.JColorToColor(PlayerManager.mapData.Effect.AudioSpectrumColor);
            }
        }

        IEnumerator Start()
        {
            while (true)
            {
                if (PlayerManager.mapData.Effect.AudioSpectrumUse)
                {
                    PlayerManager.playerManager.audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

                    int ii = 0;
                    for (int i = 0; i < bars.Length; i++)
                    {
                        bars[i].rectTransform.sizeDelta = Vector2.MoveTowards(bars[i].rectTransform.sizeDelta, new Vector2(5, samples[Random.Range(5, samples.Length)] * 12000 * Random.Range(0.75f, 1.25f)), 20 * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch) * GameManager.FpsDeltaTime);
                        ii++;

                        if (ii > 10)
                            ii = 0;
                    }
                }

                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}