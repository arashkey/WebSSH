﻿using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;

namespace WebSSH.Server
{


    public class CaptchaImageUtils
    {/*
        public static byte[] GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            Random random = new Random();
            using var baseMap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(baseMap);
            graphics.Clear(GetRandomLightColor(random));
            DrawCaptchaCode(width, height, captchaCode, random, graphics);
            DrawDisorderLine(random, width, height, graphics);
            AdjustRippleEffect(baseMap);

            using var ms = new MemoryStream();
            baseMap.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }

        static int GetFontSize(int imageWidth, int captchCodeCount)
        {
            var averageSize = imageWidth / captchCodeCount;

            return Convert.ToInt32(averageSize);
        }

        static Color GetRandomDeepColor()
        {
            Random random = new Random();
            var redlow = 160;
            var greenLow = 100;
            var blueLow = 160;
            return Color.FromArgb(random.Next(redlow), random.Next(greenLow), random.Next(blueLow));
        }

        static Color GetRandomLightColor()
        {
            Random random = new Random();
            var low = 200;
            var high = 255;

            var nRend = random.Next(high) % (high - low) + low;
            var nGreen = random.Next(high) % (high - low) + low;
            var nBlue = random.Next(high) % (high - low) + low;

            return Color.FromArgb(nRend, nGreen, nBlue);
        }

        static void DrawCaptchaCode(int width, int height, string captchaCode, Graphics graphics)
        {
            Random random = new Random();
            SolidBrush fontBrush = new SolidBrush(Color.Black);
            var fontSize = GetFontSize(width, captchaCode.Length);
            var font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            for (int i = 0; i < captchaCode.Length; i++)
            {
                fontBrush.Color = GetRandomDeepColor(random);

                int shiftPx = fontSize / 6;

                var x = i * fontSize + random.Next(-shiftPx, shiftPx) + random.Next(-shiftPx, shiftPx);
                var maxY = height - fontSize;
                if (maxY < 0)
                {
                    maxY = 0;
                }

                var y = random.Next(0, maxY);

                graphics.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
            }
        }

        static void DrawDisorderLine(int width, int height, Graphics graphics)
        {
            Random random = new Random();
            var linePen = new Pen(new SolidBrush(Color.Black), 3);
            for (int i = 0; i < random.Next(3, 5); i++)
            {
                linePen.Color = GetRandomLightColor(random);

                var startPoint = new Point(random.Next(0, width), random.Next(0, height));
                var endPoint = new Point(random.Next(0, width), random.Next(0, height));
                graphics.DrawLine(linePen, startPoint, endPoint);

                //var bezierPoint1 = new Point(random.Next(0, width), random.Next(0, height));
                //var bezierPoint2 = new Point(random.Next(0, width), random.Next(0, height));
                //graphics.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
            }
        }

        static void AdjustRippleEffect(Bitmap baseMap)
        {
        }*/
        public static byte[] GenerateCaptchaImage(int width, int height, string text, string? fontname = null, string? fontSize = null)
        {
            if (fontname == null)
            {
                var fonts = SystemFonts.Collection.Families;
                fontname = fonts.Last().Name;
            }

            int fontsize = string.IsNullOrEmpty(fontSize) ? 15 : int.Parse(fontSize);
            var texts = text.Split('\n');
            var bgcolor = Color.White;
            var fcolor = Color.Black;

            int offset = fontsize + 7;
            width = texts.Max(x => x.Length) * offset;
            height = Math.Max(offset * texts.Length, width / 2);

            Font font = SystemFonts.CreateFont(fontname, fontsize);

            TextOptions textOptions = new(font);
            IPathCollection glyphs = TextBuilder.GenerateGlyphs(text, textOptions);

            width = (int)glyphs.Bounds.Width + 20;
            height = (int)glyphs.Bounds.Height + 20;

            height = Math.Max(height, width / 5);

            using (Image image = new Image<Rgba32>(width, height, Color.White))
            {
                image.Mutate(x => x.DrawText(text, font, Color.Black, new PointF(10, 10)));

                using (var ms = new MemoryStream())
                {
                    image.SaveAsJpeg(ms);
                    return ms.ToArray();
                }
            }
        }
    }


}
