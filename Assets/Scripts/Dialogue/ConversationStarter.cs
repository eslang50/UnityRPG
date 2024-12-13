using DialogueEditor;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ConversationManager.Instance.EndConversation();

    }
}
