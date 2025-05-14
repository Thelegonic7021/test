using UnityEngine;

public class Reiniciar_datos : MonoBehaviour
{
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
