/* using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float minXClamp;
    [SerializeField] float maxXClamp;
    [SerializeField] float minYClamp;
    [SerializeField] float maxYClamp;

    [Range(0, 1)]
    public float smoothTime;

    Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
       if (!GameManager.Instance || !GameManager.Instance.PlayerInstance) return;

        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.y, minYClamp, maxYClamp);

        transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, smoothTime);
    }
}





/* public class Camera : MonoBehaviour
{

    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}*/




//CLASS SCRIPT

/*public class Camera : MonoBehaviour
{

    [SerializedField] Transform Player;
    [SerializedField] float minXClamp;
    [SerializedField] float minYClamp;
    [SerializedField] float maxYClamp;

   private void LateUpdates()
{
    Vector3 cameraPos = transform.position;

    cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);
    cameraPos.y = Mathf.Clamp(player.transform.position.y, minYClamp, maxYClamp);

    transform.position = cameraPos;
}




// Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
} */
