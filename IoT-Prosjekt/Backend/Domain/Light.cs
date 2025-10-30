namespace Backend.Domain
{
    public class Light : Device
    {
        public int Brightness { get; set; }

        public void SetBrightness(int brightness)
        {
            if (brightness < 0)
            {
                Brightness = 0;
            }
            if (brightness > 100)
            {
                Brightness = 100;
            }
            Brightness = brightness;
        }

    }
}
