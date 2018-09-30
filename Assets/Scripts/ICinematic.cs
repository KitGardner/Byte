using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ICinematic
{
    string CinematicName { get; set; }
    Task<bool> PlayCinematic();

}

public class Cinematic : MonoBehaviour, ICinematic
{
    public virtual string CinematicName
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
            throw new System.NotImplementedException();
        }
    }

    public virtual Task<bool> PlayCinematic()
    {
        throw new System.NotImplementedException();
    }

    public virtual void PlayTest()
    {
        throw new System.NotImplementedException();
    }
}
