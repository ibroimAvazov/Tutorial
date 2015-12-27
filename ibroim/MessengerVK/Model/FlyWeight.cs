using System.Collections.Generic;
using System.Drawing;
using Image = System.Windows.Controls.Image;

namespace MessengerVK
{
    public class FlyWeight
    {
        public abstract class Smile
        {
            public Bitmap smile { get; set; }
            public abstract void Build(string name);
        }

        public class SmileFactory
        {
            IDictionary<string, Bitmap> smiles = new Dictionary<string, Bitmap>();

            public SmileFactory()
            {
                smiles.Add(":[)]", MessengerVK.Properties.Resources._01);
                smiles.Add("(:[(])", MessengerVK.Properties.Resources._02);
                smiles.Add("😊", MessengerVK.Properties.Resources._03);
                smiles.Add("😃", MessengerVK.Properties.Resources._04);
                smiles.Add(":-D", MessengerVK.Properties.Resources._05);
                smiles.Add("😆", MessengerVK.Properties.Resources._06);
                smiles.Add("😜", MessengerVK.Properties.Resources._07);
                smiles.Add("😋", MessengerVK.Properties.Resources._08);
                smiles.Add("😍", MessengerVK.Properties.Resources._09);
                smiles.Add("😎", MessengerVK.Properties.Resources._10);
                smiles.Add("😒", MessengerVK.Properties.Resources._11);
                smiles.Add("😏", MessengerVK.Properties.Resources._12);
            }

            public Bitmap GetSmile(string key)
            {
                if (smiles.ContainsKey(key))
                    return smiles[key];
                else
                    return null;
            }
        }
    }
}