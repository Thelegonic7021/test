using UnityEngine;

public class Abrir_alcantarilla : MonoBehaviour
{
    public Suma_de_cassetes cassetesScript;
    public GameObject alcantarilla1;
    public GameObject alcantarilla2;
    void Update()
    {
        if(cassetesScript.alcantarilla_abierta == true){
            alcantarilla1.SetActive(false);
            alcantarilla2.SetActive(true);
        }
    }
}
