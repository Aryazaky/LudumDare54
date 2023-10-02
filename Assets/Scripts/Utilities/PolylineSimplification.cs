using System.Collections.Generic;
using UnityEngine;

namespace Gespell.Utilities
{
    public static class PolylineSimplification
    {
        public static List<Vector2> DouglasPeuckerSimplify(List<Vector2> points, float tolerance)
        {
            if (points == null || points.Count < 3)
            {
                return points;
            }

            int firstPoint = 0;
            int lastPoint = points.Count - 1;
            List<int> pointIndicesToKeep = new List<int>();

            // Add the first and last index to the keep list
            pointIndicesToKeep.Add(firstPoint);
            pointIndicesToKeep.Add(lastPoint);

            // The stack to keep track of the line segments to process
            Stack<int[]> stack = new Stack<int[]>();
            stack.Push(new int[] { firstPoint, lastPoint });

            // Loop until the stack is empty
            while (stack.Count > 0)
            {
                int[] currentSegment = stack.Pop();
                int startIndex = currentSegment[0];
                int endIndex = currentSegment[1];

                float maxDistance = 0f;
                int farthestPointIndex = 0;

                // Find the point with the maximum distance from the segment
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    float distance = DistanceFromPointToLine(points[i], points[startIndex], points[endIndex]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestPointIndex = i;
                    }
                }

                // If the maximum distance is greater than the tolerance, add the point to the keep list
                if (maxDistance > tolerance)
                {
                    pointIndicesToKeep.Add(farthestPointIndex);
                    stack.Push(new int[] { startIndex, farthestPointIndex });
                    stack.Push(new int[] { farthestPointIndex, endIndex });
                }
            }

            // Sort the indices and create the simplified polyline
            pointIndicesToKeep.Sort();
            List<Vector2> simplifiedPoints = new List<Vector2>();
            foreach (int index in pointIndicesToKeep)
            {
                simplifiedPoints.Add(points[index]);
            }

            return simplifiedPoints;
        }

        // Helper function to calculate the perpendicular distance from a point to a line
        private static float DistanceFromPointToLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            float numerator = Mathf.Abs((lineEnd.y - lineStart.y) * point.x - (lineEnd.x - lineStart.x) * point.y +
                                       lineEnd.x * lineStart.y - lineEnd.y * lineStart.x);
            float denominator = Mathf.Sqrt(Mathf.Pow(lineEnd.y - lineStart.y, 2) +
                                           Mathf.Pow(lineEnd.x - lineStart.x, 2));
            return numerator / denominator;
        }
    }
}