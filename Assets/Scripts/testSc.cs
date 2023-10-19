using UnityEngine;

public class testSc : MonoBehaviour
{
    [SerializeField] private Sprite Background;
    [SerializeField] private UserStats user;
    [SerializeField] private Stats enemy;
    [SerializeReference] private FightSettings Settings;
    // Start is called before the first frame update
    void Start()
    {
        Fight.StartFight(Background, user, enemy, Settings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
