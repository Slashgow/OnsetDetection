using UnityTimer;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using FirstGearGames.SmoothCameraShaker;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private LayerMask aimColliderLayerMask;

    [SerializeField]
    private PoolingSystem projectilePoolingSystem;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private PoolingSystem muzzlePoolingSystem;

    [SerializeField]
    private Transform gunEndTransform;

    [SerializeField]
    private float delayBeforeHiddingMuzzleFlash;

    [SerializeField]
    private ShakeData shootShake;

    public event Action<Note> OnHitNote;

    private Vector2 ScreenCenterPoint;
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ScreenCenterPoint = new Vector2(Screen.width / 2f , Screen.height / 2f);
            Ray ray = mainCamera.ScreenPointToRay(ScreenCenterPoint);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 999f, aimColliderLayerMask))
            {
                if(hitInfo.transform.TryGetComponent(out Note note))
                {
                    UpdateNoteInfo(note);
                }

                CameraShakerHandler.Shake(shootShake);
                SpawnProjectile(hitInfo);
                audioSource.Play();
                SpawnMuzzleFlash();
            }
        }
    }

    private void UpdateNoteInfo(Note note)
    {
        note.WasHit = true;
        note.NoteHitClassification = NoteHitRater.Instance.GetHitClassification(note.TimeRemainingBeforeHit);
        OnHitNote?.Invoke(note);
        note.PoolingSystem.AddToPool(note.gameObject);
    }

    private void SpawnMuzzleFlash()
    {
        GameObject muzzleFlashInstance = muzzlePoolingSystem.GetPrefabFromPool();
        muzzleFlashInstance.transform.SetPositionAndRotation(gunEndTransform.position, gunEndTransform.rotation);
        Timer.Register(delayBeforeHiddingMuzzleFlash, () => muzzlePoolingSystem.AddToPool(muzzleFlashInstance));
    }

    private void SpawnProjectile(RaycastHit hitInfo)
    {
        GameObject projectileGameObjectInstance = projectilePoolingSystem.GetPrefabFromPool();
        Projectile projectileInstance = projectileGameObjectInstance.GetComponent<Projectile>();
        projectileInstance.transform.SetPositionAndRotation(gunEndTransform.position, gunEndTransform.rotation);
        projectileInstance.Setup(hitInfo.point, projectilePoolingSystem);
    }
}
