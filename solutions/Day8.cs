using System;
using System.Linq;
using System.Collections.Generic;

namespace solutions
{
    public class Day8
    {
        private readonly string digits;
        private readonly int width;
        private readonly int height;

        public Day8(string digits, int width, int height)
        {
            this.digits = digits;
            this.width = width;
            this.height = height;
        }

        public IEnumerable<string> ConvertToLayers()
        {
            int index = 0;
            int layerWidth = width * height;
            while(index + layerWidth <= digits.Length)
            {
                int start = index;
                int end = index + layerWidth;

                index = end;

                yield return digits[start..end];
            }
        }

        public char[,] DecodeImage()
        {
            char[,] output = new char[height, width];
            
            var layers = ConvertToLayers();
            foreach(var layer in layers.Reverse())
            {
                int y = 0;

                for(int i = 0; i < layer.Length; i++)
                {
                    int x = i % width;

                    char pixel = layer[i];
                    
                    output[y, x] = pixel == '2' ? output[y, x] : pixel;

                    if (x == width - 1)
                    {
                        y = y < height - 1 ? ++y : 0;
                    }
                }
            }

            return output;
        }
    }
}