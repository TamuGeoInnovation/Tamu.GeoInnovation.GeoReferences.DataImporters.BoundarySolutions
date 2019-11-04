using USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.CountyFiles.AbstractClasses;

namespace USC.GISResearchLab.Common.Census.BoundarySolutionsParcels.FileLayouts.CountyFiles.Implementations
{
    public class BoundarySolutionsParcelFile : AbstractBoundarySolutionsParcelsShapefileCountyFileLayout
    {

        public BoundarySolutionsParcelFile(string parcelFileName, string stateName, bool shouldDoOnlyNamedParcels, bool shouldDoOnlyAddressableParcels)
            : base(stateName)
        {

            HasSoundexColumns = true;
            HasSoundexDMColumns = true;
            SoundexColumns = new string[] { "SIT_STR_NA", "SIT_CITY" };
            SoundexDMColumns = new string[] { "SIT_STR_NA", "SIT_CITY" };

            ShouldIncludeArea = true;
            ShouldIncludeCentroid = true;

            ExcludeColumns = new string[] { "uniqueId", "" };

            FileName = parcelFileName;

            SQLCreateTable += "CREATE TABLE [" + OutputTableName + "] (";
            SQLCreateTable += "uniqueId INT IDENTITY(1,1) NOT NULL,";
            SQLCreateTable += "APN varchar(55) DEFAULT NULL,";
            SQLCreateTable += "APN2 varchar(55) DEFAULT NULL,";
            SQLCreateTable += "STATE varchar(55) DEFAULT NULL,";
            SQLCreateTable += "COUNTY varchar(55) DEFAULT NULL,";
            SQLCreateTable += "FIPS varchar(20) DEFAULT NULL,";
            SQLCreateTable += "SIT_HSE_NU varchar(100) DEFAULT NULL,";
            SQLCreateTable += "SIT_DIR varchar(55) DEFAULT NULL,";
            SQLCreateTable += "SIT_STR_NA varchar(100) DEFAULT NULL,";
            SQLCreateTable += "SIT_STR_NA_Soundex varchar(4) DEFAULT NULL,";
            SQLCreateTable += "SIT_STR_NA_SoundexDM varchar(6) DEFAULT NULL,";
            SQLCreateTable += "SIT_STR_SF varchar(100) DEFAULT NULL,";
            SQLCreateTable += "SIT_FULL_S varchar(100) DEFAULT NULL, ";
            SQLCreateTable += "SIT_CITY varchar(100) DEFAULT NULL, ";
            SQLCreateTable += "SIT_CITY_Soundex varchar(4) DEFAULT NULL,";
            SQLCreateTable += "SIT_CITY_SoundexDM varchar(6) DEFAULT NULL,";
            SQLCreateTable += "SIT_STATE varchar(55) DEFAULT NULL,";
            SQLCreateTable += "SIT_ZIP varchar(55) DEFAULT NULL,";
            SQLCreateTable += "SIT_ZIP4 varchar(55)  DEFAULT NULL, ";
            //SQLCreateTable += "SIT_POST varchar(12) DEFAULT NULL,";
            SQLCreateTable += "LAND_VALUE varchar(100) DEFAULT NULL,";
            SQLCreateTable += "IMPR_VALUE varchar(100) DEFAULT NULL, ";
            SQLCreateTable += "TOT_VALUE varchar(100) DEFAULT NULL, ";
            SQLCreateTable += "ASSMT_YEAR varchar(55) DEFAULT NULL,";
            SQLCreateTable += "MKT_LAND_V varchar(100) DEFAULT NULL,";
            SQLCreateTable += "MKT_IMPR_V varchar(100) DEFAULT NULL, ";
            SQLCreateTable += "TOT_MKT_VA varchar(100) DEFAULT NULL,";
            SQLCreateTable += "MKT_VAL_YR varchar(55) DEFAULT NULL,";
            SQLCreateTable += "REC_DATE varchar(55) DEFAULT NULL,";
            SQLCreateTable += "SALES_PRIC varchar(100) DEFAULT NULL,";
            SQLCreateTable += "SALES_CODE varchar(100) DEFAULT NULL,";
            SQLCreateTable += "YEAR_BUILT varchar(55) DEFAULT NULL,";
            SQLCreateTable += "CONST_TYPE varchar(55) DEFAULT NULL,";
            SQLCreateTable += "STD_LAND_U varchar(55) DEFAULT NULL,";
            SQLCreateTable += "LOT_SIZE varchar(55) DEFAULT NULL,";
            SQLCreateTable += "BLDG_AREA varchar(55) DEFAULT NULL,";
            SQLCreateTable += "NO_OF_STOR varchar(55) DEFAULT NULL,";
            SQLCreateTable += "NO_OF_UNIT varchar(55) DEFAULT NULL,";
            //SQLCreateTable += "BEDROOMS varchar(5) DEFAULT NULL,";
            //SQLCreateTable += "BATHROOMS varchar(5) DEFAULT NULL,";
            SQLCreateTable += "OWNER varchar(100) DEFAULT NULL,";
            SQLCreateTable += "OWNER2 varchar(100) DEFAULT NULL,";
            SQLCreateTable += "OWNADDRESS varchar(100) DEFAULT NULL,";
            SQLCreateTable += "OWNADDRES2 varchar(100) DEFAULT NULL,";
            SQLCreateTable += "OWNCTYSTZP varchar(100) DEFAULT NULL,";
            SQLCreateTable += "Xcoord varchar(55) DEFAULT NULL,";
            SQLCreateTable += "Ycoord varchar(55) DEFAULT NULL,";
            SQLCreateTable += "minX varchar(55) DEFAULT NULL,";
            SQLCreateTable += "minY varchar(55) DEFAULT NULL,";
            SQLCreateTable += "maxX varchar(55) DEFAULT NULL,";
            SQLCreateTable += "maxY varchar(55) DEFAULT NULL,";
            SQLCreateTable += "shapeType varchar(55) DEFAULT NULL, ";
            SQLCreateTable += "shapeGeog geography DEFAULT NULL, ";
            SQLCreateTable += "shapeGeom geometry DEFAULT NULL, ";
            SQLCreateTable += "shapeArea varchar(55)  DEFAULT NULL,";
            SQLCreateTable += "centroidX varchar(55)  DEFAULT NULL,";
            SQLCreateTable += "centroidY varchar(55)  DEFAULT NULL ,";
            SQLCreateTable += "importTime numeric  DEFAULT NULL ,";
            SQLCreateTable += "CONSTRAINT [PK_" + OutputTableName + "_uniqueId] PRIMARY KEY CLUSTERED ([uniqueId] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
            SQLCreateTable += ");";


            SQLCreateTableIndexes += "CREATE NONCLUSTERED INDEX [IDX_" + OutputTableName + "] ON [dbo].[" + OutputTableName + "] (	[SIT_HSE_NU] ASC,	[SIT_STR_NA_Soundex] ASC,	[SIT_CITY_Soundex] ASC,	[SIT_ZIP] ASC,	[uniqueId] ASC ) INCLUDE ( [APN],[SIT_DIR],[SIT_STR_NA],[SIT_STR_SF],[SIT_CITY],[SIT_STATE],[SIT_ZIP4],[shapeArea],[centroidX],[centroidY]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] ";

            SQLPostInsertTableDeleteNamedStreetsOnly += " DELETE FROM ";
            SQLPostInsertTableDeleteNamedStreetsOnly += "  [dbo].[" + StateName + "] ";
            SQLPostInsertTableDeleteNamedStreetsOnly += " WHERE ";
            SQLPostInsertTableDeleteNamedStreetsOnly += "  [dbo].[" + StateName + "].[SIT_STR_NA] IS NULL ";
            SQLPostInsertTableDeleteNamedStreetsOnly += " ; ";



            SQLPostInsertTableDeleteAddressableStreetsOnly += " DELETE FROM ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += "  [dbo].[" + StateName + "] ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += " WHERE ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += "  [dbo].[" + StateName + "].[SIT_HSE_NU] IS NULL ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += "  AND ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += "  [dbo].[" + StateName + "].[SIT_STR_NA] IS NULL ";
            SQLPostInsertTableDeleteAddressableStreetsOnly += " ; ";


            // statistics
            SQLCreateTableStatistics = "";
            SQLCreateTableStatistics += " IF  EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '" + StateName + "') ";
            SQLCreateTableStatistics += "  CREATE STATISTICS [_dta_stat_1] ON [dbo].[" + StateName + "]([SIT_STR_NA_Soundex], [SIT_CITY_Soundex], [SIT_ZIP], [SIT_HSE_NU], [uniqueId]) ;";

            SQLCreateTableStatistics += " IF  EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '" + StateName + "') ";
            SQLCreateTableStatistics += "  CREATE STATISTICS [_dta_stat_2] ON [dbo].[" + StateName + "]([SIT_ZIP], [SIT_HSE_NU], [SIT_STR_NA_Soundex]) ;";

            SQLCreateTableStatistics += " IF  EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '" + StateName + "') ";
            SQLCreateTableStatistics += "  CREATE STATISTICS [_dta_stat_3] ON [dbo].[" + StateName + "]([SIT_CITY_Soundex], [SIT_HSE_NU], [SIT_STR_NA_Soundex]) ;";

            SQLCreateTableStatistics += " IF  EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '" + StateName + "') ";
            SQLCreateTableStatistics += "  CREATE STATISTICS [_dta_stat_4] ON [dbo].[" + StateName + "]([uniqueId], [SIT_HSE_NU], [SIT_STR_NA_Soundex], [SIT_CITY_Soundex]) ;";


            // these queries are the spatial indexes required in all scenarios
            string SQLPostInsertSingleTableUpdateSpatialIndexes = "";
            SQLPostInsertSingleTableUpdateSpatialIndexes += " CREATE SPATIAL INDEX [idx_geog] ON [dbo].[" + StateName + "]   (  [shapeGeog]   )USING  GEOGRAPHY_GRID   WITH (  GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM),  CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]; ";

        }
    }
}
