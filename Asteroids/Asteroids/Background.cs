using System;
using System.Collections.Generic;
using System.Drawing;

namespace Asteroids
{
    class Background : ImageObject
    {
        protected List<ImageObject> BackgroundList;
        public Background(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize) : base(pos, dir, size, image, graphic, screenSize)
        {
            BackgroundList = new List<ImageObject>();
            for (int x = pos.X; x <= size.Width + image.Width; x += image.Width)
                for (int y = pos.Y; y < size.Height; y += image.Height)
                    BackgroundList.Add(new ImageObject(new Point(x, y), dir, image.Size, image, graphic, screenSize));
        }

        public override void Draw()
        {
            foreach (ImageObject img in BackgroundList)
                img.Draw();
        }

        public override void Update()
        {
            foreach (ImageObject img in BackgroundList)
                img.Update(-Image.Width);
        }
    }
}
