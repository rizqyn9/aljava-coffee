using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlobalController : Singleton<GlobalController>
    {
        [Header("AnimationController")]
        public float startingAnimLenght = .1f;
    }
}
