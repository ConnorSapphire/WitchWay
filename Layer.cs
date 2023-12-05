using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Layer
    {
        public int[,] Data;
        public int id;
        public string name;
        public int Width;
        public int Height;
        public Layer(int[,] data, int id, string name, int width, int height) 
        {
            this.Data = data;
            this.id = id;
            this.name = name;
            this.Width = width;
            this.Height = height;
        }
    }
}
