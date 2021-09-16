using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TapAndTap : Singleton<TapAndTap>
    {
        public Tappable firstTapGO;
        public Tappable secondTapGO;


        public void reqTap(Tappable _tap)
        {
            if (!firstTapGO)
            {
                firstTapGO = _tap;
            } else if (!secondTapGO)
            {
                secondTapGO = _tap;
                firstTapGO.transformTo(secondTapGO.lastTransform);
            } else
            {
                firstTapGO = null;
                secondTapGO = null;
            }
        }
    }
}
