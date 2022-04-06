using System;
using UnityEngine;

namespace Script.Utils
{
    public class Trigger2DCheck : MonoBehaviour
    {
        public LayerMask TargetLayer;

        private int mEnterCount;

        public int EnterCount
        {
            get { return mEnterCount; }
        }

        public bool Triggered
        {
            get { return EnterCount > 0; }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsInLayerMask(col.gameObject, TargetLayer))
            {
                mEnterCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsInLayerMask(other.gameObject, TargetLayer))
            {
                mEnterCount--;
            }
        }

        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            // TODO: 这是什么写法？
            var objectLayer = 1 << obj.layer;
            // TODO: 这是什么写法？
            return (mask.value & objectLayer) > 0;
        }
    }
}