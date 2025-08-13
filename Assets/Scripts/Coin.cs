using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody rb;
    public bool isFlipping { get; private set; } // On met le 'set' en private pour un meilleur contrôle
    private CoinResult coinResult;

    // Un event pour notifier d'autres scripts, c'est une excellente pratique !
    public delegate void FlipEndHandler(CoinResult result);
    public event FlipEndHandler OnFlipEnd;

    [SerializeField] private AudioSource CoinTossSound;

    public enum CoinResult
    {
        Pile,
        Face,
        Indeterminee // On le garde pour l'état initial
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Flip()
    {
        if (isFlipping) return;

        StartCoroutine(FlipSequence());
    }

    private IEnumerator FlipSequence()
    {
        isFlipping = true;
        coinResult = CoinResult.Indeterminee;

        // --- PHASE 1 : Lancement Physique ---
        rb.isKinematic = false; // On s'assure que la physique est activée
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // La position de départ, au cas où
        Vector3 startPosition = transform.position;

        // Appliquer des forces pour le spectacle
        CoinTossSound.Play();
        float force = 12f;
        float torque = 20f; 
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        rb.AddTorque(transform.right * torque, ForceMode.Impulse); // Utiliser transform.right est souvent plus intuitif

        // --- PHASE 2 : Décision en plein vol ---
        // Attendre que la pièce atteigne le sommet de sa trajectoire
        yield return new WaitUntil(() => rb.linearVelocity.y < 0);

        // On décide du résultat. C'est ici que tu peux mettre ta logique (50/50, ou truqué si besoin pour le gameplay)
        coinResult = (Random.value > 0.5f) ? CoinResult.Face : CoinResult.Pile;
        Debug.Log("Résultat décidé : " + coinResult);

        // On définit la rotation finale parfaite que l'on veut atteindre
        float targetXRotation = (coinResult == CoinResult.Face) ? 90f : -90f; // -90 (ou 270) pour Pile
        Quaternion targetRotation = Quaternion.Euler(targetXRotation, transform.eulerAngles.y, transform.eulerAngles.z);

        // --- PHASE 3 : Atterrissage Contrôlé ---
        // Attendre que la pièce soit assez proche du sol pour commencer la manoeuvre finale
        float landingAltitude = 0.5f;
        yield return new WaitUntil(() => transform.position.y <= startPosition.y + landingAltitude);

        // LA PARTIE LA PLUS IMPORTANTE : On éteint le moteur physique !
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // On anime la fin de la chute et la rotation en douceur vers la cible
        float landingDuration = 0.2f; // Durée de l'animation d'atterrissage
        float elapsedTime = 0f;

        Vector3 landingPosition = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        Quaternion initialRotationForLerp = transform.rotation;

        while (elapsedTime < landingDuration)
        {
            float t = elapsedTime / landingDuration;
            // On utilise une courbe d'animation pour un atterrissage plus doux (facultatif mais pro)
            t = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease-out

            transform.position = Vector3.Lerp(transform.position, landingPosition, t);
            // Slerp est mieux pour les rotations car il gère les arcs de cercle
            transform.rotation = Quaternion.Slerp(initialRotationForLerp, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Verrouillage final pour une précision parfaite
        transform.position = landingPosition;
        transform.rotation = targetRotation;

        Debug.Log("Atterrissage contrôlé terminé. Résultat final: " + coinResult);
        isFlipping = false;

        // On notifie les autres systèmes que le lancer est terminé
        OnFlipEnd?.Invoke(coinResult);
    }
}
