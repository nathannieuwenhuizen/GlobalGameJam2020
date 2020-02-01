using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Menu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerText;

    [SerializeField]
    private Slider playerSlider;

    [SerializeField]
    private TextMeshProUGUI lapsText;

    [SerializeField]
    private Slider lapsSlider;

    void Start()
    {
        lapsSlider.value = Globals.AMOUNT_LAPS;
        playerSlider.value = Globals.AMOUNT_PLAYERS;
        playerText.text = "player: " + Globals.AMOUNT_PLAYERS;
        lapsText.text = "laps: " + Globals.AMOUNT_LAPS;

    }

    public void UpdateSettings()
    {
        Globals.AMOUNT_LAPS = (int)lapsSlider.value;
        Globals.AMOUNT_PLAYERS = (int)playerSlider.value;

        playerText.text = "player: " + Globals.AMOUNT_PLAYERS;
        lapsText.text = "laps: " + Globals.AMOUNT_LAPS;

    }
}
