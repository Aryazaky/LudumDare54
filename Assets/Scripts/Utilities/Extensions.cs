using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gespell.Utilities
{
    public static class Extensions
    {
        public static float ToFloat(this int value)
        {
            return value;
        }

        public static int FloorToInt(this float value)
        {
            return Mathf.FloorToInt(value);
        }

        public static int MaxIndex(this Array array)
        {
            if(array.Length == 0) Debug.LogWarning("Array is empty");
            return array.Length - 1;
        }

        public static IEnumerable<T> ReverseList<T>(this IEnumerable<T> list)
        {
            return list.Reverse();
        }

        public static void BlinkColor(this SpriteRenderer renderer, MonoBehaviour owner, Color initialColor,
            Color toColor, float blinkDuration = 0.2f, int blinkTimes = 1)
        {
            owner.StartCoroutine(renderer.BlinkEnumerator(initialColor, toColor, blinkDuration, blinkTimes));
        }

        private static IEnumerator BlinkEnumerator(this SpriteRenderer renderer, Color initialColor, Color toColor,
            float blinkDuration, int blinkTimes)
        {
            while (blinkTimes-- > 0)
            {
                renderer.color = toColor;
                yield return new WaitForSeconds(blinkDuration);
                renderer.color = initialColor;
                yield return new WaitForSeconds(blinkDuration);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
            {
                action(obj);
            }
        }
        
        public static IEnumerable<T> SortByDistanceTo<T>(this IEnumerable<T> collection, Vector3 position) where T : MonoBehaviour
        {
            if (collection == null)
            {
                throw new System.ArgumentNullException(nameof(collection), "Collection cannot be null.");
            }

            return collection.OrderBy(item => Vector3.Distance(item.transform.position, position));
        }
    }
}