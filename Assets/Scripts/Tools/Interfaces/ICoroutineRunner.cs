
using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{

    Coroutine StartCoroutine(IEnumerator function);
}