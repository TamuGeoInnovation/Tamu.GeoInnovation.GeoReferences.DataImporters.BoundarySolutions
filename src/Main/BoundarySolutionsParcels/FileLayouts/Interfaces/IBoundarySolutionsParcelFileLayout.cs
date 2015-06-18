using System.Collections;
using System.Data;
using USC.GISResearchLab.Common.Databases.QueryManagers;

namespace USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.Interfaces
{
    public delegate void DbfRecordReadHandler(int numberOfRecordsRead);
    public delegate void DbfNumberOfRecordsReadHandler(int numberOfRecords);

    public interface IBoundarySolutionsParcelFileLayout
    {

        #region Events

        event DbfRecordReadHandler DbfRecordRead;
        event DbfNumberOfRecordsReadHandler DbfNumberOfRecordsRead;

        #endregion


        #region Properties

        string[][] BulkCopyColumnMappings { get; set; }
        string[] ExcludeColumns { get; set; }
        string SQLCreateTable { get; set; }
        
        
        string SQLPostInsertTableDeleteNamedStreetsOnly { get; set; }
        string SQLPostInsertTableDeleteAddressableStreetsOnly { get; set; }
        
        string SQLCreateTableIndexes { get; set; }
        string SQLCreateTableStatistics { get; set; }

        string StateName { get; set; }
        string FileName { get; set; }
        string OutputTableName { get; }
        ArrayList TempDirectories { get; set; }
        ArrayList TempFiles { get; set; }
        IQueryManager DataFileQueryManager { get; set; }

        bool HasSoundexColumns { get; set; }
        string[] SoundexColumns { get; set; }
        bool HasSoundexDMColumns { get; set; }
        string[] SoundexDMColumns { get; set; }

        bool ShouldAbortOnError { get; set; }
        int NotifyAfter { get; set; }

        #endregion

        DataTable GetDataTableFromZipFile(string zipFileDirectory);
        IDataReader GetDataReaderFromZipFile(string zipFileDirectory);
        DataTable GetDataTable(string fileLocation);
        IDataReader GetDataReader(string fileLocation);

        void DeleteTempFiles();
        void DeleteTempDirectories();

    }
}
