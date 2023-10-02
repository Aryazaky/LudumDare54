using Gespell.Utilities;
using UnityEngine;

namespace Gespell.Units.HealthBars
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteHealthBar : HealthBar
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField] private bool reverseOrder = true;
        [SerializeField] private Sprite[] barSprites;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        protected override void OwnerOnHealthChanged(int value)
        {
            var maxIndex = barSprites.MaxIndex();
            if(maxIndex == -1)
            {
                Debug.LogError($"Bar sprites empty in {this}");
                return;
            }
            var index = (value.ToFloat() / Owner.MaxHealth * maxIndex).FloorToInt();
            if (reverseOrder) index = maxIndex - index;
            spriteRenderer.sprite = barSprites[index];
        }
    }
}