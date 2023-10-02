using UnityEngine;

namespace Gespell.Utilities
{
    public static class GestureUtility
    {
        public static bool IsBox(Vector2[] points, float angleTolerance, float distanceTolerance)
        {
            if (points.Length < 4)
                return false;

            // Check if the first and last points are close to each other
            if (Vector2.Distance(points[0], points[^1]) > distanceTolerance)
                return false;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[(i + 1) % points.Length];
                Vector2 p3 = points[(i + 2) % points.Length];

                Vector2 edge1 = p2 - p1;
                Vector2 edge2 = p3 - p2;

                float angle = Vector2.Angle(edge1, edge2);
                if (Mathf.Abs(angle - 90) > angleTolerance)
                    return false;
            }

            return true;
        }
        
        public static bool IsVShape(Vector2[] points, float angleThreshold)
        {
            if (points.Length < 3)
                return false;

            // Calculate direction vectors and angles between them
            for (int i = 2; i < points.Length; i++)
            {
                Vector2 prevDirection = (points[i - 1] - points[i - 2]).normalized;
                Vector2 currentDirection = (points[i] - points[i - 1]).normalized;

                float angle = Vector2.Angle(prevDirection, currentDirection);

                // Check if the angle between direction vectors is greater than the threshold
                if (angle > angleThreshold)
                    return true;
            }

            return false;
        }

        public static bool IsALine(Vector2[] points, float maxAngleDifference, float distanceTolerance)
        {
            if (points.Length < 2)
                return false;

            // Check if the first and last points are near each other
            float distance = Vector2.Distance(points[0], points[^1]);
            if (distance < distanceTolerance)
                return false;

            Vector2 referenceDirection = points[1] - points[0];

            for (int i = 2; i < points.Length; i++)
            {
                Vector2 currentDirection = points[i] - points[i - 1];

                float angleDifference = Mathf.Abs(Vector2.Angle(referenceDirection, currentDirection));

                if (angleDifference > maxAngleDifference)
                    return false;

                referenceDirection = currentDirection;
            }

            return true;
        }
        
        public static bool IsVerticalDirection(Vector3 direction, float angleThreshold = 45f)
        {
            Vector3 upDirection = Vector3.up;
            float angleToUp = Vector3.Angle(direction, upDirection);

            Vector3 downDirection = Vector3.down;
            float angleToDown = Vector3.Angle(direction, downDirection);

            return angleToUp <= angleThreshold || angleToDown <= angleThreshold;
        }

        public static bool IsCircle(Vector2[] points, float radiusTolerance, float distanceTolerance,
            float toleranceThreshold = 0.8f)
        {
            if (points.Length < 3)
                return false;
            
            // Check if the first and last points are close to each other
            if (Vector2.Distance(points[0], points[^1]) > distanceTolerance)
                return false;

            // Calculate centroid
            Vector2 centroid = Vector2.zero;
            foreach (Vector2 point in points)
            {
                centroid += point;
            }
            centroid /= points.Length;

            // Calculate average radius
            float avgRadius = 0f;
            foreach (Vector2 point in points)
            {
                avgRadius += Vector2.Distance(centroid, point);
            }
            avgRadius /= points.Length;

            // Check if points are within tolerance of average radius
            int pointsWithinTolerance = 0;
            foreach (Vector2 point in points)
            {
                float distanceToCentroid = Vector2.Distance(centroid, point);
                if (Mathf.Abs(distanceToCentroid - avgRadius) < radiusTolerance)
                {
                    pointsWithinTolerance++;
                }
            }

            // If a significant portion of points are within tolerance, consider it a circle
            return (pointsWithinTolerance / (float)points.Length) > toleranceThreshold;
        }
    }
}