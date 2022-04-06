using System;
using UnityEngine;

namespace Script
{
    public class EffectController : MonoBehaviour
    {
        private static EffectController _instance;

        public static EffectController Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _instance = this;
        }

        public Animator JumpEffect;

        public void ShowJumpEffect(Vector2 worldPos)
        {
            JumpEffect.gameObject.SetActive(true);
            JumpEffect.gameObject.transform.position = worldPos;
            JumpEffect.Play("JumpFX");
        }

        public Animator LandingEffect;

        public void ShowLandingEffect(Vector2 worldPos)
        {
            LandingEffect.gameObject.SetActive(true);
            LandingEffect.gameObject.transform.position = worldPos;
            LandingEffect.Play("LandingFX");
        }
    }
}