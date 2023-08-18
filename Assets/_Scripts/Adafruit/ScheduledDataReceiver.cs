// using System.Collections;
// using UnityEngine;
//
// namespace Adafruit
// {
//     public class ScheduledDataReceiver : DataReceiver
//     {
//         public float InvokeTimer = 1f;
//
//         protected override void Start()
//         {
//             base.Start();
//             AutoLog = false;
//             StartCoroutine(PeriodicallyInvoke_CO());
//         }
//
//         private IEnumerator PeriodicallyInvoke_CO()
//         {
//             while (true)
//             {
//                 yield return new WaitForSeconds(InvokeTimer);
//                 LogMessage(LastReceivedMessage);
//             }
//         }
//     }
// }