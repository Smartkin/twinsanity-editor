using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TwinsaityEditor.Externals.PS2ImageMaker
{
    class PS2ImageMaker
    {
        public unsafe static Progress StartPacking(string twinsPath, string imagePathName)
        {
            imagePathName = imagePathName.Insert(imagePathName.Length, '\\'.ToString());
            var ptr = start_packing(twinsPath, imagePathName);
            ProgressC progress = new ProgressC();
            progress = (ProgressC)Marshal.PtrToStructure(ptr, typeof(ProgressC));
            Progress prog = new Progress
            {
                Finished = progress.finished,
                NewFile = progress.new_file,
                NewState = progress.new_state,
                ProgressS = progress.state,
                ProgressPercentage = progress.progress
            };
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < progress.size; ++i)
            {
                builder.Append(*(progress.file_name + i));
            }
            prog.File = builder.ToString();
            return prog;
        }

        public unsafe static Progress PollProgress()
        {
            var ptr = poll_progress();
            ProgressC progress = new ProgressC();
            progress = (ProgressC)Marshal.PtrToStructure(ptr, typeof(ProgressC));
            Progress prog = new Progress
            {
                Finished = progress.finished,
                NewFile = progress.new_file,
                NewState = progress.new_state,
                ProgressS = progress.state,
                ProgressPercentage = progress.progress
            };
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < progress.size; ++i)
            {
                builder.Append(*(progress.file_name + i));
            }
            prog.File = builder.ToString();
            return prog;
        }

        public enum ProgressState
        {
            FAILED = -1,
            ENUM_FILES,
            WRITE_SECTORS,
            WRITE_FILES,
            WRITE_END,
            FINISHED,
        }

        public class Progress {
            public string File;
            public ProgressState ProgressS;
            public float ProgressPercentage;
            public bool Finished;
            public bool NewState;
            public bool NewFile;
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct ProgressC
        {
            public fixed sbyte file_name[256];
            public int size;
            public ProgressState state;
            public float progress;
            public bool finished;
            public bool new_state;
            public bool new_file;
        }

        [DllImport("PS2ImageMaker", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe static extern IntPtr start_packing(string game_path, string dest_path);
        [DllImport("PS2ImageMaker", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern IntPtr poll_progress();
    }
}
