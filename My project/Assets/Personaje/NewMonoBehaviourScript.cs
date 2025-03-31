using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class NewMonoBehaviourScript : MonoBehaviour
{
    private Animator _animator;

    void Awake(){
        _animator = GetComponent<Animator>();
    }
        
    void Start()
    {
        
    }

    
    void Update()
    {
        if ((Input.GetKey(KeyCode.RightArrow)) || Input.GetKey(KeyCode.D)){
            
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = false;
            this.GetComponent<Transform>().position += new Vector3((float) 0.035, 0, 0);
        }


        if ((Input.GetKey(KeyCode.A)) || Input.GetKey(KeyCode.LeftArrow)){
            
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = true;
        this.GetComponent<Transform>().position += new Vector3((float) -0.035, 0, 0);
        }
        if ((Input.GetKeyDown(KeyCode.W)) || Input.GetKeyDown(KeyCode.UpArrow)){
            
            _animator.Play("Jump");
            _animator.Update(Time.deltaTime);
        this.GetComponent<Transform>().position += new Vector3(0, (float) 0.055, 0);
        }

        if ((Input.GetKey(KeyCode.S)) || Input.GetKey(KeyCode.DownArrow)){
            
            _animator.Play("Idle");
            _animator.Update(Time.deltaTime);
        this.GetComponent<Transform>().position += new Vector3(0, (float) -0.035, 0);
        }

        if (Input.GetKeyDown(KeyCode.J)){
            _animator.Play("attack");
        }

        
    }
}
