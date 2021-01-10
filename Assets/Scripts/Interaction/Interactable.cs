using UnityEngine;
using UnityEngine.EventSystems;

// This is one of the core features of the game.
// Each one acts like a hub for all things that transpire
// over the course of the game.
// The script must be on a gameobject with a collider and
// an event trigger.  The event trigger should tell the
// player to approach the interactionLocation and the 
// player should call the Interact function when they arrive.

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ForceMenuTarget))]
public class Interactable : MonoBehaviour
{
	public enum ForceType{
		WATER,
		WIND,
		ICE,
		ELECTRICITY,
        NONE
	}


    public Transform interactionLocation;                   // The position and rotation the player should go to in order to interact with this Interactable.
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
                                                            // All the different Conditions and relevant Reactions that can happen based on them.
	public ReactionSet defaultWindReactionCollection;
	public ReactionSet defaultWaterReactionCollection;
	public ReactionSet defaultElecReactionCollection;
	public ReactionSet defaultIceReactionCollection;

    private GameObject windSystem;

    public void Start() {
        windSystem = Camera.main.transform.GetChild(2).gameObject;
    }


    public void Interact (ForceType force)
	{
		InteractLoop(force);

		//The Default reactions ALWAYS play, so use condition collections to check if conditions aren't met yet

		switch(force){
			case ForceType.ELECTRICITY:
				defaultElecReactionCollection.React();
			break;

			case ForceType.ICE:
				defaultIceReactionCollection.React();
			break;

			case ForceType.WATER:
				defaultWaterReactionCollection.React();
			break;

			case ForceType.WIND:
                windSystem.transform.position = interactionLocation.position;
                defaultWindReactionCollection.React();
			break;

			default: // if there is no force, something went wrong!
			break;
		}

	}

	public void InteractLoop(ForceType force){
		// Go through all the ConditionCollections...
		for (int i = 0; i < conditionCollections.Length; i++)
		{
			// ... then check and potentially react to each.  If the reaction happens, exit the function.
			if (conditionCollections[i].CheckAndReact (force)){
				//Debug.Log("Reaction found!");
				return;
			}
				
		}

	}
		
}
