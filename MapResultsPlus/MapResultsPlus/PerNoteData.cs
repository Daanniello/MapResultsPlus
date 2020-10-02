using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapResultsPlus
{
    class PerNoteData
    {
        public NoteData noteData;

        public NoteCutInfo noteCutInfo;

        public PerNoteData(NoteData noteData, NoteCutInfo noteCutInfo = null)
        {
            this.noteData = noteData;
            this.noteCutInfo = noteCutInfo;
        }
    }
}
