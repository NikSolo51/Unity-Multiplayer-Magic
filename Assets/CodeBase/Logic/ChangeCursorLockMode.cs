using UnityEngine;

namespace CodeBase.Logic
{
    public class ChangeCursorLockMode : MonoBehaviour
    {
        [SerializeField]private CursorLockMode _cursorLockMode;

        public void SetCursorMode()
        {
            Cursor.lockState = _cursorLockMode;
        }
    }
}