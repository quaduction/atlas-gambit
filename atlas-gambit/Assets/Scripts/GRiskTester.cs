using GRisk;
using UnityEngine;
using GRisk.Engine;

public class GRiskTester : MonoBehaviour
{
    public GameMaster gameMaster;
    void Start()
    {
        GR gr = gameMaster.engine;
        GRFacade g = gameMaster.facade;
        
        Debug.Log("Hello World");
    }
}
