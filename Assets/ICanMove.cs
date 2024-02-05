using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanMove
{
    public void Move();
    public void Flip();
    public void DisableMovement();
}
