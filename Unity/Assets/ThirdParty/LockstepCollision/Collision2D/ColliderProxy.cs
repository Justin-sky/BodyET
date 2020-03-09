using Lockstep.UnsafeCollision2D;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D {
    public delegate void FuncOnTriggerEvent(ColliderProxy other, ECollisionEvent type);

    public class ColliderProxy :ILPCollisionEventHandler,ILPTriggerEventHandler{
#if UNITY_EDITOR
        public Transform UnityTransform;
#endif
        public int Id;
        public int LayerType { get; set; }
        public ColliderPrefab Prefab;
        public CTransform2D Transform2D;
        public LFloat Height;
        public bool IsTrigger = true;
        public bool IsStatic = false;


        private LRect _bound;

        public FuncOnTriggerEvent OnTriggerEvent;

        private BoundsQuadTree _quadTree;

        private static int autoIncId = 0;

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y){
            Init(prefab, pos, y, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos){
            Init(prefab, pos, LFloat.zero, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y, LFloat deg){
            this.Prefab = prefab;
            _bound = prefab.GetBounds();
            Transform2D = new CTransform2D(pos, y, deg);
            unchecked {
                Id = autoIncId++;
            }
        }

        public bool _isMoved = true;

        public LVector2 pos {
            get => Transform2D.pos;
            set {
                _isMoved = true;
                Transform2D.pos = value;
            }
        }

        public LFloat y {
            get => Transform2D.y;
            set {
                _isMoved = true;
                Transform2D.y = value;
            }
        }

        public LFloat deg {
            get => Transform2D.deg;
            set {
                _isMoved = true;
                Transform2D.deg = value;
            }
        }


        public LRect GetBounds(){
            return new LRect(_bound.position + pos, _bound.size);
        }

        public virtual void OnLPTriggerEnter(ColliderProxy other){ }
        public virtual void OnLPTriggerStay(ColliderProxy other){ }
        public virtual void OnLPTriggerExit(ColliderProxy other){ }
        public virtual void OnLPCollisionEnter(ColliderProxy other){ }
        public virtual void OnLPCollisionStay(ColliderProxy other){ }
        public virtual void OnLPCollisionExit(ColliderProxy other){ }
    }
    public interface ILPCollisionEventHandler {
        void OnLPTriggerEnter(ColliderProxy other);
        void OnLPTriggerStay(ColliderProxy other);
        void OnLPTriggerExit(ColliderProxy other);
    }
    public interface ILPTriggerEventHandler {
        void OnLPTriggerEnter(ColliderProxy other);
        void OnLPTriggerStay(ColliderProxy other);
        void OnLPTriggerExit(ColliderProxy other);
    }
}