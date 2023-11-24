using Cysharp.Threading.Tasks;
using Web3Hackathon.ScreenSystem;

namespace Web3Hackathon
{
    public class NFTGameScreenModel
    {
        public void BackToGame()
        {
            ScreenManager.Instance.ChangeScreen(ScreenEnum.InGame).Forget();
        }
    }
}