using UnityEngine;

namespace Game
{
    public class GlobalController : Singleton<GlobalController>
    {
        [Header("AnimationController")]
        public float startingAnimLenght = .1f;

        [Header("Properties")]
        public float overCookDuration = 1f;
    }
}
