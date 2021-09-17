using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Buyer : MonoBehaviour
    {
        public BuyerType buyerType;
        public Transform charPrefabTransform;
        public Bubble bubbleController;


        public void spawnChar()
        {
            GameObject GO = Instantiate(buyerType.buyerPrefab, charPrefabTransform);
        }
    }
}
