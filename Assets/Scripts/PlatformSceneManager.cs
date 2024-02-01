using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
        SceneManager.LoadScene("SampleSceneIOS");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
