using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    //用来保存目标的数组
    public Transform[] obj;
    //用来改变数组的值
    private static int i = 0;

    void Start()
    {
    }

    void Update()
    {
        // 让我们的物体朝目标移动
        transform.LookAt(obj[i % obj.Length]);
        transform.Translate(Vector3.forward * Time.deltaTime * 30);
    }

    //改变目标物体
    public static void Add()
    {
        i++;
    }
}
