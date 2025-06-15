using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class camerameche : MonoBehaviour
{
    public static camerameche instace;
    [SerializeField] private float globalskaeForce=1;

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        // Start is called before the first frame update
    }
    public void camerashake(CinemachineImpulseSource impilseSorce)
    {
        impilseSorce.GenerateImpulseWithForce(globalskaeForce);
    }
}
