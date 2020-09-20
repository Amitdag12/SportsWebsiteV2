using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebsiteV2
{
    public class ExerciseList
    {
        public Dictionary<string, List<Exercise>> ExerciseDictionary;

        public ExerciseList()
        {
            if (ExerciseDictionary == null)
            {
                ExerciseDictionary = new Dictionary<string, List<Exercise>>();
            }
        }

        public void AddExercise(string MuscleGroup, string name, bool IsCompund)
        {
            Exercise exercise = new Exercise(name, IsCompund);
            List<Exercise> ExcList = ExerciseDictionary[MuscleGroup];
            ExcList.Add(exercise);
            ExerciseDictionary[MuscleGroup] = ExcList;
        }
    }
}