using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SportsWebsiteV2
{
    public class ProgramMaker
    {
        //diagram link https://app.diagrams.net/#G1g3-DBfHIAhL6WCZtoql0fXmQ1rUyPgZ_
        // constants
        private const string select = "SELECT * FROM Excrcise WHERE ";

        private const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Excrcises.mdf';Integrated Security=True";

        //----------------------------------
        private Random Rnd = new Random();

        //properties
        private ExerciseList exerciseList;

        private string exerciseTable;
        private int time;
        private int kind;

        //------------------------
        private int bigExcNum;

        private int smallExcNum;

        public ProgramMaker(int time, int kind)
        {
            exerciseList = new ExerciseList();
            this.time = time;
            this.kind = kind;
            this.bigExcNum = (time * kind + 1) / 2;
            this.smallExcNum = (time * kind + 1) / 3;
        }

        public void ExerciseGenerator()
        {//generates all exc for program
            string[] muscleGroupNames = new string[] { "Chest", "Shoulder", "Tricep", "Abs", "Back", "Bicep", "Leg" };
            foreach (string muscleGroup in muscleGroupNames)
            {
                if (IsMuscleBig(muscleGroup))
                {
                    SelectForBigMuscle(muscleGroup);
                }
                else
                {
                    SelectForSmallMuscle(muscleGroup);
                }
            }
        }

        private bool IsExcCompound(string exc)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Excrcises.mdf';Integrated Security=True";
            string commandUserString = "SELECT count(*) FROM Excrcise WHERE name='" + exc + "' AND IsCompound=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(commandUserString, connection);

                return (int)sqlCommand.ExecuteScalar() == 1;
            }
        }

        private Exercise PullExc(string muscleGroup, int IsCompound)
        {
            DataSet dataSet = new DataSet();
            Exercise exc;
            string commandUserString = select + muscleGroup + "=" + 1 + " AND IsCompound=" + IsCompound,
             excName;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(commandUserString, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            }
            if (dataSet != null)
            {
                int rowNum = dataSet.Tables[0].Rows.Count;
                do
                {
                    excName = dataSet.Tables[0].Rows[Rnd.Next(0, rowNum - 1)]["name"].ToString();
                    exc = new Exercise(excName, IsExcCompound(excName));
                } while (exerciseList.GetExcList(muscleGroup).Contains(exc));
                return exc;
            }
            return new Exercise("null", true);
        }

        private bool IsMuscleBig(string muscle)
        {//checks if a muscle is a big muscle
            string[] majorMuscleGroup = new string[] { "Chest", "Back", "Leg" };
            return Array.IndexOf(majorMuscleGroup, muscle) != -1;
        }

        private void SelectForBigMuscle(string muscleGroup)
        {//picks exc for big muscle
            Exercise smallExercise;
            Exercise bigExercise = PullExc(muscleGroup, 1);//selects the big compound exc for the big muscle group the 1 is as true
            exerciseList.AddExercise(muscleGroup, bigExercise.name, true);
            for (int i = 0; i < bigExcNum; i++)
            {
                smallExercise = PullExc(muscleGroup, 0);
                exerciseList.AddExercise(muscleGroup, smallExercise.name, IsExcCompound(smallExercise.name));
            }
        }

        private void SelectForSmallMuscle(string muscleGroup)
        {//picks exc for small muscle
            Exercise smallExercise;
            for (int i = 0; i < smallExcNum; i++)
            {
                smallExercise = PullExc(muscleGroup, 0);
                exerciseList.AddExercise(muscleGroup, smallExercise.name, IsExcCompound(smallExercise.name));
            }
        }
    }
}