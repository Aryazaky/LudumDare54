using System;
using DG.Tweening;
using UnityEngine;

namespace Gespell
{
    public class SpellBase : MonoBehaviour
    {
        // In the future, replace this with checking if animation complete
        [SerializeField] private float autoDestroyAfter = 0.5f;

        private void Start()
        {
            DOVirtual.DelayedCall(autoDestroyAfter, () => Destroy(gameObject));
        }
    }
}