namespace SharedProgram.Models
{
    public class ExportImagesModel
    {
        public Bitmap? Image { get; private set; }
        public int Index { get; private set; }
        public ExportImagesModel(Bitmap image, int index)
        {
            Image = image;
            Index = index;
        }
    }
}
