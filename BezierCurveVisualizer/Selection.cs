using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class Selection : IEnumerable
    {
        int centralPoint = -1;
        List<int> followingPoints;
        
        public Selection()
        {
            followingPoints = new List<int>();
        }

        public void SelectPoint(int index)
        {
            if (Control.ModifierKeys != Keys.Shift)
            {
                ClearSelection();
            }

            if (centralPoint == index)
            {
                Debug.WriteLine("Central point");
                centralPoint = followingPoints.Last();
                followingPoints.RemoveAt(followingPoints.Count - 1);
            }
            else if (followingPoints.Contains(index))
            {
                Debug.WriteLine("Following point");
                Debug.WriteLine(followingPoints.Remove(index));
            }
            else
            {
                Debug.WriteLine("New point");
                if (centralPoint != -1)
                {
                    followingPoints.Add(centralPoint);
                }
                centralPoint = index;
            }
        }

        public void ClearSelection()
        {
            centralPoint = -1;
            followingPoints.Clear();
        }

        public bool HasSelection()
        {
            return centralPoint != -1;
        }

        public int CentralPoint() => centralPoint;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public SelectionEnumerator GetEnumerator()
        {
            return new SelectionEnumerator(centralPoint, followingPoints);
        }
    }

    internal class SelectionEnumerator : IEnumerator
    {
        public List<int> selectedPoint;
        int position = -1;

        public SelectionEnumerator(int centralPoint, List<int> followingPoints)
        {
            selectedPoint = followingPoints;
            selectedPoint.Insert(0, centralPoint);
        }

        public object Current => selectedPoint[position];

        public bool MoveNext()
        {
            position++;
            return position < selectedPoint.Count; 
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
