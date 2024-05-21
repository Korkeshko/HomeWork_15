using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using System.Threading.Tasks;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private float finalPositionZ = 20f;
    private new Rigidbody rigidbody;
    private CancellationTokenSource cancellationTokenSource;
    private Color defaultColor;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        defaultColor = rigidbody.GetComponent<Renderer>().material.color;
    }

    #region Coroutine
    public IEnumerator MoveCoroutine(float duration)
    {
        yield return rigidbody.transform.DOMoveZ(finalPositionZ, duration)!.WaitForCompletion();
    }
    #endregion
    
    #region Async
    public async Task MoveAsyncStart(float duration)
    {
        cancellationTokenSource = new CancellationTokenSource();  

        CancellationToken cancellationToken = cancellationTokenSource.Token;
        
        await rigidbody.transform.DOMoveZ(finalPositionZ, duration).AsyncWaitForCompletion();       
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
