using System;
using System.Collections;
using UnityEngine;

namespace LFramework
{
    class CoroutineGlobal : MonoSingleton<CoroutineGlobal>
    {
        public void StartInstruction(YieldInstruction instruction, Action action)
        {
            StartCoroutine(RunActionAfterInstruction(instruction, action));
        }

        IEnumerator RunActionAfterInstruction(YieldInstruction instruction, Action action)
        {
            yield return instruction;
            action.Invoke();
        }

        public void StartInstruction(CustomYieldInstruction instruction, Action action)
        {
            StartCoroutine(RunActionAfterInstruction(instruction, action));
        }

        IEnumerator RunActionAfterInstruction(CustomYieldInstruction instruction, Action action)
        {
            yield return instruction;
            action.Invoke();
        }
    }
}
