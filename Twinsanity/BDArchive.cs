using System;
using System.IO;

namespace Twinsanity
{
    /// <summary>
    /// BD/BH archive class
    /// </summary>
    public class BDArchive
    {
        /// <summary>
        /// Simple structure of a file
        /// </summary>
        public struct File
        {
            public int NameLength;
            public string Name;
            public uint Offset;
            public uint Size;
            public MemoryStream ByteStream;
        }

        private File[] TableRecord;
        private int Records;

        /// <summary>
        /// Load the BH archive
        /// </summary>
        /// <param name="Path">Path to the archive</param>
        /// <param name="Name">Name of the archive</param>
        public void LoadTable(string Path, string Name)
        {
            Path = FormatPath(Path);
            string archivePath = Path + Name + ".BH";

            if (System.IO.File.Exists(archivePath))
            {
                FileStream Table = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
                BinaryReader TableReader = new BinaryReader(Table);
                Records = 0;
                TableReader.ReadUInt32();
                while (Table.Position < Table.Length)
                {
                    Array.Resize(ref TableRecord, Records + 1);
                    int i = TableRecord.Length - 1;
                    TableRecord[i].NameLength = (int)TableReader.ReadUInt32();
                    TableRecord[i].Name = TableReader.ReadChars(TableRecord[i].NameLength).ToString();
                    TableRecord[i].Offset = TableReader.ReadUInt32();
                    TableRecord[i].Size = TableReader.ReadUInt32();
                    Records += 1;
                }
                Table.Close();
                Table.Dispose();
            }
        }

        /// <summary>
        /// Load the BD archive
        /// </summary>
        /// <param name="Path">Path to the archive</param>
        /// <param name="Name">Name of the archive</param>
        public void LoadArchive(string Path, string Name)
        {
            Path = FormatPath(Path);
            string archivePath = Path + Name + ".BD";

            if (System.IO.File.Exists(archivePath))
            {
                FileStream Archive = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
                BinaryReader ArchiveReader = new BinaryReader(Archive);
                BinaryWriter StreamWriter;
                for (int i = 0; i <= Records - 1; i++)
                {
                    TableRecord[i].ByteStream = new MemoryStream();
                    StreamWriter = new BinaryWriter(TableRecord[i].ByteStream);
                    Archive.Position = TableRecord[i].Offset;
                    StreamWriter.Write(ArchiveReader.ReadBytes((int)TableRecord[i].Size));
                }
                Archive.Close();
                Archive.Dispose();
            }
        }

        /// <summary>
        /// Create a table to save the archives at
        /// </summary>
        /// <param name="Path">Path of the table to be created at</param>
        public void CreateTable(string Path)
        {
            Path = FormatPath(Path);
            DirectoryInfo DirectoryInfo = new DirectoryInfo(Path);
            string[] FileNames = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
            Records = FileNames.Length;
            FileStream File;
            BinaryReader FileReader;
            uint Size = 0;
            Array.Resize(ref TableRecord, Records);
            for (int i = 0; i <= Records - 1; i++)
            {
                File = new FileStream(FileNames[i], FileMode.Open, FileAccess.Read);
                FileReader = new BinaryReader(File);
                TableRecord[i].Offset = Size;
                TableRecord[i].Size = (uint)File.Length;
                Size += TableRecord[i].Size;
                TableRecord[i].Name = FileNames[i].Replace(Path, "");
                TableRecord[i].NameLength = TableRecord[i].Name.Length;
                TableRecord[i].ByteStream = new MemoryStream();
                BinaryWriter BSWriter = new BinaryWriter(TableRecord[i].ByteStream);
                BSWriter.Write(FileReader.ReadBytes((int)File.Length));
                File.Close();
                File.Dispose();
            }
        }

        /// <summary>
        /// Save the BH archive
        /// </summary>
        /// <param name="Path">Path of saving</param>
        /// <param name="Name">Name of the archive</param>
        public void SaveTable(string Path, string Name)
        {
            Path = FormatPath(Path);
            FileStream Table = new FileStream(Path + Name + ".BH", FileMode.Create, FileAccess.Write);
            BinaryWriter TableWriter = new BinaryWriter(Table);
            int Head = 1281;
            TableWriter.Write(Head);
            for (int i = 0; i <= Records - 1; i++)
            {
                TableWriter.Write(TableRecord[i].NameLength);
                for (int j = 0; j <= TableRecord[i].NameLength - 1; j++)
                    TableWriter.Write(TableRecord[i].Name[j]);
                TableWriter.Write(TableRecord[i].Offset);
                TableWriter.Write(TableRecord[i].Size);
            }
        }

        /// <summary>
        /// Save the BD archive
        /// </summary>
        /// <param name="Path">Path of saving</param>
        /// <param name="Name">Name of the archive</param>
        public void SaveArchive(string Path, string Name)
        {
            Path = FormatPath(Path);
            FileStream Archive = new FileStream(Path + Name + ".BD", FileMode.Create, FileAccess.Write);
            BinaryWriter ArchiveWriter = new BinaryWriter(Archive);
            for (int i = 0; i <= Records - 1; i++)
            {
                Archive.Position = TableRecord[i].Offset;
                ArchiveWriter.Write(TableRecord[i].ByteStream.ToArray());
            }
            Archive.Dispose();
        }

        /// <summary>
        /// Extract the archives
        /// </summary>
        /// <param name="Path">Path to extract to</param>
        public void Extract(string Path)
        {
            Path = FormatPath(Path);
            FileStream File;
            BinaryWriter FileWriter;
            for (int i = 0; i <= Records - 1; i++)
            {
                string[] folders = TableRecord[i].Name.Split('\\');
                string CheckPath = Path;
                for (int j = 0; j <= folders.Length - 2; j++)
                {
                    CheckPath += folders[j] + @"\";
                    if (!Directory.Exists(CheckPath))
                        Directory.CreateDirectory(CheckPath);
                }
                File = new FileStream(Path + TableRecord[i].Name, FileMode.Create, FileAccess.Write);
                FileWriter = new BinaryWriter(File);
                FileWriter.Write(TableRecord[i].ByteStream.ToArray());
                File.Close();
                File.Dispose();
            }
        }

        /// <summary>
        /// Realse the resources being used by the object
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i <= Records - 1; i++)
                TableRecord[i].ByteStream.Dispose();
            TableRecord = null;
            Records = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path1"></param>
        /// <param name="Path2"></param>
        /// <param name="Name"></param>
        public void ExtractOnce(string Path1, string Path2, string Name)
        {
            Path1 = FormatPath(Path1);
            Path2 = FormatPath(Path2);
            LoadTable(Path2, Name);
            FileStream File;
            BinaryWriter FileWriter;
            FileStream Archive = new FileStream(Path2 + Name + ".BD", FileMode.Open, FileAccess.Read);
            BinaryReader ArchiveReader = new BinaryReader(Archive);
            for (int i = 0; i <= Records - 1; i++)
            {
                string[] folders = TableRecord[i].Name.Split('\\');
                string CheckPath = Path1;
                for (int j = 0; j <= folders.Length - 2; j++)
                {
                    CheckPath += folders[j] + @"\";
                    if (!Directory.Exists(CheckPath))
                        Directory.CreateDirectory(CheckPath);
                }
                File = new FileStream(Path1 + TableRecord[i].Name, FileMode.Create, FileAccess.Write);
                FileWriter = new BinaryWriter(File);
                Archive.Position = TableRecord[i].Offset;
                FileWriter.Write(ArchiveReader.ReadBytes((int)TableRecord[i].Size));
                File.Close();
                File.Dispose();
            }
            Archive.Dispose();
        }

        /// <summary>
        /// Formats the path to normalize it
        /// </summary>
        /// <param name="Path">Path to format</param>
        /// <returns>Formatted path suitable for usage</returns>
        public string FormatPath(string Path)
        {
            if (!(Path[Path.Length - 1].Equals('\\')))
                return Path + @"\";
            return Path;
        }
    }
}
