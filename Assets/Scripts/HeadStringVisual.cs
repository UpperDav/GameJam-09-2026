using UnityEngine;

namespace PuppetHero
{

    [RequireComponent(typeof(LineRenderer))]
    public class HeadStringVisual : MonoBehaviour
    {
        [SerializeField] private HeadString? headString;
        [SerializeField] private Transform? holder;

        private LineRenderer? line;
        private float flashTimer;

        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.useWorldSpace = true;
        }


        private void LateUpdate()
        {
            if (holder == null)
                return;

            line!.SetPosition(0, holder.position);
            line!.SetPosition(1, transform.position);
        }
    }
}
