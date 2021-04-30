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
            print("star oneshot");
            OneShotSource.PlayOneShot(PickupSound, VolumeScale);
        }
        else if (id == 1)
        {
            //respawn
            print("respawn oneshot");
            OneShotSource.PlayOneShot(RespawnSound, VolumeScale);
        }
        else if (id == 2)
        {
            //endgame
            print("lose oneshot");
            OneShotSource.PlayOneShot(EndgameSound, VolumeScale);
        }
        else if (id == 3)
        {
            //win game
            print("win oneshot");
            OneShotSource.PlayOneShot(WinSound, VolumeScale);
        }
    }
}
