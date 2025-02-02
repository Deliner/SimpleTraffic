﻿using UnityEngine;

namespace RoadSimulator.Scripts.UI.Utils
{
    public class CursorPositionChecker
    {
        private readonly Transform _uiTransform;

        public CursorPositionChecker(Transform uiTransform)
        {
            _uiTransform = uiTransform;
        }

        public bool IsOverGUI(Vector3 position)
        {
            var i = 0;
            while (i < _uiTransform.childCount)
            {
                if (GetGlobalPosition(_uiTransform.GetChild(i).GetComponent<RectTransform>()).Contains(position))
                {
                    return true;
                }

                i++;
            }
            return false;
        }

        private static Rect GetGlobalPosition(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        }
    }
}