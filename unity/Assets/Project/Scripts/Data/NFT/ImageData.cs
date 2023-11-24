namespace Project.Scripts.Data.NFT
{
    /// <summary>
    /// status: 0: image not generated, 1: image generated, 2: image minted
    /// </summary>
    ///
    [System.Serializable]
    public class ImageData
    {
        public int id;
        public string address;
        public float lat;
        public float lng;
        public int status;
        public string image;
        public string createdAt;
        public string updatedAt;
    }
}