using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.model
{
    public class OutputModel
    {
        public string Filename { get; set; }

        public List<(string y, float probability)> EmotionProbabilities { get; set; }

    }
}
