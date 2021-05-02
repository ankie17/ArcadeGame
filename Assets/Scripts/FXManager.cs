using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource OneShotSource;
    public float VolumeScale;
    [SerializeField]
    private AudioClip PickupSound;
    [SerializeField]
    private AudioClip RespawnSound;
    [SerializeField]
    private AudioClip EndgameSound;
    [SerializeField]
    private AudioClip WinSound;

    public void PlayOneShot(int id)
    {
        if (id == 0)
        {
            OneShotSource.PlayOneShot(PickupSound, VolumeScale);
        }
        else if (id == 1)
        {
            OneShotSource.PlayOneShot(RespawnSound, VolumeScale);
        }
        else if (id == 2)
        {
            OneShotSource.PlayOneShot(EndgameSound, VolumeScale);
        }
        else if (id == 3)
        {
            OneShotSource.PlayOneShot(WinSound, VolumeScale);
        }
    }
}
