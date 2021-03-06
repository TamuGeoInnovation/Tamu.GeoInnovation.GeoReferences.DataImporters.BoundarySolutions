﻿using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.AbstractClasses;
using USC.GISResearchLab.Common.Utils.Directories;
using USC.GISResearchLab.Common.Utils.Files;

namespace USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.CountyFiles.AbstractClasses
{
    public abstract class AbstractBoundarySolutionsParcelsCountyFileLayout : AbstractBoundarySolutionsParcelsFileLayout
    {

        public AbstractBoundarySolutionsParcelsCountyFileLayout(string tableName)
            : base(tableName) { }

        public override DataTable GetDataTableFromZipFile(string zipFileDirectory)
        {
            DataTable ret = null;
            string tempDirectory = null;

            try
            {
                string directoryName = DirectoryUtils.GetDirectoryName(zipFileDirectory);
                string[] directoryParts = directoryName.Split('_');
                string countyFips = directoryParts[0];
                string zipFileName = "tl_2008_" + countyFips + "_" + FileName;

                string zipFileLocation = Path.Combine(zipFileDirectory, zipFileName);
                if (FileUtils.FileExists(zipFileLocation))
                {
                    string now = DateTime.Now.Millisecond.ToString();
                    string fileName = FileUtils.GetFileNameWithoutExtension(zipFileLocation);
                    tempDirectory = FileUtils.GetDirectoryPath(zipFileLocation) + "_temp_" + fileName + "_" + now + "\\";

                    if (Directory.Exists(tempDirectory))
                    {
                        Directory.Delete(tempDirectory, true);
                    }

                    TempDirectories.Add(tempDirectory);

                    FastZip fastZip = new FastZip();
                    fastZip.ExtractZip(zipFileLocation, tempDirectory, null);

                    bool hasShapefile = false;
                    string shapefileName = null;
                    string dbffileName = null;

                    ArrayList fileList = DirectoryUtils.getFileList(tempDirectory);
                    if (fileList != null)
                    {
                        for (int i = 0; i < fileList.Count; i++)
                        {
                            string file = (string)fileList[i];
                            string fileExtension = FileUtils.GetExtension(file);
                            if (String.Compare(fileExtension, ".shp", true) == 0)
                            {
                                hasShapefile = true;
                                shapefileName = file;
                                break;
                            }
                            if (String.Compare(fileExtension, ".dbf", true) == 0)
                            {
                                dbffileName = file;
                            }
                        }
                    }

                    if (hasShapefile)
                    {
                        ret = GetDataTable(shapefileName);
                    }
                    else
                    {
                        string dbfFileDirectory = FileUtils.GetDirectoryPath(dbffileName);
                        string tempFile = Path.Combine(dbfFileDirectory, "temp.dbf");

                        File.Copy(dbffileName, tempFile);
                        ret = GetDataTable(tempFile);
                    }
                }
            }
            catch (Exception e)
            {
                if (!String.IsNullOrEmpty(tempDirectory))
                {
                    if (Directory.Exists(tempDirectory))
                    {
                        DirectoryUtils.DeleteDirectory(tempDirectory);
                    }
                }
                throw new Exception("Error getting datatable: " + e.Message, e);
            }
            finally
            {
                if (!String.IsNullOrEmpty(tempDirectory))
                {
                    if (Directory.Exists(tempDirectory))
                    {
                        DirectoryUtils.DeleteDirectory(tempDirectory);
                    }
                }
            }

            return ret;
        }

        public override IDataReader GetDataReaderFromZipFile(string zipFileDirectory)
        {
            IDataReader ret = null;
            string tempDirectory = null;

            try
            {
                string directoryName = DirectoryUtils.GetDirectoryName(zipFileDirectory);

                string zipFileLocation = Path.Combine(zipFileDirectory, FileName);
                if (FileUtils.FileExists(zipFileLocation))
                {
                    string now = DateTime.Now.Millisecond.ToString();
                    string fileName = FileUtils.GetFileNameWithoutExtension(zipFileLocation);
                    tempDirectory = FileUtils.GetDirectoryPath(zipFileLocation) + "_temp_" + fileName + "_" + now + "\\";

                    if (Directory.Exists(tempDirectory))
                    {
                        Directory.Delete(tempDirectory, true);
                    }

                    TempDirectories.Add(tempDirectory);

                    FastZip fastZip = new FastZip();
                    fastZip.ExtractZip(zipFileLocation, tempDirectory, null);

                    bool hasShapefile = false;
                    string shapefileName = null;
                    string dbffileName = null;

                    List<String> directories = DirectoryUtils.GetSubDirectoriesAsStringList(tempDirectory);

                    if (directories.Count > 0)
                    {

                        string subDirectory = Path.Combine(tempDirectory, directories[0]);

                        ArrayList fileList = DirectoryUtils.getFileList(subDirectory);
                        if (fileList != null)
                        {
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                string file = (string)fileList[i];

                                string extractedFileName = FileUtils.GetFileNameWithoutExtension(file);

                                if (String.Compare(extractedFileName, "parcels", true) == 0)
                                {

                                    string fileExtension = FileUtils.GetExtension(file);
                                    if (String.Compare(fileExtension, ".shp", true) == 0)
                                    {
                                        hasShapefile = true;
                                        shapefileName = file;
                                        break;
                                    }
                                    if (String.Compare(fileExtension, ".dbf", true) == 0)
                                    {
                                        dbffileName = file;
                                    }
                                }
                            }
                        }

                        if (hasShapefile)
                        {

                            ret = GetDataReader(shapefileName);
                        }
                        else
                        {
                            string dbfFileDirectory = FileUtils.GetDirectoryPath(dbffileName);
                            string tempFile = Path.Combine(dbfFileDirectory, "temp.dbf");

                            File.Copy(dbffileName, tempFile);
                            ret = GetDataReader(tempFile);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (!String.IsNullOrEmpty(tempDirectory))
                {
                    if (Directory.Exists(tempDirectory))
                    {
                        DirectoryUtils.DeleteDirectory(tempDirectory);
                    }
                }
                throw new Exception("Error getting datatable: " + e.Message, e);
            }

            return ret;
        }
    }
}
