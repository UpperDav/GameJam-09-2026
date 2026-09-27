using UnityEngine;

namespace PuppetHero
{
    public class Publicmovement : MonoBehaviour
    {
        public Gauge? gauge;

        [SerializeField] public float speed = 50f;

        [SerializeField] public Vector2 direction = Vector2.up;
        public Vector2 positionInitiale;
        private bool publicPresent = false;

        private bool publicGone = true;

        [SerializeField] private AudioSource? audioSource;


        void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.1f;
        }

        void Start()
        {


        }

        void Update()
        {
            if (gauge?.PublicHappy == true)
            {

                if (publicPresent == true)
                {



                    transform.Translate(direction * speed * Time.deltaTime);

                    if (direction == Vector2.up && transform.position.y >= positionInitiale.y)
                    {
                        direction = Vector2.down;

                    }
                    else if (direction == Vector2.down && transform.position.y <= positionInitiale.y - 1f)
                    {
                        direction = Vector2.up;

                    }
                }

                else if (publicPresent == false && publicGone == true)
                {
                    transform.Translate(positionInitiale);
                    publicPresent = true;

                    publicGone = false;

                }

            }
            else
            {
                publicGone = true;

                if (transform.position.y >= -8f)
                {

                    direction = Vector2.down;
                    transform.Translate(direction * speed * Time.deltaTime);
                }
                else if (transform.position.y <= -7f)
                {
                    direction = Vector2.up;
                    transform.Translate(direction * speed * Time.deltaTime);

                }

            }
        }

        public void StartCheers()
        {
            audioSource!.volume = 1f;
        }

        public void StopCheers()
        {
            audioSource!.volume = 0.1f;
        }
    }
}
