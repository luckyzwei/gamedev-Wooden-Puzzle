using System;
using System.Collections.Generic;
using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using UnityEngine;
using UnityEngine.Serialization;
using VKSdk;
using Zenject;

namespace NutBolts.Scripts.Item
{
    public class Blocks : MonoBehaviour
    {
        [Inject] private ItemsController _itemsController;
        [Inject] private VKAudioController _vkAudioController;
        [Inject] private GameManager _gameManager;
        [FormerlySerializedAs("offset")] [SerializeField] private  Vector2 _offSet;
        [FormerlySerializedAs("attractRigid")] [SerializeField] private Rigidbody2D _rigidbody;
        [FormerlySerializedAs("ScaleX")] public bool _xScale;
        [FormerlySerializedAs("ScaleY")] public bool _yScale;
        private List<ObstacleSide> _sides = new ();
        private Dictionary<string,HingeJoint2D> _joints;
        private List<int> _keyInts;
        private List<GameObject> _maskList;
        private bool _isActive;
        public event Action<Blocks> OnFloorHit;
        public Vector2 Offset => _offSet;
        public ObstacleSide ObstacleSide { get; private set; } = new ();

        private void Awake()
        {
            _isActive = true;
            _rigidbody.isKinematic = true;
            _maskList = new List<GameObject>();
        
            _joints = new Dictionary<string,HingeJoint2D>();
        }

    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Floor")) return;
            OnFloorHit?.Invoke(this);
            _isActive = false;
            gameObject.SetActive(false);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            _vkAudioController.PlaySound($"drop{UnityEngine.Random.Range(1, 3)}");
        }
        public void Construct(Side side,List<Rigidbody2D> screwRigidbody,float maxLength, int Index,Vector3 dir, float head, float tail)
        {
            _sides = new List<ObstacleSide>();
            GetComponent<SpriteRenderer>().sortingOrder = Index;
            _joints = new Dictionary<string,HingeJoint2D>();
            _keyInts = new List<int>();
            SpriteRenderer sp = GetComponent<SpriteRenderer>();
            Vector2 size = sp.size;
            if(_xScale && _yScale)
            {
           
                sp.size = size.normalized * (maxLength) * 2f;
            }else if (_xScale)
            {
                sp.size = new Vector2(maxLength, size.y);
                transform.localPosition += dir * (head - tail) / 4f;
            
            }else if(_yScale)
            {
                sp.size = new Vector2(size.x, maxLength );
                transform.localPosition += dir * (head - tail) / 4f;
            }
       
            for(int i=0; i < screwRigidbody.Count; i++)
            {
                var hingleJoint = gameObject.AddComponent<HingeJoint2D>();

                var key = screwRigidbody[i].GetComponent<Screw>().Lit.iIndex;
                hingleJoint.connectedBody = screwRigidbody[i];
                _keyInts.Add(key);
                _joints.Add(key.ToString(), hingleJoint);
                var maskCircle = _itemsController.TakeItem("circle", screwRigidbody[i].transform.position+Vector3.forward,Quaternion.identity);
                maskCircle.transform.SetParent(transform, true);
                maskCircle.GetComponent<CircleRound>().Construct(Index);
                _maskList.Add(maskCircle);
           
            }
            ObstacleSide = new ObstacleSide();
            ObstacleSide.dots = new List<int>(side.dots);
            ObstacleSide.id = side.id;
            ObstacleSide.eulerAngle = transform.localEulerAngles;
            ObstacleSide.position = transform.localPosition;
        
            Invoke(nameof(JointWiggle),0.1f);
        }

        private void JointWiggle()
        {
            for(int i=0; i<_maskList.Count; i++)
            {
                string key = _keyInts[i].ToString();          
          
                _joints[key].connectedAnchor = _maskList[i].transform.localPosition;
                _joints[key].anchor = _maskList[i].transform.localPosition;

            }
            _rigidbody.isKinematic = false;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            UpdateObstacle();
        }

        public void RemoveScrew(Screw sc) 
        {
            UpdateObstacle();
            int key = -1;
            foreach(var pair in _joints)
            {
                if (pair.Value.connectedBody.GetComponent<Screw>().Lit.iIndex == sc.Lit.iIndex)
                {
                    key = int.Parse(pair.Key);
                    break;
                }
            }
            if (key != -1)
            {
                Destroy(_joints[key.ToString()]);
                _joints.Remove(key.ToString());
                _keyInts.Remove(key);
            }
            _rigidbody.AddTorque(1f);       
        }

        private Quaternion _quaternion;
        private Vector3 _euler;
        public void ConnectScrew(Screw sc)
        {
            var point = transform.InverseTransformPoint(sc.transform.position);
            var hingleJoint = AddJoint();
            hingleJoint.breakAction = JointBreakAction2D.Ignore;
            hingleJoint.connectedBody = sc.rigidboy2D;
            hingleJoint.connectedAnchor = point;
            hingleJoint.anchor = point;
            var keyJoint = sc.Lit.iIndex;
            _keyInts.Add(keyJoint);
            _joints.Add(keyJoint.ToString(), hingleJoint);
            UpdateObstacle();
        }


        private HingeJoint2D AddJoint()
        {       
            return gameObject.AddComponent<HingeJoint2D>();
        }

        private void UpdateObstacle()
        {
            ObstacleSide.dots.Clear();
            foreach(HingeJoint2D joint in _joints.Values)
            {
                ObstacleSide.dots.Add(joint.connectedBody.GetComponent<Screw>().Lit.iIndex);
            }             
            ObstacleSide.eulerAngle = transform.localEulerAngles;
            ObstacleSide.position = transform.localPosition;
        }
        public void Save()
        {
            if (!_isActive) return;
            UpdateObstacle();
            var ob = new ObstacleSide();
            ob.dots = new List<int>(ObstacleSide.dots.ToArray());
            ob.id = this.ObstacleSide.id;
            ob.eulerAngle = this.ObstacleSide.eulerAngle;
            ob.position = this.ObstacleSide.position;
            _sides.Add(ob);
        }
        public void PositionBefore()
        {
            gameObject.SetActive(true);
            if (!_isActive)
            {
                _isActive = true;
                gameObject.SetActive(true);
                Side side = _gameManager.GetSideById(ObstacleSide.id);
                if (side.prefabName != string.Empty)
                {
                    side.dots = new List<int>(ObstacleSide.dots);
                    _gameManager.Field.GenerateSide(side, side.id);
                }
            }

            if (_sides.Count <= 0) return;
            ObstacleSide = _sides[^1];
            _sides.RemoveAt(_sides.Count - 1);
           
            for(int i=0; i<_joints.Count; i++)
            {
                var key = _keyInts[i];
                Destroy(_joints[key.ToString()]);
            }
            _joints.Clear();
            _keyInts.Clear();
            transform.localEulerAngles = ObstacleSide.eulerAngle;
            transform.position = ObstacleSide.position;
            foreach(int d in ObstacleSide.dots)
            {
                var hingleJoint = gameObject.AddComponent<HingeJoint2D>();
                var sc = _gameManager.FindLit(d).Screw;
                var p= transform.InverseTransformPoint(sc.transform.position);
                hingleJoint.connectedBody = sc.rigidboy2D;
                hingleJoint.connectedAnchor= p;
                hingleJoint.anchor = p;
              
                int key = sc.Lit.iIndex;
                _keyInts.Add(key);
                _joints.Add(key.ToString(), hingleJoint);
            }
        }
    }
}
