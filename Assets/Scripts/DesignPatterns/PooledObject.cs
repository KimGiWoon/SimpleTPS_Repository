using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    // 부모의 역할하는 클래스로 지정
    public abstract class PooledObject : MonoBehaviour
    {
        public ObjectPool ObjPool { get; private set; }

        public void PooledInit(ObjectPool objPool)
        {
            ObjPool = objPool;
        }

        public void ReturnPool()
        {
            ObjPool.PushPool(this);
        }
    }
}

