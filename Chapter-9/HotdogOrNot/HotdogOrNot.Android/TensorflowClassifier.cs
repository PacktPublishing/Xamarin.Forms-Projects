using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Tensorflow.Contrib.Android;

namespace HotdogOrNot.Droid
{
    public class TensorflowClassifier : IClassifier
    {
        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public void Classify(byte[] bytes)
        {
            var assets = Application.Context.Assets;

            var inferenceInterface = new TensorFlowInferenceInterface(assets, "hotdog-or-not-model.pb");

            var sr = new StreamReader(assets.Open("hotdog-or-not-labels.txt"));
            var labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim())
                                        .Where(s => !string.IsNullOrEmpty(s)).ToList();

            var bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, 227, 227, false)
                               .Copy(Bitmap.Config.Argb8888, false);

            var floatValues = new float[227 * 227 * 3];
            var intValues = new int[227 * 227];

            resizedBitmap.GetPixels(intValues, 0, 227, 0, 0, 227, 227);

            for (int i = 0; i < intValues.Length; ++i)
            {
                var val = intValues[i];
                floatValues[i * 3 + 0] = ((val & 0xFF) - 104);
                floatValues[i * 3 + 1] = (((val >> 8) & 0xFF) - 117);
                floatValues[i * 3 + 2] = (((val >> 16) & 0xFF) - 123);
            }

            var outputs = new float[labels.Count];
            inferenceInterface.Feed("Placeholder", floatValues, 1, 227, 227, 3);
            inferenceInterface.Run(new[] { "loss" });
            inferenceInterface.Fetch("loss", outputs);

            var result = new Dictionary<string, float>();

            for (var i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                result.Add(label, outputs[i]);
            }

            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));
        }
    }
}