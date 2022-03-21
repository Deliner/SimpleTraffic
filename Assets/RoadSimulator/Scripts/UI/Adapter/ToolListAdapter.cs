using System.Collections.Generic;
using Kawaiiju.Traffic.LevelEditor;
using RoadSimulator.Scripts.UI.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoadSimulator.Scripts.UI.Adapter
{
    public class ToolListAdapter : ToolListAdapter.ToolViewHolder.ICallback
    {
        private ICallback _callback;
        private Data _data;

        private int _lastToolSelectedIndex = -1;

        private List<ToolViewHolder> _viewList = new();

        public void Init(ICallback callback, Data data)
        {
            _callback = callback;
            _data = data;

            CreateView();
        }

        public void OnToolSelected(bool isSelected, int index)
        {
            if (isSelected)
            {
                DropLastToolSelection();
                _lastToolSelectedIndex = index;

                _callback.OnToolSelected(index);
            }
            else
            {
                if (index == _lastToolSelectedIndex)
                {
                    _callback.OnNoToolIsSelected();
                    _lastToolSelectedIndex = -1;
                }
            }
        }

        private void DropLastToolSelection()
        {
            if (_lastToolSelectedIndex != -1)
            {
                _viewList[_lastToolSelectedIndex].DropSelection();
            }
        }

        private void CreateView()
        {
            var i = 0;
            foreach (var tool in _data.ToolList)
            {
                var instance = Object.Instantiate(_data.ToolObject, _data.ListTransform, false);
                InitializeItemView(instance, tool, i);
                i++;
            }
        }

        private void InitializeItemView(GameObject viewGameObject, Tool tool, int index)
        {
            var view = new ToolViewHolder(viewGameObject.transform, index, this);
            view.Init(tool);

            _viewList.Add(view);
        }

        public interface ICallback
        {
            public void OnToolSelected(int index);
            public void OnNoToolIsSelected();
        }

        private class ToolViewHolder : ToolView.ICallback
        {
            private readonly ICallback _callback;
            private readonly ToolView _toolView;
            private readonly int _index;

            public ToolViewHolder(Component rootView, int index, ICallback callback)
            {
                _toolView = rootView.GetComponent<ToolView>();
                _callback = callback;
                _index = index;
            }

            public void Init(Tool tool)
            {
                _toolView.Init(this, tool.GetToolName());
            }

            public void DropSelection()
            {
                _toolView.Unselect();
            }

            public void OnToolSelected(bool isSelected)
            {
                _callback.OnToolSelected(isSelected, _index);
            }

            public interface ICallback
            {
                public void OnToolSelected(bool isSelected, int index);
            }
        }

        public class Data
        {
            public readonly RectTransform ListTransform;
            public readonly GameObject ToolObject;
            public readonly List<Tool> ToolList;

            public Data(RectTransform listTransform, GameObject toolObject, List<Tool> toolList)
            {
                ListTransform = listTransform;
                ToolObject = toolObject;
                ToolList = toolList;
            }
        }
    }
}