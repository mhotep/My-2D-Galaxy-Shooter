using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ThrusterBar : MonoBehaviour
{
    //[SerializeField]
    public int max;
    //[SerializeField]
    public int current;
    //[SerializeField]
    public Image Mask;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float fillamount = (float)current / (float)max;
        Mask.fillAmount = fillamount;

    }
}
