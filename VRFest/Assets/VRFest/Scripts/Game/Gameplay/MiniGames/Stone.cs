using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public GameObject stonePrefab;
    public Transform position;

    public void Fly(HVRGrabberBase baseGrabberBase, HVRGrabbable grabbable)
    {
        var stone = Instantiate(stonePrefab, grabbable.gameObject.transform.position, Quaternion.identity);
        var dir = position.position - grabbable.transform.position;
        stone.GetComponent<Rigidbody>().AddForce(dir * 375);
    }
}
