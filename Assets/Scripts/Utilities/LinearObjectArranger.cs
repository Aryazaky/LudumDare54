using UnityEngine;

namespace Gespell.Utilities
{
    public class LinearObjectArranger : MonoBehaviour
    {
        [SerializeField] private Vector3 startVector = new Vector3(0, 0, 0); // Starting vector for arrangement
        [SerializeField] private Vector3 endVector = new Vector3(10, 0, 0); // Ending vector for arrangement

        private void Start()
        {
            ArrangeChildren();
        }

        public void ArrangeChildren()
        {
            int childCount = transform.childCount;
            if (childCount <= 1)
            {
                // Nothing to arrange if there is only one child or none.
                return;
            }

            float step = 1f / (childCount - 1);
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                float lerpFactor = step * i;
                Vector3 newPosition = Vector3.Lerp(startVector, endVector, lerpFactor);
                child.position = newPosition;
                child.GetComponent<SpriteRenderer>().sortingOrder = i;
            }
        }

        private void OnValidate()
        {
            ArrangeChildren();
        }

        private void OnTransformChildrenChanged()
        {
            ArrangeChildren();
        }
    }
}