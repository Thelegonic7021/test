using UnityEngine;
using System.Collections;

namespace TMPro.Examples
{
    public class MyCameraController : MonoBehaviour
    {
        public enum CameraModes { Follow, Isometric, Free }

        private Transform cameraTransform;
        private Transform dummyTarget;
        public Transform CameraTarget;

        public float FollowDistance = 30.0f;
        public float MaxFollowDistance = 100.0f;
        public float MinFollowDistance = 2.0f;

        public float ElevationAngle = 30.0f;
        public float MaxElevationAngle = 85.0f;
        public float MinElevationAngle = 0f;

        public float OrbitalAngle = 0f;
        public CameraModes CameraMode = CameraModes.Follow;

        public bool MovementSmoothing = true;
        public bool RotationSmoothing = false;
        private bool previousSmoothing;

        public float MovementSmoothingValue = 25f;
        public float RotationSmoothingValue = 5.0f;
        public float MoveSensitivity = 2.0f;

        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 desiredPosition;
        private float mouseX;
        private float mouseY;
        private Vector3 moveVector;
        private float mouseWheel;

        void Awake()
        {
            if (QualitySettings.vSyncCount > 0)
                Application.targetFrameRate = 60;
            else
                Application.targetFrameRate = -1;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                Input.simulateMouseWithTouches = false;

            cameraTransform = transform;
            previousSmoothing = MovementSmoothing;
        }

        void Start()
        {
            if (CameraTarget == null)
            {
                dummyTarget = new GameObject("Camera Target").transform;
                CameraTarget = dummyTarget;
            }
        }

        void LateUpdate()
        {
            GetPlayerInput();

            if (CameraTarget != null)
            {
                if (CameraMode == CameraModes.Isometric)
                {
                    desiredPosition = CameraTarget.position + Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * new Vector3(0, 0, -FollowDistance);
                }
                else if (CameraMode == CameraModes.Follow)
                {
                    desiredPosition = CameraTarget.position + CameraTarget.TransformDirection(Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * new Vector3(0, 0, -FollowDistance));
                }

                if (MovementSmoothing)
                {
                    cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref currentVelocity, MovementSmoothingValue * Time.fixedDeltaTime);
                }
                else
                {
                    cameraTransform.position = desiredPosition;
                }

                if (RotationSmoothing)
                    cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(CameraTarget.position - cameraTransform.position), RotationSmoothingValue * Time.deltaTime);
                else
                    cameraTransform.LookAt(CameraTarget);
            }
        }

        void GetPlayerInput()
        {
            moveVector = Vector3.zero;
            mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            float touchCount = Input.touchCount;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || touchCount > 0)
            {
                mouseWheel *= 10;

                if (Input.GetKeyDown(KeyCode.I)) CameraMode = CameraModes.Isometric;
                if (Input.GetKeyDown(KeyCode.F)) CameraMode = CameraModes.Follow;
                if (Input.GetKeyDown(KeyCode.S)) MovementSmoothing = !MovementSmoothing;

                if (Input.GetMouseButton(1))
                {
                    mouseY = Input.GetAxis("Mouse Y");
                    mouseX = Input.GetAxis("Mouse X");

                    ElevationAngle = Mathf.Clamp(ElevationAngle - mouseY * MoveSensitivity, MinElevationAngle, MaxElevationAngle);
                    OrbitalAngle = (OrbitalAngle + mouseX * MoveSensitivity) % 360;
                }

                if (touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 delta = Input.GetTouch(0).deltaPosition;
                    ElevationAngle = Mathf.Clamp(ElevationAngle - delta.y * 0.1f, MinElevationAngle, MaxElevationAngle);
                    OrbitalAngle = (OrbitalAngle + delta.x * 0.1f) % 360;
                }

                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 300, 1 << 10 | 1 << 11 | 1 << 12 | 1 << 14))
                    {
                        if (hit.transform == CameraTarget)
                            OrbitalAngle = 0;
                        else
                        {
                            CameraTarget = hit.transform;
                            OrbitalAngle = 0;
                            MovementSmoothing = previousSmoothing;
                        }
                    }
                }

                if (Input.GetMouseButton(2))
                {
                    if (dummyTarget == null)
                    {
                        dummyTarget = new GameObject("Camera Target").transform;
                        dummyTarget.position = CameraTarget.position;
                        dummyTarget.rotation = CameraTarget.rotation;
                        CameraTarget = dummyTarget;
                        previousSmoothing = MovementSmoothing;
                        MovementSmoothing = false;
                    }
                    else if (dummyTarget != CameraTarget)
                    {
                        dummyTarget.position = CameraTarget.position;
                        dummyTarget.rotation = CameraTarget.rotation;
                        CameraTarget = dummyTarget;
                        previousSmoothing = MovementSmoothing;
                        MovementSmoothing = false;
                    }

                    mouseY = Input.GetAxis("Mouse Y");
                    mouseX = Input.GetAxis("Mouse X");
                    moveVector = cameraTransform.TransformDirection(mouseX, mouseY, 0);
                    dummyTarget.Translate(-moveVector, Space.World);
                }
            }

            if (touchCount == 2)
            {
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);
                float prev = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
                float curr = (t0.position - t1.position).magnitude;
                float delta = prev - curr;
                FollowDistance = Mathf.Clamp(FollowDistance + delta * 0.25f, MinFollowDistance, MaxFollowDistance);
            }

            if (Mathf.Abs(mouseWheel) > 0.01f)
                FollowDistance = Mathf.Clamp(FollowDistance - mouseWheel * 5.0f, MinFollowDistance, MaxFollowDistance);
        }
    }
}
