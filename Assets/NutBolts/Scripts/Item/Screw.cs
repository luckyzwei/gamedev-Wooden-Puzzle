using System;
using System.Collections.Generic;
using UnityEngine;

namespace NutBolts.Scripts.Item
{
    public class Screw : MonoBehaviour
    {
        public Action<Screw,Hole> OnMove;
        public Animator animator;
        private CircleCollider2D circleCollider;
        private List<Obstacle> joints;
        [HideInInspector]
        public Rigidbody2D rigidboy2D;
        private Lit l;

        private void Start()
        {
     
            TurnOffToolLight();
            rigidboy2D = GetComponent<Rigidbody2D>();
            rigidboy2D.isKinematic = true;
        }
    
        public void DeActivate()
        {
            if (circleCollider == null)
            {
                circleCollider = GetComponent<CircleCollider2D>();
            }
            animator.transform.localPosition = Vector3.zero;
            transform.localPosition = Vector3.zero;
            circleCollider.isTrigger = false;      
            gameObject.SetActive(false);
        }
        public void OnWait()
        {
            animator.ResetTrigger("NormalTrigger");
            animator.SetTrigger("PressTrigger");
       
        }
        public void DeWait()
        {
            animator.ResetTrigger("PressTrigger");
            animator.SetTrigger("NormalTrigger");
        }
        public void Activate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            gameObject.SetActive(true);
            animator.ResetTrigger("NormalTrigger");
            animator.ResetTrigger("PressTrigger");
      
        }
        public void Reactivate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            gameObject.SetActive(true);
            animator.ResetTrigger("NormalTrigger");
            animator.ResetTrigger("PressTrigger");
            var point = transform.position;
            RaycastHit2D[] hitAll = Physics2D.RaycastAll(point, Vector2.zero, 10, 1 << 8);
      
            joints = new List<Obstacle>();
            for(int i=0; i<hitAll.Length; i ++)
            {
                Obstacle o = hitAll[i].collider.GetComponent<Obstacle>();
                SetJoints(o);
                o.JointScrew(this, point);
            }
        }
   
        public void OnRelease()
        {
            if (circleCollider == null)
            {
                circleCollider = GetComponent<CircleCollider2D>();
            }
            circleCollider.isTrigger = true;
            if(joints!=null)
                for(int i=0; i<joints.Count; i++)
                {
                    joints[i].ReleaseScrew(this);
                }

        }
        public void SetJoints(Obstacle joint)
        {
            if (joints == null)
            {
                joints = new List<Obstacle>();
            }
            joints.Add(joint);
        }
        public Lit GetLit()
        {
            return l;
        }
        public void SetLit(Lit lit)
        {
            this.l = lit;
        }
        public void TurnOnToolLight()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        public void TurnOffToolLight()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public enum ScrewState
    {
        Normal=0,
        Waiting=1,
    }
}