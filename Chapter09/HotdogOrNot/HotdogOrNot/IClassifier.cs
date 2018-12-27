using System;

namespace HotdogOrNot
{
    public interface IClassifier
    {
        void Classify(byte[] bytes);
        event EventHandler<ClassificationEventArgs> ClassificationCompleted;
    }
}
