using UnityEngine;

public class Suma_de_cassetes : MonoBehaviour
{
    public Recoger_cassete recoger_CasseteScript;
    
    bool verdad = true;
    public bool alcantarilla_abierta = false;

    void Update()
    {
        if (Recoger_cassete.cantidad == 4 && verdad == true){
            alcantarilla_abierta = true;
            Debug.Log("4 Cassetes recolectados");
            verdad = false;
        }
    }
}
