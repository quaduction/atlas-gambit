using TMPro;
using UnityEngine;

namespace GRisk.Interface
{
    [RequireComponent(typeof(TMP_Text))]
    public class FountainNotification : MonoBehaviour
    {
        public TMP_Text fountainText;

        private static readonly int underlayDilate = Shader.PropertyToID("_UnderlayDilate");
        private static readonly float offset = 0.08f;

        private float totalLife = 0;
        private float life = 0;

        private Camera vrCamera;

        private void Awake()
        {
            fountainText = GetComponent<TMP_Text>();
            vrCamera = Camera.main;
        }

        private void Update()
        {
            float msElapsed = Time.deltaTime * 1000.0f;

            life -= msElapsed;

            if (life <= 0)
            {
                Destroy(gameObject);
                return;
            }

            float lifeElapsed = totalLife - life;
            float normalizedTime = Mathf.Clamp01(lifeElapsed / totalLife);

            // fading and "floating away" over lifetime.
            // fading should have a curve of sorts, where it stays mostly opaque
            // then quickly fades away as its life ends

            // 1. Fading (The opacity decreases over time)
            // We can use normalizedTime to interpolate the color's alpha component
            Color startColor = fountainText.color;
            float alpha = 1.0f;

            // Example fading curve: Stay opaque for first 60% of life, then fade quickly.
            if (normalizedTime > 0.6f)
            {
                alpha = Mathf.Lerp(1.0f, 0.0f, (normalizedTime - 0.6f) / 0.2f);
            }

            // Apply the calculated color and alpha
            fountainText.color = startColor * new Color(1, 1, 1, alpha);

            // 2. Floating Away (Optional: slight vertical movement as it dies)
            // You can modify the Y position slightly based on normalizedTime
            float verticalMovement = normalizedTime * (transform.localScale.y / 64);
            transform.localPosition += new Vector3(0, verticalMovement, 0);
        }

        private void updateRotation()
        {
            Vector3 direction = -(vrCamera.transform.position - transform.position);

            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = targetRot;
        }

        public void initialize(string msg, Transform origin, Color color, int duration)
        {
            totalLife = duration;
            life = totalLife;

            fountainText.SetText(msg);
            fountainText.fontSize = 16;
            fountainText.color = color;

            fountainText.alignment = TextAlignmentOptions.Center;
            fountainText.fontMaterial.SetFloat(underlayDilate, 1f);

            transform.position = origin.position + Vector3.up * offset;

            // set sizing here relative to scene somehow, like a target size or something.
            // we're in VR, so a unit of 1 is about 1m, a scale of 0.05 seems to work fine but that's hardcoded
            transform.localScale = Vector3.one * 0.04f;

            updateRotation();
        }

        public static void spawn(string msg, Transform origin, Color? color = null, int duration = 1000)
        {
            color ??= Color.white;

            // instance an object with a proper FountainNotification somehow, better than this?

            GameObject notif = new(
                "FountainNotification",
                typeof(TextMeshPro),
                typeof(FountainNotification)
            );

            notif.GetComponent<FountainNotification>().initialize(msg, origin, (Color)color, duration);
        }
    }
}