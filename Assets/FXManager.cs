using UnityEngine;

public class FXManager : MonoBehaviour
{
    public AudioSource OneShotSource;
    public float VolumeScale;
    public AudioClip PickupSound;
    public AudioClip RespawnSound;
    public AudioClip EndgameSound;
    public AudioClip WinSound;

    public void PlayOneShot(int id)
    {
        if (id == 0)
        {
            //star
            OneShotSource.PlayOneShot(PickupSound, VolumeScale);
        }
        else if (id == 1)
        {
            //respawn
            OneShotSource.PlayOneShot(RespawnSound, VolumeScale);
        }
        else if (id == 2)
        {
            //endgame
            OneShotSource.PlayOneShot(EndgameSound, VolumeScale);
        }
        else if (id == 3)
        {
            //win game
            OneShotSource.PlayOneShot(WinSound, VolumeScale);
        }
    }
}
