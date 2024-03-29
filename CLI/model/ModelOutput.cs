﻿using Microsoft.ML.Transforms.Image;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CLI.FacialExpressionDetector;

namespace CLI
{
    internal class ModelOutput
    {
        public string Filename { get; set; }

        public List<(string emotion, float probability)> EmotionProbabilities { get; set; }
    }
}
