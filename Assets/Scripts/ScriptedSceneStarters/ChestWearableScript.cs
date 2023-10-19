using UnityEngine;


public class ChestWearableScript : MonoBehaviour
{
    public Color laserRed = new Vector4 (1.988f, 0.438f, 0.438f, 1.0f);
    public Wearable WearableToGive;

    public bool hasItem = true;

    void OnTriggerStay2D(Collider2D col)
    {
        // Note: the parent of this game object is "PF Props Chest"
        // PF Props Chest (parent)
        //   |__ Shadow
        //   |__ OpenChestGiveWearable (this)
        //Add check to flag in ifStatement, to make it run once (NKD)
        //NVM, way easier to just delete it lmao 
        if(Input.GetKeyUp(KeyCode.E) || Input.GetKeyDown(KeyCode.E)){ //&& hasItem) {
            transform.parent.GetComponent<Renderer>().material.color = laserRed;
            Player.PlayerData.Wearables.Add(WearableToGive);
            hasItem =false;
        }

        // Prevent ourselves from giving the wearable again, so just delete this gameobject
        // we are attached to (which includes this script (component) and other components.
        //or, we can use a flag that activates once it's given  (NKD)
        Destroy(gameObject);
    }
}
