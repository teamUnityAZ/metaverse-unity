using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_movement : MonoBehaviour
{
    // public GameObject wayPoint;
    public List<Transform> wayPoints = new List<Transform>();

    public GameObject npcStaff;
    public int index;
    public Animator npcAnimator;

    public float speed;
    public int currentWayPointID;
    public float rotationSpeed = 5.0f;

    Vector3 last_position;
    Vector3 current_position;
    public float reachDistance;

    public bool movenow;
    public float distance;
    public Quaternion rotation;

    public Transform StaffLookto;





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //movenow = true;
            NPCManager.Instance.m_IsNPCMoving = false;
            NPCManager.Instance.ChangeStaffMemberAnimatorState(true);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        movenow = true;
    //        Gamemanager._InstanceGM._isNPCMoving = false;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //movenow = false;
            NPCManager.Instance.m_IsNPCMoving = true;
            NPCManager.Instance.ChangeStaffMemberAnimatorState(false);
        }
    }




    void Start()
    {
        last_position = transform.position;
        speed = 0.75f;
    }
    
    void Update()
    {
        distance = Vector3.Distance(wayPoints[currentWayPointID].localPosition, transform.localPosition);

        if (NPCManager.Instance.m_IsNPCMoving)
        {
            if (speed != 0.75f)
            {
                speed = 0.75f;
            }

            transform.position = Vector3.MoveTowards(transform.localPosition, wayPoints[currentWayPointID].localPosition, Time.deltaTime * speed);
            transform.LookAt(wayPoints[currentWayPointID].localPosition);

            //npcAnimator.SetFloat("Blend", 1f * speed, speed, Time.deltaTime);
            if (distance <= reachDistance)
            {
                currentWayPointID++;
            }
        }
        else if (!NPCManager.Instance.m_IsNPCMoving)
        {
            //npcAnimator.SetFloat("Blend", 0 * speed, speed, Time.deltaTime);
            StaffLookto.transform.LookAt(Gamemanager._InstanceGM.mainPlayer.transform.position);
            transform.LookAt(StaffLookto);
            speed = 0;
        }

        if (currentWayPointID >= wayPoints.Count)
        {
            currentWayPointID = 0;
        }
    }
}
