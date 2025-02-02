﻿using System.Collections.Generic;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.UI.Adapter;
using UnityEngine;

namespace RoadSimulator.Scripts.UI.View
{
    public class ToolListView : MonoBehaviour, ToolListAdapter.ICallback
    {
        [SerializeField] private RectTransform listTransform;
        [SerializeField] private GameObject toolObject;

        private ToolListAdapter _toolListAdapter;
        private ICallback _callback;

        private List<ITool> _toolList;

        public void Init(ICallback callback, List<ITool> toolList)
        {
            _callback = callback;
            _toolList = toolList;

            _toolListAdapter = new ToolListAdapter();
            var data = new ToolListAdapter.Data(listTransform, toolObject, toolList);
            _toolListAdapter.Init(this, data);
        }

        public interface ICallback
        {
            public void OnNewToolSelected(ITool tool);
            public void OnAllToolsUnselected();
        }

        public void OnToolSelected(int index)
        {
            _callback.OnNewToolSelected(_toolList[index]);
        }

        public void OnNoToolIsSelected()
        {
            _callback.OnAllToolsUnselected();
        }
    }
}