using Cinemachine;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    [SerializeField] private float globalShakeForce = 1f;
    
    public void ScreenShake()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(10);
    }
}
