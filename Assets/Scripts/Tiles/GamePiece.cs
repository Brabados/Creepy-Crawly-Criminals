using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int _Xpos;
    private int _Ypos;
    private bool _BeingCleared = false;
    private GridController _Grid;
    private GridController.Type _Type;
    private IEnumerator _MoveCoroutine;
    public AnimationClip ClearAnimation;


    public bool moveable;
    public bool clearable;
    public bool coloured;
    public int XPos
    {
        get { return _Xpos; }
        set
        {
            if(moveable)
            {
                _Xpos = value;
            }
        }
    }
    public int YPos
    {
        get { return _Ypos; }
        set
        {
            if (moveable)
            {
                _Ypos = value;
            }
        }
    }

    public bool BeingCleared
    {
        get { return _BeingCleared; }
    }

    public GridController Grid
    {
        get { return _Grid; }
    }

    public GridController.Type Type
    {
        get { return _Type; }
    }

    public virtual void Initalize(int x, int y, GridController grid, GridController.Type type)
    {
        _Xpos = x;
        _Ypos = y;
        _Grid = grid;
        _Type = type;

    }

    public void Move(int NewX, int NewY, float time)
    {
       
        if(_MoveCoroutine != null)
        {
            StopCoroutine(_MoveCoroutine);
        }
        _MoveCoroutine = MoveCoroutine(NewX, NewY, time);
        StartCoroutine(_MoveCoroutine);
        
    }

    private IEnumerator MoveCoroutine(int x, int y, float time)
    {
        _Xpos = x;
        _Ypos = y;
        Vector3 start = transform.localPosition;
        Vector3 end = Grid.Worldposition(x, y);
        for (float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            this.transform.localPosition = Vector3.Lerp(start, end, t / time);
            yield return 0;
        }

        this.transform.localPosition = end;
    }

    private void OnMouseDown()
    {
        if(moveable)
        {
            Grid.ClickAndHold(this);
        }
        
    }

    private void OnMouseEnter()
    {
        Grid.NewSpace(this);
    }

    private void OnMouseUp()
    {
        Grid.Release();
    }

    public void Clear()
    {
        if (this.isActiveAndEnabled)
        {
            _BeingCleared = true;
            StartCoroutine(ClearCoroutine());
        }
    }

    private IEnumerator ClearCoroutine()
    {
        Animator Animation = GetComponent<Animator>();
        if(Animation != null)
        {
            Animation.Play(ClearAnimation.name);
            yield return new WaitForSeconds(ClearAnimation.length / 2);
            Destroy(gameObject);
        }
        else 
        {
            yield return new WaitForSeconds(Grid.FillTime);
            Destroy(gameObject);
        }

    }

}
