using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnHandler : MonoBehaviour
    {
        public bool onFilled = false;
        public buyerMenuPrototype buyerMenuPrototype;

        private void Start()
        {
            BuyerManager.Instance.spawnHandlers.Add(this);
        }
    }
}
