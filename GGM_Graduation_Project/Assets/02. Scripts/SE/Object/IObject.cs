using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObject
{
    //void Interaction(GameObject ingredient = null);
    GameObject Interaction(GameObject ingredient = null);
}


/* private void Update()
 {
     if (interactive)
     {
         // 인터랙터 키기. 업데이트 말고 매니져 싱글톤을 하나 만들어서 그걸 켜주는 식으로 하면 좋을 뜻.
     }
 }*/