using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class StatusPopUpAnimation : MonoBehaviour
    {
        public AnimationCurve opacityCurve;
        public AnimationCurve scaleCurve;
        public AnimationCurve heightCurve;

        private TextMeshProUGUI text;
        private float time = 0;
        private Vector3 origin;

        private void Awake()
        {
            text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            origin = transform.position;
        }

        private void Update()
        {
            text.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
            transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
            transform.position = origin + new Vector3(0, 1, heightCurve.Evaluate(time));
            time += Time.deltaTime;
        }
    }
}