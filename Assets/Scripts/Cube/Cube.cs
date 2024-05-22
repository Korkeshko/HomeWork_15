using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private float finalPositionZ = 20f;
    private new Rigidbody rigidbody;
    private CancellationTokenSource cancellationTokenSource;
    private Color defaultColor;
    private Tween tweenCoroutine;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        defaultColor = rigidbody.GetComponent<Renderer>().material.color;
    }

    #region Coroutine
    public IEnumerator MoveCoroutine(float duration)
    {
        tweenCoroutine = rigidbody.transform.DOMoveZ(finalPositionZ, duration);
        yield return tweenCoroutine.WaitForCompletion();

        // Старый вариант (при уничтожении объекта DOTween кидает Warning)
        //yield return rigidbody.transform.DOMoveZ(finalPositionZ, duration).WaitForCompletion();
    }
    #endregion
    
    #region Async
    public async Task MoveAsyncStart(float duration)
    {
        cancellationTokenSource = new CancellationTokenSource();  
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        
        TweenerCore<Vector3, Vector3, VectorOptions> tweenAsync = rigidbody.transform.DOMoveZ(finalPositionZ, duration);
        var task = tweenAsync.AsyncWaitForCompletion();
        
        await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken));
        if (cancellationToken.IsCancellationRequested)
        {
            tweenAsync.Kill();
        }

        // Старый вариант (при уничтожении объекта DOTween кидает Warning)
        //await rigidbody.transform.DOMoveZ(finalPositionZ, duration).AsyncWaitForCompletion(); 

        cancellationToken.ThrowIfCancellationRequested();
    }
    #endregion

    #region Mouse events
    public void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    { 
        if (tweenCoroutine != null && tweenCoroutine.active) 
        {
            tweenCoroutine.Kill();
        }
        
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }    
    } 

    public void OnMouseEnter()
    {     
        Color color = new (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        rigidbody.GetComponent<Renderer>().material.color = color;
    }

    public void OnMouseExit()
    {
        rigidbody.GetComponent<Renderer>().material.color = defaultColor;
    }  
    #endregion 
}