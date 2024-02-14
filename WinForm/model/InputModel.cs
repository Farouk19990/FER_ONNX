using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinForm.model.yolov8;

namespace WinForm.model
{
    internal class InputModel
    {
        [ImageType(OnnxConfig.ImageHeight, OnnxConfig.ImageWidth)]
        public MLImage ImageAsBitmap { get; set; }
    }
}
