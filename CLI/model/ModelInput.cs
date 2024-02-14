using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CLI.FacialExpressionDetector;

namespace CLI.model
{
    public class ModelInput
    {
        [ImageType(FERPlusOnnxConfig.ImageHeight, FERPlusOnnxConfig.ImageWidth)]
        public MLImage ImageAsBitmap { get; set; }
    }
}
