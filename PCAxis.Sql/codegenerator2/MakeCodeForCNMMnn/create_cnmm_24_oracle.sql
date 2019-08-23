--generert create db script for oracle

CREATE TABLE Attribute(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Attribute VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,AttributeColumn VARCHAR2(41 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(25 CHAR)
 ,SequenceNo NUMBER NOT NULL ENABLE
 ,Description VARCHAR2(200 CHAR)
 ,ValueSet VARCHAR2(30 CHAR)
 ,ColumnLength NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Attribute PRIMARY KEY (MainTable,Attribute ) 
);

COMMENT ON COLUMN Attribute.MainTable IS 'CNMM:Name of the main table to which the attribute is linked.';
COMMENT ON COLUMN Attribute.Attribute IS 'CNMM:Name of the attribute';
COMMENT ON COLUMN Attribute.AttributeColumn IS 'CNMM:Name of the column. The length is set to accommodate future use of prefixes';
COMMENT ON COLUMN Attribute.PresText IS 'CNMM:Presentation text used by the retrieval interface.';
COMMENT ON COLUMN Attribute.SequenceNo IS 'CNMM:The attributes place in the data table column or the place within the column. The first attribute for a given main table has the value 1.';
COMMENT ON COLUMN Attribute.Description IS 'CNMM:Description of the attribute.';
COMMENT ON COLUMN Attribute.ValueSet IS 'CNMM:Name of the value set to which the values are linked.
See further description in table ValueSet.

Can be null – for example if the attribute contains a comment.';
COMMENT ON COLUMN Attribute.ColumnLength IS 'CNMM:Number of stored characters in the data column.';
COMMENT ON COLUMN Attribute.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Attribute.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Attribute IS 'CNMM:The table contains information on the attribute on the observation values.

See further information in the separate document: Attributes in the Nordic SQL Data Model.';

CREATE TRIGGER Attribute_BUPSE BEFORE INSERT OR UPDATE ON Attribute
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Attribute;


-------

CREATE TABLE Attribute_ENG(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Attribute VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(25 CHAR)
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Attribute_ENG PRIMARY KEY (MainTable,Attribute ) 
);

COMMENT ON COLUMN Attribute_ENG.MainTable IS 'CNMM: SL version:Name of the main table to which the attribute is linked.';
COMMENT ON COLUMN Attribute_ENG.Attribute IS 'CNMM: SL version:Name of the attribute';
COMMENT ON COLUMN Attribute_ENG.PresText IS 'CNMM: SL version:Presentation text used by the retrieval interface.';
COMMENT ON COLUMN Attribute_ENG.Description IS 'CNMM: SL version:Description of the attribute.';
COMMENT ON COLUMN Attribute_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Attribute_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Attribute_ENG IS 'CNMM: SL: The table contains information on the attribute on the observation values.

See further information in the separate document: Attributes in the Nordic SQL Data Model.';

CREATE TRIGGER Attribute_ENG_BUPSE BEFORE INSERT OR UPDATE ON Attribute_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Attribute_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ColumnCode(
 MetaTable VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ColumnName VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,Code VARCHAR2(10 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ColumnCode PRIMARY KEY (MetaTable,ColumnName,Code ) 
);

COMMENT ON COLUMN ColumnCode.MetaTable IS 'CNMM:Name of metadata table.';
COMMENT ON COLUMN ColumnCode.ColumnName IS 'CNMM:Name of column.';
COMMENT ON COLUMN ColumnCode.Code IS 'CNMM:Acceptable code alternative.';
COMMENT ON COLUMN ColumnCode.PresText IS 'CNMM:Descriptive presentation text for code alternatives.';
COMMENT ON COLUMN ColumnCode.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ColumnCode.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ColumnCode IS 'CNMM:The table is a list and description of all the acceptable codes for the metadata fields in the metadata database.';

CREATE TRIGGER ColumnCode_BUPSE BEFORE INSERT OR UPDATE ON ColumnCode
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ColumnCode;


-------

CREATE TABLE ColumnCode_ENG(
 MetaTable VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ColumnName VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,Code VARCHAR2(10 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ColumnCode_ENG PRIMARY KEY (MetaTable,ColumnName,Code ) 
);

COMMENT ON COLUMN ColumnCode_ENG.MetaTable IS 'CNMM: SL version:Name of metadata table.';
COMMENT ON COLUMN ColumnCode_ENG.ColumnName IS 'CNMM: SL version:Name of column.';
COMMENT ON COLUMN ColumnCode_ENG.Code IS 'CNMM: SL version:Acceptable code alternative.';
COMMENT ON COLUMN ColumnCode_ENG.PresText IS 'CNMM: SL version:Descriptive presentation text for code alternatives.';
COMMENT ON COLUMN ColumnCode_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ColumnCode_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ColumnCode_ENG IS 'CNMM: SL: The table is a list and description of all the acceptable codes for the metadata fields in the metadata database.';

CREATE TRIGGER ColumnCode_ENG_BUPSE BEFORE INSERT OR UPDATE ON ColumnCode_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ColumnCode_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE Contents(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,PresTextS VARCHAR2(80 CHAR)
 ,PresCode VARCHAR2(8 CHAR) NOT NULL ENABLE
 ,Copyright VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,StatAuthority VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Producer VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LastUpdated DATE
 ,Published DATE
 ,Unit VARCHAR2(60 CHAR) NOT NULL ENABLE
 ,PresDecimals NUMBER NOT NULL ENABLE
 ,PresCellsZero VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,PresMissingLine VARCHAR2(2 CHAR)
 ,AggregPossible VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,RefPeriod VARCHAR2(80 CHAR)
 ,StockFA VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,BasePeriod VARCHAR2(20 CHAR)
 ,CFPrices VARCHAR2(1 CHAR)
 ,DayAdj VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,SeasAdj VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FootnoteContents VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FootnoteVariable VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FootnoteValue VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FootnoteTime VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,StoreColumnNo NUMBER NOT NULL ENABLE
 ,StoreFormat VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,StoreNoChar NUMBER NOT NULL ENABLE
 ,StoreDecimals NUMBER NOT NULL ENABLE
 ,MetaId VARCHAR2(100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Contents PRIMARY KEY (MainTable,Contents ) 
);

COMMENT ON COLUMN Contents.MainTable IS 'CNMM:Name of the main table to which the content columns are linked. See further description in the table MainTable.';
COMMENT ON COLUMN Contents.Contents IS 'CNMM:Name of the data columns in the data table. 

A main tables content columns must have unique names within that main table but the same column name can occur in other main tables. 

The name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN Contents.PresText IS 'CNMM:Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading. 

The presentation text should be unique within a main table. 

The text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment. 

The text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.';
COMMENT ON COLUMN Contents.PresTextS IS 'CNMM:Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents. 

The text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop. 

If there is no text, the field should be NULL.';
COMMENT ON COLUMN Contents.PresCode IS 'CNMM:Presentation code.';
COMMENT ON COLUMN Contents.Copyright IS 'CNMM:Code for copyright ownership.

All content columns within a main table must belong to the same category, i.e. the same code should be in all the fields.';
COMMENT ON COLUMN Contents.StatAuthority IS 'CNMM:Code for the authority responsible for the statistics (statistical authority). 

All content columns in a main table must belong to the same statistical authority. 

Data is taken from the column OrganizationCode in the table Organization. For a more detailed description, see that table.';
COMMENT ON COLUMN Contents.Producer IS 'CNMM:All content columns in a main table must have the same producer. 

Data is taken from the column OrganizationCode in the table Organization. For a more detailed description, see that table.';
COMMENT ON COLUMN Contents.LastUpdated IS 'CNMM:The date of the most recent update (incl. exact time) of the main tables data tables on the production server. The date remains unchanged when copying over to the external servers. The field is updated automatically by the programs used for loading and updating of data. 

If there is no text, the field should be NULL.';
COMMENT ON COLUMN Contents.Published IS 'CNMM:The most recent date for the official release of data, i.e. the date and exact time for the transfer of the main tables data tables to the external servers. The field is updated automatically by the program used for the transfer. 

If there is no text, the field should be NULL.';
COMMENT ON COLUMN Contents.Unit IS 'CNMM:Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents. 

The unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.';
COMMENT ON COLUMN Contents.PresDecimals IS 'CNMM:The number of decimals that are to be presented in the table presentation when retrievals are made.';
COMMENT ON COLUMN Contents.PresCellsZero IS 'CNMM:The data table, in principle, only stores cells with values that are not zero. Cells containing zero are only stored if another content column in the same data table has a value other than zero (0) in the corresponding cell. In this field, it should be stated how data cells that are not stored, i.e. cells for which data is missing in all content columns, should be presented. Alternatives: 

Y = Yes. The cells are assumed to contain zero. Presented as zero (0). 
N = No. Data cannot be given. 

If PresCellsZero is N, the field PresMissingLine should be used to indicate how these cells should be presented. See description of PresMissingLine in Contents. See also the description of the table SpecialCharacter.';
COMMENT ON COLUMN Contents.PresMissingLine IS 'CNMM:The field is used by the retrieval programs for presenting cells that should not be presented as zero, i.e. when PresCellsZero in Contents is N. The field can either be NULL or contain the identity for a special character. The identity must in that case exist in the column CharacterType in the table SpecialCharacter, and the character must exist in the column PresCharacter in this table. If PresMissingLine is NULL, DefaultCodeMissingLine in MetaAdm is used. 

See also descriptions of: the column PresCellsZero in the table Contents,  the columns CharacterType and PresCharacter in the table SpecialCharacter and the table MetaAdm.';
COMMENT ON COLUMN Contents.AggregPossible IS 'CNMM:Shows if the content can be aggregated or not. Applies to all distributed variables for the content column. There are the following alternatives: 

Y = Yes 
N = No 

If AggregPossible = N, the possibility to both aggregate and group the data in the retrieval interface is eliminated.';
COMMENT ON COLUMN Contents.RefPeriod IS 'CNMM:RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S". 

If the reference time is not available, the field should be NULL.';
COMMENT ON COLUMN Contents.StockFA IS 'CNMM:Shows whether the statistical material is of the type stock, flow or average. There are the following alternatives: 

F = Flow. Measurement time refers to a specific period. The result describes events that occurred successively during the measurement period. 
A = Average. The result is made up of an average value of observation values at different measurement times. 
S = Stock. The measurement time refers to a specific point in time. 
X = Other';
COMMENT ON COLUMN Contents.BasePeriod IS 'CNMM:The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.';
COMMENT ON COLUMN Contents.CFPrices IS 'CNMM:Current/fixed prices. Alternatives: 
F = Fixed prices. 
C = Current prices. 

In cases where data are not relevant, the field should be NULL.';
COMMENT ON COLUMN Contents.DayAdj IS 'CNMM:Shows whether the statistical material is calendar adjusted or not during the measurement period. There are the following alternatives: 

Y = Yes 
N = No';
COMMENT ON COLUMN Contents.SeasAdj IS 'CNMM:Shows whether the statistical material is seasonally adjusted or not, i.e. adjusted for different periodical variations during the measurement period that may have affected the result. There are the following alternatives: 

Y = Yes 
N = No';
COMMENT ON COLUMN Contents.FootnoteContents IS 'CNMM:Shows whether there is a footnote attached to the content column (FootnoteType = 2). There are the following alternatives: 

B = Both obligatory and optional footnotes exist. 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist. 
N = There are no footnotes. 

The field is automatically updated when a footnote is attached or taken away from the content column.';
COMMENT ON COLUMN Contents.FootnoteVariable IS 'CNMM:Shows whether there is a footnote attached to a specific variable in the content column (FootnoteType = 3). There are the following alternatives: 

B = Both obligatory and optional footnotes exist. 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist. 
N = There are no footnotes. 

The field is automatically updated when a footnote is attached or taken away from a variable in the content column.';
COMMENT ON COLUMN Contents.FootnoteValue IS 'CNMM:Shows whether there is a footnote attached to a specific value in the content column (footnote type = 4).

There are the following alternatives: 

B = Both obligatory and optional footnotes exist. 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist. 
N = There are no footnotes. 

The field is automatically updated when a footnote is attached or taken away from a value in the content column.';
COMMENT ON COLUMN Contents.FootnoteTime IS 'CNMM:Shows if there is a footnote attached to a specific point in time in the content column (FootnoteType = 4). There are the following alternatives: 

B = Both obligatory and optional footnotes exist. 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist. 
N = There are no footnotes. 

The field is automatically updated when a footnote is attached or taken away from a point in time in the content column.';
COMMENT ON COLUMN Contents.StoreColumnNo IS 'CNMM:Here the content columns (data columns) order of storage amongst themselves in the data table is shown.';
COMMENT ON COLUMN Contents.StoreFormat IS 'CNMM:Specifies  the storage format for data cells for the content column. There are the following alternatives: 
C = Varchar. Can only be used to store unit information in the data tables. There must exist a column with name unit and the filed Contents.Unit should be set to ‘%Datatable’ 
I = Integer. For numbers of size 2 147 483 647 to - 2 147 483 648. 
N = Numeric. For larger numbers and always when the material is stored with decimals. 
S = Smallint. For numbers of size 32 767 to - 32 768. 

Also see the description of column StoreNoChar.';
COMMENT ON COLUMN Contents.StoreNoChar IS 'CNMM:Number of stored characters in the data column. Number of characters should be: 

2, if StoreFormat = S, 
4, if StoreFormat = I, 
2-17 (decimals included, decimal point excluded), if StoreFormat = N 
1-30 characters, if StoreFormat = C. 

Also see the description of column StoreFormat in the table Contents.';
COMMENT ON COLUMN Contents.StoreDecimals IS 'CNMM:Number of stored decimals (0-15). 

Data should be included in StoreNoChar if StoreFormat = N.';
COMMENT ON COLUMN Contents.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN Contents.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Contents.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Contents IS 'CNMM:The table contains information on the content of the data table(s).The content columns name is the same as the name of the corresponding data columns in the data table.';

CREATE TRIGGER Contents_BUPSE BEFORE INSERT OR UPDATE ON Contents
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Contents;


-------

CREATE TABLE Contents_ENG(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,PresTextS VARCHAR2(80 CHAR)
 ,Unit VARCHAR2(60 CHAR) NOT NULL ENABLE
 ,RefPeriod VARCHAR2(80 CHAR)
 ,BasePeriod VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Contents_ENG PRIMARY KEY (MainTable,Contents ) 
);

COMMENT ON COLUMN Contents_ENG.MainTable IS 'CNMM: SL version:Name of the main table to which the content columns are linked. See further description in the table MainTable.';
COMMENT ON COLUMN Contents_ENG.Contents IS 'CNMM: SL version:Name of the data columns in the data table. 

A main tables content columns must have unique names within that main table but the same column name can occur in other main tables. 

The name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN Contents_ENG.PresText IS 'CNMM: SL version:Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading. 

The presentation text should be unique within a main table. 

The text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment. 

The text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.';
COMMENT ON COLUMN Contents_ENG.PresTextS IS 'CNMM: SL version:Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents. 

The text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop. 

If there is no text, the field should be NULL.';
COMMENT ON COLUMN Contents_ENG.Unit IS 'CNMM: SL version:Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents. 

The unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.';
COMMENT ON COLUMN Contents_ENG.RefPeriod IS 'CNMM: SL version:RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S". 

If the reference time is not available, the field should be NULL.';
COMMENT ON COLUMN Contents_ENG.BasePeriod IS 'CNMM: SL version:The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.';
COMMENT ON COLUMN Contents_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Contents_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Contents_ENG IS 'CNMM: SL: The table contains information on the content of the data table(s).The content columns name is the same as the name of the corresponding data columns in the data table.';

CREATE TRIGGER Contents_ENG_BUPSE BEFORE INSERT OR UPDATE ON Contents_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Contents_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ContentsTime(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,TimePeriod VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ContentsTime PRIMARY KEY (MainTable,Contents,TimePeriod ) 
);

COMMENT ON COLUMN ContentsTime.MainTable IS 'CNMM:The name of the main table that the point in time in the content column is linked to. See further description in the table MainTable.';
COMMENT ON COLUMN ContentsTime.Contents IS 'CNMM:The name of the content column that the point in time is linked to. 
See description in the table Contents.';
COMMENT ON COLUMN ContentsTime.TimePeriod IS 'CNMM:Code for point in time, i.e. 2003, 2003Q1, 2003M01. 

Rules for how codes should be constructed are available in the column StoreFormat in the table TimeScale.';
COMMENT ON COLUMN ContentsTime.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ContentsTime.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ContentsTime IS 'CNMM:The table links points in time for which data is stored, to a content column.';

CREATE TRIGGER ContentsTime_BUPSE BEFORE INSERT OR UPDATE ON ContentsTime
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ContentsTime;


------------------------------------------------------------

CREATE TABLE DataStorage(
 ProductCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ServerName VARCHAR2(8 CHAR) NOT NULL ENABLE
 ,DatabaseName VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_DataStorage PRIMARY KEY (ProductCode ) 
);

COMMENT ON COLUMN DataStorage.ProductCode IS 'CNMM:Unique identifier for at product group.';
COMMENT ON COLUMN DataStorage.ServerName IS 'CNMM:Name of the server where the database is situated.';
COMMENT ON COLUMN DataStorage.DatabaseName IS 'CNMM:Name of the database where the products data tables are stored.';
COMMENT ON COLUMN DataStorage.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN DataStorage.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE DataStorage IS 'CNMM:The table contains information on which database the data tables for the statistical product are stored in. ';

CREATE TRIGGER DataStorage_BUPSE BEFORE INSERT OR UPDATE ON DataStorage
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END DataStorage;


------------------------------------------------------------

CREATE TABLE Footnote(
 FootnoteNo NUMBER NOT NULL ENABLE
 ,FootnoteType VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,ShowFootnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,MandOpt VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FootnoteText VARCHAR2(3000 CHAR) NOT NULL ENABLE
 ,PresCharacter VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Footnote PRIMARY KEY (FootnoteNo ) 
);

COMMENT ON COLUMN Footnote.FootnoteNo IS 'CNMM:Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm. 
 
See further in the description of the table MetaAdm.';
COMMENT ON COLUMN Footnote.FootnoteType IS 'CNMM:Code for the type of footnote. There are the following alternatives: 
 
1 = footnote on subject area 
2 = footnote on content column 
3 = footnote on variable + content column 
4 = footnote on value/time + content column 
5 = footnote on variable 
6 = footnote on value 
7 = footnote on main table 
8 = footnote on sub-table 
9 = footnote on value + main table 
A = footnote on statistical area (level 2) 
B = footnote on product (level 3) 
C = footnote on table group (level 4) 
Q = footnote on grouping';
COMMENT ON COLUMN Footnote.ShowFootnote IS 'CNMM:Contains information on when the footnote should be shown in the outdata program, i.e. when content is selected for a table, when the table is presented or both. 
There are the following alternatives: 

B = show both at selection and presentation 
P = show at presentation
S = shown upon selection';
COMMENT ON COLUMN Footnote.MandOpt IS 'CNMM:Code for whether the footnote is classified as "optional" or "mandatory". 
Alternatives: 

O = optional 
M = mandatory';
COMMENT ON COLUMN Footnote.FootnoteText IS 'CNMM:Text in the footnote. Written as consecutive text, starting with a capital letter. 

NB! Double quotation marks should not be used as this causes problems in PC-AXIS.';
COMMENT ON COLUMN Footnote.PresCharacter IS 'CNMM:Special character or special characters to be associated with the footnote';
COMMENT ON COLUMN Footnote.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Footnote.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Footnote IS 'CNMM:The table contains footnote texts and information on footnotes.';

CREATE TRIGGER Footnote_BUPSE BEFORE INSERT OR UPDATE ON Footnote
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Footnote;


-------

CREATE TABLE Footnote_ENG(
 FootnoteNo NUMBER NOT NULL ENABLE
 ,FootnoteText VARCHAR2(3000 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Footnote_ENG PRIMARY KEY (FootnoteNo ) 
);

COMMENT ON COLUMN Footnote_ENG.FootnoteNo IS 'CNMM: SL version:Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm. 
 
See further in the description of the table MetaAdm.';
COMMENT ON COLUMN Footnote_ENG.FootnoteText IS 'CNMM: SL version:Text in the footnote. Written as consecutive text, starting with a capital letter. 

NB! Double quotation marks should not be used as this causes problems in PC-AXIS.';
COMMENT ON COLUMN Footnote_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Footnote_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Footnote_ENG IS 'CNMM: SL: The table contains footnote texts and information on footnotes.';

CREATE TRIGGER Footnote_ENG_BUPSE BEFORE INSERT OR UPDATE ON Footnote_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Footnote_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE FootnoteContTime(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,TimePeriod VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,Cellnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteContTime PRIMARY KEY (MainTable,Contents,TimePeriod,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteContTime.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteContTime.Contents IS 'CNMM:Name of content column. 
 
See further in the description of the table Contents.';
COMMENT ON COLUMN FootnoteContTime.TimePeriod IS 'CNMM:Point in time that the footnote relates to. 
 
See descriptions in table TimeScale and ContentsTime.';
COMMENT ON COLUMN FootnoteContTime.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteContTime.Cellnote IS 'CNMM:State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content columns variables, e.g. Region and Time or Region and Sex. 
Alternatives: 
Y = Yes, the footnote is a cell footnote 
N = No ';
COMMENT ON COLUMN FootnoteContTime.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteContTime.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteContTime IS 'CNMM:The table links footnotes to a point in time for a specific content column.';

CREATE TRIGGER FootnoteContTime_BUPSE BEFORE INSERT OR UPDATE ON FootnoteContTime
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteContTime;


------------------------------------------------------------

CREATE TABLE FootnoteContValue(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,Cellnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteContValue PRIMARY KEY (MainTable,Contents,Variable,ValuePool,ValueCode,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteContValue.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteContValue.Contents IS 'CNMM:Name of content column. 
 
See further in the description of the table Contents.';
COMMENT ON COLUMN FootnoteContValue.Variable IS 'CNMM:Name of variable. 
 
See further in the description of the table Variable.';
COMMENT ON COLUMN FootnoteContValue.ValuePool IS 'CNMM:Name of value pool. 
 
See further in the description of the table ValuePool.';
COMMENT ON COLUMN FootnoteContValue.ValueCode IS 'CNMM:Code for the value that the footnote relates to. 
 
See further in the description of the table Value.';
COMMENT ON COLUMN FootnoteContValue.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteContValue.Cellnote IS 'CNMM:State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content columns variables, e.g. Region and Time or Region and Sex. 
Alternatives: 
Y = Yes, the footnote is a cell footnote 
N = No ';
COMMENT ON COLUMN FootnoteContValue.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteContValue.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteContValue IS 'CNMM:The table links footnotes to a value for a specific content column.';

CREATE TRIGGER FootnoteContValue_BUPSE BEFORE INSERT OR UPDATE ON FootnoteContValue
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteContValue;


------------------------------------------------------------

CREATE TABLE FootnoteContVbl(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteContVbl PRIMARY KEY (MainTable,Contents,Variable,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteContVbl.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteContVbl.Contents IS 'CNMM:Name of content column. 
 
See further in the description of the table Contents.';
COMMENT ON COLUMN FootnoteContVbl.Variable IS 'CNMM:Name of variable. 
 
See further in the description of the table Variable.';
COMMENT ON COLUMN FootnoteContVbl.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteContVbl.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteContVbl.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteContVbl IS 'CNMM:The table links footnotes to a variable for a specific content column.';

CREATE TRIGGER FootnoteContVbl_BUPSE BEFORE INSERT OR UPDATE ON FootnoteContVbl
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteContVbl;


------------------------------------------------------------

CREATE TABLE FootnoteContents(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Contents VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteContents PRIMARY KEY (MainTable,Contents,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteContents.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteContents.Contents IS 'CNMM:Name of content column. 
 
See further in the description of the table Contents.';
COMMENT ON COLUMN FootnoteContents.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteContents.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteContents.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteContents IS 'CNMM:The table links footnotes to a content column.';

CREATE TRIGGER FootnoteContents_BUPSE BEFORE INSERT OR UPDATE ON FootnoteContents
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteContents;


------------------------------------------------------------

CREATE TABLE FootnoteGrouping(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteGrouping PRIMARY KEY (Grouping,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteGrouping.Grouping IS 'CNMM:Name of groupong 
See further in the description of the table Gruoping.';
COMMENT ON COLUMN FootnoteGrouping.FootnoteNo IS 'CNMM:Number of the footnote.
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteGrouping.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteGrouping.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteGrouping IS 'CNMM:The table links footnotes to a grouping.';

CREATE TRIGGER FootnoteGrouping_BUPSE BEFORE INSERT OR UPDATE ON FootnoteGrouping
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteGrouping;


------------------------------------------------------------

CREATE TABLE FootnoteMainTable(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteMainTable PRIMARY KEY (MainTable,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteMainTable.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteMainTable.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteMainTable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteMainTable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteMainTable IS 'CNMM:The table links footnotes to a main table.

N.B this table can only be used as long as all contents of the main table has the same connections in the ContentsTime table. Footnotes that are linked in this way should have type = 9. This is the same type that is used for notes with FootnoteMaintValue. By using that type the footnote can be specified so that it is valid for the intersection of more than one column thereby assigning the footnote to a subset of the data.';

CREATE TRIGGER FootnoteMainTable_BUPSE BEFORE INSERT OR UPDATE ON FootnoteMainTable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteMainTable;


------------------------------------------------------------

CREATE TABLE FootnoteMaintTime(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,TimePeriod VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteMaintTime PRIMARY KEY (MainTable,TimePeriod,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteMaintTime.MainTable IS 'CNMM:Name of main table';
COMMENT ON COLUMN FootnoteMaintTime.TimePeriod IS 'CNMM:Timeperiod';
COMMENT ON COLUMN FootnoteMaintTime.FootnoteNo IS 'CNMM:Footnote number';
COMMENT ON COLUMN FootnoteMaintTime.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteMaintTime.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteMaintTime IS 'CNMM:Footnote for points in time or timeperiods for a main table.

N.B this table can only be used as long as all contents of the main table has the same connections in the ContentsTime table. Footnotes that are linked in this way should have type = 9. This is the same type that is used for notes with FootnoteMaintValue. By using that type the footnote can be specified so that it is valid for the intersection of more than one column thereby assigning the footnote to a subset of the data.';

CREATE TRIGGER FootnoteMaintTime_BUPSE BEFORE INSERT OR UPDATE ON FootnoteMaintTime
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteMaintTime;


------------------------------------------------------------

CREATE TABLE FootnoteMaintValue(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteMaintValue PRIMARY KEY (MainTable,Variable,ValuePool,ValueCode,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteMaintValue.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteMaintValue.Variable IS 'CNMM:Name of variable. 
 
See further in the description of the table Variable.';
COMMENT ON COLUMN FootnoteMaintValue.ValuePool IS 'CNMM:Name of value pool. 
 
See further in the description of the table ValuePool.';
COMMENT ON COLUMN FootnoteMaintValue.ValueCode IS 'CNMM:Code for the value that the footnote relates to. 
 
See further in the description of the table Value.';
COMMENT ON COLUMN FootnoteMaintValue.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteMaintValue.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteMaintValue.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteMaintValue IS 'CNMM:The table links footnotes to a value for a specific content column.';

CREATE TRIGGER FootnoteMaintValue_BUPSE BEFORE INSERT OR UPDATE ON FootnoteMaintValue
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteMaintValue;


------------------------------------------------------------

CREATE TABLE FootnoteMenuSel(
 Menu VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,Selection VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteMenuSel PRIMARY KEY (Menu,Selection,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteMenuSel.Menu IS 'CNMM:Code for relevant menu level. 
 
See further in the description of the table MenuSelection.';
COMMENT ON COLUMN FootnoteMenuSel.Selection IS 'CNMM:Code for the nearest underlying eligible alternative in the relevant menu level.';
COMMENT ON COLUMN FootnoteMenuSel.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteMenuSel.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteMenuSel.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteMenuSel IS 'CNMM:The table links footnotes to all levels above the MainTable.';

CREATE TRIGGER FootnoteMenuSel_BUPSE BEFORE INSERT OR UPDATE ON FootnoteMenuSel
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteMenuSel;


------------------------------------------------------------

CREATE TABLE FootnoteSubTable(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,SubTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteSubTable PRIMARY KEY (MainTable,SubTable,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteSubTable.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN FootnoteSubTable.SubTable IS 'CNMM:Name of sub-table 
 
See further in the description of the table SubTable.';
COMMENT ON COLUMN FootnoteSubTable.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteSubTable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteSubTable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteSubTable IS 'CNMM:The table links footnotes to a main table.';

CREATE TRIGGER FootnoteSubTable_BUPSE BEFORE INSERT OR UPDATE ON FootnoteSubTable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteSubTable;


------------------------------------------------------------

CREATE TABLE FootnoteValue(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteValue PRIMARY KEY (ValuePool,ValueCode,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteValue.ValuePool IS 'CNMM:Name of value pool. 
 
See further in the description of the table ValuePool.';
COMMENT ON COLUMN FootnoteValue.ValueCode IS 'CNMM:Code for the value that the footnote relates to. 
 
See further in the description of the table Value.';
COMMENT ON COLUMN FootnoteValue.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteValue.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteValue.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteValue IS 'CNMM:The table links footnotes to a value.';

CREATE TRIGGER FootnoteValue_BUPSE BEFORE INSERT OR UPDATE ON FootnoteValue
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteValue;


------------------------------------------------------------

CREATE TABLE FootnoteValueSetValue(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteValueSetValue PRIMARY KEY (ValuePool,ValueSet,ValueCode,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteValueSetValue.ValuePool IS 'CNMM:Name of value pool.

See further in the description of the table ValuePool.';
COMMENT ON COLUMN FootnoteValueSetValue.ValueSet IS 'CNMM:Name of value set.

See further in the description of the table ValueSet.';
COMMENT ON COLUMN FootnoteValueSetValue.ValueCode IS 'CNMM:Code for the value that the footnote relates to.

See further in the description of the table Value.';
COMMENT ON COLUMN FootnoteValueSetValue.FootnoteNo IS 'CNMM:Number of the footnote.

See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteValueSetValue.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteValueSetValue.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteValueSetValue IS 'CNMM:The table links footnotes to a valueset.';

CREATE TRIGGER FootnoteValueSetValue_BUPSE BEFORE INSERT OR UPDATE ON FootnoteValueSetValue
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteValueSetValue;


------------------------------------------------------------

CREATE TABLE FootnoteVariable(
 Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FootnoteNo NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_FootnoteVariable PRIMARY KEY (Variable,FootnoteNo ) 
);

COMMENT ON COLUMN FootnoteVariable.Variable IS 'CNMM:Name of the variable that the footnote relates to. 
 
See further in the description of the table Variable.';
COMMENT ON COLUMN FootnoteVariable.FootnoteNo IS 'CNMM:Number of the footnote. 
 
See further in the description of the table Footnote.';
COMMENT ON COLUMN FootnoteVariable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN FootnoteVariable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE FootnoteVariable IS 'CNMM:The table links footnotes to a variable.';

CREATE TRIGGER FootnoteVariable_BUPSE BEFORE INSERT OR UPDATE ON FootnoteVariable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END FootnoteVariable;


------------------------------------------------------------

CREATE TABLE Grouping(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR) NOT NULL ENABLE
 ,Hierarchy VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,GroupPres VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,Description VARCHAR2(200 CHAR)
 ,MetaId VARCHAR2(100 CHAR)
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
 ,DefaultInGui VARCHAR2(1 CHAR)
, CONSTRAINT PK_Grouping PRIMARY KEY (Grouping ) 
);

COMMENT ON COLUMN Grouping.Grouping IS 'CNMM:Name of grouping. 
 
The name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number. 
 
The name is written beginning with a capital letter.';
COMMENT ON COLUMN Grouping.ValuePool IS 'CNMM:Name of the value pool that the grouping belongs to. 

See further in the description of the table Organization.';
COMMENT ON COLUMN Grouping.PresText IS 'CNMM:Presentation text for grouping. 

Used in the retrieval interface when selecting a grouping under "Classification". The text should also be able to be used in PC-AXIS as a replacement for the usual variable text, when retrieving grouped material, in the stub or heading and in the title when presenting a table. 

The text should be short and descriptive and begin with a capital letter.';
COMMENT ON COLUMN Grouping.Hierarchy IS 'CNMM:Shows if the grouping is hierarchic or not. Can be: 

N = No 
B = Balanced 
U = Unbalanced 

For non-hierarchic groupings Hierarchy should always be N. 
In a balanced hierarchy all branches are the same length, i.e. with the same number of levels. In an unbalanced hierarchy the number of levels and the length of the levels can vary within the hierarchy. ';
COMMENT ON COLUMN Grouping.GroupPres IS 'CNMM:Code which indicates how a grouping should be presented, as an aggregated value, integral value or both. There are the following alternatives: 

A = aggregated value should be shown 
I = integral value should be shown 
B = both aggregated and integral values should be shown .';
COMMENT ON COLUMN Grouping.Description IS 'CNMM:Description of grouping. Should give an idea of how the grouping has been put together. 
 
Written beginning with a capital letter. 
If a description is not available, the field should be NULL.';
COMMENT ON COLUMN Grouping.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN Grouping.SortCode IS 'CNMM:Sorting code to enable the presentation of the groupings within a value pool in a logical order. 
If there is no sorting code, the field should be NULL. 

Field makes it possible to control the sortorder of the valuesets and groupings that are displayed in the dropdown for a variable on the selection page of PX-Web';
COMMENT ON COLUMN Grouping.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Grouping.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';
COMMENT ON COLUMN Grouping.DefaultInGui IS 'CNMM:Field will define if a valueset or grouping shall be selected by default for a variable of the selection page of PX-Web.
If DefaultInGui is specified in both SubtableVariable and Grouping, the one in Grouping will take precedence';

COMMENT ON TABLE Grouping IS 'CNMM:The table describes the groupings which exist in the database. It is used for the grouping of values for presentation purposes.';

CREATE TRIGGER Grouping_BUPSE BEFORE INSERT OR UPDATE ON Grouping
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Grouping;


-------

CREATE TABLE Grouping_ENG(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Grouping_ENG PRIMARY KEY (Grouping ) 
);

COMMENT ON COLUMN Grouping_ENG.Grouping IS 'CNMM: SL version:Name of grouping. 
 
The name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number. 
 
The name is written beginning with a capital letter.';
COMMENT ON COLUMN Grouping_ENG.PresText IS 'CNMM: SL version:Presentation text for grouping. 

Used in the retrieval interface when selecting a grouping under "Classification". The text should also be able to be used in PC-AXIS as a replacement for the usual variable text, when retrieving grouped material, in the stub or heading and in the title when presenting a table. 

The text should be short and descriptive and begin with a capital letter.';
COMMENT ON COLUMN Grouping_ENG.SortCode IS 'CNMM: SL version:Sorting code to enable the presentation of the groupings within a value pool in a logical order. 
If there is no sorting code, the field should be NULL. 

Field makes it possible to control the sortorder of the valuesets and groupings that are displayed in the dropdown for a variable on the selection page of PX-Web';
COMMENT ON COLUMN Grouping_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Grouping_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Grouping_ENG IS 'CNMM: SL: The table describes the groupings which exist in the database. It is used for the grouping of values for presentation purposes.';

CREATE TRIGGER Grouping_ENG_BUPSE BEFORE INSERT OR UPDATE ON Grouping_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Grouping_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE GroupingLevel(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,LevelNo NUMBER NOT NULL ENABLE
 ,LevelText VARCHAR2(250 CHAR)
 ,GeoAreaNo NUMBER
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_GroupingLevel PRIMARY KEY (Grouping,LevelNo ) 
);

COMMENT ON COLUMN GroupingLevel.Grouping IS 'CNMM:Name of grouping.

See further in the description of the table Grouping.';
COMMENT ON COLUMN GroupingLevel.LevelNo IS 'CNMM:Number for sorting a level within a grouping. The highest level should always be 1.';
COMMENT ON COLUMN GroupingLevel.LevelText IS 'CNMM:The name of the level.';
COMMENT ON COLUMN GroupingLevel.GeoAreaNo IS 'CNMM:Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL. 
 
The identification number should also be included in the table TextCatalog. For further information see description of TextCatalog.';
COMMENT ON COLUMN GroupingLevel.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN GroupingLevel.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE GroupingLevel IS 'CNMM:The table describes the levels within a grouping. 

The table has to exist for both hierarchical and non-hierarchical groupings, but does not have to be used for the non-hierarchical. ';

CREATE TRIGGER GroupingLevel_BUPSE BEFORE INSERT OR UPDATE ON GroupingLevel
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END GroupingLevel;


-------

CREATE TABLE GroupingLevel_ENG(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,LevelNo NUMBER NOT NULL ENABLE
 ,LevelText VARCHAR2(250 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_GroupingLevel_ENG PRIMARY KEY (Grouping,LevelNo ) 
);

COMMENT ON COLUMN GroupingLevel_ENG.Grouping IS 'CNMM: SL version:Name of grouping.

See further in the description of the table Grouping.';
COMMENT ON COLUMN GroupingLevel_ENG.LevelNo IS 'CNMM: SL version:Number for sorting a level within a grouping. The highest level should always be 1.';
COMMENT ON COLUMN GroupingLevel_ENG.LevelText IS 'CNMM: SL version:The name of the level.';
COMMENT ON COLUMN GroupingLevel_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN GroupingLevel_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE GroupingLevel_ENG IS 'CNMM: SL: The table describes the levels within a grouping. 

The table has to exist for both hierarchical and non-hierarchical groupings, but does not have to be used for the non-hierarchical. ';

CREATE TRIGGER GroupingLevel_ENG_BUPSE BEFORE INSERT OR UPDATE ON GroupingLevel_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END GroupingLevel_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE Link(
 LinkId NUMBER NOT NULL ENABLE
 ,Link VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,LinkType VARCHAR2(10 CHAR)
 ,LinkFormat VARCHAR2(1 CHAR)
 ,LinkText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,PresCategory VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,LinkPres VARCHAR2(1 CHAR)
 ,SortCode VARCHAR2(20 CHAR)
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Link PRIMARY KEY (LinkId ) 
);

COMMENT ON COLUMN Link.LinkId IS 'CNMM:Identifying code for the link.';
COMMENT ON COLUMN Link.Link IS 'CNMM:Link written in HTML.';
COMMENT ON COLUMN Link.LinkType IS 'CNMM:Describes the type of link. There are the following alternatives: 

TableF = table ahead in the same database 
TableB = previous table in the same database 
TableRel = related table in the same database 
TableProc = table for percentage calculation 
Dok = documentation (internal or external) 
TableRelEx = related table in other countries 
Website = related web site 
Temasite = theme web site 
Analys = analysis 

For the four first above mentioned alternatives the column LinkFormat should be M. In other cases LinkFormat should be U.';
COMMENT ON COLUMN Link.LinkFormat IS 'CNMM:Indicates whether the link is an Internet address outside databases (a URL) or a link to a master table in the database. The options are:

U = URL
M = maintable';
COMMENT ON COLUMN Link.LinkText IS 'CNMM:Presentation text for the link, i.e. the text that is visible for the user in the Internet interface.';
COMMENT ON COLUMN Link.PresCategory IS 'CNMM:Presentation category for the link. There are three alternatives: 

O = Public, i.e. link accessible for all users 
I = Internal, i.e. table is only accessible for internal users. 
P = Private, i.e. table is only accessible for certain internal users. 

The code is always written with capital letters.';
COMMENT ON COLUMN Link.LinkPres IS 'CNMM:Shows how the link should be presented in the Internet interface. There are three alternatives: 

D = Presented directly 
I = Presented under an icon or similar 
B = Presented both directly and under an icon';
COMMENT ON COLUMN Link.SortCode IS 'CNMM:Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the Internet interface. 
If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN Link.Description IS 'CNMM:Description of link. 
If a description is not available, the field should be NULL.';
COMMENT ON COLUMN Link.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Link.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Link IS 'CNMM:The table contains one or several links to menu levels in the table MenuSelection. ';

CREATE TRIGGER Link_BUPSE BEFORE INSERT OR UPDATE ON Link
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Link;


-------

CREATE TABLE Link_ENG(
 LinkId NUMBER NOT NULL ENABLE
 ,Link VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,LinkText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Link_ENG PRIMARY KEY (LinkId ) 
);

COMMENT ON COLUMN Link_ENG.LinkId IS 'CNMM: SL version:Identifying code for the link.';
COMMENT ON COLUMN Link_ENG.Link IS 'CNMM: SL version:Link written in HTML.';
COMMENT ON COLUMN Link_ENG.LinkText IS 'CNMM: SL version:Presentation text for the link, i.e. the text that is visible for the user in the Internet interface.';
COMMENT ON COLUMN Link_ENG.SortCode IS 'CNMM: SL version:Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the Internet interface. 
If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN Link_ENG.Description IS 'CNMM: SL version:Description of link. 
If a description is not available, the field should be NULL.';
COMMENT ON COLUMN Link_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Link_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Link_ENG IS 'CNMM: SL: The table contains one or several links to menu levels in the table MenuSelection. ';

CREATE TRIGGER Link_ENG_BUPSE BEFORE INSERT OR UPDATE ON Link_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Link_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE LinkMenuSelection(
 Menu VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,Selection VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,LinkId NUMBER NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_LinkMenuSelection PRIMARY KEY (Menu,Selection,LinkId ) 
);

COMMENT ON COLUMN LinkMenuSelection.Menu IS 'CNMM:Code for relevant menu level. 

See description of the table MenuSelection.';
COMMENT ON COLUMN LinkMenuSelection.Selection IS 'CNMM:The code for the nearest underlying eligible alternative in the relevant menu level.';
COMMENT ON COLUMN LinkMenuSelection.LinkId IS 'CNMM:Identifying code for the link.';
COMMENT ON COLUMN LinkMenuSelection.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN LinkMenuSelection.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE LinkMenuSelection IS 'CNMM:The table connects links (table Link) to menu selection (table MenuSelection).';

CREATE TRIGGER LinkMenuSelection_BUPSE BEFORE INSERT OR UPDATE ON LinkMenuSelection
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END LinkMenuSelection;


------------------------------------------------------------

CREATE TABLE MainTable(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,TableStatus VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,PresTextS VARCHAR2(150 CHAR)
 ,ContentsVariable VARCHAR2(80 CHAR)
 ,TableId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresCategory VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,FirstPublished DATE
 ,SpecCharExists VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,SubjectCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,MetaId VARCHAR2(100 CHAR)
 ,ProductCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,TimeScale VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MainTable PRIMARY KEY (MainTable ) 
);

COMMENT ON COLUMN MainTable.MainTable IS 'CNMM:Summarised names of the statistical material and its underlying sub-tables with stored data. 
 
The name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN MainTable.TableStatus IS 'CNMM:Code for the tables status, giving information on where the table fits into the production and publishing processes. Table status decides whether a table can be seen or not in the indata and outdata programs. 

There are the following alternatives: 
M = Only metadata input 
E = The table is new and empty data tables have been created 
U = The table is currently being updated and retrievals cannot be done 
N = The table is loaded with new, but not yet officially released data, only accessible for product staff 
O = The table is ready for official release, only accessible for product staff, locked for updating 
D = The table is not updated but is accessible to everybody
A = The table is still updating and the table is accessible to all (with authorisation according to PresCategory)';
COMMENT ON COLUMN MainTable.PresText IS 'CNMM:The descriptive text which is presented when selecting a table in the retrieval interface. 

The text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times. 

The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.';
COMMENT ON COLUMN MainTable.PresTextS IS 'CNMM:Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main tables PresText but should not contain information on variable or time. 

Used in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file. 

Should begin with a capital letter and not end with a full-stop. The text should not include a % symbol. 

If a short presentation text is not available, the field should be NULL.';
COMMENT ON COLUMN MainTable.ContentsVariable IS 'CNMM:Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]" 

If the field is not used, it should be NULL. ';
COMMENT ON COLUMN MainTable.TableId IS 'CNMM:Unique identity for main table. Can be used in requests from the end users, etc. ';
COMMENT ON COLUMN MainTable.PresCategory IS 'CNMM:Presentation category for all sub-tables and content columns in the table. There are three alternatives: 

O = Official, i.e. tables that are officially released and accessible to all users on the external servers or are available on the production server, with plans for official release. (Only tables with PresCategory = O are on the external servers) 
I = Internal, i.e. tables are only accessible for internal users.The table should never be published to the general public.
P = Private, i.e. tables are only accessible for certain internal users.
The code is also written with capital letters.';
COMMENT ON COLUMN MainTable.FirstPublished IS 'CNMM:States when a main table was first published. The value is optional.';
COMMENT ON COLUMN MainTable.SpecCharExists IS 'CNMM:Specifies if a column for special symbols exists in the data table (s) and if it should be used at retrieval. 
There are the following alternatives: 

Y = Yes, column for special symbols exists in the database, wich should be used at retrieval 
E = Yes, column for special symbols exists in the database, but is not used at retrieval 
N = No, column for special symbols does not exist in the database 

If SpecCharExists =Y, there is an extra content column for all content columns in the data table. These special columns have the same names as the content column they belong to, with the suffix X. The format is varchar(2), i.e. the same as CharacterType in the table SpecialCharacter. ';
COMMENT ON COLUMN MainTable.SubjectCode IS 'CNMM:Code for subject area, i.e. BE';
COMMENT ON COLUMN MainTable.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN MainTable.ProductCode IS 'CNMM:Id for the product that corresponds to the table. The field is optional.';
COMMENT ON COLUMN MainTable.TimeScale IS 'CNMM:Name on the time scale that is used in the material. See further description of the table TimeScale.';
COMMENT ON COLUMN MainTable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MainTable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MainTable IS 'CNMM:This table is the central compilation point for material and contains general information for the data tables that are linked to the table.';

CREATE TRIGGER MainTable_BUPSE BEFORE INSERT OR UPDATE ON MainTable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MainTable;


-------

CREATE TABLE MainTable_ENG(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,PresTextS VARCHAR2(150 CHAR)
 ,ContentsVariable VARCHAR2(80 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MainTable_ENG PRIMARY KEY (MainTable ) 
);

COMMENT ON COLUMN MainTable_ENG.MainTable IS 'CNMM: SL version:Summarised names of the statistical material and its underlying sub-tables with stored data. 
 
The name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN MainTable_ENG.PresText IS 'CNMM: SL version:The descriptive text which is presented when selecting a table in the retrieval interface. 

The text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times. 

The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.';
COMMENT ON COLUMN MainTable_ENG.PresTextS IS 'CNMM: SL version:Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main tables PresText but should not contain information on variable or time. 

Used in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file. 

Should begin with a capital letter and not end with a full-stop. The text should not include a % symbol. 

If a short presentation text is not available, the field should be NULL.';
COMMENT ON COLUMN MainTable_ENG.ContentsVariable IS 'CNMM: SL version:Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]" 

If the field is not used, it should be NULL. ';
COMMENT ON COLUMN MainTable_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MainTable_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MainTable_ENG IS 'CNMM: SL: This table is the central compilation point for material and contains general information for the data tables that are linked to the table.';

CREATE TRIGGER MainTable_ENG_BUPSE BEFORE INSERT OR UPDATE ON MainTable_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MainTable_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE MainTablePerson(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PersonCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,RolePerson VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MainTablePerson PRIMARY KEY (MainTable,PersonCode,RolePerson ) 
);

COMMENT ON COLUMN MainTablePerson.MainTable IS 'CNMM:Name of main table. 

See further in the description of the table MainTable.';
COMMENT ON COLUMN MainTablePerson.PersonCode IS 'CNMM:Code for the person responsible, contact person or person responsible for updating for the main table. 

For description, see the table Person.';
COMMENT ON COLUMN MainTablePerson.RolePerson IS 'CNMM:Code that shows the role of responsible person, contact person and/or person responsible for updating. There are the following alternatives: 

M = Main contact person (one person) 
C = Contact person (0 – several persons) 
U = Person responsible for updating (1 – several persons) 
I = Person responsible for international reporting (0 - 1 person).
V = Person that verifies not yet published data  (0 – several persons) ';
COMMENT ON COLUMN MainTablePerson.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MainTablePerson.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MainTablePerson IS 'CNMM:The table links the person responsible, i.e. the contact person and person responsible for updating, to the main tables. An unlimited number of persons can be linked to a main table.';

CREATE TRIGGER MainTablePerson_BUPSE BEFORE INSERT OR UPDATE ON MainTablePerson
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MainTablePerson;


------------------------------------------------------------

CREATE TABLE MainTableVariableHierarchy(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ShowLevels NUMBER
 ,AllLevelsStored VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MainTableVariableHierarchy PRIMARY KEY (MainTable,Variable,Grouping ) 
);

COMMENT ON COLUMN MainTableVariableHierarchy.MainTable IS 'CNMM:Name of main table. 
 
See further in the description of the table MainTable.';
COMMENT ON COLUMN MainTableVariableHierarchy.Variable IS 'CNMM:Name of variable. 
 
See further in the description of the table Variable.';
COMMENT ON COLUMN MainTableVariableHierarchy.Grouping IS 'CNMM:Name of grouping. 
 
See further in the description of the table Grouping.';
COMMENT ON COLUMN MainTableVariableHierarchy.ShowLevels IS 'CNMM:The number of open levels that will be shown at menu selection the first time the tree is displayed. Must be 0 if all levels shall be shown.';
COMMENT ON COLUMN MainTableVariableHierarchy.AllLevelsStored IS 'CNMM:Shows if all levels shall be stored or not. Can be: 
 
Y = Yes 
N = No';
COMMENT ON COLUMN MainTableVariableHierarchy.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MainTableVariableHierarchy.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MainTableVariableHierarchy IS 'CNMM:The table links a grouping to a main table. ';

CREATE TRIGGER MainTableVariableHierarchy_BUPSE BEFORE INSERT OR UPDATE ON MainTableVariableHierarchy
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MainTableVariableHierarchy;


------------------------------------------------------------

CREATE TABLE MenuSelection(
 Menu VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,Selection VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR)
 ,PresTextS VARCHAR2(20 CHAR)
 ,Description VARCHAR2(200 CHAR)
 ,LevelNo VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,Presentation VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,MetaId VARCHAR2(100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MenuSelection PRIMARY KEY (Menu,Selection ) 
);

COMMENT ON COLUMN MenuSelection.Menu IS 'CNMM:Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters. ';
COMMENT ON COLUMN MenuSelection.Selection IS 'CNMM:The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters.';
COMMENT ON COLUMN MenuSelection.PresText IS 'CNMM:Presentation text for MenuSelection. ';
COMMENT ON COLUMN MenuSelection.PresTextS IS 'CNMM:Short presentation text for MenuSelection. 
 
If a short presentation text is not available, the field should be NULL.';
COMMENT ON COLUMN MenuSelection.Description IS 'CNMM:Descriptive text for MenuSelection. 
 
If a description is not available, the field should be NULL.';
COMMENT ON COLUMN MenuSelection.LevelNo IS 'CNMM:Number of menu level, where 1 refers to the highest level. 
A type of object should always have the same LevelNo. 

The highest level number should be given in the table MetaAdm (see description in that table).';
COMMENT ON COLUMN MenuSelection.SortCode IS 'CNMM:Sorting code to dictate the presentation order for the eligible alternatives on each level. 
 
If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN MenuSelection.Presentation IS 'CNMM:Shows how a menu alternative can be used. Alternatives: 
 
A = Active, visible and can be selected 
P = Passive, is visible but cannot be selected 
N = Not shown in the menu';
COMMENT ON COLUMN MenuSelection.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN MenuSelection.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MenuSelection.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MenuSelection IS 'CNMM:The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases. 
All records in MenuSelection should also be in the corresponding MenuSelections for secondary languages (if any).';

CREATE TRIGGER MenuSelection_BUPSE BEFORE INSERT OR UPDATE ON MenuSelection
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MenuSelection;


-------

CREATE TABLE MenuSelection_ENG(
 Menu VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,Selection VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR)
 ,PresTextS VARCHAR2(20 CHAR)
 ,Description VARCHAR2(200 CHAR)
 ,SortCode VARCHAR2(20 CHAR)
 ,Presentation VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MenuSelection_ENG PRIMARY KEY (Menu,Selection ) 
);

COMMENT ON COLUMN MenuSelection_ENG.Menu IS 'CNMM: SL version:Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters. ';
COMMENT ON COLUMN MenuSelection_ENG.Selection IS 'CNMM: SL version:The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters.';
COMMENT ON COLUMN MenuSelection_ENG.PresText IS 'CNMM: SL version:Presentation text for MenuSelection. ';
COMMENT ON COLUMN MenuSelection_ENG.PresTextS IS 'CNMM: SL version:Short presentation text for MenuSelection. 
 
If a short presentation text is not available, the field should be NULL.';
COMMENT ON COLUMN MenuSelection_ENG.Description IS 'CNMM: SL version:Descriptive text for MenuSelection. 
 
If a description is not available, the field should be NULL.';
COMMENT ON COLUMN MenuSelection_ENG.SortCode IS 'CNMM: SL version:Sorting code to dictate the presentation order for the eligible alternatives on each level. 
 
If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN MenuSelection_ENG.Presentation IS 'CNMM: SL version:Shows how a menu alternative can be used. Alternatives: 
 
A = Active, visible and can be selected 
P = Passive, is visible but cannot be selected 
N = Not shown in the menu';
COMMENT ON COLUMN MenuSelection_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MenuSelection_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MenuSelection_ENG IS 'CNMM: SL: The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases. 
All records in MenuSelection should also be in the corresponding MenuSelections for secondary languages (if any).';

CREATE TRIGGER MenuSelection_ENG_BUPSE BEFORE INSERT OR UPDATE ON MenuSelection_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MenuSelection_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE MetaAdm(
 Property VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,Value VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_MetaAdm PRIMARY KEY (Property ) 
);

COMMENT ON COLUMN MetaAdm.Property IS 'CNMM:Name of system variable. There are the following alternatives: 
- LastFootnoteNo 
- MenuLevels 
- SpecCharSum 
- NoOfLanguage 
- Language1, Language2 and so on
- Codepage1 
- DataNotAvailable 
- DataNoteSum 
- DataSymbolSum 
- DataSymbolNil 
- PxDataFormat 
- KeysUpperLimit 
- DefaultCodeMissingLine 

NoOfLanguage indicates the number of languages that exists in the model for the meta database presentation texts, and that can be used by the retrieval interfaces. For each language there should be a line like: Language1, Language2 and so on. Language1 should always include the main language. See also the description of the column Value in this table. 

For each language there should also be a row in the table TextCatalog. For further information, see this table. 

Regarding DefaultCodeMissingLine see also descriptions in: PresCellsZero och PresMissingLine in Contents, and CharacterType and PresCharacter in SpecialCharacter.';
COMMENT ON COLUMN MetaAdm.Value IS 'CNMM:Value of system variable. Contains one value per property. 

For the property LastFootnoteNo, Value should contain the last used footnote number in the table Footnote. 

For the property MenuLevels, Value should contains the highest used level number in the table MenuSelection. 

For the property SpecCharSum, Value should contain the highest acceptable value for character type in the table SpecCharacter. 

For the property NoOfLanguage Value should contain the number of languages that exists in the metadata model. 

For the property Language1 Value should contain the main language of the model. The code is written in three capital letters, i.e. SVE. 

For the property  Language2 , Language3 etc., Value should contain the other languages of the model. The code is written in three capital letters, i.e. ENG, ESP. The code is used as a suffix in the extra tables that should exist in the meta database, i.e. SubTable_ENG, SubTable_ESP. 

For the property Codepage1: The characters that can be used and  how they should be presented. Is used at creating the keyword Codepage in the px file and at converting to XML. 
Three different examples: iso-8859-1, windows-1251, big5. 

For the property DataNotAvailable: The value that should be presented, if  the data cell contains NULL and NPM-character is missing. If the value exists in the table SpecialCharacter, it is used, otherwise the character in the table DataNotAvailable is used.  The value is a reference to CharacterType in the SpecialCharacter table. 
Example: .. (two dots). 

For the property DataNoteSum: The value that should be presented after the sum, if data cells with different NPM marking is summarized. The value is a reference to CharacterType in the SpecialCharacter table. 
Example:  * 
1A + 2B = 3* 

For the property DataSymbolSum: The value that should be presented if data cells with different NPM character are summarized and no sum can be created. The value is a reference to CharacterType in the SpecialCharacter table. 
Example: N.A. 
. + .. = N.A. 

For the property DataSymbolNil: the value that should be presented at absolute 0 (zero) in the table SpecialCharacter. The value is a reference to CharacterType in the SpecialCharacter table. 
Example: - 

For the property PxDataFormat: Matrix = all retreivals should be stored in matrix format. 
Keysnn = retreivals with keys are remade 
nn &gt; read data cells *100 / presented number of data cells 
Example: 40 
(Default is Matrix.) 

For the property KeysUpperLimit: Maximum number of data cells that the presented matrix may contain, if the retreival should be possible to do with Keys. If greater, the retreival is made in matrix format. 
Example: 1000000 

For the property DefaultCodeMissingLine: The value that should be presented in data cells that are not stored. Is used if neither presentation with 0 or special character have been specified. The value is a reference to CharacterType in the SpecialCharacter table. 

See also the description of the column Property in this table, and also the table TextCatalog.';
COMMENT ON COLUMN MetaAdm.Description IS 'CNMM:Must contain a description of the property.';
COMMENT ON COLUMN MetaAdm.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN MetaAdm.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE MetaAdm IS 'CNMM:The table contains system variables and their values.';

CREATE TRIGGER MetaAdm_BUPSE BEFORE INSERT OR UPDATE ON MetaAdm
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END MetaAdm;


------------------------------------------------------------

CREATE TABLE MetabaseInfo(
 Model VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ModelVersion VARCHAR2(10 CHAR) NOT NULL ENABLE
 ,DatabaseRole VARCHAR2(20 CHAR) NOT NULL ENABLE
, CONSTRAINT PK_MetabaseInfo PRIMARY KEY (Model ) 
);

COMMENT ON COLUMN MetabaseInfo.Model IS 'CNMM:This field can be used to give information about general characteristics of the database. 
E.g. if the data is on macro or micro level.';
COMMENT ON COLUMN MetabaseInfo.ModelVersion IS 'CNMM:Version number for the metadata model that the metadata database uses.';
COMMENT ON COLUMN MetabaseInfo.DatabaseRole IS 'CNMM:Role of database. Can be: 

- Production 
- Operation 
- Test';

COMMENT ON TABLE MetabaseInfo IS 'CNMM:The table contains information on the relevant data model,version and the database role.';


------------------------------------------------------------

CREATE TABLE Organization(
 OrganizationCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,OrganizationName VARCHAR2(60 CHAR) NOT NULL ENABLE
 ,Department VARCHAR2(60 CHAR)
 ,Unit VARCHAR2(60 CHAR)
 ,WebAddress VARCHAR2(100 CHAR)
 ,MetaId VARCHAR2(100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Organization PRIMARY KEY (OrganizationCode ) 
);

COMMENT ON COLUMN Organization.OrganizationCode IS 'CNMM:Identification for the organization';
COMMENT ON COLUMN Organization.OrganizationName IS 'CNMM:Name of authority/organisation in full text, including any official abbreviation in brackets, e.g. Statistics Sweden (SCB). 

Text should begin with a capital letter.';
COMMENT ON COLUMN Organization.Department IS 'CNMM:Name of the department or equivalent that produces the statistics. ';
COMMENT ON COLUMN Organization.Unit IS 'CNMM:Name of unit or equivalent.';
COMMENT ON COLUMN Organization.WebAddress IS 'CNMM:Internet address to the authoritys/organisations website. Written as, for example: 
www.scb.se 

If Internet address is not available, the field should be NULL.';
COMMENT ON COLUMN Organization.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN Organization.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Organization.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Organization IS 'CNMM:The table contains information on the authorities and organizations that are responsible for or produce statistics.';

CREATE TRIGGER Organization_BUPSE BEFORE INSERT OR UPDATE ON Organization
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Organization;


-------

CREATE TABLE Organization_ENG(
 OrganizationCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,OrganizationName VARCHAR2(60 CHAR) NOT NULL ENABLE
 ,Department VARCHAR2(60 CHAR)
 ,Unit VARCHAR2(60 CHAR)
 ,WebAddress VARCHAR2(100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Organization_ENG PRIMARY KEY (OrganizationCode ) 
);

COMMENT ON COLUMN Organization_ENG.OrganizationCode IS 'CNMM: SL version:Identification for the organization';
COMMENT ON COLUMN Organization_ENG.OrganizationName IS 'CNMM: SL version:Name of authority/organisation in full text, including any official abbreviation in brackets, e.g. Statistics Sweden (SCB). 

Text should begin with a capital letter.';
COMMENT ON COLUMN Organization_ENG.Department IS 'CNMM: SL version:Name of the department or equivalent that produces the statistics. ';
COMMENT ON COLUMN Organization_ENG.Unit IS 'CNMM: SL version:Name of unit or equivalent.';
COMMENT ON COLUMN Organization_ENG.WebAddress IS 'CNMM: SL version:Internet address to the authoritys/organisations website. Written as, for example: 
www.scb.se 

If Internet address is not available, the field should be NULL.';
COMMENT ON COLUMN Organization_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Organization_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Organization_ENG IS 'CNMM: SL: The table contains information on the authorities and organizations that are responsible for or produce statistics.';

CREATE TRIGGER Organization_ENG_BUPSE BEFORE INSERT OR UPDATE ON Organization_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Organization_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE Person(
 PersonCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,OrganizationCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Forename VARCHAR2(50 CHAR)
 ,Surname VARCHAR2(50 CHAR) NOT NULL ENABLE
 ,PhonePrefix VARCHAR2(4 CHAR) NOT NULL ENABLE
 ,PhoneNo VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,FaxNo VARCHAR2(20 CHAR)
 ,Email VARCHAR2(80 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Person PRIMARY KEY (PersonCode ) 
);

COMMENT ON COLUMN Person.PersonCode IS 'CNMM:Identifying code for person (or group) responsible. ';
COMMENT ON COLUMN Person.OrganizationCode IS 'CNMM:Code for the organization or authority that produces and/or is responsible for the statistical material. 

See further description of the table Organization.';
COMMENT ON COLUMN Person.Forename IS 'CNMM:Responsible persons first name 
For groups, this data is not available and should therefore be NULL.';
COMMENT ON COLUMN Person.Surname IS 'CNMM:Surname of the person responsible or name of the group responsible.';
COMMENT ON COLUMN Person.PhonePrefix IS 'CNMM:Prefix for telephone number, so that the number is valid internationally. 

E.g. for Swedish telephone numbers the prefix is +46.';
COMMENT ON COLUMN Person.PhoneNo IS 'CNMM:Complete national telephone numbers, i.e. without international prefix. 

Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.';
COMMENT ON COLUMN Person.FaxNo IS 'CNMM:Complete national fax machine numbers, i.e. without international prefix. 

Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space. 
If there is no fax number, this field should be NULL.';
COMMENT ON COLUMN Person.Email IS 'CNMM:E-mail address for person or group responsible, if available. 
If an e-mail address is not available, the field should be NULL. 

Written with lower case letters.';
COMMENT ON COLUMN Person.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Person.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Person IS 'CNMM:The table contains information on all persons (or alternatively groups) which are contact persons for content and/or responsible for updating statistics in the database.';

CREATE TRIGGER Person_BUPSE BEFORE INSERT OR UPDATE ON Person
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Person;


------------------------------------------------------------

CREATE TABLE SecondaryLanguage(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Language VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,CompletelyTranslated VARCHAR2(1 CHAR)
 ,Published VARCHAR2(1 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SecondaryLanguage PRIMARY KEY (MainTable,Language ) 
);

COMMENT ON COLUMN SecondaryLanguage.MainTable IS 'CNMM:Name of main table';
COMMENT ON COLUMN SecondaryLanguage.Language IS 'CNMM:Name of secondary language.';
COMMENT ON COLUMN SecondaryLanguage.CompletelyTranslated IS 'CNMM:Code which shows whether all the tables presentation texts are translated to English or not. This column is necessary so that it is possible to determine from the retrieval interface whether the table will be shown in English or not. 

Valid values:
Y - the table is completely translated to the secondary language
N - the table is not completely translated to the secondary language';
COMMENT ON COLUMN SecondaryLanguage.Published IS 'CNMM:Shows if the secondary language is published or not.

Valid values:
Y = yes
N = no';
COMMENT ON COLUMN SecondaryLanguage.UserId IS 'CNMM:';
COMMENT ON COLUMN SecondaryLanguage.LogDate IS 'CNMM:';

COMMENT ON TABLE SecondaryLanguage IS 'CNMM:Information about secondary languages (if any)';

CREATE TRIGGER SecondaryLanguage_BUPSE BEFORE INSERT OR UPDATE ON SecondaryLanguage
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SecondaryLanguage;


------------------------------------------------------------

CREATE TABLE SpecialCharacter(
 CharacterType VARCHAR2(2 CHAR) NOT NULL ENABLE
 ,PresCharacter VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,AggregPossible VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,DataCellPres VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,DataCellFilled VARCHAR2(1 CHAR)
 ,PresText VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SpecialCharacter PRIMARY KEY (CharacterType ) 
);

COMMENT ON COLUMN SpecialCharacter.CharacterType IS 'CNMM:Identifying code for the special character. 

Given in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm). 

If PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm.';
COMMENT ON COLUMN SpecialCharacter.PresCharacter IS 'CNMM:The special character as presented for the user when the table is presented when retrieved.';
COMMENT ON COLUMN SpecialCharacter.AggregPossible IS 'CNMM:Used to show whether the data cell with the special character can be aggregated or not. There are the following alternatives: 

Y = Yes 
N = No 

If AggregPossible = Y, the specific data cell, even if not shown, can be included in an aggregation.';
COMMENT ON COLUMN SpecialCharacter.DataCellPres IS 'CNMM:Provides the retrieval programs with information concerning the presentation of a special character;  with data and special character or with special character only. 

There are the following alternatives: 
Y = The data cell should be presented together with the special character 
N = The special character alone should be presented';
COMMENT ON COLUMN SpecialCharacter.DataCellFilled IS 'CNMM:Shows whether the data cell must be filled in or not. There are the following alternatives: 

V = Value must be filled in 
N = No, the data cell should not be  filled in but should be NULL 
F = Any, i.e. the data cell can be filled in or can be NULL. 
0 = The data cell should contain 0 (zero) only';
COMMENT ON COLUMN SpecialCharacter.PresText IS 'CNMM:Explanation to what is written in PresCharacter. 

If there is no presentation text, the field should be NULL.';
COMMENT ON COLUMN SpecialCharacter.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN SpecialCharacter.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE SpecialCharacter IS 'CNMM:The table contains information on the special characters that are used in the databases data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given.';

CREATE TRIGGER SpecialCharacter_BUPSE BEFORE INSERT OR UPDATE ON SpecialCharacter
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SpecialCharacter;


-------

CREATE TABLE SpecialCharacter_ENG(
 CharacterType VARCHAR2(2 CHAR) NOT NULL ENABLE
 ,PresCharacter VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SpecialCharacter_ENG PRIMARY KEY (CharacterType ) 
);

COMMENT ON COLUMN SpecialCharacter_ENG.CharacterType IS 'CNMM: SL version:Identifying code for the special character. 

Given in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm). 

If PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm.';
COMMENT ON COLUMN SpecialCharacter_ENG.PresCharacter IS 'CNMM: SL version:The special character as presented for the user when the table is presented when retrieved.';
COMMENT ON COLUMN SpecialCharacter_ENG.PresText IS 'CNMM: SL version:Explanation to what is written in PresCharacter. 

If there is no presentation text, the field should be NULL.';
COMMENT ON COLUMN SpecialCharacter_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN SpecialCharacter_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE SpecialCharacter_ENG IS 'CNMM: SL: The table contains information on the special characters that are used in the databases data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given.';

CREATE TRIGGER SpecialCharacter_ENG_BUPSE BEFORE INSERT OR UPDATE ON SpecialCharacter_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SpecialCharacter_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE SubTable(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,SubTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,CleanTable VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SubTable PRIMARY KEY (MainTable,SubTable ) 
);

COMMENT ON COLUMN SubTable.MainTable IS 'CNMM:The name of the main table, to which the sub-table is linked. See further description in the table MainTable.';
COMMENT ON COLUMN SubTable.SubTable IS 'CNMM:Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written. 
Name of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only. 

NB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.';
COMMENT ON COLUMN SubTable.PresText IS 'CNMM:Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables. 

The text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end. 

For data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable. 

For data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables. 

The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.';
COMMENT ON COLUMN SubTable.CleanTable IS 'CNMM:Shows whether the sub-tables values can be aggregated or not.

There are the following alternatives: 
Y = Yes
N = No

SCB: this value is not used - the field is set to X for all new subtables';
COMMENT ON COLUMN SubTable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN SubTable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE SubTable IS 'CNMM:The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases. The data tables are identified using the main tables name + sub-tables name.';

CREATE TRIGGER SubTable_BUPSE BEFORE INSERT OR UPDATE ON SubTable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SubTable;


-------

CREATE TABLE SubTable_ENG(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,SubTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(250 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SubTable_ENG PRIMARY KEY (MainTable,SubTable ) 
);

COMMENT ON COLUMN SubTable_ENG.MainTable IS 'CNMM: SL version:The name of the main table, to which the sub-table is linked. See further description in the table MainTable.';
COMMENT ON COLUMN SubTable_ENG.SubTable IS 'CNMM: SL version:Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written. 
Name of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only. 

NB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.';
COMMENT ON COLUMN SubTable_ENG.PresText IS 'CNMM: SL version:Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables. 

The text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end. 

For data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable. 

For data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables. 

The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.';
COMMENT ON COLUMN SubTable_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN SubTable_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE SubTable_ENG IS 'CNMM: SL: The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases. The data tables are identified using the main tables name + sub-tables name.';

CREATE TRIGGER SubTable_ENG_BUPSE BEFORE INSERT OR UPDATE ON SubTable_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SubTable_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE SubTableVariable(
 MainTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,SubTable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueSet VARCHAR2(30 CHAR)
 ,VariableType VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,StoreColumnNo NUMBER NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,DefaultInGui VARCHAR2(1 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_SubTableVariable PRIMARY KEY (MainTable,SubTable,Variable ) 
);

COMMENT ON COLUMN SubTableVariable.MainTable IS 'CNMM:The name of the main table to which the variable and the sub-table are linked. See further description in the MainTable table.';
COMMENT ON COLUMN SubTableVariable.SubTable IS 'CNMM:Name of sub-table 
 
Data can be missing, with a dash in its place. 
 
See further description in the table SubTable.';
COMMENT ON COLUMN SubTableVariable.Variable IS 'CNMM:Variable name, which makes up the column name for metadata in the data table. See further description in the table Variable.';
COMMENT ON COLUMN SubTableVariable.ValueSet IS 'CNMM:Name of value set. See further description in the table ValueSet. 
For rows with variable types V and G, the name of the value set must be filled in. For VariableType = T, the field is left empty, as there is no value set for the variable Time.';
COMMENT ON COLUMN SubTableVariable.VariableType IS 'CNMM:Code for type of variable. There are three alternatives: 

- V = variable, i.e. dividing variable, not time. 
- G = geographical information for map program. 
- T = time. 

If VariableType = G, the field GeoAreaNo in the tables ValueSet and Grouping should be filled in (however not yet implemented).';
COMMENT ON COLUMN SubTableVariable.StoreColumnNo IS 'CNMM:The variables column number in the data table. 
The variable Time should always be included and be the last column in the data table. If the material is divided by region, the variable Region should be the first column. 
Written as 1, 2, 3, etc.';
COMMENT ON COLUMN SubTableVariable.SortCode IS 'CNMM:The <i>Sortcode</i> field makes it possible to control the sortorder of the valuesets that are displayed in the dropdown for a variable on the selection page of PX-Web';
COMMENT ON COLUMN SubTableVariable.DefaultInGui IS 'CNMM:The <i>Default</i> field will define if a valueset or grouping shall be selected by default for a variable of the selection page of PX-Web. Values can be: Y/N';
COMMENT ON COLUMN SubTableVariable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN SubTableVariable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE SubTableVariable IS 'CNMM:The table links variables with value sets in the sub-tables. The variable name is made up of the name for the corresponding metadata column in the data tables.';

CREATE TRIGGER SubTableVariable_BUPSE BEFORE INSERT OR UPDATE ON SubTableVariable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END SubTableVariable;


------------------------------------------------------------

CREATE TABLE TextCatalog(
 TextCatalogNo NUMBER NOT NULL ENABLE
 ,TextType VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR) NOT NULL ENABLE
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_TextCatalog PRIMARY KEY (TextCatalogNo ) 
);

COMMENT ON COLUMN TextCatalog.TextCatalogNo IS 'CNMM:Identity of text. ';
COMMENT ON COLUMN TextCatalog.TextType IS 'CNMM:Type of text. The texts should be fixed for use in PC-AXIS. 

Alternatives: 
- ContentsVariable 
- GeoAreaNo 
- Language1, Language2 (and so on)';
COMMENT ON COLUMN TextCatalog.PresText IS 'CNMM:The text that should be shown. Can be the name of a map file etc., or a language. The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm.';
COMMENT ON COLUMN TextCatalog.Description IS 'CNMM:Description of text. 

If a description is not available, the field should be NULL.';
COMMENT ON COLUMN TextCatalog.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN TextCatalog.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE TextCatalog IS 'CNMM:The table contains information on joint texts.';

CREATE TRIGGER TextCatalog_BUPSE BEFORE INSERT OR UPDATE ON TextCatalog
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END TextCatalog;


-------

CREATE TABLE TextCatalog_ENG(
 TextCatalogNo NUMBER NOT NULL ENABLE
 ,TextType VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(100 CHAR) NOT NULL ENABLE
 ,Description VARCHAR2(200 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_TextCatalog_ENG PRIMARY KEY (TextCatalogNo ) 
);

COMMENT ON COLUMN TextCatalog_ENG.TextCatalogNo IS 'CNMM: SL version:Identity of text. ';
COMMENT ON COLUMN TextCatalog_ENG.TextType IS 'CNMM: SL version:Type of text. The texts should be fixed for use in PC-AXIS. 

Alternatives: 
- ContentsVariable 
- GeoAreaNo 
- Language1, Language2 (and so on)';
COMMENT ON COLUMN TextCatalog_ENG.PresText IS 'CNMM: SL version:The text that should be shown. Can be the name of a map file etc., or a language. The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm.';
COMMENT ON COLUMN TextCatalog_ENG.Description IS 'CNMM: SL version:Description of text. 

If a description is not available, the field should be NULL.';
COMMENT ON COLUMN TextCatalog_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN TextCatalog_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE TextCatalog_ENG IS 'CNMM: SL: The table contains information on joint texts.';

CREATE TRIGGER TextCatalog_ENG_BUPSE BEFORE INSERT OR UPDATE ON TextCatalog_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END TextCatalog_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE TimeScale(
 TimeScale VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,TimeScalePres VARCHAR2(1 CHAR)
 ,Regular VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,TimeUnit VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,Frequency NUMBER
 ,StoreFormat VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_TimeScale PRIMARY KEY (TimeScale ) 
);

COMMENT ON COLUMN TimeScale.TimeScale IS 'CNMM:Name of timescale, i.e.Year, Month, Quarter. 
Should not contain dash (applies for retrievals in PC-AXIS).';
COMMENT ON COLUMN TimeScale.PresText IS 'CNMM:Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case. 
Presentation text used when selecting time when making a retrieval from databases.';
COMMENT ON COLUMN TimeScale.TimeScalePres IS 'CNMM:Shows if the timescale should be presented in table heading instead of the word Time. Can be: 

Y = Yes 
N = No ';
COMMENT ON COLUMN TimeScale.Regular IS 'CNMM:Shows if timescale is regular or not. Can be: 

Y = Yes 
N = No 

An example of an irregular timescale is an election year. 

Data is primarily accompanying information when retrieving statistics to a file.';
COMMENT ON COLUMN TimeScale.TimeUnit IS 'CNMM:Code for TimeUnit. Used as accompanying information when retrieving a statistics file. The following alternatives are possible: 

Q = quarter 
A = academic year 
M = month 
X = 3 years 
S = split year 
Y = year
T= term';
COMMENT ON COLUMN TimeScale.Frequency IS 'CNMM:Shows how many points in time the relevant timescale contains per calendar year, i.e.: 

1 for timescale year, 
4 for timescale quarter, 
12 for timescale month. 

For irregular and regular timescales, where points in time do not occur consecutively (i.e. every other year), the field should be NULL.';
COMMENT ON COLUMN TimeScale.StoreFormat IS 'CNMM:Description of storage format for the point in time in the timescale. There are the following alternatives: 

yyyy for timescales where TimeUnit = Y, 
yyyy-yyyy for timescales where TimeUnit = T, 
yyyy/yy for timescales where TimeUnit = A, 
yyyy/yyyy for timescales where TimeUnit = P, 
yyyyQq for timescales where TimeUnit = Q, 
yyyyMmm for timescales where TimeUnit = M. 

For a description of time units, see column TimeUnit in the table TimeScale.';
COMMENT ON COLUMN TimeScale.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN TimeScale.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE TimeScale IS 'CNMM:The table describes the timescales that exist in the database.';

CREATE TRIGGER TimeScale_BUPSE BEFORE INSERT OR UPDATE ON TimeScale
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END TimeScale;


-------

CREATE TABLE TimeScale_ENG(
 TimeScale VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_TimeScale_ENG PRIMARY KEY (TimeScale ) 
);

COMMENT ON COLUMN TimeScale_ENG.TimeScale IS 'CNMM: SL version:Name of timescale, i.e.Year, Month, Quarter. 
Should not contain dash (applies for retrievals in PC-AXIS).';
COMMENT ON COLUMN TimeScale_ENG.PresText IS 'CNMM: SL version:Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case. 
Presentation text used when selecting time when making a retrieval from databases.';
COMMENT ON COLUMN TimeScale_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN TimeScale_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE TimeScale_ENG IS 'CNMM: SL: The table describes the timescales that exist in the database.';

CREATE TRIGGER TimeScale_ENG_BUPSE BEFORE INSERT OR UPDATE ON TimeScale_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END TimeScale_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE VSValue(
 ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_VSValue PRIMARY KEY (ValueSet,ValuePool,ValueCode ) 
);

COMMENT ON COLUMN VSValue.ValueSet IS 'CNMM:Name of the value set to which the values are linked. 
 
See further description in table ValueSet.';
COMMENT ON COLUMN VSValue.ValuePool IS 'CNMM:Name of the value pool to which the value set belongs. 
 
See further description in table ValuePool.';
COMMENT ON COLUMN VSValue.ValueCode IS 'CNMM:Code for the values that are linked to the value set. 
 
See further description in table Value.';
COMMENT ON COLUMN VSValue.SortCode IS 'CNMM:Sorting code for values within the value set. Dictates the presentation order for the value sets values when retrieving from the database and presenting the table. 

So that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead. 

If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN VSValue.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN VSValue.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE VSValue IS 'CNMM:The table links values for a value pool to a value set, for which data is stored in the data table.';

CREATE TRIGGER VSValue_BUPSE BEFORE INSERT OR UPDATE ON VSValue
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END VSValue;


-------

CREATE TABLE VSValue_ENG(
 ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_VSValue_ENG PRIMARY KEY (ValueSet,ValuePool,ValueCode ) 
);

COMMENT ON COLUMN VSValue_ENG.ValueSet IS 'CNMM: SL version:Name of the value set to which the values are linked. 
 
See further description in table ValueSet.';
COMMENT ON COLUMN VSValue_ENG.ValuePool IS 'CNMM: SL version:Name of the value pool to which the value set belongs. 
 
See further description in table ValuePool.';
COMMENT ON COLUMN VSValue_ENG.ValueCode IS 'CNMM: SL version:Code for the values that are linked to the value set. 
 
See further description in table Value.';
COMMENT ON COLUMN VSValue_ENG.SortCode IS 'CNMM: SL version:Sorting code for values within the value set. Dictates the presentation order for the value sets values when retrieving from the database and presenting the table. 

So that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead. 

If there is no sorting code, the field should be NULL.';
COMMENT ON COLUMN VSValue_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN VSValue_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE VSValue_ENG IS 'CNMM: SL: The table links values for a value pool to a value set, for which data is stored in the data table.';

CREATE TRIGGER VSValue_ENG_BUPSE BEFORE INSERT OR UPDATE ON VSValue_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END VSValue_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE Value(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Unit VARCHAR2(30 CHAR)
 ,ValueTextS VARCHAR2(250 CHAR)
 ,ValueTextL VARCHAR2(1100 CHAR)
 ,MetaId VARCHAR2(100 CHAR)
 ,Footnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Value PRIMARY KEY (ValuePool,ValueCode ) 
);

COMMENT ON COLUMN Value.ValuePool IS 'CNMM:Name of the value pool that the value belongs to. See further description of table ValuePool.';
COMMENT ON COLUMN Value.ValueCode IS 'CNMM:Code for value or group. 

Value code should agree with the code in the corresponding classification or standard if there is one. 

Because the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size. 

Capitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.';
COMMENT ON COLUMN Value.SortCode IS 'CNMM:Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases. 

The sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text. 

NB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.';
COMMENT ON COLUMN Value.Unit IS 'CNMM:Can be used to state the unit so that a value can have different units.

If the field is filled in with a unit, the column Unit in the table Contents should be filled with %Value. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.

See also description of the table Contents.';
COMMENT ON COLUMN Value.ValueTextS IS 'CNMM:Short presentation text for value and group. 

To be visible in the retrieval interfaces, it requires that: 
- The field ValueTextExists in ValuePool is either S (Short value text exists) or B (Both short and long value text exists) and 
- The field ValuePres in ValuePool or ValueSet is either A (Both code and short value text should be presented) or S (Short value text should be presented). 

The text is written in lower case letters, except for abbreviations etc. 

See also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet.';
COMMENT ON COLUMN Value.ValueTextL IS 'CNMM:Value text, presentation text for value and group. 

To be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the values value pool must be L. 

ValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pools values are presented either with or without value texts. 

The text is written in lower case, with the exception of abbreviations, etc. 

See also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet.';
COMMENT ON COLUMN Value.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN Value.Footnote IS 'CNMM:Shows whether there is a footnote linked to the value (FootnoteType 6). There are the following alternatives: 
 
B = Both obligatory and optional footnotes exist 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist 
N = There are no footnotes';
COMMENT ON COLUMN Value.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Value.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Value IS 'CNMM:The table describes the values in the value pool.';

CREATE TRIGGER Value_BUPSE BEFORE INSERT OR UPDATE ON Value
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Value;


-------

CREATE TABLE Value_ENG(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,Unit VARCHAR2(30 CHAR)
 ,ValueTextS VARCHAR2(250 CHAR)
 ,ValueTextL VARCHAR2(1100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Value_ENG PRIMARY KEY (ValuePool,ValueCode ) 
);

COMMENT ON COLUMN Value_ENG.ValuePool IS 'CNMM: SL version:Name of the value pool that the value belongs to. See further description of table ValuePool.';
COMMENT ON COLUMN Value_ENG.ValueCode IS 'CNMM: SL version:Code for value or group. 

Value code should agree with the code in the corresponding classification or standard if there is one. 

Because the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size. 

Capitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.';
COMMENT ON COLUMN Value_ENG.SortCode IS 'CNMM: SL version:Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases. 

The sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text. 

NB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.';
COMMENT ON COLUMN Value_ENG.Unit IS 'CNMM: SL version:Can be used to state the unit so that a value can have different units.

If the field is filled in with a unit, the column Unit in the table Contents should be filled with %Value. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.

See also description of the table Contents.';
COMMENT ON COLUMN Value_ENG.ValueTextS IS 'CNMM: SL version:Short presentation text for value and group. 

To be visible in the retrieval interfaces, it requires that: 
- The field ValueTextExists in ValuePool is either S (Short value text exists) or B (Both short and long value text exists) and 
- The field ValuePres in ValuePool or ValueSet is either A (Both code and short value text should be presented) or S (Short value text should be presented). 

The text is written in lower case letters, except for abbreviations etc. 

See also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet.';
COMMENT ON COLUMN Value_ENG.ValueTextL IS 'CNMM: SL version:Value text, presentation text for value and group. 

To be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the values value pool must be L. 

ValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pools values are presented either with or without value texts. 

The text is written in lower case, with the exception of abbreviations, etc. 

See also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet.';
COMMENT ON COLUMN Value_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Value_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Value_ENG IS 'CNMM: SL: The table describes the values in the value pool.';

CREATE TRIGGER Value_ENG_BUPSE BEFORE INSERT OR UPDATE ON Value_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Value_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ValueGroup(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,GroupCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,GroupLevel NUMBER NOT NULL ENABLE
 ,ValueLevel NUMBER NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValueGroup PRIMARY KEY (Grouping,GroupCode,ValueCode ) 
);

COMMENT ON COLUMN ValueGroup.Grouping IS 'CNMM:Name of grouping.

See further in the description of the table Grouping.';
COMMENT ON COLUMN ValueGroup.GroupCode IS 'CNMM:
See further in the description of the table Value.';
COMMENT ON COLUMN ValueGroup.ValueCode IS 'CNMM:Code for the value contained in the group. Retrieved from the table Value, column value code.
See further in the description of the table Value
Se beskrivning av tabellen Varde.';
COMMENT ON COLUMN ValueGroup.ValuePool IS 'CNMM:The name of the value set, the grouping is attached.
See further in the description of the table ValuePool

Se beskrivning av tabellen Vardeforrad.';
COMMENT ON COLUMN ValueGroup.GroupLevel IS 'CNMM:Indicates witch level the group code belongs to.';
COMMENT ON COLUMN ValueGroup.ValueLevel IS 'CNMM:Indicates witch level the value code belongs to.';
COMMENT ON COLUMN ValueGroup.SortCode IS 'CNMM:Code for sorting groups within a group, in order to present them in a logical order.

If any group within a grouping of a range is the sort code, all the teams have that. If the sort code is missing, the field shall be NULL.';
COMMENT ON COLUMN ValueGroup.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValueGroup.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValueGroup IS 'CNMM:The table link values/ value set with groups';

CREATE TRIGGER ValueGroup_BUPSE BEFORE INSERT OR UPDATE ON ValueGroup
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValueGroup;


-------

CREATE TABLE ValueGroup_ENG(
 Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,GroupCode VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValueCode VARCHAR2(400 CHAR) NOT NULL ENABLE
 ,SortCode VARCHAR2(20 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValueGroup_ENG PRIMARY KEY (Grouping,GroupCode,ValueCode ) 
);

COMMENT ON COLUMN ValueGroup_ENG.Grouping IS 'CNMM: SL version:Name of grouping.

See further in the description of the table Grouping.';
COMMENT ON COLUMN ValueGroup_ENG.GroupCode IS 'CNMM: SL version:
See further in the description of the table Value.';
COMMENT ON COLUMN ValueGroup_ENG.ValueCode IS 'CNMM: SL version:Code for the value contained in the group. Retrieved from the table Value, column value code.
See further in the description of the table Value
Se beskrivning av tabellen Varde.';
COMMENT ON COLUMN ValueGroup_ENG.SortCode IS 'CNMM: SL version:Code for sorting groups within a group, in order to present them in a logical order.

If any group within a grouping of a range is the sort code, all the teams have that. If the sort code is missing, the field shall be NULL.';
COMMENT ON COLUMN ValueGroup_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValueGroup_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValueGroup_ENG IS 'CNMM: SL: The table link values/ value set with groups';

CREATE TRIGGER ValueGroup_ENG_BUPSE BEFORE INSERT OR UPDATE ON ValueGroup_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValueGroup_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ValuePool(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValuePoolAlias VARCHAR2(20 CHAR)
 ,PresText VARCHAR2(80 CHAR)
 ,Description VARCHAR2(200 CHAR) NOT NULL ENABLE
 ,ValueTextExists VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,ValuePres VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,MetaId VARCHAR2(100 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValuePool PRIMARY KEY (ValuePool ) 
);

COMMENT ON COLUMN ValuePool.ValuePool IS 'CNMM:Name of value pool. 

If there is only one variable belonging to a particular value pool, the variable and value pool should have the same name. 

The name should begin with a capital letter.';
COMMENT ON COLUMN ValuePool.ValuePoolAlias IS 'CNMM:Can be used to give the valuepool an alternative name.';
COMMENT ON COLUMN ValuePool.PresText IS 'CNMM:Presentation text for the value pool. 
 If there is no text, the field should be NULL.';
COMMENT ON COLUMN ValuePool.Description IS 'CNMM:Description of value pool. 
 
Should also contain information on the principles used for sorting the value pools values (i.e....sorting by particular principle,....sorting by value code). 
 
Written beginning with a capital letter.';
COMMENT ON COLUMN ValuePool.ValueTextExists IS 'CNMM:Here it is stated whether there are texts or not for the value pools values. There are the following alternatives: 

L = Long value text exists 
S = Short value text exists 
B = Both long and short value text exist 
N = No value texts for any values 


In the table Value (see descriptions of these columns) there are two columns for value texts, ValueTextS (for short texts) and ValueTextL (for long texts). If ValueTextExists = L, the value text is taken from column ValueTextL in the table Value. If ValueTextExists = S, the value text is taken from column ValueTextS in the table Value. If ValueTextExists = B, the value presentation is determined by what is specified in the field ValuePres in the tables ValuePool or ValueSet. If ValueTextExists = N, the values are presented only by a code in the retrieval interface. ';
COMMENT ON COLUMN ValuePool.ValuePres IS 'CNMM:Here it is shown how the values should be presented after retrieval. There are the following alternatives: 
 
A = Both code and short text should be presented 
B = Both code and long text should be presented 
C = Value code should be presented 
T = Long value text should be presented 
S = Short value text should be presented';
COMMENT ON COLUMN ValuePool.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN ValuePool.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValuePool.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValuePool IS 'CNMM:The table describes which value pools exist in the database. The value pool brings together all values and aggregates for a classification or a variation of a classification.';

CREATE TRIGGER ValuePool_BUPSE BEFORE INSERT OR UPDATE ON ValuePool
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValuePool;


-------

CREATE TABLE ValuePool_ENG(
 ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValuePoolAlias VARCHAR2(20 CHAR)
 ,PresText VARCHAR2(80 CHAR)
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValuePool_ENG PRIMARY KEY (ValuePool ) 
);

COMMENT ON COLUMN ValuePool_ENG.ValuePool IS 'CNMM: SL version:Name of value pool. 

If there is only one variable belonging to a particular value pool, the variable and value pool should have the same name. 

The name should begin with a capital letter.';
COMMENT ON COLUMN ValuePool_ENG.ValuePoolAlias IS 'CNMM: SL version:Can be used to give the valuepool an alternative name.';
COMMENT ON COLUMN ValuePool_ENG.PresText IS 'CNMM: SL version:Presentation text for the value pool. 
 If there is no text, the field should be NULL.';
COMMENT ON COLUMN ValuePool_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValuePool_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValuePool_ENG IS 'CNMM: SL: The table describes which value pools exist in the database. The value pool brings together all values and aggregates for a classification or a variation of a classification.';

CREATE TRIGGER ValuePool_ENG_BUPSE BEFORE INSERT OR UPDATE ON ValuePool_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValuePool_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ValueSet(
 ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR)
 ,Description VARCHAR2(200 CHAR) NOT NULL ENABLE
 ,ValuePool VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,ValuePres VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,GeoAreaNo NUMBER
 ,MetaId VARCHAR2(100 CHAR)
 ,SortCodeExists VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,Footnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
 ,EliminationCode VARCHAR2(20 CHAR)
 ,EliminationMethod VARCHAR2(20 CHAR)
, CONSTRAINT PK_ValueSet PRIMARY KEY (ValueSet ) 
);

COMMENT ON COLUMN ValueSet.ValueSet IS 'CNMM:Name of the stored value set. 
 
The name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.';
COMMENT ON COLUMN ValueSet.PresText IS 'CNMM:Presentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file. 

If the field is not used, it should be NULL.';
COMMENT ON COLUMN ValueSet.Description IS 'CNMM:Description of the content of the value set. 

The text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total. 

Text should begin with a capital letter.';
COMMENT ON COLUMN ValueSet.ValuePool IS 'CNMM:Name of the value pool that the value set belongs to. See further description of the table ValuePool.';
COMMENT ON COLUMN ValueSet.ValuePres IS 'CNMM:Used to show how the values in a value set should be presented when being retrieved. There are the following alternatives: 

A = Both code and short text should be presented 
B = Both code and long text should be presented 
C = Value code should be presented 
S = Short value text should be presented 
T = Long value text should be presented 
V = Presentation format is taken from the column ValuePres in the table ValuePool';
COMMENT ON COLUMN ValueSet.GeoAreaNo IS 'CNMM:Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL. 
 
The identification number should also be included in the table TextCatalog. For further information see description of TextCatalog.';
COMMENT ON COLUMN ValueSet.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN ValueSet.SortCodeExists IS 'CNMM:Code showing whether there is a particular sorting order for the value set. Can be: 
 
Y = Yes 
N = No 
 
If SortCodeExists = Y, the sorting code must be in VSValue for all values included in the value set. 
If SortCodeExists = N, the sorting code for the value pool is used (SortCode in the table Value).';
COMMENT ON COLUMN ValueSet.Footnote IS 'CNMM:Shows whether there is a footnote linked to a value in the value set (FootNoteType 6). There are the following alternatives: 

B = Both obligatory and optional footnotes exist 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist 
N = There are no footnotes';
COMMENT ON COLUMN ValueSet.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValueSet.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';
COMMENT ON COLUMN ValueSet.EliminationCode IS 'CNMM:For those rows with <b>Elimination</b> =”C”, the column <b>EliminationCode</b> should contain the code of the value that are to be used for elimination. 
Rows with <b>Elimination</b> &lt;&gt;”C” should not have any value in <b>EliminationCode</b>';
COMMENT ON COLUMN ValueSet.EliminationMethod IS 'CNMM:For the Elimination column in ValueSet table suggests Denmark that’s the values code for elimination will be moved in another column, e.g.  EliminationCode and change the name column name Elimination to EliminationMethod.
<b>EliminationMethod</b> column will have the values:
"N" or null/empty (no elimination)
"A" (aggregation) 
"C" (code)

Move values codes for elimination to a new column, so that the codes N and A can’t be confused with codes that are to be used for elimination.
For those rows with <b>EliminationMethod</b> ="C", the column <b>EliminationCode</b> should contain the code of the value that are to be used for elimination. 
Rows with <b>EliminationMethod</b> &lt;&gt;"C" should not have any value in <b>EliminationCode</b>
N = No elimination value, i.e. the variable cannot be eliminated 
A = Elimination value is obtained by aggregation of all values in the value set 
ValueCode = a selected value, included in the value set, that should be used at elimination.
';

COMMENT ON TABLE ValueSet IS 'CNMM:The table describes the value sets that exist for the different value pools.';

CREATE TRIGGER ValueSet_BUPSE BEFORE INSERT OR UPDATE ON ValueSet
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValueSet;


-------

CREATE TABLE ValueSet_ENG(
 ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR)
 ,Description VARCHAR2(200 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValueSet_ENG PRIMARY KEY (ValueSet ) 
);

COMMENT ON COLUMN ValueSet_ENG.ValueSet IS 'CNMM: SL version:Name of the stored value set. 
 
The name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.';
COMMENT ON COLUMN ValueSet_ENG.PresText IS 'CNMM: SL version:Presentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file. 

If the field is not used, it should be NULL.';
COMMENT ON COLUMN ValueSet_ENG.Description IS 'CNMM: SL version:Description of the content of the value set. 

The text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total. 

Text should begin with a capital letter.';
COMMENT ON COLUMN ValueSet_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValueSet_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValueSet_ENG IS 'CNMM: SL: The table describes the value sets that exist for the different value pools.';

CREATE TRIGGER ValueSet_ENG_BUPSE BEFORE INSERT OR UPDATE ON ValueSet_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValueSet_ENG_BUPSE;


------------------------------------------------------------

CREATE TABLE ValueSetGrouping(
 ValueSet VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,Grouping VARCHAR2(30 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_ValueSetGrouping PRIMARY KEY (ValueSet,Grouping ) 
);

COMMENT ON COLUMN ValueSetGrouping.ValueSet IS 'CNMM:Name of the stored value set. 
 
See description of table ValueSet.';
COMMENT ON COLUMN ValueSetGrouping.Grouping IS 'CNMM:Name of grouping. 
 
See further in the description of the table Grouping.';
COMMENT ON COLUMN ValueSetGrouping.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN ValueSetGrouping.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE ValueSetGrouping IS 'CNMM:The table connects value set to grouping';

CREATE TRIGGER ValueSetGrouping_BUPSE BEFORE INSERT OR UPDATE ON ValueSetGrouping
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END ValueSetGrouping;


------------------------------------------------------------

CREATE TABLE Variable(
 Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,VariableInfo VARCHAR2(200 CHAR)
 ,MetaId VARCHAR2(100 CHAR)
 ,Footnote VARCHAR2(1 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Variable PRIMARY KEY (Variable ) 
);

COMMENT ON COLUMN Variable.Variable IS 'CNMM:Name of distributed statistical variable. Name of metadata column for the variable in the data table. 
 
The variable name must be unique within a main table. 
 
The name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN Variable.PresText IS 'CNMM:Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval. 

The entire text should be written in lower case letters, with the exception of abbreviations, etc.';
COMMENT ON COLUMN Variable.VariableInfo IS 'CNMM:Descriptive information on variables, primarily for internal use, to facilitate the selection of a variable when drawing up new tables. 
 
If there is no text, the field should be NULL.';
COMMENT ON COLUMN Variable.MetaId IS 'CNMM:MetaId can be used to link the information in this table to an external system.';
COMMENT ON COLUMN Variable.Footnote IS 'CNMM:Shows whether there is a footnote linked to the variable (FootnoteType 5). There are the following alternatives: 

B = Both obligatory and optional footnotes exist 
V = One or several optional footnotes exist. 
O = One or several obligatory footnotes exist 
N = There are no footnotes';
COMMENT ON COLUMN Variable.UserId IS 'CNMM:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Variable.LogDate IS 'CNMM:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Variable IS 'CNMM:The table contains the distributed statistical variables in the database.';

CREATE TRIGGER Variable_BUPSE BEFORE INSERT OR UPDATE ON Variable
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Variable;


-------

CREATE TABLE Variable_ENG(
 Variable VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,PresText VARCHAR2(80 CHAR) NOT NULL ENABLE
 ,UserId VARCHAR2(20 CHAR) NOT NULL ENABLE
 ,LogDate timestamp(9) NOT NULL ENABLE
, CONSTRAINT PK_Variable_ENG PRIMARY KEY (Variable ) 
);

COMMENT ON COLUMN Variable_ENG.Variable IS 'CNMM: SL version:Name of distributed statistical variable. Name of metadata column for the variable in the data table. 
 
The variable name must be unique within a main table. 
 
The name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.';
COMMENT ON COLUMN Variable_ENG.PresText IS 'CNMM: SL version:Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval. 

The entire text should be written in lower case letters, with the exception of abbreviations, etc.';
COMMENT ON COLUMN Variable_ENG.UserId IS 'CNMM: SL version:User identification for the person who has added or updated the current row.';
COMMENT ON COLUMN Variable_ENG.LogDate IS 'CNMM: SL version:Date and time for the addition or updating of the current row.';

COMMENT ON TABLE Variable_ENG IS 'CNMM: SL: The table contains the distributed statistical variables in the database.';

CREATE TRIGGER Variable_ENG_BUPSE BEFORE INSERT OR UPDATE ON Variable_ENG
FOR EACH ROW
BEGIN
 IF (:NEW.LOGDATE IS NULL) OR (:NEW.LOGDATE  = :OLD.LOGDATE) THEN
   :NEW.USERID := USER;
   :NEW.LOGDATE := systimestamp;
  END IF;
END Variable_ENG_BUPSE;


------------------------------------------------------------




---------------------------------------
-- GRANTS         O_PCAX_ROLE
GRANT SELECT ON ATTRIBUTE TO O_PCAX_ROLE;
GRANT SELECT ON ATTRIBUTE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON COLUMNCODE TO O_PCAX_ROLE;
GRANT SELECT ON COLUMNCODE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON CONTENTS TO O_PCAX_ROLE;
GRANT SELECT ON CONTENTS_ENG TO O_PCAX_ROLE;
GRANT SELECT ON CONTENTSTIME TO O_PCAX_ROLE;
GRANT SELECT ON DATASTORAGE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTTIME TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTVALUE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTVBL TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTENTS TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEGROUPING TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTABLE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTTIME TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTVALUE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMENUSEL TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTESUBTABLE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVALUE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVALUESETVALUE TO O_PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVARIABLE TO O_PCAX_ROLE;
GRANT SELECT ON GROUPING TO O_PCAX_ROLE;
GRANT SELECT ON GROUPING_ENG TO O_PCAX_ROLE;
GRANT SELECT ON GROUPINGLEVEL TO O_PCAX_ROLE;
GRANT SELECT ON GROUPINGLEVEL_ENG TO O_PCAX_ROLE;
GRANT SELECT ON LINK TO O_PCAX_ROLE;
GRANT SELECT ON LINK_ENG TO O_PCAX_ROLE;
GRANT SELECT ON LINKMENUSELECTION TO O_PCAX_ROLE;
GRANT SELECT ON MAINTABLE TO O_PCAX_ROLE;
GRANT SELECT ON MAINTABLE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON MAINTABLEPERSON TO O_PCAX_ROLE;
GRANT SELECT ON MAINTABLEVARIABLEHIERARCHY TO O_PCAX_ROLE;
GRANT SELECT ON MENUSELECTION TO O_PCAX_ROLE;
GRANT SELECT ON MENUSELECTION_ENG TO O_PCAX_ROLE;
GRANT SELECT ON METAADM TO O_PCAX_ROLE;
GRANT SELECT ON METABASEINFO TO O_PCAX_ROLE;
GRANT SELECT ON ORGANIZATION TO O_PCAX_ROLE;
GRANT SELECT ON ORGANIZATION_ENG TO O_PCAX_ROLE;
GRANT SELECT ON PERSON TO O_PCAX_ROLE;
GRANT SELECT ON SECONDARYLANGUAGE TO O_PCAX_ROLE;
GRANT SELECT ON SPECIALCHARACTER TO O_PCAX_ROLE;
GRANT SELECT ON SPECIALCHARACTER_ENG TO O_PCAX_ROLE;
GRANT SELECT ON SUBTABLE TO O_PCAX_ROLE;
GRANT SELECT ON SUBTABLE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON SUBTABLEVARIABLE TO O_PCAX_ROLE;
GRANT SELECT ON TEXTCATALOG TO O_PCAX_ROLE;
GRANT SELECT ON TEXTCATALOG_ENG TO O_PCAX_ROLE;
GRANT SELECT ON TIMESCALE TO O_PCAX_ROLE;
GRANT SELECT ON TIMESCALE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VSVALUE TO O_PCAX_ROLE;
GRANT SELECT ON VSVALUE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VALUE TO O_PCAX_ROLE;
GRANT SELECT ON VALUE_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VALUEGROUP TO O_PCAX_ROLE;
GRANT SELECT ON VALUEGROUP_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VALUEPOOL TO O_PCAX_ROLE;
GRANT SELECT ON VALUEPOOL_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VALUESET TO O_PCAX_ROLE;
GRANT SELECT ON VALUESET_ENG TO O_PCAX_ROLE;
GRANT SELECT ON VALUESETGROUPING TO O_PCAX_ROLE;
GRANT SELECT ON VARIABLE TO O_PCAX_ROLE;
GRANT SELECT ON VARIABLE_ENG TO O_PCAX_ROLE;


---------------------------------------
-- GRANTS         PCAX_ROLE
GRANT SELECT ON ATTRIBUTE TO PCAX_ROLE;
GRANT SELECT ON ATTRIBUTE_ENG TO PCAX_ROLE;
GRANT SELECT ON COLUMNCODE TO PCAX_ROLE;
GRANT SELECT ON COLUMNCODE_ENG TO PCAX_ROLE;
GRANT SELECT ON CONTENTS TO PCAX_ROLE;
GRANT SELECT ON CONTENTS_ENG TO PCAX_ROLE;
GRANT SELECT ON CONTENTSTIME TO PCAX_ROLE;
GRANT SELECT ON DATASTORAGE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTE_ENG TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTTIME TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTVALUE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTVBL TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTECONTENTS TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEGROUPING TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTABLE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTTIME TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMAINTVALUE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEMENUSEL TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTESUBTABLE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVALUE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVALUESETVALUE TO PCAX_ROLE;
GRANT SELECT ON FOOTNOTEVARIABLE TO PCAX_ROLE;
GRANT SELECT ON GROUPING TO PCAX_ROLE;
GRANT SELECT ON GROUPING_ENG TO PCAX_ROLE;
GRANT SELECT ON GROUPINGLEVEL TO PCAX_ROLE;
GRANT SELECT ON GROUPINGLEVEL_ENG TO PCAX_ROLE;
GRANT SELECT ON LINK TO PCAX_ROLE;
GRANT SELECT ON LINK_ENG TO PCAX_ROLE;
GRANT SELECT ON LINKMENUSELECTION TO PCAX_ROLE;
GRANT SELECT ON MAINTABLE TO PCAX_ROLE;
GRANT SELECT ON MAINTABLE_ENG TO PCAX_ROLE;
GRANT SELECT ON MAINTABLEPERSON TO PCAX_ROLE;
GRANT SELECT ON MAINTABLEVARIABLEHIERARCHY TO PCAX_ROLE;
GRANT SELECT ON MENUSELECTION TO PCAX_ROLE;
GRANT SELECT ON MENUSELECTION_ENG TO PCAX_ROLE;
GRANT SELECT ON METAADM TO PCAX_ROLE;
GRANT SELECT ON METABASEINFO TO PCAX_ROLE;
GRANT SELECT ON ORGANIZATION TO PCAX_ROLE;
GRANT SELECT ON ORGANIZATION_ENG TO PCAX_ROLE;
GRANT SELECT ON PERSON TO PCAX_ROLE;
GRANT SELECT ON SECONDARYLANGUAGE TO PCAX_ROLE;
GRANT SELECT ON SPECIALCHARACTER TO PCAX_ROLE;
GRANT SELECT ON SPECIALCHARACTER_ENG TO PCAX_ROLE;
GRANT SELECT ON SUBTABLE TO PCAX_ROLE;
GRANT SELECT ON SUBTABLE_ENG TO PCAX_ROLE;
GRANT SELECT ON SUBTABLEVARIABLE TO PCAX_ROLE;
GRANT SELECT ON TEXTCATALOG TO PCAX_ROLE;
GRANT SELECT ON TEXTCATALOG_ENG TO PCAX_ROLE;
GRANT SELECT ON TIMESCALE TO PCAX_ROLE;
GRANT SELECT ON TIMESCALE_ENG TO PCAX_ROLE;
GRANT SELECT ON VSVALUE TO PCAX_ROLE;
GRANT SELECT ON VSVALUE_ENG TO PCAX_ROLE;
GRANT SELECT ON VALUE TO PCAX_ROLE;
GRANT SELECT ON VALUE_ENG TO PCAX_ROLE;
GRANT SELECT ON VALUEGROUP TO PCAX_ROLE;
GRANT SELECT ON VALUEGROUP_ENG TO PCAX_ROLE;
GRANT SELECT ON VALUEPOOL TO PCAX_ROLE;
GRANT SELECT ON VALUEPOOL_ENG TO PCAX_ROLE;
GRANT SELECT ON VALUESET TO PCAX_ROLE;
GRANT SELECT ON VALUESET_ENG TO PCAX_ROLE;
GRANT SELECT ON VALUESETGROUPING TO PCAX_ROLE;
GRANT SELECT ON VARIABLE TO PCAX_ROLE;
GRANT SELECT ON VARIABLE_ENG TO PCAX_ROLE;

