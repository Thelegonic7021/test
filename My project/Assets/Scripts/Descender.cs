using UnityEngine;
using System.Collections;

public class Descender : MonoBehaviour
{
    
    private PlatformEffector2D effector;
    public float startWaitTime;
    public float waitedTime;
    public Movimiento movimientoScript;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        
    }

    
    void Update()
    {
        if (movimientoScript.tocar_suelo == true){

            if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)){
                waitedTime = startWaitTime;
            }

            if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
                if(waitedTime<=0){
                    effector.rotationalOffset = 180f;
                    waitedTime = startWaitTime;
                    StartCoroutine(Esperar());
                    
                }
                else{
                    waitedTime -= Time.deltaTime;
                }
            }
        }
    }

    public IEnumerator Esperar(){
        yield return new WaitForSeconds(0.25f);
        effector.rotationalOffset = 0;
    }
}
