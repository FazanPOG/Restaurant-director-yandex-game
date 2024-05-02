using UnityEngine;

namespace Modules.Common.Scripts.Interfaces
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Mode mode;

        private Camera cam;

        private void Start() => cam = Camera.main;

        private void LateUpdate()
        {
            switch (mode)
            {
                case Mode.LookAt:
                    transform.LookAt(cam.transform.position);
                    break;
                case Mode.LookAtInverted:
                    Vector3 dirFromCamera = transform.position - cam.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case Mode.CameraForward:
                    transform.forward = cam.transform.forward;
                    break;
                case Mode.CameraForwardInverted:
                    transform.forward = -cam.transform.forward;
                    break;
            }
        }

        private enum Mode
        {
            LookAt,
            LookAtInverted,
            CameraForward,
            CameraForwardInverted,
        }
    }
}
