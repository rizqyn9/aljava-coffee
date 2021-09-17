using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Buyer : MonoBehaviour
    {
        public buyerMenuPrototype buyerMenuPrototype;
        public Transform charPrefabTransform;
        public Bubble bubbleController;


        public void spawnChar()
        {
            GameObject GO = Instantiate(buyerMenuPrototype.buyerType.buyerPrefab, charPrefabTransform);
        }
    }
}
