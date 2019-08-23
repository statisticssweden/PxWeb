using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;
//using PCAxis.Sql.QueryLib;

//This code is generated.  Not anymore!

namespace PCAxis.Sql.QueryLib_21 {

    public partial class MetaQuery {
        #region for Contents
        public Dictionary<string, ContentsRow> GetContentsRows(string aMainTable) {
            Dictionary<string, ContentsRow> myOut = new Dictionary<string, ContentsRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetContents_SQLString_NoWhere();
            //
            // WHERE CNT.MainTable = '<aMainTable>'
            //
            sqlString += " WHERE " + DB.Contents.MainTableCol.Is(aMainTable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable);
            }

            foreach (DataRow sqlRow in myRows) {
                ContentsRow outRow = new ContentsRow(sqlRow, DB, mLanguageCodes, (IMetaVersionComparator) this);
                myOut.Add(outRow.Contents, outRow);
            }
            return myOut;
        }

        private String GetContents_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";
            if( metaVersionGE("2.1")) {
                sqlString += DB.Contents.PresMissingLineCol.ForSelect() + " , ";
            }


            sqlString +=
                DB.Contents.MainTableCol.ForSelect() + ", " +
                DB.Contents.ContentsCol.ForSelect() + ", " +
                DB.Contents.PresTextCol.ForSelect() + ", " +
                DB.Contents.PresTextSCol.ForSelect() + ", " +
                DB.Contents.PresCodeCol.ForSelect() + ", " +
                DB.Contents.CopyrightCol.ForSelect() + ", " +
                DB.Contents.StatAuthorityCol.ForSelect() + ", " +
                DB.Contents.ProducerCol.ForSelect() + ", " +
                DB.Contents.LastUpdatedCol.ForSelect() + ", " +
                DB.Contents.PublishedCol.ForSelect() + ", " +
                DB.Contents.UnitCol.ForSelect() + ", " +
                DB.Contents.PresDecimalsCol.ForSelect() + ", " +
                DB.Contents.PresCellsZeroCol.ForSelect() + ", " +
                DB.Contents.AggregPossibleCol.ForSelect() + ", " +
                DB.Contents.RefPeriodCol.ForSelect() + ", " +
                DB.Contents.StockFACol.ForSelect() + ", " +
                DB.Contents.BasePeriodCol.ForSelect() + ", " +
                DB.Contents.CFPricesCol.ForSelect() + ", " +
                DB.Contents.DayAdjCol.ForSelect() + ", " +
                DB.Contents.SeasAdjCol.ForSelect() + ", " +
                DB.Contents.FootnoteContentsCol.ForSelect() + ", " +
                DB.Contents.FootnoteTimeCol.ForSelect() + ", " +
                DB.Contents.FootnoteValueCol.ForSelect() + ", " +
                DB.Contents.FootnoteVariableCol.ForSelect() + ", " +
                DB.Contents.StoreNoCharCol.ForSelect() + ", " +
                DB.Contents.StoreDecimalsCol.ForSelect() + ", " +
                DB.Contents.StoreFormatCol.ForSelect() + ", " +
                DB.Contents.StoreColumnNoCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.ContentsLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Contents.PresTextCol);
                    sqlString += ", " + DB.ContentsLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.Contents.PresTextSCol);
                    sqlString += ", " + DB.ContentsLang2.UnitCol.ForSelectWithFallback(langCode, DB.Contents.UnitCol);
                    sqlString += ", " + DB.ContentsLang2.RefPeriodCol.ForSelectWithFallback(langCode, DB.Contents.RefPeriodCol);
                    sqlString += ", " + DB.ContentsLang2.BasePeriodCol.ForSelectWithFallback(langCode, DB.Contents.BasePeriodCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetContentsRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.Contents.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.ContentsLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Contents.MainTableCol.Is(DB.ContentsLang2.MainTableCol, langCode) +
                                 " AND " + DB.Contents.ContentsCol.Is(DB.ContentsLang2.ContentsCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for Contents

        #region for DataStorage
        //returns the single "row" found when all PKs are spesified
        public DataStorageRow GetDataStorageRow(string aProductId) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetDataStorage_SQLString_NoWhere();
            sqlString += " WHERE " + DB.DataStorage.ProductIdCol.Is(aProductId) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ProductId = " + aProductId);
            }

            DataStorageRow myOut = new DataStorageRow(myRows[0], DB, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetDataStorage_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.DataStorage.ProductIdCol.ForSelect() + ", " +
                DB.DataStorage.ServerNameCol.ForSelect() + ", " +
                DB.DataStorage.DatabaseNameCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetDataStorageRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.DataStorage.GetNameAndAlias();
            return sqlString;
        }

        #endregion for DataStorage

        #region for Grouping
        //returns the single "row" found when all PKs are spesified
        public GroupingRow GetGroupingRow(string aValuePool, string aGrouping) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetGrouping_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Grouping.GroupingCol.Is(aGrouping)  + 
                             " AND " +DB.Grouping.ValuePoolCol.Is(aValuePool) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValuePool = " + aValuePool + " Grouping = " + aGrouping);
            }

            GroupingRow myOut = new GroupingRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetGrouping_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Grouping.ValuePoolCol.ForSelect() + ", " +
                DB.Grouping.GroupingCol.ForSelect() + ", " +
                DB.Grouping.PresTextCol.ForSelect() + ", " +
                DB.Grouping.DescriptionCol.ForSelect() + ", " +
                DB.Grouping.GroupPresCol.ForSelect() + ", " +
                DB.Grouping.GeoAreaNoCol.ForSelect() + ", " +
                DB.Grouping.KDBidCol.ForSelect() + ", " +
                DB.Grouping.SortCodeCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.GroupingLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Grouping.PresTextCol);
                    sqlString += ", " + DB.GroupingLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.Grouping.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetGroupingRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Grouping.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.GroupingLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.GroupingLang2.GroupingCol, langCode) +
                                 " AND " + DB.Grouping.ValuePoolCol.Is(DB.GroupingLang2.ValuePoolCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for Grouping

        #region for MainTable
        //returns the single "row" found when all PKs are spesified
        public MainTableRow GetMainTableRow(string aMainTable) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMainTable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MainTable.MainTableCol.Is(aMainTable) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable);
            }

            MainTableRow myOut = new MainTableRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetMainTable_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";

            if( metaVersionLE("2.0")) {
                sqlString += DB.MainTable.StatusEngCol.ForSelect() + " , ";
            }

            sqlString +=
                DB.MainTable.MainTableCol.ForSelect() + ", " +
                DB.MainTable.TableStatusCol.ForSelect() + ", " +
                DB.MainTable.PresTextCol.ForSelect() + ", " +
                DB.MainTable.PresTextSCol.ForSelect() + ", " +
                DB.MainTable.ContentsVariableCol.ForSelect() + ", " +
                DB.MainTable.TableIdCol.ForSelect() + ", " +
                DB.MainTable.PresCategoryCol.ForSelect() + ", " +
                DB.MainTable.SpecCharExistsCol.ForSelect() + ", " +
                DB.MainTable.SubjectCodeCol.ForSelect() + ", " +
                DB.MainTable.ProductIdCol.ForSelect() + ", " +
                DB.MainTable.TimeScaleCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.MainTableLang2.PresTextCol.ForSelectWithFallback(langCode, DB.MainTable.PresTextCol);
                    sqlString += ", " + DB.MainTableLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.MainTable.PresTextSCol);
                    sqlString += ", " + DB.MainTableLang2.ContentsVariableCol.ForSelectWithFallback(langCode, DB.MainTable.ContentsVariableCol);
                    if (metaVersionGE("2.1"))
                    {
                        sqlString +=  " , " + DB.MainTableLang2.StatusCol.ForSelect(langCode);
                    }
                }
            }

            sqlString += " /" + "*** SQLID: GetMainTableRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MainTable.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.MainTableLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.MainTable.MainTableCol.Is(DB.MainTableLang2.MainTableCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for MainTable

        #region for MainTablePerson
        public Dictionary<string, MainTablePersonRow> GetMainTablePersonRows(string aMainTable, string aRolePerson) {
            Dictionary<string, MainTablePersonRow> myOut = new Dictionary<string, MainTablePersonRow>();
            SqlDbConfig dbconf = DB;

            string sqlString = GetMainTablePerson_SQLString_NoWhere();
            //
            // WHERE MTP.MainTable = '<aMainTable>'
            //    AND MTP.RolePerson = '<aRolePerson>'
            //
            sqlString += " WHERE " + DB.MainTablePerson.MainTableCol.Is(aMainTable) + 
                         " AND " +DB.MainTablePerson.RolePersonCol.Is(aRolePerson);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            //Only Role Head and Updated must exist
            if (myRows.Count < 1 && (aRolePerson.Equals(DB.Codes.RoleHead, StringComparison.InvariantCultureIgnoreCase) || aRolePerson.Equals(DB.Codes.RoleUpdate, StringComparison.InvariantCultureIgnoreCase))) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable +  " RolePerson = " + aRolePerson);
            }

            foreach (DataRow sqlRow in myRows) {
                MainTablePersonRow outRow = new MainTablePersonRow(sqlRow, DB, (IMetaVersionComparator) this);
                myOut.Add(outRow.PersonCode, outRow);
            }
            return myOut;
        }

        private String GetMainTablePerson_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MainTablePerson.MainTableCol.ForSelect() + ", " +
                DB.MainTablePerson.PersonCodeCol.ForSelect() + ", " +
                DB.MainTablePerson.RolePersonCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMainTablePersonRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.MainTablePerson.GetNameAndAlias();
            return sqlString;
        }

        #endregion for MainTablePerson

        #region for MenuSelection
        /* For SubjectArea*/
        //returns the single "row" found when all PKs are spesified
        public MenuSelectionRow GetMenuSelectionRow(string aMenu, string aSelection) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMenuSelection_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MenuSelection.MenuCol.Is(aMenu)  + 
                             " AND " +DB.MenuSelection.SelectionCol.Is(aSelection) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," Menu = " + aMenu + " Selection = " + aSelection);
            }

            MenuSelectionRow myOut = new MenuSelectionRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetMenuSelection_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MenuSelection.MenuCol.ForSelect() + ", " +
                DB.MenuSelection.SelectionCol.ForSelect() + ", " +
                DB.MenuSelection.PresTextCol.ForSelect() + ", " +
                DB.MenuSelection.PresTextSCol.ForSelect() + ", " +
                DB.MenuSelection.DescriptionCol.ForSelect() + ", " +
                DB.MenuSelection.LevelNoCol.ForSelect() + ", " +
                DB.MenuSelection.SortCodeCol.ForSelect() + ", " +
                DB.MenuSelection.PresentationCol.ForSelect() + ", " +
                DB.MenuSelection.InternalIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.MenuSelectionLang2.PresTextCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresTextCol);
                    sqlString += ", " + DB.MenuSelectionLang2.PresTextSCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresTextSCol);
                    sqlString += ", " + DB.MenuSelectionLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.MenuSelection.DescriptionCol);
                    sqlString += ", " + DB.MenuSelectionLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.MenuSelection.SortCodeCol);
                    sqlString += ", " + DB.MenuSelectionLang2.PresentationCol.ForSelectWithFallback(langCode, DB.MenuSelection.PresentationCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetMenuSelectionRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MenuSelection.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.MenuSelectionLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.MenuSelection.MenuCol.Is(DB.MenuSelectionLang2.MenuCol, langCode) +
                                 " AND " + DB.MenuSelection.SelectionCol.Is(DB.MenuSelectionLang2.SelectionCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for MenuSelection

        #region for MetaAdm
        //returns the single "row" found when all PKs are spesified
        public MetaAdmRow GetMetaAdmRow(string aProperty) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMetaAdm_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MetaAdm.PropertyCol.IsUppered(aProperty) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," Property = " + aProperty);
            }

            MetaAdmRow myOut = new MetaAdmRow(myRows[0], DB, (IMetaVersionComparator) this); 
            return myOut;
        }

        //returns the all  "rows" found in database
        public Dictionary<string, MetaAdmRow> GetMetaAdmAllRows() {
            string sqlString = GetMetaAdm_SQLString_NoWhere();
            Dictionary<string, MetaAdmRow> myOut = new Dictionary<string, MetaAdmRow>();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1) {
                throw new PCAxis.Sql.Exceptions.DbException(44, "MetaAdm", "METAADM");
            }

            foreach (DataRow sqlRow in myRows) {
                MetaAdmRow outRow = new MetaAdmRow(sqlRow, DB, (IMetaVersionComparator) this );
                myOut.Add(outRow.Property, outRow);
            }
            return myOut;
        }


        private String GetMetaAdm_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MetaAdm.PropertyCol.ForSelect() + ", " +
                DB.MetaAdm.ValueCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMetaAdmAllRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.MetaAdm.GetNameAndAlias();
            return sqlString;
        }

        #endregion for MetaAdm

        #region for MetabaseInfo
        //returns the single "row" found when all PKs are spesified
        public MetabaseInfoRow GetMetabaseInfoRow(string aModel) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetMetabaseInfo_SQLString_NoWhere();
            sqlString += " WHERE " + DB.MetabaseInfo.ModelCol.IsUppered(aModel) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," Model = " + aModel);
            }

            MetabaseInfoRow myOut = new MetabaseInfoRow(myRows[0], DB, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetMetabaseInfo_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.MetabaseInfo.ModelCol.ForSelect() + ", " +
                DB.MetabaseInfo.ModelVersionCol.ForSelect() + ", " +
                DB.MetabaseInfo.DatabaseRoleCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetMetabaseInfoRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.MetabaseInfo.GetNameAndAlias();
            return sqlString;
        }

        #endregion for MetabaseInfo

        #region for Organization
        //returns the single "row" found when all PKs are spesified
        public OrganizationRow GetOrganizationRow(string aOrganizationCode) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetOrganization_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Organization.OrganizationCodeCol.Is(aOrganizationCode) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," OrganizationCode = " + aOrganizationCode);
            }

            OrganizationRow myOut = new OrganizationRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetOrganization_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";
            if( metaVersionGE("2.1")) {
                sqlString += DB.Organization.WebAddressCol.ForSelect() + " , ";
            }


            sqlString +=
                DB.Organization.OrganizationCodeCol.ForSelect() + ", " +
                DB.Organization.OrganizationNameCol.ForSelect() + ", " +
                DB.Organization.DepartmentCol.ForSelect() + ", " +
                DB.Organization.UnitCol.ForSelect() + ", " +
                DB.Organization.InternalIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.OrganizationLang2.OrganizationNameCol.ForSelectWithFallback(langCode, DB.Organization.OrganizationNameCol);
                    sqlString += ", " + DB.OrganizationLang2.DepartmentCol.ForSelectWithFallback(langCode, DB.Organization.DepartmentCol);
                    sqlString += ", " + DB.OrganizationLang2.UnitCol.ForSelectWithFallback(langCode, DB.Organization.UnitCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetOrganizationRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Organization.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.OrganizationLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Organization.OrganizationCodeCol.Is(DB.OrganizationLang2.OrganizationCodeCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for Organization

        #region for Person
        //returns the single "row" found when all PKs are spesified
        public PersonRow GetPersonRow(string aPersonCode) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetPerson_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Person.PersonCodeCol.Is(aPersonCode) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," PersonCode = " + aPersonCode);
            }

            PersonRow myOut = new PersonRow(myRows[0], DB, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetPerson_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Person.PersonCodeCol.ForSelect() + ", " +
                DB.Person.ForenameCol.ForSelect() + ", " +
                DB.Person.SurnameCol.ForSelect() + ", " +
                DB.Person.OrganizationCodeCol.ForSelect() + ", " +
                DB.Person.PhonePrefixCol.ForSelect() + ", " +
                DB.Person.PhoneNoCol.ForSelect() + ", " +
                DB.Person.FaxNoCol.ForSelect() + ", " +
                DB.Person.EmailCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetPersonRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Person.GetNameAndAlias();
            return sqlString;
        }

        #endregion for Person

        #region for SpecialCharacter
        //returns the single "row" found when all PKs are spesified
        public SpecialCharacterRow GetSpecialCharacterRow(string aCharacterType) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetSpecialCharacter_SQLString_NoWhere();
            sqlString += " WHERE " + DB.SpecialCharacter.CharacterTypeCol.Is(aCharacterType) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," CharacterType = " + aCharacterType);
            }

            SpecialCharacterRow myOut = new SpecialCharacterRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }

        //returns the all  "rows" found in database
        public Dictionary<string, SpecialCharacterRow> GetSpecialCharacterAllRows() {
            string sqlString = GetSpecialCharacter_SQLString_NoWhere();
            Dictionary<string, SpecialCharacterRow> myOut = new Dictionary<string, SpecialCharacterRow>();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1) {
                throw new PCAxis.Sql.Exceptions.DbException(44, "SpecialCharacter", "SPECIALTECKEN");
            }

            foreach (DataRow sqlRow in myRows) {
                SpecialCharacterRow outRow = new SpecialCharacterRow(sqlRow, DB, mLanguageCodes, (IMetaVersionComparator) this );
                myOut.Add(outRow.CharacterType, outRow);
            }
            return myOut;
        }


        private String GetSpecialCharacter_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";
            if( metaVersionGE("2.1")) {
                sqlString += DB.SpecialCharacter.DataCellPresCol.ForSelect() + " , ";
            }
            if( metaVersionGE("2.1")) {
                sqlString += DB.SpecialCharacter.DataCellFilledCol.ForSelect() + " , ";
            }


            sqlString +=
                DB.SpecialCharacter.CharacterTypeCol.ForSelect() + ", " +
                DB.SpecialCharacter.PresCharacterCol.ForSelect() + ", " +
                DB.SpecialCharacter.AggregPossibleCol.ForSelect() + ", " +
                DB.SpecialCharacter.PresTextCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.SpecialCharacterLang2.PresCharacterCol.ForSelectWithFallback(langCode, DB.SpecialCharacter.PresCharacterCol);
                    sqlString += ", " + DB.SpecialCharacterLang2.PresTextCol.ForSelectWithFallback(langCode, DB.SpecialCharacter.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetSpecialCharacterAllRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.SpecialCharacter.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.SpecialCharacterLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.SpecialCharacter.CharacterTypeCol.Is(DB.SpecialCharacterLang2.CharacterTypeCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for SpecialCharacter

        #region for SubTable
        public Dictionary<string, SubTableRow> GetSubTableRows(string aMainTable) {
            Dictionary<string, SubTableRow> myOut = new Dictionary<string, SubTableRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetSubTable_SQLString_NoWhere();
            //
            // WHERE STB.MainTable = '<aMainTable>'
            //
            sqlString += " WHERE " + DB.SubTable.MainTableCol.Is(aMainTable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable);
            }

            foreach (DataRow sqlRow in myRows) {
                SubTableRow outRow = new SubTableRow(sqlRow, DB, mLanguageCodes, (IMetaVersionComparator) this);
                myOut.Add(outRow.SubTable, outRow);
            }
            return myOut;
        }

        private String GetSubTable_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.SubTable.SubTableCol.ForSelect() + ", " +
                DB.SubTable.MainTableCol.ForSelect() + ", " +
                DB.SubTable.PresTextCol.ForSelect() + ", " +
                DB.SubTable.CleanTableCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.SubTableLang2.PresTextCol.ForSelectWithFallback(langCode, DB.SubTable.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetSubTableRows_01 ***" + "/ ";
            sqlString += " FROM " + DB.SubTable.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.SubTableLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.SubTable.MainTableCol.Is(DB.SubTableLang2.MainTableCol, langCode) +
                                 " AND " + DB.SubTable.SubTableCol.Is(DB.SubTableLang2.SubTableCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for SubTable

        #region for SubTableVariable
        //returns the single "row" found when all PKs are spesified
        public SubTableVariableRow GetSubTableVariableRow(string aMainTable, string aSubTable, string aVariable) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetSubTableVariable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(aMainTable)  + 
                             " AND " +DB.SubTableVariable.SubTableCol.Is(aSubTable)  + 
                             " AND " +DB.SubTableVariable.VariableCol.Is(aVariable) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," MainTable = " + aMainTable + " SubTable = " + aSubTable + " Variable = " + aVariable);
            }

            SubTableVariableRow myOut = new SubTableVariableRow(myRows[0], DB, (IMetaVersionComparator) this); 
            return myOut;
        }

        public Dictionary<string, SubTableVariableRow> GetSubTableVariableRowskeyVariable(string aMainTable, string aSubTable) {
            Dictionary<string, SubTableVariableRow> myOut = new Dictionary<string, SubTableVariableRow>();
            SqlDbConfig dbconf = DB;
            string sqlString = GetSubTableVariable_SQLString_NoWhere();
            //
            // WHERE STV.MainTable = '<aMainTable>'
            //    AND STV.SubTable = '<aSubTable>'
            //
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(aMainTable) + 
                         " AND " +DB.SubTableVariable.SubTableCol.Is(aSubTable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            if (myRows.Count < 1) {
                throw new PCAxis.Sql.Exceptions.DbException(35,  " MainTable = " + aMainTable +  " SubTable = " + aSubTable);
            }

            foreach (DataRow sqlRow in myRows) {
                SubTableVariableRow outRow = new SubTableVariableRow(sqlRow, DB, (IMetaVersionComparator) this);
                myOut.Add(outRow.Variable, outRow);
            }
            return myOut;
        }

        private String GetSubTableVariable_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.SubTableVariable.MainTableCol.ForSelect() + ", " +
                DB.SubTableVariable.SubTableCol.ForSelect() + ", " +
                DB.SubTableVariable.VariableCol.ForSelect() + ", " +
                DB.SubTableVariable.ValueSetCol.ForSelect() + ", " +
                DB.SubTableVariable.VariableTypeCol.ForSelect() + ", " +
                DB.SubTableVariable.StoreColumnNoCol.ForSelect();

            sqlString += " /" + "*** SQLID: GetSubTableVariableRowskeyVariable_01 ***" + "/ ";
            sqlString += " FROM " + DB.SubTableVariable.GetNameAndAlias();
            return sqlString;
        }

        #endregion for SubTableVariable

        #region for TextCatalog
        /*This table is not suitable for generated extractions such as Get...*/

        private String GetTextCatalog_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.TextCatalog.TextCatalogNoCol.ForSelect() + ", " +
                DB.TextCatalog.TextTypeCol.ForSelect() + ", " +
                DB.TextCatalog.PresTextCol.ForSelect() + ", " +
                DB.TextCatalog.DescriptionCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.TextCatalogLang2.TextTypeCol.ForSelectWithFallback(langCode, DB.TextCatalog.TextTypeCol);
                    sqlString += ", " + DB.TextCatalogLang2.PresTextCol.ForSelectWithFallback(langCode, DB.TextCatalog.PresTextCol);
                    sqlString += ", " + DB.TextCatalogLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.TextCatalog.DescriptionCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetSubTableVariableRowskeyVariable_01 ***" + "/ ";
            sqlString += " FROM " + DB.TextCatalog.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.TextCatalogLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.TextCatalog.TextCatalogNoCol.Is(DB.TextCatalogLang2.TextCatalogNoCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for TextCatalog

        #region for TimeScale
        //returns the single "row" found when all PKs are spesified
        public TimeScaleRow GetTimeScaleRow(string aTimeScale) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetTimeScale_SQLString_NoWhere();
            sqlString += " WHERE " + DB.TimeScale.TimeScaleCol.Is(aTimeScale) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," TimeScale = " + aTimeScale);
            }

            TimeScaleRow myOut = new TimeScaleRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetTimeScale_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";
            sqlString +=
                DB.TimeScale.TimeScalePresCol.ForSelect() + " , " +
                DB.TimeScale.TimeScaleCol.ForSelect() + ", " +
                DB.TimeScale.PresTextCol.ForSelect() + ", " +
                DB.TimeScale.RegularCol.ForSelect() + ", " +
                DB.TimeScale.TimeUnitCol.ForSelect() + ", " +
                DB.TimeScale.FrequencyCol.ForSelect() + ", " +
                DB.TimeScale.StoreFormatCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.TimeScaleLang2.PresTextCol.ForSelectWithFallback(langCode, DB.TimeScale.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetTimeScaleRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.TimeScale.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.TimeScaleLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.TimeScale.TimeScaleCol.Is(DB.TimeScaleLang2.TimeScaleCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for TimeScale

        #region for ValuePool
        //returns the single "row" found when all PKs are spesified
        public ValuePoolRow GetValuePoolRow(string aValuePool) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetValuePool_SQLString_NoWhere();
            sqlString += " WHERE " + DB.ValuePool.ValuePoolCol.Is(aValuePool) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValuePool = " + aValuePool);
            }

            ValuePoolRow myOut = new ValuePoolRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetValuePool_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.ValuePool.ValuePoolCol.ForSelect() + ", " +
                DB.ValuePool.PresTextCol.ForSelect() + ", " +
                DB.ValuePool.DescriptionCol.ForSelect() + ", " +
                DB.ValuePool.ValueTextExistsCol.ForSelect() + ", " +
                DB.ValuePool.ValuePresCol.ForSelect() + ", " +
                DB.ValuePool.KDBIdCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.ValuePoolLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValuePool.PresTextCol);
                    if (this.metaVersionGE("2.2"))
                    {
                        sqlString += "," + DB.ValuePoolLang2.ValuePoolAliasCol.ForSelect(langCode);
                    }
                    else
                    {
                        sqlString += "," + DB.ValuePoolLang2.ValuePoolEngCol.ForSelect(langCode);
                    }
                }
            }

            sqlString += " /" + "*** SQLID: GetValuePoolRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValuePool.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.ValuePoolLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValuePool.ValuePoolCol.Is(DB.ValuePoolLang2.ValuePoolCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for ValuePool

        #region for ValueSet
        //returns the single "row" found when all PKs are spesified
        public ValueSetRow GetValueSetRow(string aValueSet) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetValueSet_SQLString_NoWhere();
            sqlString += " WHERE " + DB.ValueSet.ValueSetCol.Is(aValueSet) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValueSet = " + aValueSet);
            }

            ValueSetRow myOut = new ValueSetRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetValueSet_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";
            if( metaVersionGE("2.1")) {
                sqlString += DB.ValueSet.PresTextCol.ForSelect() + " , ";
            }


            sqlString +=
                DB.ValueSet.ValueSetCol.ForSelect() + ", " +
                DB.ValueSet.DescriptionCol.ForSelect() + ", " +
                DB.ValueSet.EliminationCol.ForSelect() + ", " +
                DB.ValueSet.ValuePoolCol.ForSelect() + ", " +
                DB.ValueSet.ValuePresCol.ForSelect() + ", " +
                DB.ValueSet.GeoAreaNoCol.ForSelect() + ", " +
                DB.ValueSet.KDBIdCol.ForSelect() + ", " +
                DB.ValueSet.SortCodeExistsCol.ForSelect() + ", " +
                DB.ValueSet.FootnoteCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    if( metaVersionGE("2.1")){
                        sqlString += ", " + DB.ValueSetLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValueSet.PresTextCol);
                    }
                    sqlString += ", " + DB.ValueSetLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.ValueSet.DescriptionCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetValueSetRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.ValueSet.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.ValueSetLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValueSet.ValueSetCol.Is(DB.ValueSetLang2.ValueSetCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for ValueSet

        #region for Variable
        //returns the single "row" found when all PKs are spesified
        public VariableRow GetVariableRow(string aVariable) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetVariable_SQLString_NoWhere();
            sqlString += " WHERE " + DB.Variable.VariableCol.Is(aVariable) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," Variable = " + aVariable);
            }

            VariableRow myOut = new VariableRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetVariable_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.Variable.VariableCol.ForSelect() + ", " +
                DB.Variable.PresTextCol.ForSelect() + ", " +
                DB.Variable.VariableInfoCol.ForSelect() + ", " +
                DB.Variable.FootnoteCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.VariableLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Variable.PresTextCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetVariableRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.Variable.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.VariableLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Variable.VariableCol.Is(DB.VariableLang2.VariableCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for Variable

        #region for VSGroup
        //returns the single "row" found when all PKs are spesified
        public VSGroupRow GetVSGroupRow(string aValueSet, string aGrouping, string aGroupCode, string aValueCode, string aValuePool) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetVSGroup_SQLString_NoWhere();
            sqlString += " WHERE " + DB.VSGroup.ValueSetCol.Is(aValueSet)  + 
                             " AND " +DB.VSGroup.GroupingCol.Is(aGrouping)  + 
                             " AND " +DB.VSGroup.GroupCodeCol.Is(aGroupCode)  + 
                             " AND " +DB.VSGroup.ValueCodeCol.Is(aValueCode)  + 
                             " AND " +DB.VSGroup.ValuePoolCol.Is(aValuePool) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValueSet = " + aValueSet + " Grouping = " + aGrouping + " GroupCode = " + aGroupCode + " ValueCode = " + aValueCode + " ValuePool = " + aValuePool);
            }

            VSGroupRow myOut = new VSGroupRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetVSGroup_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.VSGroup.ValueSetCol.ForSelect() + ", " +
                DB.VSGroup.GroupingCol.ForSelect() + ", " +
                DB.VSGroup.GroupCodeCol.ForSelect() + ", " +
                DB.VSGroup.ValueCodeCol.ForSelect() + ", " +
                DB.VSGroup.ValuePoolCol.ForSelect() + ", " +
                DB.VSGroup.SortCodeCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.VSGroupLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.VSGroup.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetVSGroupRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.VSGroup.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.VSGroupLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.VSGroup.ValueSetCol.Is(DB.VSGroupLang2.ValueSetCol, langCode) +
                                 " AND " + DB.VSGroup.GroupingCol.Is(DB.VSGroupLang2.GroupingCol, langCode) +
                                 " AND " + DB.VSGroup.GroupCodeCol.Is(DB.VSGroupLang2.GroupCodeCol, langCode) +
                                 " AND " + DB.VSGroup.ValueCodeCol.Is(DB.VSGroupLang2.ValueCodeCol, langCode) +
                                 " AND " + DB.VSGroup.ValuePoolCol.Is(DB.VSGroupLang2.ValuePoolCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for VSGroup

        #region for VSValue
        //returns the single "row" found when all PKs are spesified
        public VSValueRow GetVSValueRow(string aValueSet, string aValuePool, string aValueCode) {
            //SqlDbConfig dbconf = DB;
            string sqlString = GetVSValue_SQLString_NoWhere();
            sqlString += " WHERE " + DB.VSValue.ValueSetCol.Is(aValueSet)  + 
                             " AND " +DB.VSValue.ValuePoolCol.Is(aValuePool)  + 
                             " AND " +DB.VSValue.ValueCodeCol.Is(aValueCode) ;

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1) {
                throw new PCAxis.Sql.Exceptions.DbException(36," ValueSet = " + aValueSet + " ValuePool = " + aValuePool + " ValueCode = " + aValueCode);
            }

            VSValueRow myOut = new VSValueRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator) this); 
            return myOut;
        }


        private String GetVSValue_SQLString_NoWhere() {  
            //SqlDbConfig dbconf = DB;   
            string sqlString = "SELECT ";


            sqlString +=
                DB.VSValue.ValueSetCol.ForSelect() + ", " +
                DB.VSValue.ValuePoolCol.ForSelect() + ", " +
                DB.VSValue.ValueCodeCol.ForSelect() + ", " +
                DB.VSValue.SortCodeCol.ForSelect();


            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += ", " + DB.VSValueLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.VSValue.SortCodeCol);
                }
            }

            sqlString += " /" + "*** SQLID: GetVSValueRow_01 ***" + "/ ";
            sqlString += " FROM " + DB.VSValue.GetNameAndAlias();

            foreach (String langCode in mLanguageCodes) {
                if (DB.isSecondaryLanguage(langCode)) {
                    sqlString += " LEFT JOIN "  + DB.VSValueLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.VSValue.ValueSetCol.Is(DB.VSValueLang2.ValueSetCol, langCode) +
                                 " AND " + DB.VSValue.ValuePoolCol.Is(DB.VSValueLang2.ValuePoolCol, langCode) +
                                 " AND " + DB.VSValue.ValueCodeCol.Is(DB.VSValueLang2.ValueCodeCol, langCode);
                }
            }

            return sqlString;
        }

        #endregion for VSValue

    }
}
