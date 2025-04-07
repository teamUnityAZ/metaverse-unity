using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    [Header("StaffMembers")]
    public GameObject[] m_StaffMembers;

    public bool m_IsNPCMoving;



    void Awake()
    {
        Instance = this;    
    }

    void Start()
    {
        
    }

    public void ChangeStaffMemberAnimatorState(bool l_State)
    {
        for (int i = 0; i < m_StaffMembers.Length; i++)
        {
            m_StaffMembers[i].GetComponent<NPC_movement>().npcAnimator.SetBool("Idle", l_State);
        }
    }

    //private void Update()
    //{
    //    for(int i=0;i<m_StaffMembers.Length;i++)
    //    {
    //        m_StaffMembers[i].transform.GetComponent<NPC_movement>().VirtualUpdateOne();
    //    }
    //}
}
