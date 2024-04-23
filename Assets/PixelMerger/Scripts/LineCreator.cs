using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField] LineRenderer _line;
    [SerializeField] LayerMask _gameLayer;
    

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D RH2D = Physics2D.Raycast(transform.position,Vector2.down,100,_gameLayer);
        if (RH2D)
        {
            _line.SetPosition(0,transform.position);
            _line.SetPosition(1,RH2D.point);
        }
    }
}
