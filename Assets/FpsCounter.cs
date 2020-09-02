using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour {
    public TMP_Text txtObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtObj.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
