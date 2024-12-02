using UnityEngine;
public interface IMovablePlatform
{
     public void OnTriggerEnter(Collider other);
     public void OnTriggerExit(Collider other);
}
