using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Target to Follow")]
    [Tooltip("Asigna aquí el Transform del jugador")]
    public Transform target;

    [Header("Clamp X Limits")]
    [Tooltip("Límite mínimo en X donde la cámara puede ir")]
    public float minX = -10f;
    [Tooltip("Límite máximo en X donde la cámara puede ir")]
    public float maxX = 10f;

    [Header("Parallax Backgrounds")]
    [Tooltip("Fondo lejano que hará parallax")]
    public Transform farBackground;
    [Tooltip("Fondo medio que hará parallax")]
    public Transform middleBackground;

    [Header("Parallax Factors")]
    [Range(0f,1f), Tooltip("0 = no se mueve, 1 = mismo movimiento")]
    public float farParallaxFactor = 0.2f;
    [Range(0f,1f), Tooltip("0 = no se mueve, 1 = mismo movimiento")]
    public float middleParallaxFactor = 0.5f;

    [Header("Camera Zoom")]
    [Tooltip("Tamaño ortográfico de la cámara (zoom)")]
    public float orthographicSize = 5f;

    private Camera cam;
    private float lastXPos;
    private float camZ;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraController: no hay Target asignado.");
            enabled = false;
            return;
        }

        cam.orthographicSize = orthographicSize;
        lastXPos = transform.position.x;
        camZ = transform.position.z;
    }

    void LateUpdate()
    {
        // Sincronizar zoom
        if (cam.orthographicSize != orthographicSize)
            cam.orthographicSize = orthographicSize;

        // Mover cámara X con clamp
        float desiredX = Mathf.Clamp(target.position.x, minX, maxX);
        transform.position = new Vector3(desiredX, transform.position.y, camZ);

        // Parallax
        float deltaX = desiredX - lastXPos;
        if (farBackground  != null) farBackground.position  += Vector3.right * (deltaX * farParallaxFactor);
        if (middleBackground != null) middleBackground.position += Vector3.right * (deltaX * middleParallaxFactor);

        lastXPos = desiredX;
    }

    void OnValidate()
    {
        if (cam == null) cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = orthographicSize;
        if (minX > maxX) maxX = minX;
    }
}
