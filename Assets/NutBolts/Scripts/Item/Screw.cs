using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NutBolts.Scripts.Item
{
    public class Screw : MonoBehaviour
    {
        public Action<Screw,ScrewHole> onMove;
        [SerializeField] private Animator _animatorContoler;
        [SerializeField] private GameObject _toolLight;
        private CircleCollider2D circleCollider;
        private List<Blocks> joints;
    
        [HideInInspector]
        public Rigidbody2D rigidboy2D;
        public Fire Lit { get; set; }
        
        public bool Light { get => _toolLight.activeSelf; set => _toolLight.SetActive(value); }

        public Animator animator => _animatorContoler;

        private void Start()
        {
            Light = false;
            rigidboy2D = GetComponent<Rigidbody2D>();
            rigidboy2D.isKinematic = true;
        }
    
        public void MakeNotActive()
        {
            if (circleCollider == null)
            {
                circleCollider = GetComponent<CircleCollider2D>();
            }
            _animatorContoler.transform.localPosition = Vector3.zero;
            transform.localPosition = Vector3.zero;
            circleCollider.isTrigger = false;      
            gameObject.SetActive(false);
        }
        public void StartWaiting()
        {
            _animatorContoler.ResetTrigger("NormalTrigger");
            _animatorContoler.SetTrigger("PressTrigger");
       
        }
        public void StopWaiting()
        {
            _animatorContoler.ResetTrigger("PressTrigger");
            _animatorContoler.SetTrigger("NormalTrigger");
        }
        public void Activate()
        {
            if (_animatorContoler == null)
            {
                _animatorContoler = GetComponent<Animator>();
            }
            gameObject.SetActive(true);
            _animatorContoler.ResetTrigger("NormalTrigger");
            _animatorContoler.ResetTrigger("PressTrigger");
      
        }
        public void Reincornate()
        {
            if (_animatorContoler == null)
            {
                _animatorContoler = GetComponent<Animator>();
            }
            gameObject.SetActive(true);
            _animatorContoler.ResetTrigger("NormalTrigger");
            _animatorContoler.ResetTrigger("PressTrigger");
            var point = transform.position;
            RaycastHit2D[] hitAll = Physics2D.RaycastAll(point, Vector2.zero, 10, 1 << 8);
      
            joints = new List<Blocks>();
            for(int i=0; i<hitAll.Length; i ++)
            {
                Blocks o = hitAll[i].collider.GetComponent<Blocks>();
                AssignJoints(o);
                o.ConnectScrew(this);
            }
        }
   
        public void ReleaseScre()
        {
            if (circleCollider == null)
            {
                circleCollider = GetComponent<CircleCollider2D>();
            }
            circleCollider.isTrigger = true;
            if (joints == null) return;
            foreach (var joint in joints)
            {
                joint.RemoveScrew(this);
            }

        }
        public void AssignJoints(Blocks joint)
        {
            joints ??= new List<Blocks>();
            joints.Add(joint);
        }
    }
    public enum ScrewState
    {
        Normal=0,
        Waiting=1,
    }
}