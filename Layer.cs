using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchWay
{
    internal class Layer
    {
        public int[,] data;
        public int id;
        public string name;
        public int width;
        public int height;
        public Layer(int[,] data, int id, string name, int width, int height) 
        {
            this.data = data;
            this.id = id;
            this.name = name;
            this.width = width;
            this.height = height;
        }
    }
}
