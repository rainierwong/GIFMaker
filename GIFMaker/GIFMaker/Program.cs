using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GIFMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("图片位置，回车两次结束输入图片");
            var str = string.Empty;
            var savePath = "E://1.gif";
            var pathes = new List<string>();
            do
            {
                str = Console.ReadLine();
                if (File.Exists(str)) pathes.Add(str);
                else break;
            } while (true);
            Console.WriteLine("开始合成...");

            using (var img = Image.FromFile(pathes[0]))
            {
                using (var eps = new EncoderParameters(1))
                {
                    eps.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                    img.Save(savePath, ImageCodecInfo.GetImageEncoders().FirstOrDefault(it => it.FormatID == ImageFormat.Gif.Guid), eps);

                    eps.Param[0].Dispose();
                }

                //start loop
                var temp = Image.FromFile(pathes[1]);
                var eps1 = new EncoderParameters(1);
                eps1.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionTime);
                img.SaveAdd(temp, eps1);
                //stop loop
                temp.Dispose();

                var loopCount = img.GetPropertyItem(0x5101);
                loopCount.Value = new byte[] { 0, 0, 0, 0 };
            }

            var bytes = File.ReadAllBytes(savePath);
            var delayByte = BitConverter.GetBytes(1);
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                if (bytes[i] == 0x21 && bytes[i + 1] == 0xf9)
                {
                    bytes[i + 4] = delayByte[0];
                    bytes[i + 5] = delayByte[1];
                }
            }


            Console.WriteLine("已输出至'{0}', 任意键退出", savePath);
            Console.ReadKey();
        }
    }
}
