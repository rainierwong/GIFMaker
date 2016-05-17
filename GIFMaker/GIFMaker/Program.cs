using System;
using System.Collections.Generic;
using System.IO;
using Gif.Components;
using System.Drawing;

namespace GIFMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("图片位置，回车两次结束输入图片");
            var str = string.Empty;
            var outputPath = "E://test.gif";
            var pathes = new List<string>();
            do
            {
                str = Console.ReadLine();
                if (File.Exists(str)) pathes.Add(str);
                else break;
            } while (true);
            Console.WriteLine("开始合成...");

            var e = new AnimatedGifEncoder();
            e.Start(outputPath);
            e.SetDelay(200);
            e.SetRepeat(0);

            for (int i = 0; i < pathes.Count; i++)
                e.AddFrame(Image.FromFile(pathes[i]));
            e.Finish();

            Console.WriteLine("已输出至'{0}', 任意键退出", outputPath);
            Console.ReadKey();
        }
    }
}
