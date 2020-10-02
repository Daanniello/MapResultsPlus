using IPA.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapResultsPlus
{
    public class LevelEndedData
    {
        private NoteDataCollection _noteDataCollection;

        public LevelEndedData()
        {
            Logger.log.Info("PauseOnMiss created!");
        }

        /// <summary>
        /// Checks if the endscreen is shown 
        /// </summary>
        /// <param name="oldScene"></param>
        /// <param name="newScene"></param>
        /// <returns></returns>
        public bool InGameCheck(Scene oldScene, Scene newScene)
        {
            //Creates a new NoteDataCollection on map start 
            if (newScene.name == "GameCore")
            {
                Logger.log.Info("User is now in the playing scene");
                _noteDataCollection = new NoteDataCollection();
                _noteDataCollection.CollectNoteData(oldScene, newScene);


                return true;
            }

            //Get all data after the map has ended. By failed or finished
            if (oldScene.name == "GameCore")
            {

                //Get the LevelCompletionResults with preparelevelcompletionresult
                var prepareLevelCompletionResults = Resources.FindObjectsOfTypeAll<PrepareLevelCompletionResults>().FirstOrDefault();

                var levelCompletionResult = prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.None, LevelCompletionResults.LevelEndAction.None);
                Logger.log.Info("Logging LevelCompletionData -----------------------------------------------------");
                Logger.log.Info("Avg cut score: " + levelCompletionResult.averageCutScore.ToString());
                Logger.log.Info("Song duration: " + levelCompletionResult.songDuration.ToString());
                Logger.log.Info("Missed count: " + levelCompletionResult.missedCount.ToString());
                

                //Get the beatmap data
                var beatmapData = ReflectionUtil.GetField<BeatmapData, PrepareLevelCompletionResults>(prepareLevelCompletionResults, "_beatmapData");
                Logger.log.Info("Logging BeatMapData -----------------------------------------------------");
                Logger.log.Info("Beatmap bomb count: " + beatmapData.bombsCount.ToString());



                //NoteData collection with time as the key and perNoteData as value. Includes noteData and noteCutInfo.
                var noteDataCollecion = _noteDataCollection.GetNoteDataCollection();
                Logger.log.Info("Logging NoteDataCollection -----------------------------------------------------");
                Logger.log.Info("noteDataCollection count: " + noteDataCollecion.Count.ToString());
                Logger.log.Info("First note cutDirection: " + noteDataCollecion.First().Value.noteData.cutDirection.ToString());


                var avgNoteSliceSpeedPerPostion = new DataPerNotePositionList();
                var avgNoteWobblePerPostion = new DataPerNotePositionList();
                var avgNoteOnTimePerPostion = new DataPerNotePositionList();
                var avgPointsPerPostion = new DataPerNotePositionList();

                foreach (var noteData in noteDataCollecion)
                {
                    var perNoteData = noteData.Value;

                    //Note Data
                    Logger.log.Info("noteData ID: " + perNoteData.noteData.id);
                    Logger.log.Info("noteData LineIndex: " + perNoteData.noteData.lineIndex);
                    Logger.log.Info("noteData NoteLineLayer: " + perNoteData.noteData.noteLineLayer);
                    Logger.log.Info("noteData NoteLineLayer int: " + (int)perNoteData.noteData.noteLineLayer);
                    Logger.log.Info("noteData Note Type: " + perNoteData.noteData.noteType.ToString());

                    var notePosition = (int)perNoteData.noteData.noteLineLayer + "" + perNoteData.noteData.lineIndex;

                    //If a note missed there is no NoteCutInfo 
                    if (perNoteData.noteCutInfo == null)
                    {
                        Logger.log.Info("noteData: Missed");
                    }
                    //If the note was not missed, data from the CutInfo
                    else
                    {
                        //Logger.log.Info("noteData Cut Distance center: " + perNoteData.noteCutInfo.cutDistanceToCenter);
                        //Logger.log.Info("noteData Swing Counter before: " + perNoteData.noteCutInfo.swingRatingCounter.beforeCutRating);
                        //Logger.log.Info("noteData Swing Counter after: " + perNoteData.noteCutInfo.swingRatingCounter.afterCutRating);
                        //Logger.log.Info("noteData Cut Point: " + perNoteData.noteCutInfo.cutPoint);
                        Logger.log.Info("noteData Slice Speed: " + perNoteData.noteCutInfo.saberSpeed);
                        avgNoteSliceSpeedPerPostion.AddNoteDataOnPosition(int.Parse(notePosition), perNoteData.noteCutInfo.saberSpeed);

                        Logger.log.Info("noteData Wobble: " + perNoteData.noteCutInfo.cutDirDeviation);
                        avgNoteWobblePerPostion.AddNoteDataOnPosition(int.Parse(notePosition), perNoteData.noteCutInfo.cutDirDeviation);

                        Logger.log.Info("noteData On time: " + perNoteData.noteCutInfo.timeDeviation);
                        avgNoteOnTimePerPostion.AddNoteDataOnPosition(int.Parse(notePosition), perNoteData.noteCutInfo.timeDeviation);

                        ScoreModel.RawScoreWithoutMultiplier(perNoteData.noteCutInfo, out int before, out int after, out int accuracy);
                        int total = before + after + accuracy;
                        Logger.log.Info("Points: " + total);
                        avgPointsPerPostion.AddNoteDataOnPosition(int.Parse(notePosition), total);
                    }


                }
                Logger.log.Info("Calculating avg slice speed!");
                var avgNotePositionData = avgNoteSliceSpeedPerPostion.CalculateAvgFromEveryNotePosition();
                foreach (var avgNoteData in avgNotePositionData)
                {
                    Logger.log.Info($"Note position speed avg {avgNoteData.Key}: {avgNoteData.Value}");
                }

                Logger.log.Info("Calculating avg Wobble!");
                avgNotePositionData = avgNoteWobblePerPostion.CalculateAvgFromEveryNotePosition();
                foreach (var avgNoteData in avgNotePositionData)
                {
                    Logger.log.Info($"Note position wobble avg {avgNoteData.Key}: {avgNoteData.Value}");
                }

                Logger.log.Info("Calculating avg On Time!");
                avgNotePositionData = avgNoteOnTimePerPostion.CalculateAvgFromEveryNotePosition();
                foreach (var avgNoteData in avgNotePositionData)
                {
                    Logger.log.Info($"Note position On Time avg {avgNoteData.Key}: {avgNoteData.Value}");
                }

                Logger.log.Info("Calculating avg Points!");
                avgNotePositionData = avgPointsPerPostion.CalculateAvgFromEveryNotePosition();
                foreach (var avgNoteData in avgNotePositionData)
                {
                    Logger.log.Info($"Note position Points avg {avgNoteData.Key}: {avgNoteData.Value}");
                }

                //Get the ScoreData with reflection out of the leaderboardScoreUploader
                //var LeaderboardScoreUploader = Resources.FindObjectsOfTypeAll<LeaderboardScoreUploader>().FirstOrDefault();
                //var Scores = ReflectionUtil.GetField<List<LeaderboardScoreUploader.ScoreData>, LeaderboardScoreUploader>(LeaderboardScoreUploader, "_scoresToUpload");
                //Logger.log.Info("Logging Reflection Data -----------------------------------------------------");

                return false;
            }
            return false;
        }
    }
}
