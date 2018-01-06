using UnityEngine;
using System.Collections;

public class Miller_Copy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        knife = GameObject.Find("Knife");
        miller = GameObject.Find("Miller");
        set = GameObject.Find("set");
	}

    GameObject knife, sk, miller, set;
    public GameObject knifehead;
    public GameObject pk;
    float time = 0;

	// Update is called once per frame
    void Update()
    {
		if (miller.GetComponent<Miller_Data_Trans>()==null || (!miller.GetComponent<Miller_Data_Trans>().running)) return;
        time += Time.deltaTime;
        if (time > 0.2f)
        {
            time = 0.0f;
            if (miller.GetComponent<Miller_Data_Trans>().connect && miller.GetComponent<Miller_Control>().zspeed.y==0)
            {
                sk = Instantiate(knife);
                sk.transform.parent = set.transform;

                sk.transform.position = new Vector3(knife.transform.position.x-0.5f, knife.transform.position.y - 0.0239f, knife.transform.position.z);
                if (sk.transform.position.y > 0.5f) Destroy(sk);
            }
        }
    }
}
