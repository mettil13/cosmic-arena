using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public Animator creditsAnimator;
    private void Start()
    {
        creditsAnimator.Play("AnimationCredits");
    }
    void Update()
    {
        //Se preme un qualsiasi tasto, torna al main menu
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }

        //Quando finisce l'animazione, torna al Main Menu
        if (!creditsAnimator.GetCurrentAnimatorStateInfo(0).IsName("AnimationCredits")){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
