using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySineMovement : MonoBehaviour
{

    [SerializeField]
    float _curveSpeed = 5;
    [SerializeField]
    float _moveSpeed = 0.1f;
    [SerializeField]
    private float _curveRadius = 2f;

    private float _fTime = 0;
    private Vector3 vLastPos = Vector3.zero;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SineMovement()
    {
        vLastPos = transform.position;

        _fTime += Time.deltaTime * _curveSpeed;

        Vector3 vSin = new Vector3(Mathf.Sin(_fTime) * _curveRadius, 0, 0);
        Vector3 vLin = new Vector3(_moveSpeed, _moveSpeed, 0);

        transform.position += (vSin + vLin) * Time.deltaTime;
    }
}
