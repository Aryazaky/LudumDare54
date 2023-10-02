using UnityEngine;

namespace Gespell
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteHealthBar : HealthBar
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] barSprites;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        protected override void UnitOnHealthChanged(int value)
        {
            spriteRenderer.sprite = barSprites[value / unit.MaxHealth * barSprites.Length];
        }
    }
}