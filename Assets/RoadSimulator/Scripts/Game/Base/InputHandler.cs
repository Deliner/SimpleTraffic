using System;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Base
{
    public class InputHandler
    {
        private const int ClickButton = 0;
        private const int MoveButton = 1;

        private readonly ICallback _callback;

        private State _state = State.WaitInput;

        private Event _currentEvent;

        private Vector3 _oldMousePosition = Vector3.zero;

        public InputHandler(ICallback callback)
        {
            _callback = callback;
        }

        public void HandleInput()
        {
            switch (_state)
            {
                case State.WaitInput:
                    StateWaitInput();
                    break;
                case State.CheckClickSuccess:
                    StateCheckClickSuccess();
                    break;
                case State.CheckPressedMovement:
                    StateCheckPressedMovement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StateWaitInput()
        {
            if ((_currentEvent = IsClickStart()) != null)
            {
                _state = State.CheckClickSuccess;
            }
            else if ((_currentEvent = IsCameraStartMoveEvent()) != null)
            {
                _state = State.CheckPressedMovement;
            }
            else if (_oldMousePosition != Input.mousePosition)
            {
                _oldMousePosition = Input.mousePosition;
                _callback.OnNewMousePosition(Input.mousePosition);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _callback.OnRotate(Input.GetAxis("Mouse ScrollWheel") < 0);
            }
        }

        private static ClickEvent IsClickStart()
        {
            if (Input.GetMouseButtonDown(ClickButton))
            {
                return new MouseClickEvent();
            }

            return Input.touches.Length == 1 ? new TouchClickEvent() : null;
        }

        private static CameraMoveEvent IsCameraStartMoveEvent()
        {
            if (Input.GetMouseButtonDown(MoveButton))
            {
                return new MouseCameraMoveEvent();
            }

            return Input.touches.Length == 2 ? new TouchCameraMoveEvent() : null;
        }

        private void StateCheckClickSuccess()
        {
            if (_currentEvent is ClickEvent clickEvent)
            {
                if (clickEvent.IsFinished())
                {
                    _callback.OnClick(clickEvent.GetPosition());
                }
            }

            Reset();
        }

        private void StateCheckPressedMovement()
        {
            if (_currentEvent is CameraMoveEvent moveEvent)
            {
                _callback.OnPressedMove(moveEvent.GetOldPosition(), moveEvent.GetNewPosition());

                if (moveEvent.IsFinished())
                {
                    Reset();
                }
                else
                {
                    _currentEvent = _currentEvent is MouseCameraMoveEvent ? new MouseCameraMoveEvent() : new TouchCameraMoveEvent();
                }

                return;
            }

            Reset();
        }

        private void Reset()
        {
            _state = State.WaitInput;
            _currentEvent = null;
        }

        public interface ICallback
        {
            public void OnPressedMove(Vector3 oldPosition, Vector3 newPosition);
            public void OnClick(Vector3 position);

            public void OnNewMousePosition(Vector3 position);

            public void OnRotate(bool clockwise);
        }

        private enum State
        {
            WaitInput,
            CheckClickSuccess,
            CheckPressedMovement
        }

        private abstract class Event
        {
            public abstract bool IsFinished();
        }

        private abstract class ClickEvent : Event
        {
            public abstract Vector3 GetPosition();
        }

        private class MouseClickEvent : ClickEvent
        {
            private readonly long _eventTime;
            private readonly Vector3 _position;

            public MouseClickEvent()
            {
                _eventTime = DateTime.Now.Millisecond;
                _position = Input.mousePosition;
            }

            public override bool IsFinished()
            {
                return !Input.GetMouseButtonDown(ClickButton) && DateTime.Now.Millisecond - _eventTime > 0;
            }

            public override Vector3 GetPosition()
            {
                return _position;
            }
        }

        private class TouchClickEvent : ClickEvent
        {
            private readonly Vector3 _position;

            public TouchClickEvent()
            {
                _position = Input.touches[0].position;
            }

            public override bool IsFinished() //TODO check this shit, maybe doesnt work on phones
            {
                return Input.touches.Length == 0;
            }

            public override Vector3 GetPosition()
            {
                return _position;
            }
        }

        private abstract class CameraMoveEvent : Event
        {
            public abstract Vector3 GetOldPosition();
            public abstract Vector3 GetNewPosition();
        }

        private class MouseCameraMoveEvent : CameraMoveEvent
        {
            private readonly Vector3 _oldPosition;

            public MouseCameraMoveEvent()
            {
                _oldPosition = Input.mousePosition;
            }

            public override bool IsFinished()
            {
                return Input.GetMouseButtonUp(MoveButton);
            }

            public override Vector3 GetOldPosition()
            {
                return _oldPosition;
            }

            public override Vector3 GetNewPosition()
            {
                return Input.mousePosition;
            }
        }

        private class TouchCameraMoveEvent : CameraMoveEvent
        {
            private readonly Vector3 _oldPosition;

            public TouchCameraMoveEvent()
            {
                _oldPosition = TwoTouchesToVector(Input.touches[0], Input.touches[1]);
            }

            public override bool IsFinished()
            {
                return Input.touches.Length != 2;
            }

            private static Vector3 TwoTouchesToVector(Touch firstTouch, Touch secondTouch)
            {
                return (firstTouch.position + secondTouch.position) / 2;
            }

            public override Vector3 GetOldPosition()
            {
                return _oldPosition;
            }

            public override Vector3 GetNewPosition()
            {
                return TwoTouchesToVector(Input.touches[0], Input.touches[1]);
            }
        }
    }
}