using UnityEngine;

namespace CodeBase.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis => UnityAxis();

        public override Vector2 ViewAxis
        {
            get => MouseAxis();
        }

        public override bool IsJumpButtonDown()
        {
            return UnityEngine.Input.GetButtonDown("Jump");
        }

        public override bool IsClickButtonDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public override bool IsRightClickButtonDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(1);
        }

        public override bool IsRightClickButtonUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(1);
        }

        public override bool IsClickButtonPress()
        {
            return UnityEngine.Input.GetMouseButton(0);
        }

        public override float ScrollAxis
        {
            get
            {
                if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
                {
                    return 1;
                }
                else if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}