using CommonObjects;
using FaceRecognitionDatabase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FaceRecognition.DBStorage
{
    public class DatabaseDAL : ITrainDataDAL, IImageDAL
    {     
        public DatabaseDAL()
        {
        }

        public IEnumerable<ImageLabel> GetImages()
        {
            using (var context = new FaceRecognitionContext())
            {
                var imageLabels = context.ImageLabels.AsNoTracking();
                return imageLabels.ToList();
            }
        }

        public Dictionary<string, int> GetLabelMap()
        {
            var labelsDict = new Dictionary<string, int>();
            using (var context = new FaceRecognitionContext())
            {
                labelsDict = context.ImageLabels.AsNoTracking()
                       .ToDictionary(a => a.Label, b => b.ImageLabelId);
            }
            labelsDict["unknown"] = 0;
            return labelsDict;
        }

        public ImageLabel Get(int id)
        {
            using (var context = new FaceRecognitionContext())
            {
                return context.ImageLabels.FirstOrDefault(e => e.ImageLabelId == id);
            }
        }

        public void Add(IEnumerable<ImageLabel> images)
        {
            using (var context = new FaceRecognitionContext())
            {
                context.ImageLabels.AddRange(images);
                context.SaveChanges();
            }
        }

        public void Add(ImageLabel image)
        {
            using (var context = new FaceRecognitionContext())
            {
                context.ImageLabels.Add(image);
                context.SaveChanges();
            }           
        }

        public void Delete(int id)
        {
            using (var context = new FaceRecognitionContext())
            {
                var imageLabelToDelete = context.ImageLabels.FirstOrDefault(e => e.ImageLabelId == id);
                if (imageLabelToDelete != null)
                {
                    context.ImageLabels.Remove(imageLabelToDelete);
                    context.SaveChanges();
                }
            }
        }

        public void SetLabel(int imageLabelId, string label)
        {
            using (var context = new FaceRecognitionContext())
            {
                var imageLabelToSetLabel = context.ImageLabels.FirstOrDefault(e => e.ImageLabelId == imageLabelId);
                if (imageLabelToSetLabel != null)
                {
                    imageLabelToSetLabel.Label = label;
                    context.SaveChanges();
                }
            }
        }
    }
}
