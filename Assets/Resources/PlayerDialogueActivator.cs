using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BerserkPixel.Prata;


public class PlayerDialogueActivator : MonoBehaviour
   {
        private Interaction interaction;

        private void Update()
        {
            if (interaction != null && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        public void ReadyForInteraction(Interaction newInteraction)
        {
            interaction = newInteraction;
        }

        public void Interact()
        {
            if (interaction != null)
            {
                DialogManager.Instance.Talk(interaction);
            }
        }
    }

