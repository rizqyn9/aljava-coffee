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
            bubbleController.gameObject.SetActive(false);
        }

        public void showToScene(Vector2 _vector2)
        {
            StartCoroutine(spawn(3f, _vector2));

        }

        IEnumerator spawn(float _delay, Vector2 _vector2)
        {
            gameObject.transform.LeanMove(_vector2, 2f);
            yield return new WaitForSeconds(_delay);
            bubbleController.gameObject.SetActive(true);
        }
    }
}
