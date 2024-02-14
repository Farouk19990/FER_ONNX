using System.Windows.Forms;
using WinForm.model;

namespace WinForm
{
    public partial class Form1 : Form
    {
        // ONNX model scorer
        readonly yolov8 _yolo = new yolov8() ;
        private readonly OpenFileDialog openFileDialog1;
        private Image selectedImage;
        public Form1()
        {
            InitializeComponent();

            AddResultLabels();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    selectedImage = Image.FromFile(selectedFilePath);
                    pb.Image = selectedImage;
                    string[] imagePaths = new string[] { selectedFilePath };
             
                    var emotionProbabilities = _yolo.DetectEmotionsInImageFiles(imagePaths);
                
                   

                    foreach (var fileWithScore in emotionProbabilities)
                    {
                       
                        Console.Write("* ");

                        for (int i = 0; i < fileWithScore.EmotionProbabilities.Count; i++)
                        {
                            _emotionLabels[i].Text = $@"{fileWithScore.EmotionProbabilities[i].probability:P} {fileWithScore.EmotionProbabilities[i].y}";
                        }
                        Console.ForegroundColor = ConsoleColor.Cyan;

                        Console.WriteLine($"{fileWithScore.Filename}");

                        Console.ForegroundColor = ConsoleColor.Gray;
                       

                    }


                }
            }
     
        }
        readonly Label[] _emotionLabels = new Label[8];
        private void AddResultLabels()
        {
            
            for (var index = 0; index < _emotionLabels.Length; index++)
            {
                var emotionLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Calibri", 13.8F, FontStyle.Bold,
                        GraphicsUnit.Point, 0),
                    Location = new System.Drawing.Point(13, 10),
                    Size = new System.Drawing.Size(200, 30),
                    TabIndex = 0,
                    Text = $@"{"0%".PadRight(4)} {yolov8.OnnxConfig.Labels[index]}"
                };

                _emotionLabels[index] = emotionLabel;
                flp.Controls.Add(_emotionLabels[index]);
            }
        }
    }
}
