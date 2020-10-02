using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapResultsPlus
{
    class DataPerNotePositionList
    {
        //First layer on the grid 
        public List<object> Position00;
        public List<object> Position01;
        public List<object> Position02;
        public List<object> Position03;

        //Second layer on the grid 
        public List<object> Position10;
        public List<object> Position11;
        public List<object> Position12;
        public List<object> Position13;

        //Third layer on the grid
        public List<object> Position20;
        public List<object> Position21;
        public List<object> Position22;
        public List<object> Position23;


        public DataPerNotePositionList()
        {
            Position00 = new List<object>();
            Position01 = new List<object>();
            Position02 = new List<object>();
            Position03 = new List<object>();

            Position10 = new List<object>();
            Position11 = new List<object>();
            Position12 = new List<object>();
            Position13 = new List<object>();

            Position20 = new List<object>();
            Position21 = new List<object>();
            Position22 = new List<object>();
            Position23 = new List<object>();
        }

        /// <summary>
        /// Adds specific data from one note to the specific List by note position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        public void AddNoteDataOnPosition(int position, object value)
        {
            switch (position)
            {
                case 00:
                    Position00.Add(value);
                    break;
                case 01:
                    Position01.Add(value);
                    break;
                case 02:
                    Position02.Add(value);
                    break;
                case 03:
                    Position03.Add(value);
                    break;

                case 10:
                    Position10.Add(value);
                    break;
                case 11:
                    Position11.Add(value);
                    break;
                case 12:
                    Position12.Add(value);
                    break;
                case 13:
                    Position13.Add(value);
                    break;

                case 20:
                    Position20.Add(value);
                    break;
                case 21:
                    Position21.Add(value);
                    break;
                case 22:
                    Position22.Add(value);
                    break;
                case 23:
                    Position23.Add(value);
                    break;

            }
        }

        //Todo: Make this function generic
        /// <summary>
        /// Calculates the average from values inside a list
        /// </summary>
        /// <param name="doubleList"></param>
        /// <returns></returns>
        public double CalculateAvgFromList(List<object> doubleList)
        {
            double count = 0;
            foreach(var f in doubleList)
            {
                count += Convert.ToDouble(f);
            }

            return count / doubleList.Count();
        }

        /// <summary>
        /// Returns a dictionary of averages on all note positions in the grid
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> CalculateAvgFromEveryNotePosition()
        {
            var avgNotePositionList = new Dictionary<string, double>();
            avgNotePositionList.Add("00", CalculateAvgFromList(Position00));
            avgNotePositionList.Add("01", CalculateAvgFromList(Position01));
            avgNotePositionList.Add("02", CalculateAvgFromList(Position02));
            avgNotePositionList.Add("03", CalculateAvgFromList(Position03));

            avgNotePositionList.Add("10", CalculateAvgFromList(Position10));
            avgNotePositionList.Add("11", CalculateAvgFromList(Position11));
            avgNotePositionList.Add("12", CalculateAvgFromList(Position12));
            avgNotePositionList.Add("13", CalculateAvgFromList(Position13));

            avgNotePositionList.Add("20", CalculateAvgFromList(Position10));
            avgNotePositionList.Add("21", CalculateAvgFromList(Position11));
            avgNotePositionList.Add("22", CalculateAvgFromList(Position12));
            avgNotePositionList.Add("23", CalculateAvgFromList(Position13));

            return avgNotePositionList;
        }
    }
}
