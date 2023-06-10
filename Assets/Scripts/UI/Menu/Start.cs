using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class Start : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}
