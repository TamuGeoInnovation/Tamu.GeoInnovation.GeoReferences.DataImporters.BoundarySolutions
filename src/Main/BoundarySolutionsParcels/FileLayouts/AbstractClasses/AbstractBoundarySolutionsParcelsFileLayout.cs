﻿using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.Interfaces;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Databases.ConnectionStringManagers;
using USC.GISResearchLab.Common.Databases.QueryManagers;
using USC.GISResearchLab.Common.Utils.Files;

namespace USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.AbstractClasses
{
    public abstract class AbstractBoundarySolutionsParcelsFileLayout : IBoundarySolutionsParcelFileLayout
    {

        #region Events

        public event DbfRecordReadHandler DbfRecordRead;
        public event DbfNumberOfRecordsReadHandler DbfNumberOfRecordsRead;

        #endregion

        #region Properties

        public IQueryManager DataFileQueryManager { get; set; }
        public string SQLCreateTable { get; set; }


        public string SQLPostInsertTableDeleteNamedStreetsOnly { get; set; }
        public string SQLPostInsertTableDeleteAddressableStreetsOnly { get; set; }

        public string SQLCreateTableIndexes { get; set; }
        public string SQLCreateTableStatistics { get; set; }

        public string[] ExcludeColumns { get; set; }

        public bool HasSoundexColumns { get; set; }
        public string[] SoundexColumns { get; set; }



        public bool HasSoundexDMColumns { get; set; }
        public string[] SoundexDMColumns { get; set; }


        public bool ShouldIncludeGeometryProjected { get; set; }

        public bool ShouldIncludeArea { get; set; }
        public bool ShouldIncludeCentroid { get; set; }


        public string StateName { get; set; }
        public string FileName { get; set; }

        public string[][] BulkCopyColumnMappings { get; set; }
        public ArrayList TempFiles { get; set; }
        public ArrayList TempDirectories { get; set; }


        public string OutputTableName
        {
            get
            {
                string ret = "";
                if (!String.IsNullOrEmpty(StateName))
                {
                    ret = StateName;
                }
                else
                {
                    ret = FileUtils.GetFileNameWithoutExtension(FileName);
                }
                return ret;
            }
        }

        public bool ShouldAbortOnError { get; set; }
        public int NotifyAfter { get; set; }
        public int BatchWriteSize { get; set; }

        #endregion

        public AbstractBoundarySolutionsParcelsFileLayout()
        {
            TempFiles = new ArrayList();
            TempDirectories = new ArrayList();
        }


        public AbstractBoundarySolutionsParcelsFileLayout(string tableName)
            : this()
        {
            StateName = tableName;
        }

        public abstract DataTable GetDataTableFromZipFile(string zipFileDirectory);

        public abstract IDataReader GetDataReaderFromZipFile(string zipFileDirectory);

        public virtual DataTable GetDataTable(string fileLocation)
        {
            DataTable ret = null;

            try
            {
                string databasePath = fileLocation;
                string databaseDirectory = FileUtils.GetDirectoryPath(databasePath, false);
                string databaseName = FileUtils.GetFileName(databasePath);
                string tableName = FileUtils.GetFileNameWithoutExtension(databasePath);
                string databaseExtension = FileUtils.GetExtension(databaseName);

                DataProviderType dataProviderType = DataProviderTypes.FromDatabaseExtension(databaseExtension);
                DatabaseType databaseType = DatabaseTypes.FromDatabaseExtension(databaseExtension);

                IConnectionStringManager connectionStringManager = ConnectionStringManagerFactory.GetConnectionStringManager(databaseType, databaseDirectory, databaseName, null, null, null);
                string connectionString = connectionStringManager.GetConnectionString(dataProviderType);
                DataFileQueryManager = new QueryManager(dataProviderType, databaseType, connectionString);

                string sql = "select * from [" + databaseName + "]";
                SqlCommand cmd = new SqlCommand(sql);

                DataFileQueryManager.AddParameters(cmd.Parameters);
                ret = DataFileQueryManager.ExecuteDataTable(CommandType.Text, cmd.CommandText, true);

            }
            catch (Exception e)
            {
                throw new Exception("Error getting datatable: " + e.Message, e);
            }

            return ret;
        }

        public virtual IDataReader GetDataReader(string fileLocation)
        {
            IDataReader ret = null;

            try
            {
                string databasePath = fileLocation;
                string databaseDirectory = FileUtils.GetDirectoryPath(databasePath, false);
                string databaseName = FileUtils.GetFileName(databasePath);
                string tableName = FileUtils.GetFileNameWithoutExtension(databasePath);
                string databaseExtension = FileUtils.GetExtension(databaseName);

                DataProviderType dataProviderType = DataProviderTypes.FromDatabaseExtension(databaseExtension);
                DatabaseType databaseType = DatabaseTypes.FromDatabaseExtension(databaseExtension);

                IConnectionStringManager connectionStringManager = ConnectionStringManagerFactory.GetConnectionStringManager(databaseType, databaseDirectory, databaseName, null, null, null);
                string connectionString = connectionStringManager.GetConnectionString(dataProviderType);
                DataFileQueryManager = new QueryManager(dataProviderType, databaseType, connectionString);

                string sql = "select * from [" + databaseName + "]";
                SqlCommand cmd = new SqlCommand(sql);

                DataFileQueryManager.Open();
                DataFileQueryManager.AddParameters(cmd.Parameters);
                ret = DataFileQueryManager.ExecuteReader(CommandType.Text, cmd.CommandText, false);

            }
            catch (Exception e)
            {
                throw new Exception("Error getting GetDataReader: " + e.Message, e);
            }

            return ret;
        }

        public void dbfRecordRead(int numberOfRecordsRead)
        {
            if (DbfRecordRead != null)
            {
                DbfRecordRead(numberOfRecordsRead);
            }
        }

        public void dbfNumberOfRecordsRead(int numberOfRecords)
        {
            if (DbfNumberOfRecordsRead != null)
            {
                DbfNumberOfRecordsRead(numberOfRecords);
            }
        }


        public void DeleteTempFiles()
        {
            if (TempFiles != null)
            {
                for (int i = 0; i < TempFiles.Count; i++)
                {
                    if (File.Exists((string)TempFiles[i]))
                    {
                        File.Delete((string)TempFiles[i]);
                    }
                }
            }
        }

        public void DeleteTempDirectories()
        {
            if (TempDirectories != null)
            {
                for (int i = 0; i < TempDirectories.Count; i++)
                {
                    if (Directory.Exists((string)TempDirectories[i]))
                    {
                        Directory.Delete((string)TempDirectories[i], true);
                    }
                }
            }
        }
    }
}
