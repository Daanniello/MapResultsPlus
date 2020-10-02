using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapResultsPlus
{
    class NoteDataCollection
    {
        private Dictionary<float, PerNoteData> _noteDataCollection;

        private ScoreController _scoreController;

        public NoteDataCollection()
        {
            _noteDataCollection = new Dictionary<float, PerNoteData>();
        }

        public void CollectNoteData(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameCore")
            {
                _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();

                _scoreController.noteWasCutEvent += ScoreController_noteWasCutEvent;
                _scoreController.noteWasMissedEvent += ScoreController_noteWasMissedEvent;
                //_scoreController.multiplierDidChangeEvent += _scoreController_multiplierDidChangeEvent;

            }
            if (oldScene.name == "GameCore")
            {
                _scoreController.noteWasCutEvent -= ScoreController_noteWasCutEvent;
                _scoreController.noteWasMissedEvent -= ScoreController_noteWasMissedEvent;
                //_scoreController.multiplierDidChangeEvent -= _scoreController_multiplierDidChangeEvent;
                
                
            }
        }

        /*
        private void _scoreController_multiplierDidChangeEvent(int arg1, float arg2)
        {
            Logger.log.Info("multiplier change!");
            if (arg2 == 0)
            {
                Logger.log.Info("get audio controller");
                var audioTimeSyncController = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                Logger.log.Info("get timesincestart");
                try
                {
                    var timeScale = ReflectionUtil.GetField<float, AudioTimeSyncController>(audioTimeSyncController, "_timeScale");
                    var timeSinceStart = Time.time * timeScale;
                    Logger.log.Info("Multiplier break!----------------------");
                    Logger.log.Info($"Multiplier: {arg1.ToString()} progress: { arg2.ToString()} time: {timeScale}");
                }
                catch(Exception ex)
                {
                    Logger.log.Error(ex.StackTrace);
                }
 
            }            
        }
        */

        public Dictionary<float, PerNoteData> GetNoteDataCollection()
        {
            return _noteDataCollection;
        }

        private void ScoreController_noteWasMissedEvent(NoteData noteData, int arg2)
        {
            _noteDataCollection.Add(noteData.id, new PerNoteData(noteData));
            
        }

        private void ScoreController_noteWasCutEvent(NoteData noteData, NoteCutInfo noteCutInfo, int arg3)
        {            
            _noteDataCollection.Add(noteData.id, new PerNoteData(noteData, noteCutInfo));
            
        }
    }
}
