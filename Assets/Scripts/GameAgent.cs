﻿using System;
using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;
using Random = System.Random;
using Vector2 = RVO.Vector2;

public class GameAgent : MonoBehaviour
{
    [HideInInspector] public int sid = -1;

    public bool isArrived = false;

    /** Random number generator. */
    private Random m_random = new Random();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if (sid >= 0)
        {
            Vector2 pos = Simulator.Instance.getAgentPosition(sid);
            Vector2 vel = Simulator.Instance.getAgentPrefVelocity(sid);
            transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
            if (Math.Abs(vel.x()) > 0.01f && Math.Abs(vel.y()) > 0.01f)
                transform.forward = new Vector3(vel.x(), 0, vel.y()).normalized;
        }

        if (isArrived)
        {
            //到达目的地，静止状态 
            Simulator.Instance.setAgentPrefVelocity(sid, new Vector2(0, 0));
            if (sid == 0)
            {
                Debug.Log($" speed: {Simulator.Instance.getAgentVelocity(sid)}");
            }
            Simulator.Instance.setAgentVelocity(sid, new Vector2(0, 0)); //设置速度为0，完全静止状态
            Simulator.Instance.setAgentRadius(sid, 10.0f);
            Simulator.Instance.setAgentMaxSpeed(sid, 0f);

            return;
        }
        else
        {
            Simulator.Instance.setAgentRadius(sid, 2.0f);
            Simulator.Instance.setAgentMaxSpeed(sid, 2.0f);
        }


        // if (!Input.GetMouseButton(1))
        // {
        //     Simulator.Instance.setAgentPrefVelocity(sid, new Vector2(0, 0));
        //     return;
        // }

        Vector2 goalVector = GameMainManager.Instance.mousePosition - Simulator.Instance.getAgentPosition(sid);
        if (RVOMath.absSq(goalVector) > 1.0f)
        {
            goalVector = RVOMath.normalize(goalVector);
        }

        Simulator.Instance.setAgentPrefVelocity(sid, goalVector);
        
        /* Perturb a little to avoid deadlocks due to perfect symmetry. */
        float angle = (float) m_random.NextDouble()*2.0f*(float) Math.PI;
        float dist = (float) m_random.NextDouble()*0.0001f;
        
        Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
                                                     dist*
                                                     new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)));
    }
}